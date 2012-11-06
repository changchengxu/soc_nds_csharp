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
    public partial class gf_SysRoleAuthority : Form
    {
        public string roleno;
        public string rolename;
        DataTable dt;

        public gf_SysRoleAuthority()
        {
            InitializeComponent();
        }

        private void gf_SysRoleAuthority_Load(object sender, EventArgs e)
        {
            tvRoleOperation.BackColor = HDIC_Command.setColor();

            lbl_roleName.Text = rolename;

            dt = HDIC_DB.GetList("select menuNo,menuName,parentNo from SysMenuDisplay  order by menuNo");
            AddTree("", (TreeNode)null);
            tvRoleOperation.Nodes[0].Expand();

        }


        public void AddTree(string ParentID, TreeNode pNode)
        {
            try
            {
                DataView dvTree = new DataView(dt);
                //过滤ParentID,得到当前的所有子节点
                dvTree.RowFilter = "parentNo= '" + ParentID + "'";
                foreach (DataRowView Row in dvTree)
                {
                    if (pNode == null)
                    {    //添加根节点
                        TreeNode Node = new TreeNode();
                        Node.Text = Row["menuName"].ToString();
                        Node.Tag = Row["menuNo"].ToString();
                        tvRoleOperation.Nodes.Add(Node);
                        AddTree(Row["menuNo"].ToString(), Node);    //再次递归
                    }
                    else
                    {   //添加当前节点的子节点
                        TreeNode Node = new TreeNode();
                        Node.Text = Row["menuName"].ToString();
                        Node.Tag = Row["menuNo"].ToString();
                        pNode.Nodes.Add(Node);
                        AddTree(Row["menuNo"].ToString(), Node);     //再次递归                   
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void tvRoleOperation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bindDgv();
        }

        #region 绑定
        bool sign = false;// 如果 false 说明SysRoleMenu没有该数据，如果true表明有该数据。
        private void bindDgv()
        {
            try
            {
                string sqlstr = @"select SysRoleMenu.isSelect as '选择',SysMenuDisplay.menuNo as '菜单编号',SysMenuDisplay.menuName as '菜单名称'from SysRoleMenu inner join SysMenuDisplay 
                                on SysRoleMenu.muOpt=SysMenuDisplay.menuNo  where 
                  SysRoleMenu.roleNo='" + roleno + "'and SysMenuDisplay.menuNo='" + tvRoleOperation.SelectedNode.Tag.ToString() + "'";
                using (DataTable dt = HDIC_DB.GetList(sqlstr))
                {
                    dgvRoleOperation.DataSource = null;
                    //dgvRoleOperation.Columns.Clear();
                    dgvRoleOperation.Refresh();

                    if (dt.Rows.Count > 0)
                    {
                        //dgvRoleOperation.AutoGenerateColumns = false;
                        dgvRoleOperation.DataSource = dt;
                        sign = true;
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr["选择"] = 0;
                        dr["菜单编号"] = tvRoleOperation.SelectedNode.Tag.ToString();
                        dr["菜单名称"] = tvRoleOperation.SelectedNode.Text.ToString();
                        dt.Rows.Add(dr);
                        dgvRoleOperation.DataSource = dt;
                        sign = false;
                    }

                    //检索是不是菜单，如果是则将选择框设置隐藏。
                    if (HDIC_DB.sqlQuery("select count(*) from SysMenuDisplay where isModel=1 and menuNo='" + tvRoleOperation.SelectedNode.Tag.ToString() + "'") != "0")
                    {
                        //dgvRoleOperation.Rows[0].Cells["isSelect"].ReadOnly = false;
                        dgvRoleOperation.Columns["选择"].Visible = false;
                    }
                    else
                    {
                        dgvRoleOperation.Columns["选择"].Visible = true;
                    }

                    //单元格内容居中显示
                    for (int j = 0; j < dgvRoleOperation.Columns.Count; j++)
                    {
                        dgvRoleOperation.Rows[0].Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }

            catch (Exception ex)
            {
                HDIC_Message.ShowErrorDialog(this, ex.Message);
            }
        }
        #endregion

        #region 提交
        private void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlstr = "";

                if (sign)//SysRoleMenu 表中存在该数据时
                {
                    if (Convert.ToInt32(dgvRoleOperation.Rows[0].Cells["选择"].Value) == 1)//如果选中 直接修改isSelect为true
                    {
                        sqlstr = @"update SysRoleMenu set isSelect=1
                               where roleNo='" + roleno + "' and muOpt='" + tvRoleOperation.SelectedNode.Tag.ToString() + "'";
                    }
                    else  //如果取消选中 直接删除该行数据。为了避免以后庞大的冗余影响检索速度
                    {
                        sqlstr = @"delete SysRoleMenu 
                                  where roleNo='" + roleno + "' and muOpt='" + tvRoleOperation.SelectedNode.Tag.ToString() + "'";
                    }

                    HDIC_DB.ExcuteNonQuery(sqlstr, null);
                }
                else   //SysRoleMenu 表中不存在该数据时
                {
                    string sql = @"select SysRoleMenu.isSelect as '选择',SysMenuDisplay.menuNo as '菜单编号',SysMenuDisplay.menuName as '菜单名称'from SysRoleMenu inner join SysMenuDisplay 
                                on SysRoleMenu.muOpt=SysMenuDisplay.menuNo  where 
                                 SysRoleMenu.roleNo='" + roleno + "'and SysMenuDisplay.menuNo='" + tvRoleOperation.SelectedNode.Tag.ToString() + "'";
                    using (DataTable dt = HDIC_DB.GetList(sql))
                    {
                        if (dt.Rows.Count == 0)//避免多次点击提交
                        {
                            sqlstr = @"insert into SysRoleMenu(roleNo,muOpt,isSelect) values('" + roleno + "','" + tvRoleOperation.SelectedNode.Tag.ToString().Trim() + "','" + Convert.ToInt32(dgvRoleOperation.Rows[0].Cells["选择"].Value) + "')";

                            HDIC_DB.ExcuteNonQuery(sqlstr, null);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                HDIC_Message.ShowErrorDialog(this, ex.ToString());
            }
        }

#endregion

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
