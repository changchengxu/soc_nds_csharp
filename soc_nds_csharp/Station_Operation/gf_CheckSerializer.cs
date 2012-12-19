using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.Message;
using HDICSoft.DB;
using HDICSoft.Func;
using HDICSoft.Command;
using soc_protocol;
using System.Runtime.InteropServices;
using barcode_scanner; 

namespace soc_nds_csharp.Station_Operation
{
    public partial class gf_CheckSerializer : Form
    {
        BarCodeHook BarCode = new BarCodeHook();  //定义扫描仪对象

        System.IO.Ports.SerialPort mSpSlot;//串口对象定义
        UartProtocol Protocol;
        Int32 ChipID = 0;

        public gf_CheckSerializer()
        {
            InitializeComponent();
            tableLayoutPanel1.BackColor = HDIC_Command.setColor();
            this.BackColor = HDIC_Command.setColor();

            this.richtxt_LincenseBoard.Font = new System.Drawing.Font("SimSun", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richtxt_LincenseAdo.Font = new System.Drawing.Font("SimSun", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_CAID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_STBID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_SmartCardID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richtxt_Tips.Font = new System.Drawing.Font("SimSun", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richtxt_connect.Font = new System.Drawing.Font("SimSun", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            //文本内容居中显示
            richtxt_LincenseAdo.SelectionAlignment = HorizontalAlignment.Center;
            richtxt_LincenseBoard.SelectionAlignment = HorizontalAlignment.Center;

            BarCode.BarCodeEvent += new BarCodeHook.BarCodeDelegate(BarCode_BarCodeEvent);
        }

        private void gf_CheckSerializer_Load(object sender, EventArgs e)
        {
            HDIC_Command.STBType = 1;//这个是测试 当前是村村通


            //打开串口
            //mSpSlot = new Uart();
            //mSpSlot.loadConfig();
            //mSpSlot.open();

            mSpSlot = Uart.slot;
            if (mSpSlot.IsOpen == false)
            {
                HDIC_Message.ShowWarnDialog(null, "串口打开失败，请检查串口.\r\n");
            }

            //Protocol = new UartProtocol(mSpSlot.slot);//初始化uart对象 
            Protocol = new UartProtocol(mSpSlot);//初始化uart对象 

            richtxt_connect.TabStop = false;
            richtxt_Tips.TabStop = false;
            richtxt_LincenseBoard.Enabled = false;
            richtxt_LincenseAdo.Enabled = false;
            richtxt_connect.Enabled = false;
            richtxt_Tips.Enabled = false;
            txt_CAID.Enabled = false;
            txt_STBID.Enabled = false;
            txt_SmartCardID.Enabled = false;
            btn_begin.Focus();

            #region 让文本框失去焦点
            txt_CAID.TabStop = false;
            txt_SmartCardID.TabStop = false;
            txt_STBID.TabStop = false;
            richtxt_LincenseAdo.TabStop = false;
            richtxt_LincenseBoard.TabStop = false;
           #endregion
        }
        ///////////////////////////////////////////////////////////////////////////////扫描仪
        /// <summary>
        /// 扫描仪
        /// </summary>
         private delegate void ShowInfoDelegate(BarCodeHook.BarCodes barCode);  
        void BarCode_BarCodeEvent(BarCodeHook.BarCodes barCode)  
        {  
            ShowInfo(barCode);  
        }

        bool CheckCAID = false;
        bool CheckSTBID = false;
        bool CheckSmartCardID = false;

         private void ShowInfo(BarCodeHook.BarCodes barCode)  
        {  
            if (this.InvokeRequired)  
            {  
                this.BeginInvoke(new ShowInfoDelegate(ShowInfo), new object[] { barCode });  
            }  
            else  
            {  
                //textBox1.Text = barCode.KeyName;  
                //textBox2.Text = barCode.VirtKey.ToString();  
                //textBox3.Text = barCode.ScanCode.ToString();  
                //textBox4.Text = barCode.AscII.ToString();  
                //textBox5.Text = barCode.Chr.ToString();  
                //textBox6.Text = barCode.IsValid ? barCode.BarCode : "";  
                
                
                if (barCode.IsValid == true && barCode.BarCode.Trim().Length == 16) //获取扫描枪中的数据赋值到文本框
                {
                    txt_STBID.Text = barCode.IsValid ? barCode.BarCode : "";
                }
                else if (barCode.IsValid == true && barCode.BarCode.Trim().Length == 11)
                {
                    txt_CAID.Text = barCode.IsValid ? barCode.BarCode : "";
                }
                else if (barCode.IsValid == true && barCode.BarCode.Trim().Length == 12)
                {
                    txt_SmartCardID.Text = barCode.IsValid ? barCode.BarCode : "";
                }
              
            }  
        }  
        ////////////////////////////////////////////////////////////////////////////////////

        private void initControl()
        {
            ChipID = 0;

            CheckCAID = false;
            CheckSTBID = false;
            CheckSmartCardID = false;

            richtxt_LincenseBoard.Text = "";
            richtxt_LincenseAdo.Text="";
            txt_CAID.Text="";
            txt_STBID.Text="";
            txt_SmartCardID.Text = "";
            richtxt_Tips.Text="";

            this.richtxt_LincenseBoard.ForeColor = System.Drawing.Color.Black;
            this.richtxt_LincenseAdo.ForeColor = System.Drawing.Color.Black;
            this.txt_CAID.ForeColor = System.Drawing.Color.Black;
            this.txt_SmartCardID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_Tips.ForeColor = System.Drawing.Color.Black;
            this.richtxt_connect.ForeColor = System.Drawing.Color.ForestGreen;
            btn_begin.Focus();
        }

        private void btn_begin_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void btn_begin_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.KeyCode == Keys.Enter || Keys.KeyCode==Keys.Back)
            {
                Connect();
            }
        }

        private void Connect()
        {
          
            Int32 index = CommandSerial();

            if (index != 0)
            {
                richtxt_connect.ForeColor = System.Drawing.Color.Red;
                btn_begin.Enabled = true;
            }

            if (index == -1)
            {
                richtxt_connect.Text = "连接失败，请重新连接!";
                richtxt_Tips.Text += "连接失败，请重新连接!\r\n";
                HDIC_Message.ShowWarnDialog(this, "向机顶盒发送命令或者接收命令出错");
            }
            else if (index == -2)
            {
                richtxt_connect.Text = "连接失败，请重新连接!";
                richtxt_Tips.Text += "连接失败，请重新连接!\r\n";
                HDIC_Message.ShowWarnDialog(this, "向机顶盒连接失败");
            }

            else if (index == -3)
            {
                richtxt_connect.Text = "获取ChipID信息失败!";
                richtxt_Tips.Text += "获取ChipID信息失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "向机顶盒获取ChipID信息失败");
            }
            else if (index == -4)
            {
                richtxt_connect.Text = "向机顶盒获取的ChipID不正确";
                richtxt_Tips.Text += "向机顶盒获取的ChipID不正确\r\n";
            }
            else if (index == -10)
            {
                richtxt_connect.Text = "数据库中没有找到该ChipID!";
                richtxt_Tips.Text += "数据库中没有找到该ChipID!\r\n";
                HDIC_Message.ShowWarnDialog(this, "数据库中没有找到该ChipID!");
            }
            else if (index == -11)
            {
                richtxt_connect.Text = "该ChipID已经校验过，无需再次校验!";
                richtxt_Tips.Text += "该ChipID已经校验过，无需再次校验!\r\n";
            }
            else if (index == -20)
            {
                richtxt_connect.Text = "获取机顶盒的序列化数据失败!";
                richtxt_Tips.Text += "获取机顶盒的序列化数据失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "向机顶盒获取序列化数据失败");
            }
            else if (index == -21)
            {
                richtxt_connect.Text = "搜索数据库中的序列化数据失败!";
                richtxt_Tips.Text += "搜索数据库中的序列化数据失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "搜索数据库中的序列化数据失败，请检查当前ChipID或服务器网络!");
            }
            else if (index == -22)
            {
                richtxt_connect.Text = "序列化数据转换字符数组时，发生错误!";
                richtxt_Tips.Text += "序列化数据转换字符数组时，发生错误!\r\n";
            }
            else if (index == -23)
            {
                richtxt_connect.Text = "序列化校验失败！请检查或返回第一个工位!";
                richtxt_Tips.Text += "序列化校验失败！请检查或返回第一个工位!\r\n";
                HDIC_Message.ShowWarnDialog(this, "序列化校验失败！请检查或返回第一个工位!");
            }
           
            btn_begin.Focus();
        }


        private Int32 CommandSerial()
        {
            btn_begin.Enabled = false;

            initControl();

            // richtxt_Connect
            richtxt_connect.Text = "正在建立连接,请稍后... ...";
            
            //连接STB
            Int32 index = 0;
            Byte[] cmdlineACK = { };//只获取命令行前四个byte即可（主要用于判断当前什么命令）
            index = Protocol.Command(SERCOM_TYPE.COM_CONNECT, 0, 0, null, ref cmdlineACK);//调用类 ，发送命令
            if (index != 0)
            {
                return -1;
            }
            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_HANDINFO)
            {
                index = Protocol.Command(SERCOM_TYPE.COM_OK, 0, 0, null, ref cmdlineACK);//调用类 ，尝试连接
                if (index != 0)
                {
                    return -2;
                }
                richtxt_connect.Text = "连接成功，请勿断电!";
                richtxt_Tips.Text += "连接成功，请勿断电!\r\n";
            }


            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_START)
            {
                index = Protocol.Command(SERCOM_TYPE.COM_CHIPID, 0, 4, null, ref cmdlineACK);//调用类 ，获取ChipID信息
                if (index != 0)
                {
                    return -3;
                }
            }

            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_CHIPID)
            {
                #region 验证ChIPID
                ChipID = 0;

                Int32 a = (Int32)cmdlineACK[(Int32)Index.buffer + 3];
                Int32 b = (Int32)(cmdlineACK[(Int32)Index.buffer + 2] << 8);
                Int32 c = (Int32)(cmdlineACK[(Int32)Index.buffer + 1] << 16);
                Int32 d = (Int32)(cmdlineACK[(Int32)Index.buffer] << 24);
                ChipID = a + b + c + d;
                if (ChipID == 0)
                {
                    return -4;//向机顶盒获取ChipID失败
                }
                if (!FindChipID())
                {
                    return -10; // 数据库中没有找到该ChipID
                }
                else if(CheckChipID())
                {
                    return -11;//已经校验过该ChipID无需再次校验
                }

                index = Protocol.Command(SERCOM_TYPE.COM_GETLICENSE, 0, 88, null, ref cmdlineACK);//调用类 ，获取STB中Flash信息
                if (index != 0)
                {
                    return -20;
                }
                #endregion
            }

            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_GETLICENSEOK)//下面是将从STB获取到的“序列化数据”赋值到文本框中
            {
                #region 校验序列化数据

               string StrBarcode = "";  //byte[] bb = new byte[cmdlineACK.Length - 5];
                for (int i = 0; i < cmdlineACK.Length - 5; i++)
                {
                    if (i != 0 && i % 20 == 0)//用于分页
                    {
                        StrBarcode += "\r\n";
                    }
                    StrBarcode += String.Format("{0:X02}", cmdlineACK[(Int32)Index.buffer + i]).ToString().Trim();//从STB获取ChipInfo
                    //bb[i]= cmdlineACK[(Int32)Index.buffer + i];
                }
               richtxt_LincenseBoard.Text = StrBarcode;  //string aa = byteToHexStr(bb);
                string ChipInfo = SearchSerializeData();
                if (ChipInfo.Trim() == "0")//搜索ChipInfo失败
                {
                    return -21;
                }
                else
                {
                    StrBarcode = "";

                    richtxt_connect.Text = "搜索序列化数据成功";
                    richtxt_Tips.Text += "搜索序列化数据成功\r\n";

                    Byte[] DbChipInfoBuffer = new Byte[ChipInfo.Length / 2];
                    if (!HDIC_Func.CStringToByte(ChipInfo, ref DbChipInfoBuffer))
                    {
                        return -22;
                    }
                    for (int i = 0; i < DbChipInfoBuffer.Length; i++)
                    {
                        if (i != 0 && i % 20 == 0)//用于分页
                        {
                            StrBarcode += "\r\n";
                        }
                        StrBarcode += String.Format("{0:X02}", DbChipInfoBuffer[i]).ToString().Trim();//从STB获取ChipInfo
                    }
                    richtxt_LincenseAdo.Text = StrBarcode;

                    if (richtxt_LincenseAdo.Text.Trim() != richtxt_LincenseBoard.Text.Trim())//校验序列化数据
                    {
                        return -23;
                    }

                    richtxt_connect.Text = "序列化校验成功! 请扫描机顶盒标签上的条形码!";
                    richtxt_Tips.Text += "序列化校验成功!\r\n";
                }
                #endregion

                BarCode.Start();  //开始监听扫描枪
                
            }
         
