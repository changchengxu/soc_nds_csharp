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
    public partial class gf_SysRoleAuthority : Form
    {
        public string roleno;
        public string rolename;
        DataTable dt;

        public gf_SysRoleAuthority()
        {
            InitializeComponent();

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

    }
}
