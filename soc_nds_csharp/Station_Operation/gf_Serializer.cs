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

//Delspace(t_send.Text);   //去掉所有空格，整合数据,记住

namespace soc_nds_csharp.Station_Operation
{

    public partial class gf_Serializer : Form
    {
        // Create a new Mutex. The creating thread does not own the
        // Mutex.
        private Mutex mutex = new Mutex();//互斥体

        Uart mSpSlot;//串口对象定义
        UartProtocol Protocol;
        Int32 m_STBType = 01;//临时，到时候看看如何获取
        Int32 ChipID = 0;
        Int32 STBOpIndex;//当前流水号（工位一选择工位线时得到的）

        public gf_Serializer()
        {
            InitializeComponent();
            this.richtxt_Connect.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            this.richtxt_info.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
           
            this.richtxt_SerialID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            this.richtxt_CAID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
           
            this.richtxt_ChipID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            this.richtxt_STBID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
           
            this.richtxt_SmartCardID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            this.richtxt_ManufacturerID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            this.richtxt_ModelID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
           
            this.richtxt_HardwareID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            

        }

        private void gf_Serializer_Load(object sender, EventArgs e)
        {

            //打开串口
            mSpSlot = new Uart();
            mSpSlot.loadConfig();
            mSpSlot.open();

            Protocol = new UartProtocol(mSpSlot.slot);//初始化uart对象 

            variable_assignment();

            btn_begin.Focus();
        }

        private void initControl()
        {
            richtxt_Connect.Enabled = false;
            richtxt_info.Enabled = false;
            richtxt_SerialID.Enabled = false;
            richtxt_CAID.Enabled = false;
            richtxt_ChipID.Enabled = false;
            richtxt_STBID.Enabled = false;
            richtxt_ManufacturerID.Enabled = false; richtxt_ModelID.Enabled = false; richtxt_HardwareID.Enabled = false;

            richtxt_STBID.Text = "";
            richtxt_CAID.Text = "";
            richtxt_ChipID.Text = "";
            richtxt_SmartCardID.Text = "";
            richtxt_ManufacturerID.Text = "";
            richtxt_ModelID.Text = "";
            richtxt_HardwareID.Text = "";
            richtxt_SerialID.Text = "";

            richtxt_Connect.Text = "";
            richtxt_info.Text = "";

            this.richtxt_Connect.ForeColor = System.Drawing.Color.ForestGreen;
            this.richtxt_info.ForeColor = System.Drawing.Color.Black;
            this.richtxt_SerialID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_CAID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_ChipID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_STBID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_SmartCardID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_ManufacturerID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_ModelID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_HardwareID.ForeColor = System.Drawing.Color.Black;
        }

        private void btn_begin_KeyDown(object sender, KeyEventArgs e)
        {
            // Wait until it is safe to enter.
            mutex.WaitOne();

            if (Keys.KeyCode == Keys.Enter)
            {
                Connect();
            }

            // Release the Mutex.
            mutex.ReleaseMutex();
        }

        private void btn_begin_Click(object sender, EventArgs e)
        {
            // Wait until it is safe to enter.
            mutex.WaitOne();

            Connect();

            // Release the Mutex.
            mutex.ReleaseMutex();

        }

