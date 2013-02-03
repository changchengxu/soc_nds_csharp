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
    public partial class gf_RemoteFlashProtect : Form
    {
        System.IO.Ports.SerialPort mSpSlot;//串口对象定义
        UartProtocol Protocol;
        Int32 ReceiveLength = 0;//接收串口数据包中数据的长度
        Int32 ChipID = 0;


        public gf_RemoteFlashProtect()
        {
            InitializeComponent();
            this.BackColor = HDIC_Command.setColor();

            this.richtxt_info.Font = new System.Drawing.Font("SimSun", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        }

        private void gf_RemoteFlashProtect_Load(object sender, EventArgs e)
        {
            mSpSlot = Uart.slot;
            Protocol = new UartProtocol(mSpSlot);//初始化uart对象 

            btn_RemoteFlash.Focus();
            richtxt_info.Enabled = false;

            timer1.Interval = 1000;
            timer1.Enabled = false;

            btn_RemoteFlash.Enabled = true;

        }

        private void btn_RemoteFlash_Click(object sender, EventArgs e)
        {
            Connect();

        }

        private void btn_RemoteFlash_KeyPress(object sender, KeyPressEventArgs e)
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
                    btn_RemoteFlash.Focus();
                    btn_RemoteFlash.Enabled = true;

                    return;
                }
            }
            this.richtxt_info.ForeColor = System.Drawing.Color.ForestGreen;

            Int32 index = CommandSerial();
            if (index < 0)
            {
                richtxt_info.ForeColor = System.Drawing.Color.Red;
                btn_RemoteFlash.Enabled = true;
            }

            if (index == -1)
            {
                HDIC_Message.ShowWarnDialog(this, "向机顶盒连接失败！");
            }
            else if (index == -11)
            {
                HDIC_Message.ShowWarnDialog(this, "解除Flash写保护失败！");
            }
            else if (index == -12)
            {
                HDIC_Message.ShowWarnDialog(this, "向机顶盒发送解除Flash写保护命令失败！");
            }
            else if (index == -100)
            {
                HDIC_Message.ShowWarnDialog(this, "发送的命令包创建失败！");
            }
            else if (index == -110)
            {
                HDIC_Message.ShowWarnDialog(this, "发送命令包失败！");
            }
            else if (index == -120)
            {
                HDIC_Message.ShowWarnDialog(this, "接收机顶盒信息超时！");
            }
            btn_RemoteFlash.Focus();
        }

        private Int32 CommandSerial()
        {
            btn_RemoteFlash.Enabled = false;


            // richtxt_Connect
            richtxt_info.Text = "正在建立连接,请稍后... ...";

            Int32 index = 0;
            Byte[] cmdlineACK = { };//获取收到的命令（主要用于判断当前什么命令）
            try
            {
                index = Protocol.Command(SERCOM_TYPE.COM_NULL, null, ReceiveLength, ref cmdlineACK);//调用类 ，发送命令
            }
            catch
            {

            }

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
            index = Protocol.Command(SERCOM_TYPE.COM_CONNECT, null, ReceiveLength, ref cmdlineACK);//调用类 ，发送命令
            if (index != 0)
            {
                return index;
            }
            if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_HANDINFO)
            {
                return -1;
            }
            index = Protocol.Command(SERCOM_TYPE.COM_OK, null, ReceiveLength, ref cmdlineACK);//调用类 ，尝试连接
            if (index != 0)
            {
                return index;
            }

            if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_START)
            {
                return -1;
            }
            richtxt_info.Text = "连接成功，请勿断电!";

            ///////////////////向下位机发送解除flash写保护
            index = Protocol.Command(SERCOM_TYPE.COM_REMOVEFLASHWP, null, ReceiveLength, ref cmdlineACK);
            if (index != 0)
            {
                return index;
            }
            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_ERROR)
            {
                return -11;
            }
            else if(cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_REMOVEFLASHWPOK)
            {
                richtxt_info.Text = "解除flash写保护成功！";
                btn_RemoteFlash.Enabled = true;

            }
            else
            {
                return -12;
            }
            return 0;
        }

        Int32 timeCount = 60;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeCount--;

            if (timeCount == 0)
            {
                timer1.Enabled = false;
                HDIC_Message.ShowWarnDialog(this, "接收机顶盒数据超时");
                btn_RemoteFlash.Enabled = true;
                timeCount = 60;
                btn_RemoteFlash.Focus();
            }
            else
            {
                //System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
                //st.Start();

                Connect();

                //st.Stop();
                //HDIC_Message.ShowWarnDialog(null, "当前实例测量出总运行时间" + st.ElapsedMilliseconds + "毫秒");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            mSpSlot.Close();
            this.Close();
        }

    }
}
