using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.DB;
using HDICSoft.Message;
using HDICSoft.Func;
using HDICSoft.Command;
using System.IO.Ports;
using System.Threading;
using soc_protocol;
using barcode_scanner; 

//Delspace(t_send.Text);   //去掉所有空格，整合数据,记住

namespace soc_nds_csharp.Station_Operation
{

    public partial class gf_Serializer : Form
    {
        // Create a new Mutex. The creating thread does not own the
        // Mutex.
        private Mutex mutex = new Mutex();//互斥体

        BarCodeHook BarCode = new BarCodeHook();  //定义扫描仪对象

        System.IO.Ports.SerialPort mSpSlot ;//串口对象定义
        UartProtocol Protocol;

        long STBOpIndex;//当前流水号（工位一选择工位线时得到的）
        long ProductIndex;//根据当前流水号拼凑生产流水号
        Int32 ReceiveLength = 0;//接收串口数据包中数据的长度

        UInt32 ChipID = 0;
        Int32 STBType = 0;//机顶盒类型 目前下位机暂定0是村村通
        Int32 MFID, MDID, HDID;

        System.Threading.Semaphore mSemaphore;//用于为扫描枪设置超时时间

        gf_SelectPipeLine mObj;//目的和主窗体练习，打开则主窗体不可点击，关闭主窗体恢复

        public gf_Serializer(gf_SelectPipeLine obj)
        {
            InitializeComponent();
            tableLayoutPanel1.BackColor = HDIC_Command.setColor();
            this.BackColor = HDIC_Command.setColor();

            mObj = obj;
            //Int32[] aa = new Int32[11];
            //aa[0]=4;aa[1]=3;aa[2]=1;aa[3]=1;aa[4]=7;aa[5]=4;aa[6]=6;aa[7]=5;aa[8]=6;aa[9]=1;
            //CalculateLuhnAlgorithm(aa, 11);
            
            this.richtxt_Connect.Font = new System.Drawing.Font("SimSun", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            this.richtxt_info.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
           
            this.txt_SerialID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            this.txt_CAID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
           
            this.txt_ChipID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            this.txt_STBID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
           
            this.txt_SmartCardID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            this.richtxt_ManufacturerID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            this.richtxt_ModelID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
           
            this.richtxt_HardwareID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        }

        private void gf_Serializer_Load(object sender, EventArgs e)
        {
            mSpSlot = Uart.slot;

            BarCode.BarCodeEvent += new BarCodeHook.BarCodeDelegate(BarCode_BarCodeEvent);

            //Protocol = new UartProtocol(mSpSlot.slot);//初始化uart对象
            Protocol = new UartProtocol(mSpSlot);//初始化uart对象 

            richtxt_Connect.Enabled = false;
            richtxt_info.Enabled = false;
            txt_SerialID.Enabled = false;
            txt_CAID.Enabled = false;
            txt_ChipID.Enabled = false;
            txt_STBID.Enabled = false;
            richtxt_ManufacturerID.Enabled = false; richtxt_ModelID.Enabled = false; richtxt_HardwareID.Enabled = false;

            #region 失去焦点
            richtxt_Connect.TabStop = false;
            richtxt_info.TabStop = false;
            txt_SerialID.TabStop = false;
            txt_CAID.TabStop = false;
            txt_STBID.TabStop = false;
            txt_ChipID.TabStop = false;
            richtxt_ManufacturerID.TabStop = false;
            richtxt_ModelID.TabStop = false;
            richtxt_HardwareID.TabStop = false;
            #endregion

            TSCVariableInit();

            initControl();
            btn_begin.Enabled = true;

            mSemaphore = new System.Threading.Semaphore(0, 1);

            timer1.Interval = 1000;
            timer1.Enabled = false;

        }

        private void initControl()
        {
            ChipID = 0;
            STBType = 0;//机顶盒类型
            MFID = 0; MDID = 0; HDID = 0;

            txt_STBID.Text = "";
            txt_CAID.Text = "";
            txt_ChipID.Text = "";
            txt_SmartCardID.Text = "";
            richtxt_ManufacturerID.Text = "";
            richtxt_ModelID.Text = "";
            richtxt_HardwareID.Text = "";
            txt_SerialID.Text = "";

            richtxt_Connect.Text = "";
            richtxt_info.Text = "";

            this.richtxt_Connect.ForeColor = System.Drawing.Color.ForestGreen;
            this.richtxt_info.ForeColor = System.Drawing.Color.Black;
            this.txt_SerialID.ForeColor = System.Drawing.Color.Black;
            this.txt_CAID.ForeColor = System.Drawing.Color.Black;
            this.txt_ChipID.ForeColor = System.Drawing.Color.Black;
            this.txt_STBID.ForeColor = System.Drawing.Color.Black;
            this.txt_SmartCardID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_ManufacturerID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_ModelID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_HardwareID.ForeColor = System.Drawing.Color.Black;

            btn_begin.Focus();
        }

        #region 扫描仪
        /// <summary>
        /// 扫描仪
        /// </summary>
        private delegate void ShowInfoDelegate(BarCodeHook.BarCodes barCode);
        void BarCode_BarCodeEvent(BarCodeHook.BarCodes barCode)
        {
            ShowInfo(barCode);
        }

      static  BarCodeHook.BarCodes mBarCode;//用于判断当前的回车是键盘还是扫描仪按下的。
        private void ShowInfo(BarCodeHook.BarCodes barCode)
        {
            mBarCode = barCode;
            if (this.InvokeRequired)
            {
                //this.BeginInvoke(new ShowInfoDelegate(ShowInfo), new object[] { barCode });
                this.BeginInvoke(new ShowInfoDelegate(ShowInfo), new object[] { mBarCode });
            }
            else
            {
                //if (barCode.IsValid == true && barCode.BarCode.Trim().Length == 12)
                if (mBarCode.IsValid == true && mBarCode.BarCode.Trim().Length == 12)
                {
                    //txt_SmartCardID.Text = barCode.BarCode;
                    txt_SmartCardID.Text = mBarCode.BarCode;

                }
            }
            btn_begin.Focus();
        }
         #endregion

        private void btn_begin_KeyDown(object sender, KeyEventArgs e)
        {
            // Wait until it is safe to enter.
            mutex.WaitOne();

            if (Keys.KeyCode == Keys.Enter || Keys.KeyCode==Keys.Back)//回车或者空格
            {
                Thread thread = new Thread(new ThreadStart(DoWork));
                thread.Start();
            }

            // Release the Mutex.
            mutex.ReleaseMutex();
        }

        private void btn_begin_Click(object sender, EventArgs e)
        {
            // Wait until it is safe to enter.
            mutex.WaitOne();

            Thread thread = new Thread(new ThreadStart(DoWork));
            thread.Start();
          
            // Release the Mutex.
            mutex.ReleaseMutex();

        }

        public void DoWork()
        {
            MyInvoke mi = new MyInvoke(Connect);
            this.BeginInvoke(mi);
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
                    btn_begin.Focus();
                    btn_begin.Enabled = true;

                    return;
                }
            }

