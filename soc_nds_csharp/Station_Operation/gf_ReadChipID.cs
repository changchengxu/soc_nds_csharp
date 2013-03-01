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
    public partial class gf_ReadChipID : Form
    {
        System.IO.Ports.SerialPort mSpSlot;//串口对象定义
        UartProtocol Protocol;
        Int32 ReceiveLength = 0;//接收串口数据包中数据的长度
        UInt32 ChipID = 0;

        public gf_ReadChipID()
        {
            InitializeComponent();
            this.BackColor = HDIC_Command.setColor();

            this.richtxt_info.Font = new System.Drawing.Font("SimSun", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richtxt_info.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void gf_ReadChipID_Load(object sender, EventArgs e)
        {
            mSpSlot = Uart.slot;
            Protocol = new UartProtocol(mSpSlot);//初始化uart对象 

            btn_ReadChipID.Focus();

            //timer1.Interval = 1000;
            //timer1.Enabled = false;

            btn_ReadChipID.Enabled = true;

            //Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btn_ReadChipID_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(Connect));
            thread.Start();
        }

        private void btn_ReadChipID_KeyPress(object sender, KeyPressEventArgs e)
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
                    if (btn_ReadChipID.IsHandleCreated)
                    this.BeginInvoke(new MethodInvoker(delegate()
                   {
                       HDIC_Message.ShowWarnDialog(null, "串口打开失败，请检查串口.\r\n");
                       btn_ReadChipID.Focus();
                       btn_ReadChipID.Enabled = true;
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
                SetInfoDelegateText("获取ChipID失败");

            }

            if (index == -1)
            {
                SetInfoDelegateText("向机顶盒连接失败");
            }

            else if (index == -11)
            {
                SetInfoDelegateText("向机顶盒获取ChipID信息失败");
            }
            else if (index == -12)
            {
                SetInfoDelegateText("机顶盒获取的ChipID不正确");
            }
            else if (index == -100)
            {
                SetInfoDelegateText("发送的命令包创建失败");
            }
            else if (index == -110)
            {
                SetInfoDelegateText("发送命令包失败");
            }
            else if (index == -120)
            {
                SetInfoDelegateText("接收机顶盒信息超时");
            }
            if (btn_ReadChipID.IsHandleCreated)
            this.BeginInvoke(new MethodInvoker(delegate()
                   {
                       btn_ReadChipID.Enabled = true;
                       btn_ReadChipID.Focus();
                   }));
            mSpSlot.Close();
        }

            private Int32 CommandSerial()
            {
                if (btn_ReadChipID.IsHandleCreated)
                this.BeginInvoke(new MethodInvoker(delegate()
            {
                btn_ReadChipID.Enabled = false;
            }));

                // richtxt_Connect
                //richtxt_info.Text = "正在建立连接,请稍后... ...";
                SetInfoDelegateText("正在建立连接,请稍后... ...");

                Int32 index = 0;
                Byte[] cmdlineACK = { };//获取收到的命令（主要用于判断当前什么命令）

                    index = Protocol.Command(SERCOM_TYPE.COM_NULL, null, ReceiveLength, ref cmdlineACK);//调用类 ，发送命令
               
                if (index != 0)
                {
                    //if (index == -120)
                    //{

                    //    timer1.Enabled = true;
                    //    return 0;
                    //}
                    //else
                    //{
                        return index;
                    //}
                }
                //timer1.Enabled = false;

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
                //richtxt_info.Text = "连接成功，请勿断电!";
                SetInfoDelegateText("连接成功，请勿断电!");
                ///////////////////从下位机获取ChipID信息
                System.Threading.Thread.Sleep(1);
                index = Protocol.Command(SERCOM_TYPE.COM_CHIPID, null, ReceiveLength + 4, ref cmdlineACK);
                if (index != 0)
                {
                    return index;
                }
                if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_CHIPID)
                {
                    return -11;
                }
                ChipID = 0;

                Int32 a = (Int32)cmdlineACK[(Int32)Index.buffer + 3];
                Int32 b = (Int32)(cmdlineACK[(Int32)Index.buffer + 2] << 8);
                Int32 c = (Int32)(cmdlineACK[(Int32)Index.buffer + 1] << 16);
                Int32 d = (Int32)(cmdlineACK[(Int32)Index.buffer] << 24);
                ChipID = (UInt32)(a + b + c + d);

                #region ChipID的处理
                string MChipID = Convert.ToInt64(Convert.ToString(ChipID, 16).ToString(), 16).ToString();
                if (Convert.ToInt64(MChipID) == 0 || Convert.ToInt64(MChipID) < 0)
                {
                    return -12;//向机顶盒获取ChipID失败
                }

                #endregion

                //richtxt_info.Text = String.Format("ChipID：{0:X08}", ChipID).ToString(); 
                //richtxt_info.Text ="ChipID："+MChipID;
                SetInfoDelegateText("ChipID：" + Convert.ToString(ChipID, 10) + "\r\n" + "ChipID：0x" + Convert.ToString(ChipID, 16));
            return 0;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //mSpSlot.Close();
            this.Close();
        }

        Int32 timeCount = 60;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //timeCount--;

            //if (timeCount == 0)
            //{
            //    timer1.Enabled = false;
            //    HDIC_Message.ShowWarnDialog(this, "接收机顶盒数据超时");
            //    richtxt_info.ForeColor = System.Drawing.Color.Red;
            //    richtxt_info.Text = "获取ChipID超时";
            //    btn_ReadChipID.Enabled = true;
            //    timeCount = 60;
            //    btn_ReadChipID.Focus();
            //}
            //else
            //{
            //    //System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
            //    //st.Start();

            //    Connect();

            //    //st.Stop();
            //    //HDIC_Message.ShowWarnDialog(null, "当前实例测量出总运行时间" + st.ElapsedMilliseconds + "毫秒");
            //}
        }
      
    }
}
