using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using HDICSoft.Func;
using HDICSoft.Message;

using System.Data;
using System.IO.Ports;

namespace soc_protocol
{
    /// <summary>
    /// 枚举类型
    /// </summary>
    public enum  Index
    {
         startcode,
         cmdone,
         cmdtwo,//这个是备用，暂时没有用到
         length,
         buffer,
         checksum,
    };

   public enum SERCOM_TYPE
    {
        COM_NULL = 0,
        COM_START,
        COM_CONNECT = 0x10,
        COM_ASKHAND,
        COM_HANDINFO,
        COM_OK,
        COM_RESET,
        COM_ALLINFO = 0x20,	/* chipid, manufactureid, modelid, hardwareid */
        COM_CHIPID,
        COM_MFID,
        COM_MDID,
        COM_HWID,
        COM_STBTYPE,
        COM_CAID,
        COM_LICENSE = 0x30,		/*授权信息*/
        COM_LICENSEOK,		/*授权信息ok*/
        COM_GETLICENSE,		/*flash上授权信息*/
        COM_GETLICENSEOK,	/*flash上授权信息ok*/
        COM_OPENFLASH,
        COM_OPENFLASHOK,
        COM_CLOSEFLASH,
        COM_CLOSEFLASHOK,
        COM_SECURITY,
        COM_SECURITYOK,

        COM_DEBUG = 0x80,	/* 发送debug信息，下位机内存的值 addr + val*/
        COM_RETURN,
        COM_FAIL,
        COM_END
    };
    //========================================================================================
   /// <summary>
   /// Uart 类
   /// </summary>
    public class UartProtocol
    {
        Byte  FRAMESTARTCODE=0x48;
        Int32 packetLength = 4096;

        SerialPort mSpSlot;

        System.Threading.Semaphore mSemaphore;

        Int32 CONTAINER_LENGTH = 5;

        static Int32 resendCount=3;//间歇重发次数

        public  UartProtocol(SerialPort SpSlot)
        {
            mSpSlot = SpSlot;

            mSemaphore = new System.Threading.Semaphore(0, 1);

           
            mSpSlot.ReceivedBytesThreshold = 5;


        }

