//************************************************************************
//版权信息：版权所有(C) 2012，HDICSoft Corporation
//公司名称：上海高清
//系统名称：soc_nds
//模块名称：
//作    者：薛长城
//创建时间：2012-10-28
//修改时间：
//修改内容： 本机器IP地址是192.168.21.1 或者 192.168.223.1
//************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using HDICSoft.DB;
using HDICSoft.Message;
using System.Data.SqlClient;
using System.Configuration;
using HDICSoft.Func;
using HDICSoft.Command;

namespace soc_nds_csharp
{
    public partial class gf_login : Form
    {
        string path = Application.StartupPath .Substring(0,Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf('\\')).LastIndexOf('\\'));

        public gf_login()
        {
            InitializeComponent();
            //txt_username.Region = new Region(GetRoundRectPath(new RectangleF(0, 0, this.txt_username.Width, this.txt_username.Height), 10f));
            //txt_pwd.Region = new Region(GetRoundRectPath(new RectangleF(0, 0, this.txt_username.Width, this.txt_username.Height), 10f));
            gbx_NetWork.Visible = false;
        }

        private void gf_login_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = HDIC_DB.GetList("select distinct roleNo,roleName from SysRole");
                cbo_userRole.DataSource = dt;
                cbo_userRole.DisplayMember = "roleName";
                cbo_userRole.ValueMember = "roleNo";
                cbo_userRole.SelectedIndex = -1;

                cbo_userRole.Focus();

            }
            catch (System.Exception ex)
            {
                #region 附加数据库
                try
                {
                    //SqlParameter[] para={new SqlParameter("@dbname",SqlDbType.NVarChar),
                    //                     new SqlParameter("@filename1",SqlDbType.NVarChar),
                    //                     new SqlParameter("@filename2",SqlDbType.NVarChar),
                    //};
                    //para[0].Value = "STBInfo";
                    //para[1].Value = path + @"\App_Data\STBInfo.mdf";
                    //para[2].Value = path + @"\App_Data\STBInfo_log.ldf";
                    // HDIC_DB.ExcuteNonQuery(CommandType.StoredProcedure,"sp_attach_db",para);
                    //数据库连接对象
                    using (SqlConnection Conn = new SqlConnection("Data Source=.;Persist Security Info=False;User ID=sa; pwd =sa"))
                    {
                        using (SqlCommand Comm = new SqlCommand())//命令
                        {
                            Comm.CommandText = "sp_attach_db";
                            Comm.CommandType = CommandType.StoredProcedure;                         //引入存储过程
                            Comm.Parameters.AddWithValue("@dbname", "STBInfo");                             //这个是数据库名字，也就是你写什么名字附加到数据库就是什么名字。
                            Comm.Parameters.AddWithValue("@filename1", path + @"\App_Data\STBInfo.mdf");   //地址
                            Comm.Parameters.AddWithValue("@filename2", path + @"\App_Data\STBInfo_log.ldf");
                            Comm.Connection = Conn;    //初始化连接对象
                            Conn.Open();   //打开连接对象
                            Comm.ExecuteNonQuery();
                            Conn.Close();   //关闭连接对象
                        }
                    }
                }
                catch 
                {
                    MessageBox.Show("附加失败:" + ex.Message);
                }

                btn_login_Click(sender, e);
                ipUserControl.Text = "";

                #endregion
            }
        }


        #region 文本框圆角
        public GraphicsPath GetRoundRectPath(RectangleF rect, float radius)
        {
            return GetRoundRectPath(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public GraphicsPath GetRoundRectPath(float X, float Y, float width, float height, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(X + radius, Y, (X + width) - (radius * 2f), Y);
            path.AddArc((X + width) - (radius * 2f), Y, radius * 2f, radius * 2f, 270f, 90f);
            path.AddLine((float)(X + width), (float)(Y + radius), (float)(X + width), (float)((Y + height) - (radius * 2f)));
            path.AddArc((float)((X + width) - (radius * 2f)), (float)((Y + height) - (radius * 2f)), (float)(radius * 2f), (float)(radius * 2f), 0f, 90f);
            path.AddLine((float)((X + width) - (radius * 2f)), (float)(Y + height), (float)(X + radius), (float)(Y + height));
            path.AddArc(X, (Y + height) - (radius * 2f), radius * 2f, radius * 2f, 90f, 90f);
            path.AddLine(X, (Y + height) - (radius * 2f), X, Y + radius);
            path.AddArc(X, Y, radius * 2f, radius * 2f, 180f, 90f);
            path.CloseFigure();
            return path;
        }

        #endregion

        private void btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            if (ipUserControl!=null)
            {
                string str = "Data Source=" + ipUserControl.Text + ";Initial Catalog=STBInfo;Persist Security Info=False;User ID=sa; pwd =sa";
                HDIC_Func.SetValue("connStringName", str);

            }
            try
            {
                if (!CheckPara())
                {
                    return;
                }
                string sqlstr = @"select a.userNo,a.userName,b.roleNo from SysUser as a inner join SysUserRole as b on a.userNo=b.userNo 
                                   where a.userName='" + txt_username.Text.Trim() + "' and a.pwd='" + txt_pwd.Text.Trim()
                                    + "' and b.roleNo='"+cbo_userRole.SelectedValue.ToString().Trim()+"'";

                using (DataTable dt =HDIC_DB.GetList(sqlstr))
                {
                    if (dt.Rows.Count <= 0)
                    {
                        cbo_userRole.SelectedIndex = -1;
                        txt_username.Text = "";
                        txt_pwd.Text = "";

                        cbo_userRole.Focus();
                        HDIC_Message.ShowWarnDialog(this, "输入信息不正确，请重新输入");
                        return;
                    }
                    else
                    {
                        HDIC_Command.UseName = dt.Rows[0]["userName"].ToString().Trim();
                        HDIC_Command.userNo = dt.Rows[0]["userNo"].ToString().Trim();
                        HDIC_Command.roleNo = dt.Rows[0]["roleNo"].ToString().Trim();
                       
                        DialogResult = DialogResult.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                HDIC_Message.ShowWarnDialog(this, ex.ToString());
            }
        }

        private bool CheckPara()
        {
            if (cbo_userRole.SelectedValue == null)
            {
                HDIC_Message.ShowWarnDialog(this, "用户类型不能为空！");
                return false;
            }
            if (txt_username.Text.Trim() == "")
            {
                HDIC_Message.ShowWarnDialog(this, "用户名不能为空！");
                return false;
            }
            if (txt_pwd.Text.Trim() == "")
            {
                HDIC_Message.ShowWarnDialog(this, "密码不能为空！");
                return false;
            }
            return true;
        }

        #region 添加自定义 IP 控件
        private IPValidateControl ipUserControl;
        private void btn_network_Click(object sender, EventArgs e)
        {
            ipUserControl = new IPValidateControl();
            gbx_NetWork.Controls.Add(ipUserControl);

            ipUserControl.Location = new System.Drawing.Point(lbl_IP.Location.X + 35, lbl_IP.Location.Y-8);
            this.lbl_IP.Name = "IPmyControl";

            gbx_NetWork.Visible = true;
        }
        #endregion

    }
}
