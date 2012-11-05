using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.DB;
using HDICSoft.Message;

namespace soc_nds_csharp.Authority
{
    public partial class gf_SysUser : Form
    {
        public gf_SysUser()
        {
            InitializeComponent();
        }

        private void gf_SysUser_Load(object sender, EventArgs e)
        {
            initCtrols();

            cbo_UserRole.DataSource = HDIC_DB.GetList("select roleNo,roleName from SysRole");
            cbo_UserRole.DisplayMember = "roleName";
            cbo_UserRole.ValueMember = "roleNo";
            cbo_UserRole.SelectedIndex = -1;

        }

#region add and edit

        private void tsb_Add_Click(object sender, EventArgs e)
        {
            initCtrols();
            panel_Edit.Visible = true;
            sign = "add";
        }

        private void tsb_Edit_Click(object sender, EventArgs e)
        {
            initCtrols();

            if (dgv_SysUser.Rows.Count > 0)
            {
                if (dgv_SysUser.CurrentRow.Index > -1)
                {
                    panel_Edit.Visible = true;
                    txt_UserName.Enabled = false;
                    sign = "edit";

                    txt_UserName.Text = dgv_SysUser.SelectedRows[0].Cells["userName"].Value.ToString().Trim();
                    txt_Remark.Text = dgv_SysUser.SelectedRows[0].Cells["remark"].Value.ToString().Trim();
                    cbo_UserRole.Text = dgv_SysUser.SelectedRows[0].Cells["roleName"].Value.ToString().Trim();
                }
            }
        }

        string sign = "";
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (sign == "add")
            {
                if (!chekPara())
                {
                    return;
                }

                    try
                    {
                        //检查数据库中是否存在该用户
                        if (HDIC_DB.sqlQuery("select * from SysUser where userName='" + txt_UserName.Text.Trim() + "'").ToString() != "0")
                        {
                            HDIC_Message.ShowWarnDialog(this, "数据库中已经存在该用户");
                            return;
                        }

                        string sqlstr = @"insert into SysUser(userName,pwd,remark) values('" + txt_UserName.Text.Trim() + "','" +
                                    txt_UserPwd.Text.Trim() + "','" + txt_Remark.Text.Trim()
                                    + "') select @@IDENTITY as userID;  insert into SysUserRole(userNo,roleNo) values( @@IDENTITY,'" + cbo_UserRole.SelectedValue.ToString().Trim() + "')";

                        if (HDIC_DB.ExcuteNonQuery(sqlstr, null) > 0)
                        {
                            HDIC_Message.ShowInfoDialog(this, "添加用户成功");
                            bindDgv();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        HDIC_Message.ShowInfoDialog(this, ex.ToString());
                        bindDgv();
                    }
            }

            else if (sign == "edit")
            {
                if (!chekPara())
                {
                    return;
                }
                    try
                    {
                        string sqlstr="update SysUser set pwd='" + txt_UserPwd.Text.Trim() + "',remark='"+txt_Remark.Text.Trim()
                                  + "' where userNo='" + dgv_SysUser.SelectedCells[0].Value.ToString().Trim()
                                  + "'; update SysUserRole set roleNo='"+cbo_UserRole.SelectedValue.ToString().Trim()
                                  +"' where where userNo='" + dgv_SysUser.SelectedCells[0].Value.ToString().Trim()+"'";

                        if (HDIC_DB.ExcuteNonQuery(sqlstr, null) > 0)
                        {
                            HDIC_Message.ShowInfoDialog(this, "修改用户成功");
                            bindDgv();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        HDIC_Message.ShowInfoDialog(this, ex.ToString());
                        bindDgv();
                    }
            }
        }

        private void btn_EditHide_Click(object sender, EventArgs e)
        {
            panel_Edit.Visible = false;
        }

#endregion

#region Query
        private void tsb_Query_Click(object sender, EventArgs e)
        {
            panel_Query.Visible = true;
        }

        private void btn_Query_Click(object sender, EventArgs e)
        {
            bindDgv();
        }

        private void btn_QueryHidden_Click(object sender, EventArgs e)
        {
            panel_Query.Visible = false;
        }


#endregion
        private void tsb_Delete_Click(object sender, EventArgs e)
        {

        }

        private void initCtrols()
        {
            panel_Edit.Visible = false;
            panel_Query.Visible = false;
            txt_search.Text = "";
            txt_UserName.Text = "";
            txt_UserPwd.Text = "";
            txt_UserPwdAgain.Text = "";

            txt_Remark.Text = "";

            txt_UserName.Enabled = true;
            sign = "";
        }

        private bool chekPara()
        {
            if (txt_UserName.Text.Trim() == "")
            {
                HDIC_Message.ShowWarnDialog(this, "用户名不能为空");
                return false;
            }
            if (txt_UserPwd.Text.Trim() == "")
            {
                HDIC_Message.ShowWarnDialog(this, "用户密码不能为空");
                return false;
            }
            if (txt_UserPwd.Text.Trim() != txt_UserPwdAgain.Text.Trim())
            {
                HDIC_Message.ShowWarnDialog(this, "两次输入的用户密码不一致");
                return false;
            }
            if (cbo_UserRole.SelectedIndex == -1)
            {
                HDIC_Message.ShowWarnDialog(this, "用户权限不能为空");
                return false;
            }
            return true;
        }

        private void bindDgv()
        {
            string sqlstr = @"select a.userNo,a.userName,c.roleName,a.remark,a.cDate from SysUser as a inner join SysUserRole as b on a.userNo=b.userNo
                            inner join SysRole as c on b.roleNo=c.roleNo  where 1=1";
            if (txt_search.Text.Trim() != "")
            {
                sqlstr += " and userName='" + txt_search.Text.Trim() + "'";
            }
            sqlstr += " order by a.cDate";

            dgv_SysUser.DataSource = HDIC_DB.GetList(sqlstr);
            dgv_SysUser.Columns[0].Visible = false;//第0行是userNo
        }

        private void tsb_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
