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
        COM_HANDINFO,
        COM_OK,
        COM_RESET,
        COM_ALLINFO = 0x20,	/* chipid, manufactureid, modelid, hardwareid */
        COM_CHIPID,
        COM_MFID,
        COM_MDID,
        COM_HWID,
        COM_LICENSE,		/*授权信息*/
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
        COM_END
    };
    //========================================================================================
   /// <summary>
   /// Uart 类
   /// </summary>
    public class UartProtocol
    {
        Byte  FRAMESTARTCODE=0x48;
        int BufferLength = 128;
        //Int32 CONTAINER_LENGTH = 5;//命令行长度 5+data.Length
        int packetLength = 256;

        SerialPort mSpSlot;

        System.Threading.Semaphore mSemaphore;

        static Byte[] mReadBuffer;//串口读到的数据
        int mBytesRead = 0;//串口读到的数据个数

        Int32 mBytesWantToRead;//读取串口指定个数的数据
       public Int32 CONTAINER_LENGTH = 5;

        public  UartProtocol(SerialPort SpSlot)
        {
            mSpSlot = SpSlot;
            mSpSlot.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(dataReceived);

            mReadBuffer = new Byte[packetLength];

            mSemaphore = new System.Threading.Semaphore(0, 1);
        }

        /// <summary>
        /// 命令总入口
        /// </summary>
        /// <param name="cmd">发送命令的类型</param>
        /// <param name="dataLength">发送命令行中数据长度</param>
        /// <param name="dataAck">发送的数据</param>
        /// <param name="dataReq">返回的数据</param>
        /// <returns></returns>
        public Int32 Command(SERCOM_TYPE cmd, int ReqdataLength,int AckdataLength, Byte[] dataReq, ref Byte[] cmdlineAck)
        {
            Int32 errCode = 0;

            // set bytes want to read before writing
            mBytesWantToRead = CONTAINER_LENGTH + AckdataLength;

            // flush serial port
            mSpSlot.DiscardInBuffer();
            mReadBuffer.Initialize();
            //////////////////////////////////////////////////////////////////////////发送
            ////////if (dataReq != null)
            ////////{
            ////////    CONTAINER_LENGTH += dataReq.Length;
            ////////}
            Byte[] packet = new Byte[CONTAINER_LENGTH + ReqdataLength];
            bool ACKData = PutCommData(cmd, ReqdataLength, dataReq, packet);//组建发送命令包
            if (!ACKData)
            {
                return -1;
            }
            try
            {
                mSpSlot.Write(packet, 0, packet.Length);       //发送命令
            }
            catch(Exception ex)
            {
                HDIC_Message.ShowWarnDialog(null, "发送命令的时候出错！\r\n"+ex.ToString());
                return -2;
            }
            //////////////////////////////////////////////////////////////////////////接收
            if (!mSemaphore.WaitOne(5000))//超时
            {
                return -5; // timeout
            }
            // read
            //Byte[] dataR = new Byte[dataIndex(mReadBuffer)];//读取指定个数的byte值
            Byte[] dataR = new Byte[mBytesWantToRead];//读取指定个数的byte值
            Int32 numbers = read(dataR);

            if (!GetCommData(dataR))
            {
                return -3;
            }
            else
            {
                cmdlineAck = dataR;  
            }
            //Array.Copy(mReadBuffer, 0, dataR, 0, dataR.Length);
            //HDIC_Message.ShowInfoDialog(null, dataR.ToString());
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

               if (sendData != null) { Packet[CONTAINER_LENGTH-1 + sendData.Length] = sum; }//应该是从第五个开始,先临时这样写
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

        /// <summary>
        /// 接受命令
        /// </summary>
        /// <param name="GetPacket"></param>
        /// <param name="cmdone"></param>
        /// <param name="dataAck"></param>
        /// <returns></returns>
        private bool GetCommData(Byte[] GetPacket)
        {
            Byte cell=0;
            // check startcode
            cell = GetPacket[(Int32)Index.startcode];
            if (cell != FRAMESTARTCODE)
            {
                HDIC_Message.ShowWarnDialog(null, String.Format("check start code[{0}] error", cell));
                return false;
            }

            ////check cmdone
            //cell = GetPacket[(Int32)Index.cmdone];
            //if (cell != (Byte)cmdone)
            //{
            //    HDIC_Message.ShowWarnDialog(null, String.Format("check command code[{0}] error", cell));
            //    return false;
            //}

            ////check cmdtwo
            //cell = GetPacket[(Int32)Index.cmdtwo];
            //if (cell != 0)
            //{
            //    HDIC_Message.ShowWarnDialog(null, String.Format("check cmdtwo code[{0}] error", cell));
            //    return false;
            //}

            ////check dataLength
            //cell = GetPacket[(Int32)Index.length];
            //checkSum += cell;
            //if (cell != dataLength)
            //{
            //    HDIC_Message.ShowWarnDialog(null, String.Format("check dataLength code[{0}] error", cell));
            //    return false;
            //}

            for (int i = 1; i < GetPacket.Length-1; i++)
            {
                cell += GetPacket[i];
            }
            //if ((Int32)GetPacket[(Int32)Index.length] > 0)
            //{
            //    // check sum
            //    for (Int32 i = (Int32)Index.buffer; i < GetPacket.Length - 1; i++)
            //    {
            //        cell += GetPacket[i];
            //    }
            //    ////get data
            //    //Array.Copy(GetPacket, (Int32)Index.buffer, dataAck, 0, dataLength);
            //}

            if (cell != GetPacket[GetPacket.Length - 1])
            {
                HDIC_Message.ShowWarnDialog(null, String.Format("check sum({0} != {1}) error", cell.ToString("X2"),
            GetPacket[GetPacket.Length - 1].ToString("X2")));
                return false;
            }

            return true;
        }

        Int32 read(Byte[] data)
        {
            Array.Copy(mReadBuffer, data, data.Length);

            // get current port buffer
            mBytesRead -= data.Length;
            if (mBytesRead < 0)
            {
                mBytesRead = 0;
            }
            return data.Length;
        }

        void dataReceived(System.Object sender, System.IO.Ports.SerialDataReceivedEventArgs e) //received data
        {
            try
            {
                if (mSpSlot.BytesToRead <= 0)
                {
                    return;
                }

                // move bytes from serial port to built-in read buffer
                mBytesRead += mSpSlot.Read(mReadBuffer, mBytesRead, mSpSlot.BytesToRead);

                if (mBytesRead >= mBytesWantToRead)
                {
                    mSemaphore.Release();
                    mBytesRead = 0; // reset
                }
            }
            catch (Exception ex)
            {
                HDIC_Message.ShowWarnDialog(null, ex.ToString());
                return;
            }
        }
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
        SerialPort mSpSlot;

        String mPortName;
        Int32 mBaudRate;
        Parity mParity;
        Int32 mDataBits;
        StopBits mStopBit;

        public void loadConfig()
        {
            string mConfigFile = String.Format("{0}\\config\\Serial.xml", HDIC_Func.GetRunningPath());
            DataTable dt = HDIC_Func.XMLToDataSet(mConfigFile).Tables["com"];
            mPortName = dt.Rows[0]["port"].ToString().Trim();
            mBaudRate = Convert.ToInt32(dt.Rows[0]["baud_rate"].ToString().Trim());
            mParity = (Parity)Enum.Parse(typeof(Parity), dt.Rows[0]["parity"].ToString().Trim());
            mDataBits = Convert.ToInt32(dt.Rows[0]["data_bits"].ToString().Trim());
            mStopBit = (StopBits)Enum.Parse(typeof(Parity), dt.Rows[0]["stop_bit"].ToString().Trim());


        }
        /// <summary>
        /// open port
        /// </summary>
        /// <returns></returns>
        public bool open()
        {
            try
            {
                if (mSpSlot == null)
                {
                    mSpSlot = new SerialPort(mPortName, mBaudRate, mParity, mDataBits, mStopBit);
                }
                if (!mSpSlot.IsOpen)
                {
                    mSpSlot.Open();
                }
            }
            catch (System.Exception ex)
            {
                HDIC_Message.ShowWarnDialog(null, "串口打开失败，请检查串口.\r\n" + ex.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// close port
        /// </summary>
        public bool close()
        {
            try
            {
                if (mSpSlot == null)
                {
                    return true;
                }
                if (mSpSlot.IsOpen)
                {
                    mSpSlot.Close();
                }
            }
            catch (System.Exception ex)
            {
                HDIC_Message.ShowWarnDialog(null, "无法关闭串口.\r\n" + ex.ToString());
                return false;
            }
            return true;
        }

        public SerialPort slot
        {
            get
            {
                return mSpSlot;
            }
        }
    }
    #endregion

}
