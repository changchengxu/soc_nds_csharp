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
         cmdtwo,
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
        COM_OPENFLASH,
        COM_OPENFLASHOK,
        COM_CLOSEFLASH,
        COM_CLOSEFLASHOK,

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
        Int32 CONTAINER_LENGTH = 5;//命令行长度 5+data.Length
        SerialPort mSpSlot;

        System.Threading.Semaphore mSemaphore;

        Byte[] mReadBuffer;//串口读到的数据
        int mBytesRead = 0;//串口读到的数据个数

        Int32 mBytesWantToRead;//读取串口指定个数的数据

        public  UartProtocol(SerialPort SpSlot)
        {
            mSpSlot = SpSlot;
            mSpSlot.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(dataReceived);

            mSemaphore = new System.Threading.Semaphore(0, 1);
        }

        /// <summary>
        /// 命令总入口
        /// </summary>
        /// <param name="cmd">发送命令的类型</param>
        /// <param name="dataAck">发送的数据</param>
        /// <param name="dataReq">返回的数据</param>
        /// <returns></returns>
        public Int32 Command(SERCOM_TYPE cmd, Byte[] dataAck, Byte[] dataReq)
        {
            Int32 errCode = 0;

            // set bytes want to read before writing
            mBytesWantToRead = ((dataReq != null) ? dataReq.Length : 0) + CONTAINER_LENGTH;

            //// flush serial port
            //mSpSlot.DiscardInBuffer();

            //////////////////////////////////////////////////////////////////////////发送
            if (dataReq != null)
            {
                CONTAINER_LENGTH += dataReq.Length;
            }
            Byte[] packet = new Byte[CONTAINER_LENGTH];
            string ACKData = PutCommData(cmd, packet, dataReq);//组建发送命令包
            if (ACKData != "0")
            {
                HDIC_Message.ShowWarnDialog(null, ACKData);
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
            if (!mSemaphore.WaitOne(12000))//超时
            {
                return -5; // timeout
            }

            // read
	       Byte[] dataR = new Byte[mBytesWantToRead];
            Array.Copy(mReadBuffer, 0, dataR, 0, dataR.Length);
            HDIC_Message.ShowInfoDialog(null, dataR.ToString());
            return errCode;
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="cmd">命令类型</param>
        /// <param name="Packet">返回包</param>
        /// <param name="sendData">要发送的数据</param>
        /// <returns></returns>
        private string PutCommData(SERCOM_TYPE cmd,Byte[] Packet, Byte[] sendData)
        {
           try
           {
               //Byte[] mstartcode =new Byte[sizeof(UInt16)];
               //uIntToByteArray(FRAMESTARTCODE, ref mstartcode);

               //Int32 index = 0;
               //mstartcode.CopyTo(Packet, index);

               Packet[(Int32)Index.startcode] =(Byte)FRAMESTARTCODE;
               Packet[(Int32)Index.cmdone] = (Byte)cmd;
               Packet[(Int32)Index.cmdtwo] = (Byte)SERCOM_TYPE.COM_NULL;
               Packet[(Int32)Index.length] = 0;//应该是先将长度转换成十六进制，在打包中。目前为配合下位机暂时是0
               if (sendData!=null)
               {
                   sendData.CopyTo(Packet, (Int32)Index.buffer);
               }
               Byte sum = 0;
               foreach (Byte cell in Packet)
               {
                   sum += cell;
               }
               Packet[Packet.Length - 1] = sum;
              
           }
           catch (System.Exception ex)
           {
               return ex.ToString();
           }
           return "0";
        }

        private string GetCommData(Byte[] GetPacket)
        {

            return "0";
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
            catch
            {

            }
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

}
