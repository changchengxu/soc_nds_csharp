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

                string a =HDIC_Func.GetRunningPath();
                HDIC_Func.SetValue("connStringName", str);

            }
            try
            {
                HDIC_DB.GetList("select * from SysUser");

                DialogResult = DialogResult.OK;

            }
            catch
            {
                #region 附加数据库
                try
                {
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
                catch (Exception ex)
                {
                    MessageBox.Show("附加失败:" + ex.Message);
                }
                #endregion
            }
        }

        #region 添加自定义控件
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
