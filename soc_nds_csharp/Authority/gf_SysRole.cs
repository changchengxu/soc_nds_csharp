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

namespace soc_nds_csharp.Authority
{
    public partial class gf_SysRole : Form
    {
        public gf_SysRole()
        {
            InitializeComponent();
        }

        private void gf_SysRole_Load(object sender, EventArgs e)
        {
            panel_Query.Visible = false;
            panel_Edit.Visible = false;
            txt_search.Text = "";
        }


        #region add and edit
        //菜单栏 添加
        private void tsbAdd_Click(object sender, EventArgs e)
        {
            sign = "add";
            panel_Edit.Visible = true;
            panel_Query.Visible = false;
        }

        //菜单栏 修改
        string oldRoleName = "";
        private void tsbModify_Click(object sender, EventArgs e)
        {
            sign = "edit";
            panel_Edit.Visible = true;
            panel_Query.Visible = false;

            if (dgv_SysRole.Rows.Count > 0)
            {
                if (dgv_SysRole.CurrentRow.Index > -1)
                {
                    txtroleName.Text = dgv_SysRole.CurrentRow.Cells["roleName"].Value.ToString();
                    txtremark.Text = dgv_SysRole.CurrentRow.Cells["remark"].Value.ToString();
                    oldRoleName = txtroleName.Text;
                }
            }

        }

        //保存数据
        string sign = "";
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (sign == "add")
            {
                if (HDIC_Message.ShowQuestionDialog(this, "确定要添加权限名称为：" + txtroleName.Text + " 的信息吗？") == DialogResult.Cancel)
                {
                    return;
                }
                if (!CheckPara())
                {
                    return;
                }
                HDIC_DB.ExcuteNonQuery("insert into SysRole(roleNo,roleName,remark) values('"+HDIC_Func.CreateKeyStr("RO")+"','" + txtroleName.Text.Trim() + "','" + txtremark.Text.Trim() + "')", null);
                HDIC_Message.ShowInfoDialog(this, "添加成功");
                bindDgv();
            }

            else if (sign == "edit")
            {
                if (HDIC_Message.ShowQuestionDialog(this, "确定要修改权限名称为：" + txtroleName.Text + " 的信息吗？") == DialogResult.Cancel)
                {
                    return;
                }
                if (!CheckPara())
                {
                    return;
                }
                if (HDIC_DB.ExcuteNonQuery("delete SysRole where roleName='" + oldRoleName + "';insert into SysRole(roleNo,roleName,remark) values('" + HDIC_Func.CreateKeyStr("RO") + "','" + txtroleName.Text.Trim() + "','" + txtremark.Text.Trim() + "')", null) > 0)
                {
                    HDIC_Message.ShowInfoDialog(this, "修改成功");
                    bindDgv();
                }

            }
        }
        //隐藏编辑面板
        private void btn_EditHide_Click(object sender, EventArgs e)
        {
            panel_Edit.Visible = false;
        }
        //添加或修改前 检查参数
        private bool CheckPara()
        {
            if (txtroleName.Text.Trim() == "")
            {
                HDIC_Message.ShowWarnDialog(this, "请填写权限名称");
                return false;
            }

            string a = HDIC_DB.sqlQuery("select count(roleName) from SysRole where roleName='" + txtroleName.Text.Trim() + "'");
            if (a != "0")
            {
                HDIC_Message.ShowWarnDialog(this, "您填写的权限名称已经存在，请重新填写");
                return false;
            }
            return true;
        }
        //添加或修改后 绑定数据
        private void bindDgv()
        {
            txtroleName.Text = "";
            txtremark.Text = "";
            btnQuery_Click(null, null);//bind to dataGridView
            panel_Edit.Visible = false;
            panel_Query.Visible = false;
        }
        #endregion

        #region Delete
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            //删除两个表
            string sqlstr = @"delete SysRole where roleName='" + dgv_SysRole.CurrentRow.Cells["roleName"].Value.ToString()
                         + "';delete SysRoleMenu where roleNo='" + dgv_SysRole.CurrentRow.Cells["roleNo"].Value.ToString() + "'";

            if (HDIC_Message.ShowQuestionDialog(this, "确定要删除权限名称为：<" + dgv_SysRole.CurrentRow.Cells["roleName"].Value.ToString().Trim() + "> 的信息吗？") == DialogResult.Cancel)
            {
                return;
            }
            if (HDIC_DB.ExcuteNonQuery(sqlstr, null) > 0)
            {
                HDIC_Message.ShowInfoDialog(this, "删除成功");
                bindDgv();
            }
        }
        #endregion

        #region Query
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            panel_Query.Visible = true;
            panel_Edit.Visible = false;
        }

        private void btn_QueryHidden_Click(object sender, EventArgs e)
        {
            panel_Query.Visible = false;
            txt_search.Text = "";
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string sqlstr = "select * from SysRole where 1=1";
            if (txt_search.Text.Trim() != "")
            {
                sqlstr += " and roleName='" + txt_search.Text.Trim() + "'";
            }
            dgv_SysRole.DataSource = HDIC_DB.GetList(sqlstr);
        }

        #endregion

        #region Role Authority
        private void tsbSelectOpt_Click(object sender, EventArgs e)
        {
            if (dgv_SysRole.Rows.Count > 0)
            {
                if (dgv_SysRole.CurrentRow.Index > -1)
                {
                    gf_SysRoleAuthority roleAuth = new gf_SysRoleAuthority();
                    roleAuth.rolename = dgv_SysRole.CurrentRow.Cells["roleName"].Value.ToString().Trim();
                    roleAuth.roleno = dgv_SysRole.CurrentRow.Cells["roleNo"].Value.ToString().Trim();
                    roleAuth.ShowDialog();
                }
                else
                {
                    HDIC_Message.ShowWarnDialog(this, "请选择相应的权限角色");
                }
            }
            else
            {
                HDIC_Message.ShowWarnDialog(this, "没有可供选择的权限角色");
            }
        }
        #endregion

    }
}
