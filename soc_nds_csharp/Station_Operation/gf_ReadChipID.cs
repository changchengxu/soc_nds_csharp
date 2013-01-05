using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.Command;
using HDICSoft.Message;
using soc_protocol;

namespace soc_nds_csharp.Station_Operation
{
    public partial class gf_ReadChipID : Form
    {
        System.IO.Ports.SerialPort mSpSlot;//串口对象定义
        UartProtocol Protocol;
        Int32 ChipID = 0;

        public gf_ReadChipID()
        {
            InitializeComponent();
            this.BackColor = HDIC_Command.setColor();

            this.richtxt_info.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        }

        private void btn_ReadChipID_Click(object sender, EventArgs e)
        {
            //打开串口
            //mSpSlot = new Uart();
            //mSpSlot.loadConfig();
            //mSpSlot.open();
            mSpSlot = Uart.slot;
            if (mSpSlot.IsOpen == false)
            {
                HDIC_Message.ShowWarnDialog(null, "串口打开失败，请检查串口.\r\n");
            }
            else
            {
                Protocol = new UartProtocol(mSpSlot);//初始化uart对象 

                Connect();
            }
        }

        private void Connect()
        {
            Int32 index = CommandSerial();
            if (index != 0)
            {
                richtxt_info.ForeColor = System.Drawing.Color.Red;
            }

            if (index == -1)
            {
                HDIC_Message.ShowWarnDialog(this, "该流水线号已经达到最大值，请重新选择流水线号");
            }
            else if (index == -2)
            {
                richtxt_info.Text = "连接失败，请重新连接!";
                HDIC_Message.ShowWarnDialog(this, "向机顶盒发送命令或者接收命令出错");
            }
            else if (index == -3)
            {
                richtxt_info.Text = "连接失败，请重新连接!";
                HDIC_Message.ShowWarnDialog(this, "与机顶盒连接失败");
            }
            else if (index == -4)
            {
                richtxt_info.Text = "向机顶盒获取信息失败!";
                HDIC_Message.ShowWarnDialog(this, "向机顶盒获取信息失败");
            }
            else if (index == -5)
            {
                richtxt_info.Text = "向机顶盒获取ChipID失败!";
                HDIC_Message.ShowWarnDialog(this, "向机顶盒获取ChipID失败，请尝试重新获取!");
            }
            btn_ReadChipID.Enabled = true;
        }

        private int CommandSerial()
        {
            ////获取所有的串口    
            //String[] ports = System.IO.Ports.SerialPort.GetPortNames();

            richtxt_info.Text = "";
            richtxt_info.ForeColor = System.Drawing.Color.ForestGreen;
            btn_ReadChipID.Enabled = false;

            this.richtxt_info.Text = "正在尝试连接,请稍后... ...";   // richtxt_Connect

            Int32 index = 0;
            //Byte[] cmdlineACK = new Byte[Protocol.CONTAINER_LENGTH - 1];//只获取命令行前四个byte即可（主要用于判断当前什么命令）
            Byte[] cmdlineACK = { };
            index = Protocol.Command(SERCOM_TYPE.COM_CONNECT,SERCOM_TYPE.COM_HANDINFO, 0, 0, null, ref cmdlineACK);//调用类 ，发送命令
            if (index != 0)
            {
                return -2;
            }
            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_HANDINFO)
            {
                index = Protocol.Command(SERCOM_TYPE.COM_OK,SERCOM_TYPE.COM_START, 0, 0, null, ref cmdlineACK);//调用类 ，尝试连接
                if (index != 0)
                {
                    return -3;
                }
                richtxt_info.Text = "连接成功，请勿断电!";
            }
            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_START)
            {
                index = Protocol.Command(SERCOM_TYPE.COM_CHIPID,SERCOM_TYPE.COM_CHIPID, 0, 4, null, ref cmdlineACK);//调用类 ，获取所有的信息
                if (index != 0)
                {
                    return -4;
                }
            }
            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_CHIPID)
            {
                Int32 a = (Int32)cmdlineACK[(Int32)Index.buffer + 3];
                Int32 b = (Int32)(cmdlineACK[(Int32)Index.buffer + 2] << 8);
                Int32 c = (Int32)(cmdlineACK[(Int32)Index.buffer + 1] << 16);
                Int32 d = (Int32)(cmdlineACK[(Int32)Index.buffer] << 24);
                ChipID = a + b + c + d;

                richtxt_info.Text = String.Format("{0:X08}", ChipID).ToString(); 
            }
            return 0;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            mSpSlot.Close();
            this.Close();
        }

    }
}
