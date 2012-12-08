﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.Message;
using HDICSoft.DB;
using HDICSoft.Func;
using soc_protocol;

namespace soc_nds_csharp.Station_Operation
{
    public partial class gf_CheckSerializer : Form
    {
        Uart mSpSlot;//串口对象定义
        UartProtocol Protocol;
        Int32 ChipID = 0;

        public gf_CheckSerializer()
        {
            InitializeComponent();

            this.richtxt_LincenseBoard.Font = new System.Drawing.Font("SimSun", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richtxt_LincenseAdo.Font = new System.Drawing.Font("SimSun", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_CAID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_SmartCardID.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richtxt_Tips.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        }

        private void gf_CheckSerializer_Load(object sender, EventArgs e)
        {
            //打开串口
            mSpSlot = new Uart();
            mSpSlot.loadConfig();
            mSpSlot.open();

            Protocol = new UartProtocol(mSpSlot.slot);//初始化uart对象 

            btn_begin.Focus();
        }

        private void initControl()
        {
            richtxt_LincenseBoard.Enabled = false;
            richtxt_LincenseAdo.Enabled = false;
            richtxt_Tips.Enabled=false;

            richtxt_LincenseBoard.Text = "";
            richtxt_LincenseAdo.Text="";
            txt_CAID.Text="";
            txt_SmartCardID.Text="";
            richtxt_Tips.Text="";

            this.richtxt_LincenseBoard.ForeColor = System.Drawing.Color.Black;
            this.richtxt_LincenseAdo.ForeColor = System.Drawing.Color.Black;
            this.txt_CAID.ForeColor = System.Drawing.Color.Black;
            this.txt_SmartCardID.ForeColor = System.Drawing.Color.Black;
            this.richtxt_Tips.ForeColor = System.Drawing.Color.ForestGreen;

        }

        private void btn_begin_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void btn_begin_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.KeyCode == Keys.Enter)
            {
                Connect();
            }
        }

        private void Connect()
        {
          
            Int32 index = CommandSerial();

            if (index != 0)
            {
                richtxt_Tips.ForeColor = System.Drawing.Color.Red;
            }

            if (index == -1)
            {
                richtxt_Tips.Text = "连接失败，请重新连接!";
                HDIC_Message.ShowWarnDialog(this, "向下位机发送命令或者接收命令出错");
            }
            else if (index == -2)
            {
                richtxt_Tips.Text = "连接失败，请重新连接!";
                HDIC_Message.ShowWarnDialog(this, "向下位机连接失败");
            }

            #region 第一种:没有验证STBID
            else if (index == -3)
            {
                richtxt_Tips.Text = "获取ChipID信息失败!";
                HDIC_Message.ShowWarnDialog(this, "向下位机获取ChipID信息失败");
            }
            else if (index == -10)
            {
                richtxt_Tips.Text = "从机顶盒获取到的ChipID和数据库不一致!";
                HDIC_Message.ShowWarnDialog(this, "从机顶盒获取到的ChipID和数据库不一致");
            }
            else if (index == -4)
            {
                richtxt_Tips.Text = "获取Lisence数据失败!";
                HDIC_Message.ShowWarnDialog(this, "向下位机获取Lisence数据失败");
            }
            else if (index == -5)
            {
                richtxt_Tips.Text = "搜索授权信息失败!";
                HDIC_Message.ShowWarnDialog(this, "搜索授权信息失败，请检查服务器或网络!");
            }
            else if (index == -6)
            {
                richtxt_Tips.Text = "序列化数据转换字符数组时，发生错误!";
            }
            else if (index == -7)
            {
                richtxt_Tips.Text = "序列化校验失败！请检查或返回第一个工位!";
                HDIC_Message.ShowWarnDialog(this, "序列化校验失败！请检查或返回第一个工位!");
            }
            else if (index == -8)
            {
                richtxt_Tips.Text = "机顶盒计算出的CAID和扫描枪扫描的CAID不一致!";
                HDIC_Message.ShowWarnDialog(this, "机顶盒计算出的CAID和扫描枪扫描的CAID不一致!");
            }
            else if (index == -88)
            {
                richtxt_Tips.Text = "数据库的CAID与扫描枪扫描的不一致!";
                HDIC_Message.ShowWarnDialog(this, "数据库的CAID与扫描枪扫描的不一致!");
            }
            else if (index == -9)
            {
                richtxt_Tips.Text = "向下位机获取ChipID失败";
            }
            else if (index == -100)
            {
                richtxt_Tips.Text = "向下位机获取STBID失败";
            }
            else if (index == -101)
            {
                richtxt_Tips.Text = "数据库的STBID与扫描枪扫描的不一致";
                HDIC_Message.ShowWarnDialog(this, "数据库的STBID与扫描枪扫描的不一致!");
            }
            #endregion

            btn_begin.Enabled = true;
        }