            Int32 index = CommandSerial();
            if (index != 0)
            {
                richtxt_Connect.ForeColor = System.Drawing.Color.Red;
                btn_begin.Enabled = true;
            }
            if (index == -1)
            {
                richtxt_Connect.Text = "当前流水线已经超过了最大值，请更换流水线!";
                HDIC_Message.ShowWarnDialog(this, "该流水线号已经达到最大值，请重新选择流水线号");
            }
            else if (index == -2)
            {
                richtxt_Connect.Text = "流水线号不能为空!";
                HDIC_Message.ShowWarnDialog(this, "流水线号不能为空");
            }

            else if (index == -10)
            {
                richtxt_info.Text += "连接失败，请重新连接!\r\n";
                richtxt_Connect.Text = "连接失败，请重新连接!";
                HDIC_Message.ShowWarnDialog(this, "与机顶盒连接失败");
            }
            else if (index == -20)
            {
                richtxt_info.Text += "从机顶盒获取机顶盒类型失败!\r\n";
                richtxt_Connect.Text = "从机顶盒机顶盒获取机顶盒类型失败!";
                HDIC_Message.ShowWarnDialog(this, "从机顶盒机顶盒获取机顶盒类型失败");
            }
            else if (index == -21)
            {
                richtxt_info.Text += "从机顶盒获取ChipID失败!\r\n";
                richtxt_Connect.Text = "从机顶盒获取ChipID失败!";
                HDIC_Message.ShowWarnDialog(this, "从机顶盒获取ChipID失败");
            }
            else if (index == -22)
            {
                richtxt_info.Text += "从机顶盒获取的ChipID不正确!\r\n";
                richtxt_Connect.Text = "从机顶盒获取的ChipID不正确!";
                HDIC_Message.ShowWarnDialog(this, "从机顶盒获取ChipID失败，请尝试重新获取!");
            }
            else if (index == -23)
            {
                richtxt_Connect.Text = "Chip id 重复，请检查是否重复序列化!";
                richtxt_info.Text += "Chip id 重复\r\n";
                HDIC_Message.ShowWarnDialog(this, "数据库中已经存在该ChipId，请检查是否重复序列化!");
            }
            else if (index == -30)
            {
                richtxt_Connect.Text = "从机顶盒获取制造商ID失败!";
                richtxt_info.Text += "从机顶盒获取制造商ID失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "从机顶盒获取制造商ID失败!");
            }
            else if (index == -40)
            {
                richtxt_Connect.Text = "从机顶盒获取机顶盒型号失败!";
                richtxt_info.Text += "从机顶盒获取机顶盒型号失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "从机顶盒获取机顶盒型号失败!");
            }
            else if (index == -50)
            {
                richtxt_Connect.Text = "从机顶盒获取硬件ID失败!";
                richtxt_info.Text += "从机顶盒获取硬件ID失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "从机顶盒获取硬件ID失败!");
            }
            else if (index == -60)
            {
                richtxt_Connect.Text = "向机顶盒发送STBID失败!";
                richtxt_info.Text += "向机顶盒发送STBID失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "向机顶盒发送STBID失败!");
            }
            else if (index == -24)
            {
                richtxt_Connect.Text = "搜索序列化数据失败，请检查服务器或网络!";
                richtxt_info.Text += "搜索序列化数据失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "搜索序列化数据失败，请检查服务器或网络!");
            }
            else if (index == -25)
            {
                HDIC_Message.ShowWarnDialog(this, "字符串转换字符数组失败");
            }
            else if (index == -70)
            {
                richtxt_Connect.Text = "写序列化数据失败!";
                richtxt_info.Text += "写序列化数据失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "写序列化数据失败");
            }
            else if (index == -71)
            {

                richtxt_Connect.Text = "Flash写保护失败";
                richtxt_info.Text += "Flash写保护失败\r\n";
            }
            else if (index == -80)
            {

                richtxt_Connect.Text = "超时! 没有在规定的时间内扫描智能卡号";
                richtxt_info.Text += "超时! 没有在规定的时间内扫描智能卡号\r\n";
            }
            else if (index == -90)
            {
                richtxt_Connect.Text = "上传序列号数据到数据库失败";
                richtxt_info.Text += "上传序列号数据到数据库失败!\r\n";
            }
            else if (index == -100)
            {
                richtxt_Connect.Text = "发送的命令包创建失败";
                richtxt_info.Text += "发送的命令包创建失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "发送的命令包创建失败");
            }
            else if (index == -110)
            {
                richtxt_Connect.Text = "发送命令包失败";
                richtxt_info.Text += "发送命令包失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "发送命令包失败");
            }
            else if (index == -120)
            {
                richtxt_Connect.Text = "接收机顶盒信息超时!";
                richtxt_info.Text += "接收机顶盒信息超时!\r\n";
                HDIC_Message.ShowWarnDialog(this, "接收机顶盒信息超时");
            }
            btn_begin.Focus();
        }

