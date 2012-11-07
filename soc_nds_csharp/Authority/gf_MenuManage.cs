using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.Message;
using HDICSoft.DB;
using HDICSoft.Command;

namespace soc_nds_csharp.Authority
{
    public partial class gf_MenuManage : Form
    {
        string path = Application.StartupPath;
           
        public gf_MenuManage()
        {
            InitializeComponent();
        }

        private void gf_MenuManage_Load(object sender, EventArgs e)
        {
            init();
            bindDgv();
            txt_subMenu.BackColor = HDIC_Command.setColor();
            dgv_Menu.BackgroundColor = HDIC_Command.setColor();
            panel_addMenu.BackColor = HDIC_Command.setColor();
            groupBox1.BackColor = HDIC_Command.setColor();

            path = path.Substring(0,path.Substring(0, path.LastIndexOf("\\")).LastIndexOf("\\"));

        }

        private void init()
        {
            panel_addMenu.Visible = false;
            txt_subMenu.Text = "";

            cbo_Menu.DataSource = HDIC_DB.GetList("select distinct menuNo,menuName from SysMenuDisplay where isModel=1");
            cbo_Menu.DisplayMember = "menuName";
            cbo_Menu.ValueMember = "menuNo";
            cbo_Menu.SelectedIndex = -1;
        }

        private void bindDgv()
        {
            dgv_Menu.DataSource = HDIC_DB.GetList("select * from SysMenuDisplay order by menuNo");
        }

        private bool checkPara()
        {
            if (txt_subMenu.Text.Trim() == "")
            {
                HDIC_Message.ShowWarnDialog(this, "没有选择子菜单");
                return false;
            }
            if (cbo_Menu.SelectedIndex==-1)
            {
                HDIC_Message.ShowWarnDialog(this, "没有选择主菜单");
                return false;
            }
            if (HDIC_DB.sqlQuery("select count(menuNo) from SysMenuDisplay where formName='" + txt_subMenu.Text.Trim() + "'") != "0")
            {
                HDIC_Message.ShowWarnDialog(this, "该菜单已经存在，请重新添加新菜单");
                return false;
            }
            return true;
        }

        string addr = "";
        private void btn_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog1 =new OpenFileDialog();
            OpenFileDialog1.InitialDirectory=path;
            OpenFileDialog1.Filter = "(c#程序)*.cs|*.cs";
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    addr = OpenFileDialog1.FileName;
                    addr = addr.Substring(addr.LastIndexOf("soc_nds_csharp"));
                    txt_subMenu.Text = addr.Substring(0,addr.LastIndexOf("."));
                    txt_subMenu.Text = txt_subMenu.Text.Replace("\\", ".");
                }
                catch (System.Exception ex)
                {
                    HDIC_Message.ShowWarnDialog(this, ex.ToString());
                }
            }
        }

        #region Panel
        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (!checkPara())
            {
                return;
            }
            //获取ID
            string sqlstr = "select max(menuNo) from SysMenuDisplay where menuNo like '" + cbo_Menu.SelectedValue.ToString().Trim() + "%'";
            string menuNoID =HDIC_DB.sqlQuery(sqlstr).Trim();
            if (menuNoID.Length == 2)
            {
                menuNoID = menuNoID + "01";
            }
            else
            {
                string temp = (Convert.ToInt32(menuNoID.Substring(2, menuNoID.Length-2)) + 1).ToString();
                if (temp.Length == 1)
                {
                    menuNoID = menuNoID.Substring(0, 2) + temp.PadLeft(temp.Length+1, '0');
                }
                else
                {
                    menuNoID = menuNoID.Substring(0, 2) + temp.PadLeft(temp.Length, '0');
                }
            }

            //获取窗体的Text值
            string formName = "";
            try
            {
                Type fType = Type.GetType(txt_subMenu.Text);
                Form f = (Form)fType.InvokeMember("乱写", System.Reflection.BindingFlags.CreateInstance, null, null, null);
                formName = f.Text;
            }
            catch (System.Exception ex)
            {
                HDIC_Message.ShowWarnDialog(this, "  添加的不是窗体文件，请重新添加 .\n"+ex.ToString());
                return;
            }

            //sql语句添加
            sqlstr = @"insert into SysMenuDisplay values('" + menuNoID + "','" + formName + "','" + txt_subMenu.Text.Trim() + "',null,'" +
                    cbo_Menu.SelectedValue.ToString().Trim()+"',0)";
            if (HDIC_DB.ExcuteNonQuery(sqlstr, null) > 0)
            {
                HDIC_Message.ShowInfoDialog(this, "添加成功");
                init();
                bindDgv(); 
            }

        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            init();
        }
        #endregion

        #region Menu
        private void tsb_Add_Click(object sender, EventArgs e)
        {
            init();
            panel_addMenu.Visible = true;
        }

        private void tsb_Delete_Click(object sender, EventArgs e)
        {
            init();
            if (dgv_Menu.Rows.Count > 0)
            {
                if (dgv_Menu.CurrentRow.Index > -1)
                {
                    if (HDIC_DB.sqlQuery("select count(menuNo) from dbo.SysMenuDisplay where isModel=1 and menuNo='" + dgv_Menu.CurrentRow.Cells["menuNo"].Value.ToString().Trim() + "'") != "0")
                    {
                        HDIC_Message.ShowWarnDialog(this, "不能删除主菜单");
                    }
                    else
                    {
                        if (HDIC_Message.ShowQuestionDialog(this, "确定要删除：《" + dgv_Menu.CurrentRow.Cells["menuName"].Value.ToString().Trim() + "》吗？") == DialogResult.OK)
                        {
                            if (HDIC_DB.ExcuteNonQuery("delete SysMenuDisplay where menuNo='" + dgv_Menu.CurrentRow.Cells["menuNo"].Value.ToString().Trim() + "'", null) > 0)
                            {
                                HDIC_Message.ShowInfoDialog(this, "删除成功");
                                bindDgv();
                            }
                        }
                    }
                }
            }
        }

        private void tsb_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
