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
                            Comm.Parameters.Add("@dbname", "STBInfo");                             //这个是数据库名字，也就是你写什么名字附加到数据库就是什么名字。
                            Comm.Parameters.Add("@filename1", path + @"\App_Data\STBInfo.mdf");   //地址
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

        private void gf_login_Load(object sender, EventArgs e)
        {

        }

        private void btn_detach_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection Conn = new SqlConnection("Data Source=.;Persist Security Info=False;User ID=sa; pwd =sa"))
                {
                    using (SqlCommand Comm = new SqlCommand())//命令
                    {
                        Conn.Open();
                        Comm.Connection = Conn;
                        Comm.CommandText = @"USE master;
                                           ALTER DATABASE STBInfo
                                           SET SINGLE_USER
                                           with ROLLBACK IMMEDIATE";
                        //上面的目的是强制断开用户
                        Comm.ExecuteNonQuery();

                        Comm.CommandText = @"sp_detach_db";
                        Comm.Parameters.Add(new SqlParameter(@"dbname", SqlDbType.NVarChar));

                        Comm.Parameters[@"dbname"].Value = "STBInfo";
                        Comm.CommandType = CommandType.StoredProcedure;
                        Comm.ExecuteNonQuery();
                       HDIC_Message.ShowInfoDialog(this,"分离数据库成功");
                        Conn.Close();


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("分离失败:" + ex.Message);
            }

        }
    }
}
