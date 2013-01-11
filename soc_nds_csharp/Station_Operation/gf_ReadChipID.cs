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

        private void gf_ReadChipID_Load(object sender, EventArgs e)
        {
            mSpSlot = Uart.slot;

            btn_ReadChipID.Focus();

            timer1.Interval = 1000;
            timer1.Enabled = false;

            btn_ReadChipID.Enabled = true;
        }

        private void btn_ReadChipID_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void btn_ReadChipID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Keys.KeyCode == Keys.Enter || Keys.KeyCode == Keys.Back)
            {
                Connect();
            }
        }
        private void Connect()
        {

            try
            {
                mSpSlot.Open();
            }
            catch
            {
                if (mSpSlot.IsOpen == false)
                {
                    HDIC_Message.ShowWarnDialog(null, "串口打开失败，请检查串口.\r\n");
                    btn_ReadChipID.Focus();
                    btn_ReadChipID.Enabled = true;

                    return;
                }
            }

            Int32 index = CommandSerial();

            if (index == -1)
            {
                HDIC_Message.ShowWarnDialog(this, "向机顶盒连接失败");
            }

            else if (index == -11)
            {
                HDIC_Message.ShowWarnDialog(this, "向机顶盒获取ChipID信息失败");
            }
            else if (index == -12)
            {
                HDIC_Message.ShowWarnDialog(this, "机顶盒获取的ChipID不正确");
            }
            else if (index == -100)
            {
                HDIC_Message.ShowWarnDialog(this, "发送的命令包创建失败");
            }
            else if (index == -110)
            {
                HDIC_Message.ShowWarnDialog(this, "发送命令包失败");
            }
            else if (index == -120)
            {
                HDIC_Message.ShowWarnDialog(this, "接收机顶盒信息超时");
            }
            btn_ReadChipID.Enabled = true;
            btn_ReadChipID.Focus();
        }

            private Int32 CommandSerial()
            {
                btn_ReadChipID.Enabled = false;

                ChipID = 0;
                btn_ReadChipID.Focus();

                Int32 index = 0;
                Byte[] cmdlineACK = { };//获取收到的命令（主要用于判断当前什么命令）

                index = Protocol.Command(SERCOM_TYPE.COM_NULL, SERCOM_TYPE.COM_NULL, null, ref cmdlineACK);//调用类 ，发送命令
                if (index != 0)
                {
                    if (index == -120)
                    {

                        timer1.Enabled = true;
                        return 0;
                    }
                    else
                    {
                        return index;
                    }
                }
                timer1.Enabled = false;

                if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_ASKHAND || cmdlineACK[(Int32)Index.cmdtwo] != (Byte)SERCOM_TYPE.COM_RETURN)
                {
                    return -1;
                }
                index = Protocol.Command(SERCOM_TYPE.COM_CONNECT, SERCOM_TYPE.COM_HANDINFO, null, ref cmdlineACK);//调用类 ，发送命令
                if (index != 0)
                {
                    return index;
                }
                if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_HANDINFO)
                {
                    return -1;
                }
                index = Protocol.Command(SERCOM_TYPE.COM_OK, SERCOM_TYPE.COM_START, null, ref cmdlineACK);//调用类 ，尝试连接
                if (index != 0)
                {
                    return index;
                }

                if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_START)
                {
                    return -1;
                }

                ///////////////////从下位机获取ChipID信息
                index = Protocol.Command(SERCOM_TYPE.COM_CHIPID, SERCOM_TYPE.COM_CHIPID, null, ref cmdlineACK);
                if (index != 0)
                {
                    return index;
                }
                if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_CHIPID)
                {
                    return -11;
                }
                Int32 a = (Int32)cmdlineACK[(Int32)Index.buffer + 3];
                Int32 b = (Int32)(cmdlineACK[(Int32)Index.buffer + 2] << 8);
                Int32 c = (Int32)(cmdlineACK[(Int32)Index.buffer + 1] << 16);
                Int32 d = (Int32)(cmdlineACK[(Int32)Index.buffer] << 24);
                ChipID = a + b + c + d;

                richtxt_info.Text = String.Format("{0:X08}", ChipID).ToString(); 
            return 0;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            mSpSlot.Close();
            this.Close();
        }

        Int32 timeCount = 60;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeCount--;

            if (timeCount == 0)
            {
                timer1.Enabled = false;
                HDIC_Message.ShowWarnDialog(this, "接收机顶盒数据超时");
                btn_ReadChipID.Focus();
                btn_ReadChipID.Enabled = true;
            }
            else
            {
                Connect();
            }
        }
      
    }
}
