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
using System.IO.Ports;
using System.Threading;
using soc_protocol;
//Delspace(t_send.Text);   //去掉所有空格，整合数据,记住

namespace soc_nds_csharp.Station_Operation
{
    /// <summary>
    /// SerialPort
    /// </summary>
    #region SerialPort
    public class Uart
    {
        String mConfigFile;
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

    //////////////////////////////////////////////////////////////////////////
    public partial class gf_Serializer : Form
    {
        // Create a new Mutex. The creating thread does not own the
        // Mutex.
        private Mutex mutex = new Mutex();//互斥体
       
        Uart mSpSlot;//串口对象定义
        UartProtocol Protocol;

        string ChipID = "12345678912";//  in order to Test

        public gf_Serializer()
        {
            InitializeComponent(); 
        }

        private void gf_Serializer_Load(object sender, EventArgs e)
        {
            initControl();

            //打开串口
            mSpSlot = new Uart();
            mSpSlot.loadConfig();
            mSpSlot.open();

            Protocol = new UartProtocol(mSpSlot.slot);//初始化uart对象

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
            richtxt_SmartCardID.Text = "";
            richtxt_ManufacturerID.Text = "";
            richtxt_ModelID.Text = "";
            richtxt_HardwareID.Text = "";
            richtxt_SerialID.Text = "";

            richtxt_Connect.Text = "";
            richtxt_info.Text = "";

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

        private int Connect()
        { 
            ////获取所有的串口    
            //String[] ports = System.IO.Ports.SerialPort.GetPortNames();
           
            initControl();

            // richtxt_Connect
            this.richtxt_Connect.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richtxt_Connect.ForeColor = System.Drawing.Color.ForestGreen;
            this.richtxt_info.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richtxt_info.ForeColor = System.Drawing.Color.Black;
            this.richtxt_Connect.Text =  "正在尝试连接,请稍后... ...";


         
//////////////////////////////////////////////////////////////////////////
//读取chipID 和 STBID
            Int32 index = 0;
            index = Protocol.Command(SERCOM_TYPE.COM_CONNECT, null, null);//调用类 ，发送命令
            if (index != 0)
            {
                HDIC_Message.ShowWarnDialog(this, "发送命令或者接收命令出错");
                return -1;
            }

//////////////////////////////////////////////////////////////////////////

            //根据ChipID生成CAID
            richtxt_CAID.Text = richtxt_ChipID.Text.Trim() + 0x80000000;

            if (FindChipID())
            {
                richtxt_Connect.Text = "Chip id 重复，请检查是否重复序列化!";
                richtxt_info.Text += "Chip id 重复";

                HDIC_Message.ShowWarnDialog(this, "数据库中已经存在该ChipId，请检查是否重复序列化!");
                return -1;
            }
            else
            {
                richtxt_Connect.Text = "获取Chip ID, manifactual id, hardwareid 成功!";
                richtxt_info.Text += "连接成功，请勿断电!\r\n获取Chip ID, manifactual id, hardwareid 成功!";
            }
//////////////////////////////////////////////////////////////////////////
            if (SearchSerializeData())
            {
                richtxt_Connect.Text = "搜索授权信息失败，请检查服务器或网络!";
                richtxt_info.Text += "\r\n搜索授权信息失败!\r\n";

                HDIC_Message.ShowWarnDialog(this, "搜索授权信息失败，请检查服务器或网络!");
                return -2;
            }
            else
            {
                richtxt_Connect.Text = "搜索授权信息成功";
                richtxt_info.Text += "\r\n搜索授权信息成功!\r\n";
            }

            return 0;
        }

        private bool FindChipID()
        {
            if (HDIC_DB.sqlQuery("select count(*) from STBData where ChipID='" + ChipID + "'") != "0")
            {
                return true;
            }
            return false;
        }

        private bool SearchSerializeData()
        {
            if (HDIC_DB.sqlQuery("select count(*) from ChipData where ChipID='" + ChipID + "'") != "0")
            {
                return true;
            }
            return false;
        }

    }
}