        private Int32 CommandSerial()
        {
            btn_begin.Enabled = false;

            initControl();

            // richtxt_Connect
            this.richtxt_Tips.Text = "正在建立连接,请稍后... ...";

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
                richtxt_Tips.Text = "连接成功，请勿断电!";
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
                    return -9;//向下位机获取ChipID失败
                }
                if (!FindChipID())
                {
                    return -10; // 重复序列化
                }

                index = Protocol.Command(SERCOM_TYPE.COM_GETLICENSE, 0, 88, null, ref cmdlineACK);//调用类 ，获取STB中Flash信息
                if (index != 0)
                {
                    return -4;
                }
                #endregion
            }

            if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_GETLICENSEOK)//下面是将从STB获取到的“序列化数据”赋值到文本框中
            {
                #region 校验序列化数据

                string StrBarcode = "";
                for (int i = 0; i < cmdlineACK.Length - 5; i++)
                {
                    if (i != 0 && i % 20 == 0)//用于分页
                    {
                        StrBarcode += "\r\n";
                    }
                    StrBarcode += String.Format("{0:X02}", cmdlineACK[(Int32)Index.buffer + i]).ToString().Trim();//从STB获取ChipInfo
                }
                richtxt_LincenseBoard.Text = StrBarcode;
                string ChipInfo = SearchSerializeData();
                if (ChipInfo.Trim() == "0")//搜索ChipInfo失败
                {
                    return -5;
                }
                else
                {
                    StrBarcode = "";

                    richtxt_Tips.Text = "搜索授权信息成功";

                    Byte[] DbChipInfoBuffer = new Byte[ChipInfo.Length / 2];
                    if (!HDIC_Func.CStringToByte(ChipInfo, ref DbChipInfoBuffer))
                    {
                        return -6;
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
                        return -7;
                    }

                    richtxt_Tips.Text = "序列化校验成功! 请扫描机顶盒标签上的CAID!";
                }
                #endregion

                #region 下面是验证CAID
                
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
                if (txt_CAID.Text.Trim() != StrCAIDbuffer.Trim())//机顶盒计算出的CAID和扫描枪扫描的CAID比较
                {
                    return -8;
                }
                else if (!FindCAID(txt_CAID.Text.Trim()))        //数据库的CAID与扫描枪扫描的CAID比较
                {
                    return -88;
                }
                richtxt_Tips.Text = "加密序列号校验成功!";
                #endregion

                #region 校验STBID
                if (txt_STBID.Text.Trim().Length == 16)
                {
                    if (!FindSTBID(txt_STBID.Text.Trim()))//数据库的STBID与扫描枪扫描的CAID比较
                    {
                        return -101;
                    }
                }

                //index = Protocol.Command(SERCOM_TYPE.COM_MFID, 0, 1, null, ref cmdlineACK);//调用类 ，获取STB中Flash信息
                //if (index != 0)
                //{
                //    return -100;//向下位机获取STBID失败
                //}
                //if (cmdlineACK[(Int32)Index.cmdone] == (Byte)SERCOM_TYPE.COM_MFID)//下面是将从STB获取到的“序列化数据”赋值到文本框中
                //{
                //    cmdlineACK[(Int32)Index.buffer]
                //}
                #endregion

            }

            return 0;

            btn_begin.Enabled = true;
        }

      //判断 ChipID
        private bool FindChipID()
        {
            if (HDIC_DB.sqlQuery("select count(*) from STBData where ChipID='" + String.Format("{0:X08}", ChipID).ToString() + "'") != "0")
            {
                return true;
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
        //判断加密序列号
        private bool FindCAID(string CAID)
        {
            if (HDIC_DB.sqlQuery("select count(*) from STBData where CAID='" + CAID + "'") != "0")
            {
                return true;
            }
            return false;
        }
        
        //判断STBID
        private bool FindSTBID(string mSTBID)
        {
            if (HDIC_DB.sqlQuery("select count(*) from STBData where STBID='" + mSTBID + "'") != "0")
            {
                return true;
            }
            return false;
        }

        private void gf_CheckSerializer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (HDIC_Message.ShowQuestionDialog(this, "确定要关闭该窗体？") == DialogResult.OK)
            {
                mSpSlot.close();
                this.Close();
            }
        }
    }
}