        Int32 timeCount = 60;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeCount--;

            if (timeCount == 0)
            {
                timer1.Enabled = false;
                richtxt_Connect.ForeColor = System.Drawing.Color.Red;
                richtxt_info.Text += "接收机顶盒数据超时!\r\n";
                richtxt_Connect.Text = "接收机顶盒数据超时!";
                HDIC_Message.ShowWarnDialog(this, "接收机顶盒数据超时");
                btn_begin.Enabled = true;
                timeCount = 60;
                btn_begin.Focus();
            }
            else
            {
                Connect();
            }
        }


        private int CommandSerial()
        {
            ////获取所有的串口    
            //String[] ports = System.IO.Ports.SerialPort.GetPortNames();

            initControl();
            btn_begin.Enabled = false;

            this.richtxt_Connect.Text = "正在尝试连接,请稍后... ...";   // richtxt_Connect

            STBOpIndex = Convert.ToInt32(HDIC_DB.sqlQuery("select STBOpIndex from STBOp where STBOpLineNum='" + HDIC_Command.STBLineNum + "' and STBOpFlag=1").Trim());
            Int32 STBOpMaxIndex = Convert.ToInt32(HDIC_DB.sqlQuery("select STBOpIndex from STBOp where STBOpLineNum='" + HDIC_Command.STBLineNum + "' and STBOpFlag=3").Trim());

            ProductIndex = Int32.Parse(HDIC_Command.STBLineNum.ToString() + STBOpIndex.ToString());
            this.txt_SerialID.Text = String.Format("{0:d07}", ProductIndex).ToString();

            if (STBOpIndex > STBOpMaxIndex)   //判断当前流水线是否是最大值
            {
                return -1;
            }

            if (txt_SerialID.Text.Trim() == "")
            {
                return -2;
            }

            //////////////////////////////////////////////////////////////////////////

            Int32 index = 0;
            Byte[] cmdlineACK = { };//获取收到的命令（主要用于判断当前什么命令）
            index = Protocol.Command(SERCOM_TYPE.COM_NULL, null, ReceiveLength, ref cmdlineACK);//调用类 ，发送命令
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
                return -10;
            }
            index = Protocol.Command(SERCOM_TYPE.COM_CONNECT, null, ReceiveLength, ref cmdlineACK);//调用类 ，发送命令
            if (index != 0)
            {
                return index;
            }
            if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_HANDINFO)
            {
                return -10;
            }
            index = Protocol.Command(SERCOM_TYPE.COM_OK, null, ReceiveLength, ref cmdlineACK);//调用类 ，尝试连接
            if (index != 0)
            {
                return index;
            }
            if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_START)
            {
                return -10;
            }
            richtxt_info.Text += "连接成功，请勿断电!\r\n";
            richtxt_Connect.Text = "连接成功，请勿断电!";
            index = Protocol.Command(SERCOM_TYPE.COM_STBTYPE, null,ReceiveLength+1, ref cmdlineACK);
            if (index != 0)
            {
                return index;
            }
            if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_STBTYPE)
            {
                return -20;
            }
            /////////////////从下位机获取机顶盒类型
            STBType = (Int32)cmdlineACK[(Int32)Index.buffer];
            //////////////////从下位机获取ChipID
            index = Protocol.Command(SERCOM_TYPE.COM_CHIPID, null,ReceiveLength+4, ref cmdlineACK);
            if (index != 0)
            {
                return index;
            }
            if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_CHIPID)
            {
                return -21;
            }
            Int32 a = (Int32)cmdlineACK[(Int32)Index.buffer + 3];
            Int32 b = (Int32)(cmdlineACK[(Int32)Index.buffer + 2] << 8);
            Int32 c = (Int32)(cmdlineACK[(Int32)Index.buffer + 1] << 16);
            Int32 d = (Int32)(cmdlineACK[(Int32)Index.buffer] << 24);


            ChipID = (UInt32)(a + b + c + d);
            //////////////////////////////////////////////////////////////////////////
            #region ChipID的处理
            txt_ChipID.Text = Convert.ToInt64(Convert.ToString(ChipID, 16).ToString(), 16).ToString();

            if (Convert.ToInt64(txt_ChipID.Text.Trim()) == 0 || Convert.ToInt64(txt_ChipID.Text.Trim()) < 0)
            {
                return -22;//向机顶盒获取ChipID失败
            }

            if (FindChipID())
            {
                return -23; // 重复序列化
            }

            #endregion

            else
            {
                #region 获取CAID
                //根据ChipID生成CAID
                Int32[] CAIDBuffer = new Int32[11];
                string CAID = string.Format("{0:d010}", ChipID ^ 0x80000000);
                // string CAID = Convert.ToInt32(Convert.ToString((ChipID ^ 0x80000000)), 16).ToString().PadLeft(8, '0');
                for (int i = 0; i < CAID.Length; i++)
                {
                    CAIDBuffer[i] = Convert.ToInt32(CAID[i].ToString());
                }
                CalculateLuhnAlgorithm(CAIDBuffer, 11);

                txt_CAID.Text = CAIDBuffer.ToString().Trim();
                string CAIDBufferStr = "";
                foreach (int temp in CAIDBuffer)
                {
                    CAIDBufferStr += temp.ToString();
                }
                txt_CAID.Text = CAIDBufferStr.Trim();
                #endregion
                ///////////////////////////////////////////从下位机获取制造商ID
                index = Protocol.Command(SERCOM_TYPE.COM_MFID, null,ReceiveLength+1,ref cmdlineACK);
                if (index != 0)
                {
                    return index;
                }
                if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_MFID)
                {
                    return -30;
                }
                MFID = (Int32)cmdlineACK[((Int32)Index.buffer)];
                richtxt_ManufacturerID.Text = String.Format("{0:X02}", MFID).ToString();

                //////////////////////////////////////////从下位机获取机顶盒型号
                index = Protocol.Command(SERCOM_TYPE.COM_MDID, null,ReceiveLength+1, ref cmdlineACK);
                if (index != 0)
                {
                    return index;
                }
                if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_MDID)
                {
                    return -40;
                }
                MDID = (Int32)cmdlineACK[((Int32)Index.buffer)];
                richtxt_ModelID.Text = String.Format("{0:X02}", MDID).ToString();
                //////////////////////////////////////////从下位机获取硬件ID
                index = Protocol.Command(SERCOM_TYPE.COM_HWID,null,ReceiveLength+1, ref cmdlineACK);
                if (index != 0)
                {
                    return index;
                }
                if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_HWID)
                {
                    return -50;
                }
                HDID = (Int32)cmdlineACK[((Int32)Index.buffer)];
                richtxt_HardwareID.Text = String.Format("{0:X02}", HDID).ToString();

                #region 生成STBID
                DateTime nowdate = DateTime.Now;
                //二.获取今天是一年当中的第几天
                int iDays = DateTime.Today.DayOfYear;

                //一.找到第一周的最后一天（先获取1月1日是星期几，从而得知第一周周末是几）
                int firstWeekend = 7 - Convert.ToInt32(DateTime.Parse(nowdate.Year + "-1-1").DayOfWeek);
                //三.（今天 减去 第一周周末）/7 等于 距第一周有多少周 再加上第一周的1 就是今天是今年的第几周了 // 刚好考虑了惟一的特殊情况就是，今天刚好在第一周内，那么距第一周就是0 再加上第一周的1 最后还是1
                int weeknums = Convert.ToInt32(Math.Ceiling((iDays - firstWeekend) / 7.0)) + 1;
                #endregion
                //制造商ID / 机顶盒类型号 / 生产年号 / 生产周号 / 生产流水号
                txt_STBID.Text = String.Format("{0:d03}{1:d02}{2:d02}{3:d02}{4:d07}", MFID, STBType, (nowdate.Year % 100), weeknums, ProductIndex);

                richtxt_Connect.Text = "从机顶盒获取信息成功!";
                richtxt_info.Text += "从机顶盒获取信息成功!\r\n";

                ///////////////////////////////////////////发送STBID到下位机
                Byte[] STBIDByte = Encoding.ASCII.GetBytes(txt_STBID.Text);
                index = Protocol.Command(SERCOM_TYPE.COM_STBIDPC,STBIDByte,ReceiveLength, ref cmdlineACK);
                if (index != 0)
                {
                    return index;
                }
                if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_STBIDPCOK)//如果下位机没有回应，上位机重发一次
                {
                    index = Protocol.Command(SERCOM_TYPE.COM_STBIDPC,STBIDByte,ReceiveLength, ref cmdlineACK);
                    if (index != 0)
                    {
                        return index;
                    }
                    if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_STBIDPCOK)
                    {
                        return -60;
                    }
                }
            }

            //从数据库搜索序列化数据
            string ChipInfo = SearchSerializeData();
            if (ChipInfo.Trim() == "0")
            {
                return -24;//搜索数据库中序列化数据（chipinfo）失败
            }
            richtxt_Connect.Text = "搜索序列化数据成功!";
            richtxt_info.Text += "搜索序列化数据成功!\r\n";

            Byte[] dataBuffer = new Byte[ChipInfo.Length / 2];
            if (!HDIC_Func.CStringToByte(ChipInfo, ref dataBuffer))
            {
                return -25;
            }

            int length = (ChipInfo.Length) / 2;
            ////////////////////////////////////////////向下位机发送写序列化数据
            index = Protocol.Command(SERCOM_TYPE.COM_FLASHWRITELICENSE, dataBuffer,ReceiveLength, ref cmdlineACK);//调用类 ，获取所有的信息
            if (index != 0)
            {
                return index;//向机顶盒发送序列化数据失败
            }
            if (cmdlineACK[(Int32)Index.cmdone] != (Byte)SERCOM_TYPE.COM_FLASHWRITELICENSEOK && cmdlineACK[(Int32)Index.cmdtwo] != (Byte)SERCOM_TYPE.COM_RETURN)
            {
                return -70;
            }

            richtxt_Connect.Text = "发送序列化数据成功!";
            richtxt_info.Text += "发送序列化数据成功!\r\n";

            ///////////////////////////////////////////////flash写保护 目前没有写
            #region flash写保护 目前没有写
            if (false) 
            {
                return -71;
            }
            //richtxt_Connect.Text = "Flash写保护成功";
            //richtxt_info.Text += "Flash写保护成功!\r\n";
            #endregion

            if (STBType == 0)//表示是村村通
            {
                richtxt_Connect.Text = "请扫描条形码的智能卡号……";
                richtxt_info.Text += "等待扫描仪扫描智能卡号... ...\r\n";
                BarCode.Start();  //开始监听扫描枪
                if (!mSemaphore.WaitOne(60000))//没有在规定的时候扫描智能卡号，暂定1分钟
                {
                    return -80;
                }
            }

            else if (STBType == 1)//表示是户户通
            {
                if (insertDB(txt_SmartCardID.Text.Trim()))
                {
                    printBar();

                    richtxt_info.Text += "上传序列号数据到数据库成功,请进行下一台!\r\n";
                    richtxt_Connect.Text = "上传序列号数据到数据库成功,请进行下一台";
                    btn_begin.Enabled = true;
                }
                else
                {
                    return -90; //上传序列化数据到数据库失败
                }
            }

            return 0;
        }
        #region 方法

        //判断 ChipID
        private bool FindChipID()
        {
            try
            {
                if (HDIC_DB.sqlQuery("select count(*) from STBData where ChipID='" + string.Format("{0:X08}", ChipID) + "'") != "0")
                {
                    return true;
                }
            }
            catch
            {
                HDIC_Message.ShowWarnDialog(this, "数据库打开失败,请检查服务器或者网络");
            }
            return false;
        }
        //C代码的Luhn算法示例
        private bool CalculateLuhnAlgorithm(int[] digits, int length)
        {
            int i;
            int sum = 0;
            bool alt = true;

            for (i = length - 2; i >= 0; i--)
            {
                if (alt)
                {
                    int temp = digits[i] * 2;
                    if (temp > 9)
                    {
                        temp -= 9;
                    }
                    sum += temp;
                }
                else
                {
                    sum += digits[i];
                }
                alt = !alt;
            }
            int modulo = sum % 10;

            if (modulo > 0)
            {
                //digits[length] = (char)((10 - modulo) + 0x30);
                //digits[length + 1] = (char)0;
                digits[length - 1] = ((10 - modulo));
            }
            else
            {
                //digits[length] = (char)0;
                digits[length - 1] = 0;
            }
            return true;
        }
       
        //查找指定的序列化数据
        private string SearchSerializeData()
        {
            string ChipIDInfo = "";
            try
            {
                ChipIDInfo = HDIC_DB.sqlQuery("select ChipInfo from ChipData where ChipID='" +txt_ChipID.Text.Trim()+ "'");
            }
            catch (System.Exception ex)
            {
                HDIC_Message.ShowWarnDialog(null, "查找指定的序列化数据时 出错，原因：\r\n" + ex.ToString());
                return "0";
            }
            return ChipIDInfo.Trim();
        }

        //将序列数据添加到数据库中
        private bool insertDB(string SmartCardID)
        {
            #region 判断智能卡号是否数据库中重复
            if (HDIC_DB.sqlQuery("select count(*) from STBData where SmartCardID='" + SmartCardID.Trim() + "'") != "0")
            {
                richtxt_info.Text += "数据库中存在该智能卡号，请检查是否重复!\r\n";
                richtxt_Connect.ForeColor = System.Drawing.Color.Red;
                richtxt_Connect.Text = "数据库中存在该智能卡号，请检查是否重复";
                HDIC_Message.ShowErrorDialog(this, "数据库中存在该智能卡号，请检查是否重复");

                return false;
            }
            #endregion

            ++STBOpIndex;
            string sqlstr="";
            //插入新的序列号数据到数据库
            try
            {
                if (SmartCardID.Trim().Length == 0)
                {
                    sqlstr = @"insert into STBData(ChipID,CAID,STBID,flag) values('" + string.Format("{0:X08}", ChipID) +
                        "','" + txt_CAID.Text.Trim() + "','" + txt_STBID.Text.Trim() +
                        "',0); update STBOp set STBOpIndex='" + STBOpIndex + "' where STBOpLineNum='" + HDIC_Command.STBLineNum +
                         "' and STBOpFlag=1; ";
                }
                else if (SmartCardID.Trim().Length > 0)
                {
                    sqlstr = @"insert into STBData(ChipID,CAID,STBID,SmartCardID,flag) values('" + string.Format("{0:X08}", ChipID) +
                       "','" + txt_CAID.Text.Trim() + "','" + txt_STBID.Text.Trim() +"','"+txt_SmartCardID.Text.Trim()+
                       "',0); update STBOp set STBOpIndex='" + STBOpIndex + "' where STBOpLineNum='" + HDIC_Command.STBLineNum +
                        "' and STBOpFlag=1; ";
                }
                if (HDIC_DB.ExcuteNonQuery(sqlstr, null) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
           
        }
      
        #region 为TSC条形码参数变量赋值 并将值赋给条形码打印机
        string tscOutPutPort = "";
        string tscWidth = "";
        string tscHeight = "";
        string tscPrintSpeed = "";
        string tscDensity = "";
        string tscSensor="";
        string tscVertical = "";
        string tscOffset = "";
        string tscX = "";
        string tscY = "";
        string tscFontType = "";
        string tscFontRotation = "";
        string tscEncodeType = "";
        string tscBarcodeHeight = "";
        string tscPrintCode = "";
        string tscCodeInterval = "";
        string tscFontMagnify1 = "";
        string tscFontMagnify2 = "";
        string tscBarCodeInterval = "";
        string tscRotate = "";
        string tscBarNarrow = "";
        string tscBarWide = "";
        string tscPrintLabelSetNum = "";
        string tscPrintLabelCopeNum = "";
        /// <summary>
        /// 为条形码参数变量赋值
        /// </summary>
        private void TSCVariableInit()
        {

            DataTable dt = HDIC_Func.XMLToDataSet(HDIC_Func.GetRunningPath() + @"config\BarcodeXml.xml").Tables["root"];
            if (dt.Rows.Count > 0)
            {
                tscOutPutPort = dt.Rows[0]["tscOutPutPort"].ToString().Trim();
                tscWidth = dt.Rows[0]["tscWidth"].ToString().Trim();
                tscHeight = dt.Rows[0]["tscHeight"].ToString().Trim();
                tscPrintSpeed = dt.Rows[0]["tscPrintSpeed"].ToString().Trim();
                tscDensity = dt.Rows[0]["tscDensity"].ToString().Trim();
                tscSensor = dt.Rows[0]["tscSensor"].ToString().Trim(); ;
                tscVertical = dt.Rows[0]["tscVertical"].ToString().Trim(); ;
                tscOffset = dt.Rows[0]["tscOffset"].ToString().Trim(); ;
                tscX = dt.Rows[0]["tscX"].ToString().Trim();
                tscY = dt.Rows[0]["tscY"].ToString().Trim();
                tscFontType=dt.Rows[0]["tscFontType"].ToString().Trim();;
                tscFontRotation=dt.Rows[0]["tscFontRotation"].ToString().Trim();;
                tscEncodeType = dt.Rows[0]["tscEncodeType"].ToString().Trim();
                tscBarcodeHeight=dt.Rows[0]["tscBarcodeHeight"].ToString().Trim();;
                tscPrintCode = dt.Rows[0]["tscPrintCode"].ToString().Trim();
                tscCodeInterval = dt.Rows[0]["tscCodeInterval"].ToString().Trim();
                tscFontMagnify1 = dt.Rows[0]["tscFontMagnify1"].ToString().Trim();
                tscFontMagnify2 = dt.Rows[0]["tscFontMagnify2"].ToString().Trim();
                tscBarCodeInterval = dt.Rows[0]["tscBarCodeInterval"].ToString().Trim();
                tscRotate = dt.Rows[0]["tscRotate"].ToString().Trim();
                tscBarNarrow = dt.Rows[0]["tscBar"].ToString().Trim();
                tscBarWide=dt.Rows[0]["tscBarWide"].ToString().Trim();;
                tscPrintLabelSetNum=dt.Rows[0]["tscPrintLabelSetNum"].ToString().Trim();;
                tscPrintLabelCopeNum=dt.Rows[0]["tscPrintLabelCopeNum"].ToString().Trim();;
            }
        }
        #endregion

        #region 博思得条形码参数变量赋值 并将值赋给条形码打印机
        string POSTEKOutPutPort="";
        string POSTEKPrintSpeed="";
        string POSTEKDensity="";
        string POSTEKHeight="";
        string POSTEKInterval="";
        string POSTEKWidth="";
        string POSTEKX="";
        string POSTEKY="";
        string POSTEKBarRotation="";
        string POSTEKEncodeType="";
        string POSTEKBarNarrow="";
        string POSTEKBarWide="";
        string POSTEKBarcodeHeight="";
        string POSTEKPrintCode="";
        string POSTEKBarCodeInterval="";
        string POSTEKCodeInterval="";
        string POSTEKFontHeight="";
        string POSTEKFontWidth="";
        string POSTEKFontType="";
        string POSTEKFontAlign="";
        string POSTEKFontWeight="";
        string POSTEKPrintLabelSetNum="";
        string POSTEKPrintLabelCopeNum="";

        private void POSTEKVariableInit()
        {
            DataTable dt = HDIC_Func.XMLToDataSet(HDIC_Func.GetRunningPath() + @"config\BarcodeXml.xml").Tables["root"];
            if (dt.Rows.Count > 0)
            {
                POSTEKOutPutPort = dt.Rows[0]["POSTEKOutPutPort"].ToString().Trim();
                POSTEKPrintSpeed = dt.Rows[0]["POSTEKPrintSpeed"].ToString().Trim();
                POSTEKDensity = dt.Rows[0]["POSTEKDensity"].ToString().Trim();
                POSTEKHeight = dt.Rows[0]["POSTEKHeight"].ToString().Trim();
                POSTEKInterval = dt.Rows[0]["POSTEKInterval"].ToString().Trim();
                POSTEKWidth = dt.Rows[0]["POSTEKWidth"].ToString().Trim();
                POSTEKX = dt.Rows[0]["POSTEKX"].ToString().Trim();
                POSTEKY = dt.Rows[0]["POSTEKY"].ToString().Trim();
                POSTEKBarRotation = dt.Rows[0]["POSTEKBarRotation"].ToString().Trim();
                POSTEKEncodeType = dt.Rows[0]["POSTEKEncodeType"].ToString().Trim();
                POSTEKBarNarrow = dt.Rows[0]["POSTEKBarNarrow"].ToString().Trim();
                POSTEKBarWide = dt.Rows[0]["POSTEKBarWide"].ToString().Trim();
                POSTEKBarcodeHeight = dt.Rows[0]["POSTEKBarcodeHeight"].ToString().Trim();
                POSTEKPrintCode = dt.Rows[0]["POSTEKPrintCode"].ToString().Trim();
                POSTEKBarCodeInterval = dt.Rows[0]["POSTEKBarCodeInterval"].ToString().Trim();
                POSTEKCodeInterval = dt.Rows[0]["POSTEKCodeInterval"].ToString().Trim();
                POSTEKFontHeight = dt.Rows[0]["POSTEKFontHeight"].ToString().Trim();
                POSTEKFontWidth = dt.Rows[0]["POSTEKFontWidth"].ToString().Trim();
                POSTEKFontType = dt.Rows[0]["POSTEKFontType"].ToString().Trim();
                POSTEKFontAlign = dt.Rows[0]["POSTEKFontAlign"].ToString().Trim();
                POSTEKFontWeight = dt.Rows[0]["POSTEKFontWeight"].ToString().Trim();
                POSTEKPrintLabelSetNum = dt.Rows[0]["POSTEKPrintLabelSetNum"].ToString().Trim();
                POSTEKPrintLabelCopeNum = dt.Rows[0]["POSTEKPrintLabelCopeNum"].ToString().Trim();
            }
        }
#endregion

        #endregion

        #region 打印标签
        private void printBar()
        {
            string CurrentPrinter = "";
              DataTable dt = HDIC_Func.XMLToDataSet(HDIC_Func.GetRunningPath() + @"config\BarcodeXml.xml").Tables["root"];
              if (dt.Rows.Count > 0)
              {
                  CurrentPrinter = dt.Rows[0]["CurrentPrinter"].ToString().Trim();
              }

            if (CurrentPrinter==dt.Rows[0]["CurrentPrinter"].ToString().Trim())
            {
            }
              HDIC_Func.TSCPrinter(tscOutPutPort, tscWidth, tscHeight, tscPrintSpeed, tscDensity, tscSensor, tscVertical, tscOffset, tscX, tscY, tscFontType, tscFontRotation, tscEncodeType, tscBarcodeHeight, tscPrintCode, tscCodeInterval, tscFontMagnify1, tscFontMagnify2, tscBarCodeInterval, txt_STBID.Text.Trim(), txt_CAID.Text.Trim(), txt_SmartCardID.Text.Trim(), tscRotate, tscBarNarrow, tscBarWide, 0, tscPrintLabelSetNum, tscPrintLabelCopeNum);
        }
        #endregion


        private void gf_Serializer_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (HDIC_Message.ShowQuestionDialog(this, "确定要关闭工位一？") == DialogResult.OK)
            //{
            mSpSlot.Close();
                mObj.eventFunc("open");
                e.Cancel = false;
            //}
            //else
            //{
            //    e.Cancel = true;
            //}
        }

        private void txt_SmartCardID_TextChanged(object sender, EventArgs e)
        {
            if (txt_SmartCardID.Text.Trim().Length == 12 && HDIC_Func.CheckObjectIsInteger(txt_SmartCardID.Text.Trim()))
            {
                BarCode.Stop();
                mSemaphore.Release();
                if (insertDB(txt_SmartCardID.Text.Trim()))
                {
                    printBar();

                    richtxt_info.Text += "上传序列号数据到数据库成功,请进行下一台!\r\n";
                    richtxt_Connect.Text = "上传序列号数据到数据库成功,请进行下一台";

                }
                else
                {
                    richtxt_info.Text += "上传序列号数据到数据库失败!\r\n";
                    richtxt_Connect.Text = "上传序列号数据到数据库失败";
                }
                btn_begin.Focus();
                btn_begin.Enabled = true;
            }
        }

        private void 工位一退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.gf_Serializer_FormClosing);
            mSpSlot.Close();
            mObj.eventFunc("open");
            this.Close();

        }
    }

}