        private void Connect()
        {
            Int32 index = CommandSerial();
            if (index != 0)
            {
                richtxt_Connect.ForeColor = System.Drawing.Color.Red;
            }

            if (index == -1)
            {
                HDIC_Message.ShowWarnDialog(this, "该流水线号已经达到最大值，请重新选择流水线号");
            }
            else if(index == -2)
            {
                richtxt_info.Text += "连接失败，请重新连接!\r\n";
                richtxt_Connect.Text = "连接失败，请重新连接!";
                HDIC_Message.ShowWarnDialog(this, "向下位机发送命令或者接收命令出错");
            }
            else if (index == -3)
            {
                richtxt_info.Text += "连接失败，请重新连接!\r\n";
                richtxt_Connect.Text = "连接失败，请重新连接!";
                HDIC_Message.ShowWarnDialog(this, "与下位机连接失败");
            }
            else if (index == -4)
            {
                richtxt_info.Text += "向下位机获取信息失败!\r\n";
                richtxt_Connect.Text = "向下位机获取信息失败!";
                HDIC_Message.ShowWarnDialog(this, "向下位机获取信息失败");
            }
            else if (index == -5)
            {
                richtxt_info.Text += "向下位机获取ChipID失败!\r\n";
                richtxt_Connect.Text = "向下位机获取ChipID失败!";
                HDIC_Message.ShowWarnDialog(this, "向下位机获取ChipID失败，请尝试重新获取!");
            }
            else if (index == -6)
            {
                richtxt_Connect.Text = "Chip id 重复，请检查是否重复序列化!";
                richtxt_info.Text += "\r\nChip id 重复\r\n";
                HDIC_Message.ShowWarnDialog(this, "数据库中已经存在该ChipId，请检查是否重复序列化!");
            }
            else if (index == -7)
            {
                richtxt_Connect.Text = "搜索授权信息失败，请检查服务器或网络!";
                richtxt_info.Text += "\r\n搜索授权信息失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "搜索授权信息失败，请检查服务器或网络!");
            }
            else if (index == -8)
            {
                HDIC_Message.ShowWarnDialog(this, "字符串转换字符数组失败");
            }
            else if (index == -9)
            {
                richtxt_Connect.Text = "向下位机发送授权信息失败!";
                richtxt_info.Text += "\r\n向下位机发送授权信息失败!\r\n";
                HDIC_Message.ShowWarnDialog(this, "向下位机发送授权信息失败");
            }
            else if (index == -10)
            {
                richtxt_info.Text += "\r\n上传授权信息到数据库失败!\r\n";
                richtxt_Connect.Text = "上传授权信息到数据库失败";
            }
            btn_begin.Enabled = true;
        }

        private int CommandSerial()
        { 
            ////获取所有的串口    
            //String[] ports = System.IO.Ports.SerialPort.GetPortNames();
           
            initControl();
            btn_begin.Enabled = false;

            this.richtxt_Connect.Text = "正在尝试连接,请稍后... ...";   // richtxt_Connect

            STBOpIndex = Convert.ToInt32(HDIC_DB.sqlQuery("select STBOpIndex from STBOp where STBOpLineNum='" + HDIC_Command.STBLineNum + "' and STBOpFlag=1").Trim());

            this.richtxt_SerialID.Text=String.Format("{0:d07}", STBOpIndex).ToString();
           
            Int32 STBOpMaxIndex = Convert.ToInt32(HDIC_DB.sqlQuery("select STBOpIndex from STBOp where STBOpLineNum='" + HDIC_Command.STBLineNum + "' and STBOpFlag=3").Trim());
            if (STBOpIndex > STBOpMaxIndex)   //判断当前流水线是否是最大值
            {
                return -1;
            }
//////////////////////////////////////////////////////////////////////////
//读取chipID 和 STBID
            Int32 index = 0;
            //Byte[] cmdlineACK = new Byte[Protocol.CONTAINER_LENGTH - 1];//只获取命令行前四个byte即可（主要用于判断当前什么命令）
            Byte[] cmdlineACK ={};
            index = Protocol.Command(SERCOM_TYPE.COM_CONNECT, 0,0, null,ref cmdlineACK);//调用类 ，发送命令
            if (index != 0)
            {
                return -2;
            }
            if(cmdlineACK[(Int32)Index.cmdone]==(Byte)SERCOM_TYPE.COM_HANDINFO)
            {
                index = Protocol.Command(SERCOM_TYPE.COM_OK, 0,0, null, ref cmdlineACK);//调用类 ，尝试连接
                if (index != 0)
                {
                    return -3;
                }
                richtxt_info.Text += "连接成功，请勿断电!\r\n";
                richtxt_Connect.Text = "连接成功，请勿断电!";
            }
            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_START)
            {
                index = Protocol.Command(SERCOM_TYPE.COM_ALLINFO,0, 7, null, ref cmdlineACK);//调用类 ，获取所有的信息
                if (index != 0)
                {
                    return -4;
                }
            }
            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_ALLINFO)
            {
                Int32 a = (Int32)cmdlineACK[(Int32)Index.buffer + 3];
                Int32 b = (Int32)(cmdlineACK[(Int32)Index.buffer + 2] << 8);
                Int32 c = (Int32)(cmdlineACK[(Int32)Index.buffer + 1] << 16);
                Int32 d = (Int32)(cmdlineACK[(Int32)Index.buffer] << 24);
                 ChipID=a + b + c + d;
            }
            //////////////////////////////////////////////////////////////////////////
            #region ChipID的处理
            if (ChipID == 0)
            {
                return -5;//向下位机获取ChipID失败
            }

