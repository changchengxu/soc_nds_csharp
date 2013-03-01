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
using System.Threading;

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

            //timer1.Interval = 1000;
            //timer1.Enabled = false;

            btn_RemoteFlash.Enabled = true;

            //Control.CheckForIllegalCrossThreadCalls = false;//这个方法不推荐，目前暂时关闭（目的是解除界面假死）

        }

        private void btn_RemoteFlash_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(Connect));
            thread.Start();
        }

        private void btn_RemoteFlash_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Keys.KeyCode == Keys.Enter || Keys.KeyCode == Keys.Back)
            {
                Thread thread = new Thread(new ThreadStart(Connect));
                thread.Start();
            }
        }
        #region 定义委托，目的多线程下UI及时更新
        delegate void InfoDelegate(string str);
        private void SetInfoDelegateText(string str)
        {
            if (richtxt_info.InvokeRequired)
            {
                Invoke(new InfoDelegate(SetInfoDelegateText), new string[] { str });
            }
            else
            {
                richtxt_info.Text = str;
            }
        }
        #endregion

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
                    if (btn_RemoteFlash.IsHandleCreated)
                    this.BeginInvoke(new MethodInvoker(delegate()
                  {
                      HDIC_Message.ShowWarnDialog(null, "串口打开失败，请检查串口.\r\n");
                      btn_RemoteFlash.Focus();
                      btn_RemoteFlash.Enabled = true;
                  }));
                    return;
                }
            }
            if (richtxt_info.IsHandleCreated)
            this.BeginInvoke(new MethodInvoker(delegate()
                  {
                      this.richtxt_info.ForeColor = System.Drawing.Color.ForestGreen;
                  }));

            Int32 index = CommandSerial();
            if (index < 0)
            {
                if (richtxt_info.IsHandleCreated)
                this.BeginInvoke(new MethodInvoker(delegate()
                 {
                     richtxt_info.ForeColor = System.Drawing.Color.Red;
                 }));
                SetInfoDelegateText("解除Flash写保护失败");
            }

            if (index == -1)
            {
                SetInfoDelegateText("向机顶盒连接失败！");
            }
            else if (index == -11)
            {
                SetInfoDelegateText("解除Flash写保护失败！");
            }
            else if (index == -12)
            {
                SetInfoDelegateText("向机顶盒发送解除Flash写保护命令失败！");
            }
            else if (index == -100)
            {
                SetInfoDelegateText("发送的命令包创建失败！");
            }
            else if (index == -110)
            {
                SetInfoDelegateText("发送命令包失败！");
            }
            else if (index == -120)
            {
                SetInfoDelegateText("接收机顶盒信息超时！");
            }
            if (btn_RemoteFlash.IsHandleCreated)
            this.BeginInvoke(new MethodInvoker(delegate()
                 {
                     btn_RemoteFlash.Enabled = true;
                     btn_RemoteFlash.Focus();
                 }));
            mSpSlot.Close();
        }

        private Int32 CommandSerial()
        {
            if (btn_RemoteFlash.IsHandleCreated)
            this.BeginInvoke(new MethodInvoker(delegate()
                 {
                     btn_RemoteFlash.Enabled = false;
                 }));
            // richtxt_Connect
             SetInfoDelegateText("正在建立连接,请稍后... ...");

            Int32 index = 0;
            Byte[] cmdlineACK = { };//获取收到的命令（主要用于判断当前什么命令）

                index = Protocol.Command(SERCOM_TYPE.COM_NULL, null, ReceiveLength, ref cmdlineACK);//调用类 ，发送命令
            if (index != 0)
            {
                    return index;
            }

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
             SetInfoDelegateText("连接成功，请勿断电!");

            ///////////////////向下位机发送解除flash写保护
            System.Threading.Thread.Sleep(4);
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
                SetInfoDelegateText("解除flash写保护成功！");
            }
            else
            {
                return -12;
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