            return 0;
        }

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }


        #region 方法
        //判断 ChipID
        private bool FindChipID()
        {
            try
            {
                if (HDIC_DB.sqlQuery("select count(*) from STBData where ChipID='" + String.Format("{0:X08}", ChipID).ToString() + "'") != "0")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HDIC_Message.ShowWarnDialog(this, "数据库打开失败，原因：\r\n" + ex.ToString());
            }
            return false;
        }

        //判断是否校验过
        private bool CheckChipID()
        {
            try
            {
                if (HDIC_DB.sqlQuery("select count(*) from STBData where ChipID='" + String.Format("{0:X08}", ChipID).ToString() + "' and CheckFlag=1") != "0")
                {
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                HDIC_Message.ShowWarnDialog(this, ex.ToString());
            }
            return false;
        }

        //查找指定的序列化数据
        private string SearchSerializeData()
        {
            string ChipIDInfo = "";
            try
            {
                ChipIDInfo = HDIC_DB.sqlQuery("select ChipInfo from ChipData where ChipID='" + Convert.ToInt32(String.Format("{0:X08}", ChipID).ToString().Trim(), 16) + "'");
            }
            catch (System.Exception ex)
            {
                HDIC_Message.ShowWarnDialog(null, "查找指定的序列化数据时 出错，原因：\r\n" + ex.ToString());
            }
            return ChipIDInfo.Trim();
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
                digits[length-1] =((10 - modulo));
            }
            else
            {
                //digits[length] = (char)0;
                digits[length-1] =0;
            }
            return true;
        }
        //判断加密序列号
        private bool FindCAID(string CAID)
        {
            try
            {
                if (HDIC_DB.sqlQuery("select count(*) from STBData where CAID='" + CAID + "'") != "0")
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
        
        //判断STBID
        private bool FindSTBID(string mSTBID)
        {
            try
            {
                if (HDIC_DB.sqlQuery("select count(*) from STBData where STBID='" + mSTBID + "'") != "0")
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

        //判断SmartCardID
        private bool FindSmartCardID(string mSmartCardID)
        {
            try
            {
                if (HDIC_DB.sqlQuery("select count(*) from STBData where SmartCardID='" + mSmartCardID + "'") != "0")
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

        #endregion

        private void gf_CheckSerializer_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (HDIC_Message.ShowQuestionDialog(this, "确定要关闭该窗体？") == DialogResult.OK)
            //{
           Uart.close();//关闭串口
            e.Cancel = false; ;//关闭本窗体
            //}
            //else
            //{
            //    e.Cancel = true;
            //}
        }

        #region 校验
        private Int32 Check(string txtInfo)
            {
            #region 校验CAID
                if (txtInfo.Trim().Length == 11 && HDIC_Func.CheckObjectIsInteger(txtInfo.Trim()))
                {
                    //#region ce ce he he 
                    ////根据ChipID生成CAID
                    //Int32[] CAIDBuffer = new Int32[11];
                    //string CAID = ((ulong)(ChipID + 0x80000000)).ToString().Trim();
                    //for (int i = 0; i < CAID.Length; i++)
                    //{
                    //    CAIDBuffer[i] =Convert.ToInt32(CAID[i].ToString());
                    //}
                    //CalculateLuhnAlgorithm(CAIDBuffer, 11);//不清楚这个到底什么用，而且文档和继平写的也不一样
                    //#endregion
                    //string CAIDBufferStr= "";
                    //foreach (int temp in CAIDBuffer)
                    //{
                    //    CAIDBufferStr += temp.ToString();
                    //}
                    //string StrCAIDbuffer = new string(CAIDBuffer);
                //if (txt_CAID.Text.Trim() != StrCAIDbuffer.Trim().Substring(0, txtInfo.Trim().Length))//机顶盒计算出的CAID和扫描枪扫描的CAID比较
                //{
                //    return -1;
                //}

                    if (false) //发送命令，获取机顶盒上CAID与标签CAID比较  目前没有做
                    {
                        return -30;
                    }

                if (!FindCAID(txtInfo.Trim()))        //数据库的CAID与扫描枪扫描的CAID比较
                {
                    return -31;
                }

                else
                {
                    return 30;
                }
                }

            #endregion

            #region 校验STBID
                if (txtInfo.Trim().Length == 16 && HDIC_Func.CheckObjectIsInteger(txtInfo.Trim()))
            {
                //index = Protocol.Command(SERCOM_TYPE.COM_MFID, 0, 1, null, ref cmdlineACK);//调用类 ，获取STB中Flash信息

                if (false) //获取机顶盒STBID和标签上STBID比较 目前没有做
                {
                    return -40;
                }

                if (!FindSTBID(txtInfo.Trim()))//数据库的STBID与扫描枪扫描的CAID比较
                {
                    return -41;
                }

                else
                {
                    return 40;
                }
               
            }
            #endregion

            #region 校验SmartCardID
                if (HDIC_Command.STBType == 1 && txtInfo.Trim().Length == 12)//智能卡号不为空，说明是村村通
            {
                if (!FindSmartCardID(txtInfo.Trim()))//数据库的STBID与扫描枪扫描的CAID比较
                {
                    return -50;
                }

                else
                {
                    return 50;
                }
            }
            #endregion

            return 0;
        }

        private void  printInfo(string txtInfo)
        {
            
                Int32 index = Check(txtInfo);
                if (index < 0)
                {
                    richtxt_connect.ForeColor = System.Drawing.Color.Red;
                }
              
                if (index == -30)
                {
                    richtxt_connect.Text = "机顶盒中的CAID和扫描枪扫描的CAID不一致!";
                    richtxt_Tips.Text += "机顶盒中的CAID和扫描枪扫描的CAID不一致!\r\n"; 
                    //HDIC_Message.ShowWarnDialog(this, "机顶盒计算出的CAID和扫描枪扫描的CAID不一致!");
                }
                else if (index == -31)
                {
                    richtxt_connect.Text = "数据库的CAID与扫描枪扫描的不一致!";
                    richtxt_Tips.Text += "数据库的CAID与扫描枪扫描的不一致!\r\n";
                    //HDIC_Message.ShowWarnDialog(this, "数据库的CAID与扫描枪扫描的不一致!");
                }
                else if (index == 30) //【大于100的是成功的】
                {
                    richtxt_connect.Text = "加密序列号校验成功!";
                    richtxt_Tips.Text += "加密序列号校验成功!\r\n";
                    CheckCAID = true;
                }
                else if (index == -40)
                {
                    richtxt_connect.Text = "机顶盒中的STBID和扫描枪扫描的STBID不一致";
                    richtxt_Tips.Text += "机顶盒中的STBID和扫描枪扫描的STBID不一致\r\n";
                    //HDIC_Message.ShowWarnDialog(null, "数据库的STBID与扫描枪扫描的不一致!");
                }
                else if (index == -41)
                {
                    richtxt_connect.Text = "数据库的STBID与扫描枪扫描的不一致";
                    richtxt_Tips.Text += "数据库的STBID与扫描枪扫描的不一致\r\n";
                    //HDIC_Message.ShowWarnDialog(null, "数据库的STBID与扫描枪扫描的不一致!");
                }
                else if (index == 40) //【大于100的是成功的】
                {
                    richtxt_connect.Text = "STBID校验成功!";
                    richtxt_Tips.Text += "STBID校验成功!\r\n";
                    CheckSTBID = true;
                }
                else if (index == -50)
                {
                    richtxt_connect.Text = "数据库的SmartCardID与扫描枪扫描的不一致";
                    richtxt_Tips.Text += "数据库的SmartCardID与扫描枪扫描的不一致\r\n";
                }
                else if (index == 50) //【大于100的是成功的】
                {
                    richtxt_connect.Text = "智能卡号校验成功";
                    richtxt_Tips.Text += "智能卡号校验成功\r\n";
                    CheckSmartCardID = true;
                }

                if (CheckCAID == true && CheckSTBID == true)//校验是否全部通过
                {
                    if ((HDIC_Command.STBType == 0) || (HDIC_Command.STBType == 1 && CheckSmartCardID == true))
                    {
                        #region 校验高级安全状态 目前没有写
                                richtxt_connect.Text = "高级安全状态为：已打开!";
                                richtxt_Tips.Text += "高级安全状态为：已打开!\r\n";
                        #endregion

                        #region 校验Flash写保护状态 目前没有写
                        richtxt_connect.Text = "Flash写保护状态：已锁定!";
                        richtxt_Tips.Text += "Flash写保护状态：已锁定!\r\n";
                        #endregion

                                //将STBData表CheckFlag标识设置成True
                        try
                        {
                            if (HDIC_DB.ExcuteNonQuery("update STBData set CheckFlag=1 where ChipID='" + String.Format("{0:X08}", ChipID).ToString() + "'") > 0)
                            {
                                richtxt_connect.Text = "校验成功,请进行下一台!";
                                richtxt_Tips.Text += "校验成功,请进行下一台!\r\n";
                            }
                        }
                        catch (System.Exception ex)
                        {
                            HDIC_Message.ShowErrorDialog(this, "校验数据成功后修改标志位失败，原因：" + ex.ToString());
                        }



                        //richtxt_Tips.Select(0, richtxt_Tips.Text.Length); 
                        //richtxt_Tips.SelectionColor = System.Drawing.Color.ForestGreen;
                        //richtxt_Tips.SelectionLength = 0; 

                        BarCode.Stop();//关闭扫描仪
                    }
                }
                btn_begin.Enabled = true;
                btn_begin.Focus();
        }
        #endregion

        /// <summary>
        /// 校验CAID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_CAID_TextChanged(object sender, EventArgs e)
        {
            if (txt_CAID.Text.Trim().Length == 11 && HDIC_Func.CheckObjectIsInteger(txt_CAID.Text.Trim()))
            {
                printInfo(txt_CAID.Text.Trim());
            }
        }
        /// <summary>
        /// 校验STBID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_STBID_TextChanged(object sender, EventArgs e)
        {
            if (txt_STBID.Text.Trim().Length == 16 && HDIC_Func.CheckObjectIsInteger(txt_STBID.Text.Trim()))
            {
                printInfo(txt_STBID.Text.Trim());
            }
        }
        /// <summary>
        /// 校验SmartCardID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_SmartCardID_TextChanged(object sender, EventArgs e)
        {
            if (txt_SmartCardID.Text.Trim().Length == 12 && HDIC_Func.CheckObjectIsInteger(txt_SmartCardID.Text.Trim()))
            {
                printInfo(txt_SmartCardID.Text.Trim());
            }
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            mSpSlot.Close();//关闭串口
            this.Close();
        }

    }
}