            richtxt_ChipID.Text = String.Format("{0:X08}", ChipID).ToString();

            if (FindChipID())
            {
                return -6; // 重复序列化
            }

            #endregion

            else
            {
                #region 获取CAID
                //根据ChipID生成CAID
                char[] CAIDBuffer = new char[128];
                string CAID = ((ulong)(ChipID + 0x80000000)).ToString().Trim();
                for (int i = 0; i < CAID.Length; i++)
                {
                    CAIDBuffer[i] = Convert.ToChar(CAID[i]);
                }
                //char[] aa = CAID.ToCharArray();
                CalculateLuhnAlgorithm(CAIDBuffer, 10);//不清楚这个到底什么用，而且文档和继平写的也不一样
                string StrCAIDbuffer = new string(CAIDBuffer);
                richtxt_CAID.Text = StrCAIDbuffer.Trim(); 
                #endregion

                richtxt_ManufacturerID.Text = String.Format("{0:X}", cmdlineACK[((Int32)Index.buffer) + 4]).ToString();
                richtxt_ModelID.Text = String.Format("{0:X}", cmdlineACK[((Int32)Index.buffer) + 5]).ToString();
                richtxt_HardwareID.Text = String.Format("{0:X}", cmdlineACK[((Int32)Index.buffer) + 6]).ToString();

                #region 获取STBID
                DateTime nowdate = DateTime.Now;
                //二.获取今天是一年当中的第几天
                int iDays = DateTime.Today.DayOfYear;

                //一.找到第一周的最后一天（先获取1月1日是星期几，从而得知第一周周末是几）
                int firstWeekend = 7 - Convert.ToInt32(DateTime.Parse(nowdate.Year + "-1-1").DayOfWeek);
                //三.（今天 减去 第一周周末）/7 等于 距第一周有多少周 再加上第一周的1 就是今天是今年的第几周了 // 刚好考虑了惟一的特殊情况就是，今天刚好在第一周内，那么距第一周就是0 再加上第一周的1 最后还是1
                int weeknums = Convert.ToInt32(Math.Ceiling((iDays - firstWeekend) / 7.0)) + 1;
                #endregion
                richtxt_STBID.Text = String.Format("{0:d03}{1:d02}{2:d02}{3:d02}{4:d07}", cmdlineACK[((Int32)Index.buffer) + 4], m_STBType, (nowdate.Year % 100), weeknums, STBOpIndex);

                richtxt_Connect.Text = "获取Chip ID, manifactual id, hardwareid 成功!";
                richtxt_info.Text += "\r\n获取Chip ID, manifactual id, hardwareid 成功!\r\n";
            }

//////////////////////////////////////////////////////////////////////////授权信息
            string ChipInfo = SearchSerializeData();
            if (ChipInfo.Trim()=="0")
            {
                return -7;//搜索数据库中授权信息（chipinfo）失败
            }
            else
            {
                richtxt_Connect.Text = "搜索授权信息成功";
                richtxt_info.Text += "\r\n搜索授权信息成功!\r\n";

                Byte[] dataBuffer=new Byte[ChipInfo.Length/2];
                if (!HDIC_Func.CStringToByte(ChipInfo, ref dataBuffer))
                {
                    return -8;
                }
               
                int length = (ChipInfo.Length)/2;

                index = Protocol.Command(SERCOM_TYPE.COM_LICENSE, length,0, dataBuffer, ref cmdlineACK);//调用类 ，获取所有的信息
                if (index != 0)
                {
                    return -9;//向下位机发送授权信息失败
                }
            }
            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_LICENSEOK && cmdlineACK[(Int32)Index.cmdtwo] == (Byte)SERCOM_TYPE.COM_RETURN)
            {
                richtxt_Connect.Text = "发送授权信息成功";
                richtxt_info.Text += "\r\n发送授权信息成功!\r\n";
                if (insertDB())
                {
                    richtxt_info.Text += "\r\n上传授权信息到数据库成功!\r\n";
                    richtxt_Connect.Text = "上传授权信息到数据库成功";

                        if (richtxt_SmartCardID.Text.Trim().Length == 12)//打印村村通条形码
                        {
                            HDIC_Func.TSCPrinter(tscWidth, tscHeight, tscPrintSpeed, tscDensity, tscX, tscY, tscEncodeType, tscGeneratelabel, richtxt_STBID.Text.Trim(), richtxt_CAID.Text.Trim(), richtxt_SmartCardID.Text.Trim(), tscRotate, tscBar, 1);
                        }
                        else                                             //打印户户通条形码
                        {
                            HDIC_Func.TSCPrinter(tscWidth, tscHeight, tscPrintSpeed, tscDensity, tscX, tscY, tscEncodeType,tscGeneratelabel, richtxt_STBID.Text.Trim(), richtxt_CAID.Text.Trim(),null, tscRotate, tscBar, 0);
                        }
                }
                else
                {
                    return -10; //上传授权信息到数据库失败
                }
            }
            btn_begin.Enabled = true;
            return 0;
        }
        //判断 ChipID
        private bool FindChipID()
        {
            if (HDIC_DB.sqlQuery("select count(*) from STBData where ChipID='" + richtxt_ChipID.Text.Trim() + "'") != "0")
            {
                return true;
            }
            return false;
        }
        //C代码的Luhn算法示例
        private bool CalculateLuhnAlgorithm(char[] digits, int length)
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
                digits[length] = (char)((10 - modulo) + 0x30);
                digits[length + 1] = (char)0;
            }
            else
            {
                digits[length] = (char)0;
            }
            return true;
        }

        //查找指定的序列化数据
        private string SearchSerializeData()
        {
            string ChipIDInfo = "";
            try
            {
                ChipIDInfo = HDIC_DB.sqlQuery("select ChipInfo from ChipData where ChipID='" + Convert.ToInt32(richtxt_ChipID.Text.Trim(), 16) + "'");
            }
            catch (System.Exception ex)
            {
                HDIC_Message.ShowWarnDialog(null, "查找指定的序列化数据时 出错，原因：\r\n" + ex.ToString());
            }
            return ChipIDInfo.Trim();
        }

        //将序列数据添加到数据库中
        private bool insertDB()
        {
            STBOpIndex++;

            string sqlstr;
            //插入新的序列号数据到数据库
            try
            {
                sqlstr =@"insert into STBData(ChipID,CAID,STBID,flag) values('"+richtxt_ChipID.Text.Trim()+
                    "','"+richtxt_CAID.Text.Trim()+"','"+richtxt_STBID.Text.Trim()+
                    "',0); update STBOp set STBOpIndex='" + STBOpIndex + "' where STBOpLineNum='" + HDIC_Command.STBLineNum +
                     "' and STBOpFlag=1; ";
                if (HDIC_DB.ExcuteNonQuery(sqlstr, null) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
           
            return true;
        }

       string tscWidth = "";
       string tscHeight = "";
       string tscPrintSpeed = "";
       string tscDensity = "";
       string tscX="";
       string tscY="";
       string tscEncodeType = "";
       string tscGeneratelabel = "";
       string tscRotate = "";
       string tscBar = "";
        int   tscFlag = 1;
        #region 为条形码参数变量赋值 并将值赋给条形码打印机
        /// <summary>
        /// 为条形码参数变量赋值
        /// </summary>
        private void variable_assignment()
        {
           
            DataTable dt = HDIC_Func.XMLToDataSet(HDIC_Func.GetRunningPath() + @"config\BarcodeXml.xml").Tables["root"];
            if (dt.Rows.Count > 0)
            {
               tscWidth        = dt.Rows[0]["tscWidth"].ToString().Trim();
               tscHeight       = dt.Rows[0]["tscHeight"].ToString().Trim();
               tscPrintSpeed   = dt.Rows[0]["tscPrintSpeed"].ToString().Trim();
               tscDensity      = dt.Rows[0]["tscDensity"].ToString().Trim();
               tscX            = dt.Rows[0]["tscX"].ToString().Trim();
               tscY            = dt.Rows[0]["tscY"].ToString().Trim();
               tscEncodeType   = dt.Rows[0]["tscEncodeType"].ToString().Trim();
               tscGeneratelabel=dt.Rows[0]["tscGeneratelabel"].ToString().Trim();
               tscRotate       = dt.Rows[0]["tscRotate"].ToString().Trim();
               tscBar          = dt.Rows[0]["tscBar"].ToString().Trim();
            }  tscFlag         = 1;
        }
        #endregion

        private void gf_Serializer_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (HDIC_Message.ShowQuestionDialog(this, "确定要关闭工位一？") == DialogResult.OK)
            {
                mSpSlot.close();
            }
        }

    }

}
