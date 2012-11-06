using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using XPExplorerBar;
using HDICSoft.DB;
using HDICSoft.Command;
using HDICSoft.Func;
using HDICSoft.Message;

namespace soc_nds_csharp
{
    public partial class gf_main : Form
    {
        private DataTable dt = null;
        public gf_main()
        {
            InitializeComponent();
        }

        private void gf_main_Load(object sender, EventArgs e)
        {
            this.BackColor = HDIC_Command.setColor();
            taskPane1.CollapseAll();
            using (gf_login login = new gf_login())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    //下面sql意思：通过用户权限编号在SysRoleMenu中获取 子菜单 和 主菜单
                      string sqlstr="";
                      if (HDIC_Command.UseName.ToLower().Trim() == "admin")//超级管理用户 可使用全部功能
                      {
                          sqlstr = "SELECT a.* FROM  SysMenuDisplay as a order by a.menuNo";
                      }
                      else
                      {
                          sqlstr = "SELECT a.* FROM  SysMenuDisplay as a " +
                        " where a.menuNo in (select muOpt from SysRoleMenu as b where  b.isSelect=1  and roleNo='" + HDIC_Command.roleNo + "')" +
                        "or a.menuNo in(select distinct left(muOpt,2) from SysRoleMenu as c where c.isSelect=1 and roleNo='" + HDIC_Command.roleNo + "')" +
                        " order by a.menuNo";
                      }
                 
                    dt = HDIC_DB.GetList(sqlstr);

                    taskPane1.Expandos.Clear();
                    AddExpando();
                }
                else
                {
                    Application.Exit();
                }

                toolStripStatusLabel1.Text="尊敬的<"+HDIC_Command.UseName+">用户，欢迎您登录直播星软件 | 今天的日期是："+DateTime.Now.ToString("yyyy-MM-dd");
            }

        }

        private void AddExpando()
        {
            dt.DefaultView.RowFilter = "isModel =1";
            for (int i = 0; i < dt.DefaultView.Count; i++)
            {
                Expando expando = new Expando();
                expando.ItemClick += new System.EventHandler(expando_ItemClick);
                expando.StateChanged += new ExpandoEventHandler(expando_StateChanged);
                expando.AutoLayout = true;
                expando.Name = dt.DefaultView[i]["picture"].ToString();
                expando.Tag = dt.DefaultView[i]["menuNo"];
                AddTaskItem(expando);
                expando.Collapsed = true;
                expando.CustomSettings.NormalBackColor = HDIC_Command.setColor();
                taskPane1.Expandos.Add(expando);
                dt.DefaultView.RowFilter = "isModel =1";
            }

        }

        //长城提示：添加点击事件
        private void expando_ItemClick(object sender, EventArgs e)
        {
            //pictureBox2.Visible = false;
            string MenuName = ((TaskItem)sender).Name;

            if (string.IsNullOrEmpty(MenuName)) return;
            //如果窗体已经创建，则将其激活，不再创建新事例
            foreach (Form form in this.MdiChildren)
            {
                if (form.GetType().FullName.ToLower() == MenuName.ToLower())
                {
                    form.Activate();
                    return;
                }
            }
            Form f;
            try
            {
                    f = (Form)Activator.CreateInstance(Type.GetType(MenuName));
                    f.Text = ((TaskItem)sender).Text;
            }
            catch
            {
                return;
            }
            f.TopLevel = false;
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }


        private void AddTaskItem(Expando ep)
        {
            dt.DefaultView.RowFilter = "parentNo = '" + ep.Tag.ToString() + "'";
            for (int i = 0; i < dt.DefaultView.Count; i++)
            {
                TaskItem taskItem = new TaskItem();
                taskItem.Name = dt.DefaultView[i]["formName"].ToString();
                taskItem.Text = dt.DefaultView[i]["menuName"].ToString();
                taskItem.Tag = dt.DefaultView[i]["menuNo"];
                //taskItem.Click += new EventHandler(taskItem_Click);
                ep.Items.Add(taskItem);
            }
        }


        private void expando_StateChanged(object sender, ExpandoEventArgs e)
        {
            SetExpandoCollapsed((Expando)sender);
            SetExpandoImage((Expando)sender, StringToBitmap(((Expando)sender).Name.ToString().Trim()), StringToBitmap(((Expando)sender).Name.ToString().Trim() + "1"));
            Expando expando = (Expando)sender;
            for (int i = 0; i < expando.Items.Count; i++)
                ((TaskItem)expando.Items[i]).Image = null;
        }


        private void SetExpandoCollapsed(Expando ExpandedExpando)
        {
            if (!ExpandedExpando.Collapsed)
            {
                foreach (Expando exp in taskPane1.Expandos)
                {
                    if (exp != ExpandedExpando)
                    {
                        exp.Collapsed = true;
                    }
                }
            }
        }

        private void SetExpandoImage(Expando exp, Bitmap collapsed, Bitmap expanded)
        {
            if (exp.Collapsed)
            {
                exp.CustomHeaderSettings.NormalBackImage = collapsed;
            }
            else
            {
                exp.CustomHeaderSettings.NormalBackImage = expanded;
            }
        }

        private Bitmap StringToBitmap(string str)
        {
            Bitmap map = null;

            str = str.Substring(str.LastIndexOf('\\') + 1);

            //switch (str.ToLower())
            switch (str)
            {
                case "account_management":
                    map = soc_nds_csharp.Properties.Resources.account_management;
                    break;
                case "account_management1":
                    map = soc_nds_csharp.Properties.Resources.account_management1;
                    break;
                case "Pipeline_Management":
                    map = soc_nds_csharp.Properties.Resources.Pipeline_Management;
                    break;
                case "Pipeline_Management1":
                    map = soc_nds_csharp.Properties.Resources.Pipeline_Management1;
                    break;
                case "Delete_database_records":
                    map = soc_nds_csharp.Properties.Resources.Delete_database_records;
                    break;
                case "Delete_database_records1":
                    map = soc_nds_csharp.Properties.Resources.Delete_database_records1;
                    break;
                case "Import_serialized_data":
                    map = soc_nds_csharp.Properties.Resources.Import_serialized_data;
                    break;
                case "Import_serialized_data1":
                    map = soc_nds_csharp.Properties.Resources.Import_serialized_data1;
                    break;
                case "Export_file":
                    map = soc_nds_csharp.Properties.Resources.Export_file;
                    break;
                case "Export_file1":
                    map = soc_nds_csharp.Properties.Resources.Export_file1;
                    break;
                case "Database_management":
                    map = soc_nds_csharp.Properties.Resources.Database_management;
                    break;
                case "Database_management1":
                    map = soc_nds_csharp.Properties.Resources.Database_management1;
                    break;
                case "Station_Operation":
                    map = soc_nds_csharp.Properties.Resources.Station_Operation;
                    break;
                case "Station_Operation1":
                    map = soc_nds_csharp.Properties.Resources.Station_Operation1;
                    break;
                default:
                    break;
            }
            return map;
        }

        private void tsm_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

#region Reset
        private void tsm_reset_Click(object sender, EventArgs e)
        {
            if (HDIC_Message.ShowQuestionDialog(this, "确定要重启程序吗？") == DialogResult.OK)
            {
                HDIC_Func.Reset();
            }
        }
#endregion

        private void gf_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (HDIC_Message.ShowQuestionDialog(this, "是否确定退出系统？") != DialogResult.OK);
        }


    }
}
