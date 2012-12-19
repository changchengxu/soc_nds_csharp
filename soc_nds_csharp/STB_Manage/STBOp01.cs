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

namespace soc_nds_csharp.STB_Manage
{
    public partial class STBOp01 : Form
    {
        #region //in order to move the form
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        #endregion

        //构造全局变量
        private int intGloID;

        public STBOp01()
        {
            InitializeComponent();
            intGloID = -1;
            btn1.Text = " 添 加 ";
            txt_pipID.Enabled = true;
        }

        public STBOp01(int intID)
        {
            InitializeComponent();
            intGloID = intID;
            btn1.Text = " 修 改 ";
            txt_pipID.Enabled = false;
            txt_pipID.Text = intGloID.ToString();
        }

        #region move the form
        private void STBClient01_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left & this.WindowState == FormWindowState.Normal)
            {
                // 移动窗体
                this.Capture = false;
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion

        /// <summary>
        /// add or edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn1_Click(object sender, EventArgs e)
        {
            if (intGloID == -1)// add
            {
                try
                {
                    object obj = HDIC_DB.ExecuteScalar("select count(*) from STBOp where STBOpLineNum='" + txt_pipID.Text.Trim() + "'", null);

                    if (obj == null || (int)obj == 0)
                    {
                        if (!CheckData())
                        {
                            return;
                        }

                        if (HDIC_DB.ExcuteNonQuery(@"insert into STBOp (STBOpLineNum,STBOpIndex,STBOpFlag) values('" + txt_pipID.Text.Trim() + "','" +
                            txt_currentPipID.Text.Trim() + "',1);insert into STBOp (STBOpLineNum,STBOpIndex,STBOpFlag) values('" + txt_pipID.Text.Trim() + "','" +
                            txt_pipIDMax.Text.Trim() + "',3)", null) > 0)
                        {
                            HDIC_Message.ShowInfoDialog(this, "添加成功");
                            this.Close();
                        }
                    }
                    else
                    {
                        HDIC_Message.ShowWarnDialog(this, "该流水线已经分配");
                        return;
                    }

                }
                catch
                {
                    HDIC_Message.ShowWarnDialog(this, "数据库打开失败,请检查服务器或者网络");
                }
            }

            else if (intGloID > 0)          //edit
            {
                if (!CheckData())
                {
                    return;
                }

                try
                {
                    if (HDIC_DB.ExcuteNonQuery(@"update STBOp set STBOpIndex='" + txt_currentPipID.Text.Trim()
                   + "' where STBOpLineNum='" + txt_pipID.Text.Trim()
                   + "' and STBOpFlag=1; update STBOp set STBOpIndex='" + txt_pipIDMax.Text.Trim()
                   + "' where STBOpLineNum='" + txt_pipID.Text.Trim() + "' and STBOpFlag=3", null) > 0)
                    {
                        HDIC_Message.ShowInfoDialog(this, "修改成功");
                        this.Close();
                    }
                    else
                    {
                        HDIC_Message.ShowWarnDialog(this, "修改该流水线失败");
                        return;
                    }
                }
                catch
                {
                    HDIC_Message.ShowWarnDialog(this, "数据库打开失败,请检查服务器或者网络");
                }

            }
        }

        private bool CheckData()
        {
            if (txt_pipID.Text.Trim() == "")
            {
                HDIC_Message.ShowWarnDialog(this, "流水号未输入");
                txt_pipID.Focus();
                return false;
            }
            else if (!HDIC_Func.CheckObjectIsInteger(txt_pipID.Text.Trim()))
            {
                HDIC_Message.ShowWarnDialog(this, "流水号输入不是数字");
                txt_pipID.Text = "";
                txt_pipID.Focus();
                return false;
            }
            else if (txt_currentPipID.Text.Trim() == "")
            {
                HDIC_Message.ShowWarnDialog(this, "当前流水号未输入");
                txt_currentPipID.Focus();
                return false;

            }
            else if (!HDIC_Func.CheckObjectIsInteger(txt_currentPipID.Text.Trim()))
            {
                HDIC_Message.ShowWarnDialog(this, "当前流水号输入不是数字");
                txt_currentPipID.Text = "";
                txt_pipIDMax.Text = "";
                txt_currentPipID.Focus();
                return false;
            }
            else if (txt_pipIDMax.Text.Trim() == "")
            {
                HDIC_Message.ShowWarnDialog(this, "流水号最大值未输入");
                txt_pipIDMax.Focus();
                return false;
            }
            else if (!HDIC_Func.CheckObjectIsInteger(txt_pipIDMax.Text.Trim()))
            {
                HDIC_Message.ShowWarnDialog(this, "流水号最大值输入不是数字");
                txt_pipIDMax.Text = "";
                txt_pipIDMax.Focus();
                return false;
            }
            else if (Convert.ToInt32(txt_currentPipID.Text.Trim()) > Convert.ToInt32(txt_pipIDMax.Text.Trim()))
            {
                HDIC_Message.ShowWarnDialog(this, "当前值不能大于最大值");
                txt_currentPipID.Text = "";
                txt_pipIDMax.Text = "";
                txt_pipIDMax.Text = "";
                return false;
            }

            ////string sqlstr="select count(STBOpIndex) from STBOp where (select max(STBOpIndex) from STBOp where STBOpFlag=3)>='"+txt_currentPipID.Text.Trim()+"'";
            ////if (HDIC_DB.sqlQuery(sqlstr)!="0")//新建流水号最好范围是连续的
            ////{
            ////     HDIC_Message.ShowWarnDialog(this, "请保持流水线的连续，当前流水号范围存在重叠现象");
            ////    txt_currentPipID.Text = "";
            ////    txt_pipIDMax.Text = "";
            ////    return false;
            ////}
            return true;
        }

        /// <summary>
        /// cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn2_Click(object sender, EventArgs e)
        {
            if (HDIC_Message.ShowQuestionDialog(this, "确定要退出吗？") == DialogResult.OK)
            {
                this.Close();
            }
        }
    }
}
