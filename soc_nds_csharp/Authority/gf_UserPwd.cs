using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.DB;
using HDICSoft.Message;
using HDICSoft.Command;

namespace soc_nds_csharp.Authority
{
    public partial class gf_UserPwd : Form
    {
        public gf_UserPwd()
        {
            InitializeComponent();
        }

        private void gf_UserPwd_Load(object sender, EventArgs e)
        {
            this.BackColor = HDIC_Command.setColor();

            txt_userName.Text = HDIC_Command.UseName;
            txt_oldPwd.Focus();
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            if (!checkPara())
            {
                return;
            }

            string sqlstr = "update SysUser set pwd='" + txt_newPwd.Text.Trim() + "' where userNo='"+HDIC_Command.userNo+"'";
            try
            {
                if (HDIC_DB.ExcuteNonQuery(sqlstr, null) > 0)
                {
                    HDIC_Message.ShowInfoDialog(this, "<"+HDIC_Command.UseName+">您好，修改密码成功");
                    init();
                }
            }
            catch (System.Exception ex)
            {
            	 HDIC_Message.ShowInfoDialog(this, "<"+HDIC_Command.UseName+">您好，修改密码失败\n"+ex.ToString());
                 txt_oldPwd.Focus();
            }
        }

        private bool checkPara()
        {
            if (txt_oldPwd.Text.Trim() == "")
            {
                HDIC_Message.ShowWarnDialog(this, "请填写原密码");
                txt_oldPwd.Focus();
                return false;
            }
            if (txt_newPwd.Text.Trim() == "")
            {
                HDIC_Message.ShowWarnDialog(this, "请填写新密码密码");
                txt_newPwd.Focus();
                return false;
            }
            if (txt_confirmPwd.Text.Trim()=="")
            {
                HDIC_Message.ShowWarnDialog(this, "请填写确认密码");
                txt_confirmPwd.Focus();
                return false;
            }
            if (txt_newPwd.Text.Trim() != txt_confirmPwd.Text.Trim())
            {
                HDIC_Message.ShowWarnDialog(this, "两次输入的密码不一致");
                txt_newPwd.Text = "";
                txt_confirmPwd.Text = "";
                txt_newPwd.Focus();
                return false;
            }
            if (HDIC_DB.sqlQuery("select count(userNo) from SysUser where userNo='" + HDIC_Command.userNo + "' and pwd='" + txt_oldPwd.Text.Trim() + "'") == "0")
            {
                HDIC_Message.ShowWarnDialog(this, "原密码输入错误");
                txt_oldPwd.Text = "";
                txt_oldPwd.Focus();
                return false;
            }
            return true;
        }

        private void init()
        {
            txt_oldPwd.Text = "";
            txt_newPwd.Text = "";
            txt_confirmPwd.Text = "";
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void txt_confirmPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_submit_Click(btn_submit, null);
            }
        }

    }
}