        /// <summary>
        /// 命令总入口
        /// </summary>
        /// <param name="cmd">发送命令的类型</param>
        /// <param name="dataLength">发送命令行中数据长度</param>
        /// <param name="dataAck">发送的数据</param>
        /// <param name="dataReq">返回的数据</param>
        /// <returns></returns>
        public Int32 Command(SERCOM_TYPE Reqcmd,SERCOM_TYPE Ackcmd, int ReqdataLength,int AckdataLength, Byte[] dataReq, ref Byte[] cmdlineAck)
        {
            mReadBuffer.Clear();
            Int32 errCode = 0;
            Int32 ReqCount = 0;

            if (ReceiveBytes != null)
            {
                Array.Clear(ReceiveBytes, 0, ReceiveBytes.Length);
            }
            mSpSlot.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(dataReceived);

            if (Ackcmd != SERCOM_TYPE.COM_NULL)
            {
                // flush serial port
                mSpSlot.DiscardInBuffer();
            //mReadBuffer.Initialize();
            //////////////////////////////////////////////////////////////////////////发送
            ReqCount = CONTAINER_LENGTH;
            if (dataReq != null)
            {
                ReqCount +=dataReq.Length;
            }
            Byte[] packet = new Byte[ReqCount];
            bool ReqData = PutCommData(Reqcmd, ReqdataLength, dataReq, packet);//组建发送命令包
            if (!ReqData)
            {
                return -100;
            }
            try
            {
                mSpSlot.Write(packet, 0, packet.Length);       //发送命令
            }
            catch
            {
                return -110;
            }

            }
            //////////////////////////////////////////////////////////////////////////接收
            if (!mSemaphore.WaitOne(1000))//超时
            {
                    return -120; // timeout
            }

                cmdlineAck = ReceiveBytes;
                mSpSlot.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(dataReceived);

                return errCode;
           
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="cmd">命令类型</param>
        /// <param name="Packet">返回包</param>
        /// <param name="sendData">要发送的数据</param>
        /// <returns></returns>
        private bool PutCommData(SERCOM_TYPE cmd,int dataLength, Byte[] sendData,Byte[] Packet)
        {
           try
           {
               //Byte[] mstartcode =new Byte[sizeof(UInt16)];
               //uIntToByteArray(FRAMESTARTCODE, ref mstartcode);

               //Int32 index = 0;
               //mstartcode.CopyTo(Packet, index);
               Byte sum = 0;

               Packet[(Int32)Index.startcode] =(Byte)FRAMESTARTCODE;
               sum += Packet[(Int32)Index.startcode];

               //Packet[(Int32)Index.protocol_type] = (Byte)FRAMEPRO_TYPE;
               //sum += Packet[(Int32)Index.protocol_type];

               //Packet[(Int32)Index.command_direction] = (Byte)FRAMECOMM_DIR;
               //sum += Packet[(Int32)Index.command_direction];

               Packet[(Int32)Index.cmdone] = (Byte)cmd;
               sum += Packet[(Int32)Index.cmdone];

               Packet[(Int32)Index.cmdtwo] = (Byte)SERCOM_TYPE.COM_NULL;
               sum += Packet[(Int32)Index.cmdtwo];

               Packet[(Int32)Index.length] = (Byte)dataLength;
               sum += (Byte)dataLength;

               if (sendData!=null)
               {
                  
                   //sendData.CopyTo(Packet, (Int32)Index.buffer);
                   for (int i = 0; i < dataLength; i++)
                   {
                       Packet[((Int32)Index.buffer)+i]=sendData[i];
                       sum += (Byte)sendData[i];
                   }
               }

               if (sendData != null) { Packet[CONTAINER_LENGTH-1 + sendData.Length] = sum; }
               else { Packet[CONTAINER_LENGTH-1] = sum; }

               if (sum==0)
               {
                   HDIC_Message.ShowWarnDialog(null, "没有内容，无法组建发送包");
                   return false;
               }
           }
           catch (System.Exception ex)
           {
               HDIC_Message.ShowWarnDialog(null, "组建发送包的时候出现错误："+ex.ToString());
               return false;
           }
           return true;
        }

        private List<byte> mReadBuffer = new List<byte>(2048);
        byte[] ReceiveBytes;
        Int32 ErrorCount = 0;
        void dataReceived(System.Object sender, System.IO.Ports.SerialDataReceivedEventArgs e) //received data
        {
            System.Threading.Thread.Sleep(50); //等待50毫秒
            try
            {
                if (mSpSlot.BytesToRead <= 0)
                {
                    return;
                }
                
                int n = mSpSlot.BytesToRead;
                byte[] buf = new byte[n];
                mSpSlot.Read(buf, 0, n);
                //1.缓存数据           
                mReadBuffer.AddRange(buf);

                #region 打印
                string log = System.Text.Encoding.ASCII.GetString(buf, 0, buf.Length);
                HDIC_Func.LogRecord(log);
                #endregion

                while (true)
                {
                    int CutBufferIndex = mReadBuffer.IndexOf(FRAMESTARTCODE);
                    if (mReadBuffer.Count > 0 && CutBufferIndex > 0)
                    {
                        mReadBuffer.RemoveRange(0, mReadBuffer.IndexOf(0x48));
                    }
                    if (mReadBuffer.Count < 5)
                    {
                        return;
                    }
                    int len = mReadBuffer[3];
                    if (len == 0 || len == 1 || len == 4 || len == 7 || len == 88|| len==11 || len==12)
                    {
                        if (len != mReadBuffer.Count - 5)//如果数据长度 ！= 数据长度位，则说明不是命令包
                        {
                            System.Threading.Thread.Sleep(5);
                            ErrorCount++;
                            if (ErrorCount == 3)//如果截取段三次检测校验位都不正确，则移除第一位字符
                            {
                                mReadBuffer.RemoveAt(0);
                                ErrorCount = 0;
                            }
                            break;
                        }

                        byte cell = 0;
                        for (int i = 0; i < 4 + len; i++)
                        {
                            cell += mReadBuffer[i];
                        }
                        if (cell == mReadBuffer[CONTAINER_LENGTH + len - 1])
                        {
                            ReceiveBytes = new byte[CONTAINER_LENGTH + len];
                            mReadBuffer.CopyTo(0, ReceiveBytes, 0, CONTAINER_LENGTH + len);
                            mReadBuffer.Clear();
                            mSemaphore.Release();
                            System.Threading.Thread.Sleep(50); //等待50毫秒
                        }
                        else
                        {
                            //System.Threading.Thread.Sleep(5);
                            //ErrorCount++;
                            //if (ErrorCount == 3)//如果截取段三次检测校验位都不正确，则移除第一位字符
                            //{
                                mReadBuffer.RemoveAt(0);
                                ErrorCount = 0;
                            //}
                            break;
                        }
                    }
                    else
                    {
                        mReadBuffer.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                HDIC_Message.ShowWarnDialog(null, ex.ToString());
                return;
            }
                ////////////////////////////////////////////
                #region 暂时屏蔽
            //////    int CutBufferIndex=mReadBuffer.IndexOf(FRAMESTARTCODE);
            //////    if (mReadBuffer.Count>0 && CutBufferIndex > 0)
            //////    {
            //////        mReadBuffer.RemoveRange(0, CutBufferIndex);
            //////    }
            //////    //2.完整性判断           
            //////    while (mReadBuffer.Count >= 5)
            //////    {
            //////        //2.1 查找数据头       
            //////        if (mReadBuffer[0] == FRAMESTARTCODE) //传输数据有帧头，用于判断        
            //////        {
            //////                int len = mReadBuffer[3];
            //////                if (mReadBuffer[1] != (Byte)SERCOM_TYPE.COM_ALLINFO || mReadBuffer[1] != (Byte)SERCOM_TYPE.COM_CHIPID)
            //////                {
            //////                    if (len > 0)
            //////                    {
            //////                        mReadBuffer.RemoveAt(0);
            //////                        return;
            //////                    }
            //////                }
            //////                //if (mReadBuffer.Count < len + CONTAINER_LENGTH) //数据区尚未接收完整         
            //////                //{
            //////                //    return;
            //////                //}
            //////                //得到完整的数据，复制到ReceiveBytes中进行校验  
            //////                ReceiveBytes = new byte[len + CONTAINER_LENGTH];
            //////                mReadBuffer.CopyTo(0, ReceiveBytes, 0, len + CONTAINER_LENGTH-1);

            //////                #region 校验
            //////                byte cell = 0;
            //////                for (int i = 0; i < ReceiveBytes.Length - 1; i++)
            //////                {
            //////                    cell += ReceiveBytes[i];
            //////                }
            //////                if (cell != ReceiveBytes[ReceiveBytes.Length - 1])
            //////                {
            //////                    //    HDIC_Message.ShowWarnDialog(null, String.Format("check sum({0} != {1}) error", cell.ToString("X2"),
            //////                    //ReceiveBytes[ReceiveBytes.Length - 1].ToString("X2")));
            //////                    //mReadBuffer.Clear();
            //////                    if (mReadBuffer.Count > 0)  {  mReadBuffer.RemoveAt(0); }
                              
            //////                    return;
            //////                }
            //////                #endregion
            //////                mReadBuffer.Clear();
            //////                mSemaphore.Release();
            //////        }
            //////        else //帧头不正确时，记得清除            
            //////        {
            //////            if (mReadBuffer.Count > 0) { mReadBuffer.RemoveAt(0); }
            //////        }        
            //////    }
            //////}
            //////catch (Exception ex)
            //////{
            //////    HDIC_Message.ShowWarnDialog(null, ex.ToString());
            //////    return;
            //////}
                #endregion
        }
        ////////////////////////////////////////////////////////////////////////////
      
        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 获取串口中有效的数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Int32 dataIndex(Byte[] data)
        {
            int l = data.Length;
            for (int i = data.Length - 1; i >= 0; i--)
            {
                if (data[i] == 0)
                {
                    l = l - 1;
                }
                else
                {
                    break;
                }
            }
            return l;

        }

       #region IntToByteArray
       private Int32 uIntToByteArray(UInt32 uInt, ref Byte[] pArray)
       {
           Int32 errCode = 0;
           //pArray = new Byte[sizeof(UInt32)];
           (pArray)[0] = Convert.ToByte((uInt & 0x0000FF00) >> (int)getMaskOffset(0x0000FF00));
           (pArray)[1] = Convert.ToByte((uInt & 0x000000FF) >> (int)getMaskOffset(0x000000FF));
           (pArray)[3] = Convert.ToByte((uInt & 0x00FF0000) >> (int)getMaskOffset(0x00FF0000));
           (pArray)[4] = Convert.ToByte((uInt & 0xFF000000) >> (int)getMaskOffset(0xFF000000));

           return errCode;
       }

       private Int32 uIntToByteArray(UInt16 uInt, ref Byte[] pArray)
       {
           Int32 errCode = 0;

           //pArray = new Byte[sizeof(UInt16)];
           (pArray)[0] = Convert.ToByte((uInt & 0x0000FF00) >> (int)getMaskOffset(0x0000FF00));
           (pArray)[1] = Convert.ToByte((uInt & 0x000000FF) >> (int)getMaskOffset(0x000000FF));

           return errCode;
       }

       UInt32 getMaskOffset(UInt32 mask)
       {
           UInt32 offset = 0;
           const UInt32 maskBit0 = 0x00000001;

           if (mask <= 0)
           {
               return offset;
           }
           for (offset = 0; offset < sizeof(UInt32) * 8; offset++)
           {
               if ((mask & maskBit0) > 0)
               { // bit0 is 1
                   break;
               }

               mask >>= 1;
           }

           return offset;
       }
       #endregion
    }
    //==============================================================================
    /// <summary>
    /// SerialPort
    /// </summary>
    #region SerialPort
    public class Uart
    {
       static SerialPort mSpSlot;

     static String mPortName;
     static Int32 mBaudRate;
     static Parity mParity;
     static Int32 mDataBits;
     static StopBits mStopBit;

        public static void loadConfig()
        {
            string mConfigFile = String.Format("{0}\\config\\Serial.xml", HDIC_Func.GetRunningPath());
            DataTable dt = HDIC_Func.XMLToDataSet(mConfigFile).Tables["com"];
            mPortName = dt.Rows[0]["port"].ToString().Trim();
            mBaudRate = Convert.ToInt32(dt.Rows[0]["baud_rate"].ToString().Trim());
            mParity = (Parity)Enum.Parse(typeof(Parity), dt.Rows[0]["parity"].ToString().Trim());
            mDataBits = Convert.ToInt32(dt.Rows[0]["data_bits"].ToString().Trim());
            mStopBit = (StopBits)Enum.Parse(typeof(Parity), dt.Rows[0]["stop_bit"].ToString().Trim());

            mSpSlot = new SerialPort(mPortName, mBaudRate, mParity, mDataBits, mStopBit);
            //open();
        }
        /// <summary>
        /// open port
        /// </summary>
        /// <returns></returns>
        public static bool open()
        {
            try
            {
                if (mSpSlot == null)
                {
                    loadConfig();
                }
                //if (!mSpSlot.IsOpen)
                //{
                    mSpSlot.Open();
                //}
            }
            catch 
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// close port
        /// </summary>
        public static bool close()
        {
            try
            {
                if (mSpSlot == null)
                {
                    return true;
                }
                //if (mSpSlot.IsOpen)
                //{
                    mSpSlot.Close();
                //}
            }
            catch (System.Exception ex)
            {
                HDIC_Message.ShowWarnDialog(null, "无法关闭串口.\r\n" + ex.ToString());
                return false;
            }
            return true;
        }

        public static SerialPort slot
        {
            get
            {
                loadConfig();
                return mSpSlot;
            }
        }
    }
    #endregion

}
