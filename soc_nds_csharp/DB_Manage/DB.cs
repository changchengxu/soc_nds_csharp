using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using HDICSoft.Message;
using System.Configuration;

namespace soc_nds_csharp.DB_Manage
{
    public partial class DB : Form
    {
        string dataBaseName = "STBInfo";
        string path = Application.StartupPath.Substring(0, Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf('\\')).LastIndexOf('\\'));
        public DB()
        {
            InitializeComponent();
        }

        #region 分离数据库
        private void btn_detach_Click(object sender, EventArgs e)
        {
            using (SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringName"].ConnectionString))
            {
                using (SqlCommand Comm = new SqlCommand())//命令
                {
                    try
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

                        Comm.Parameters[@"dbname"].Value = dataBaseName;
                        Comm.CommandType = CommandType.StoredProcedure;
                        Comm.ExecuteNonQuery();
                        HDIC_Message.ShowInfoDialog(this, "分离数据库成功");

                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("分离失败:" + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }
        }
        #endregion

        #region 备份数据库
        private void btn_BackUpDB_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbDialog = new FolderBrowserDialog();
            if (fbDialog.ShowDialog() == DialogResult.OK)
            {
                string backupstr = "backup database " + dataBaseName + " to disk='" + fbDialog.SelectedPath + "\\"+dataBaseName+".bak';";

                using (SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringName"].ConnectionString))
                {
                    using (SqlCommand Comm = new SqlCommand(backupstr, Conn))//命令
                    {
                        try
                        {
                            Conn.Open();
                            Comm.ExecuteNonQuery();
                            HDIC_Message.ShowInfoDialog(this, "数据库备份成功");

                        }
                        catch (Exception ex)
                        {
                            HDIC_Message.ShowInfoDialog(this, "数据库备份失败\n" + ex.ToString());
                        }
                        finally
                        {
                            Conn.Close();
                        }
                    }
                }
            }
        }
        #endregion

        #region 还原数据库
        private void btn_Restore_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbDialog = new FolderBrowserDialog();
            if (fbDialog.ShowDialog() == DialogResult.OK)
            {
                string restore = "restore database " + dataBaseName + " from disk='" + fbDialog.SelectedPath + "';";
                using (SqlConnection Conn = new SqlConnection())
                {
                    using (SqlCommand Comm = new SqlCommand())//命令
                    {
                        try
                        {
                            Conn.ConnectionString = "Data Source=.;Initial Catalog=master;Persist Security Info=False;User ID=sa; pwd =sa";

                            
                            Comm.Connection = Conn;
                            Conn.Open();

                            string strSQL = "select   spid   from   master..sysprocesses   where   dbid=db_id( '" + dataBaseName + "') ";//获取所有用户进程 
                            SqlDataAdapter   Da=new   SqlDataAdapter(strSQL,   Conn); 
                            DataTable   spidTable=new   DataTable(); 
                            Da.Fill(spidTable);

                            Comm.CommandType = CommandType.Text;
                            for (int iRow = 0; iRow <= spidTable.Rows.Count - 1; iRow++)
                            {
                                Comm.CommandText = "kill   " + spidTable.Rows[iRow][0].ToString();   //强行关闭用户进程 
                                Comm.ExecuteNonQuery();
                            }
                            //上面是：先杀所用访问该数据库进程




                            //-------------------------


                           //Comm.ExecuteNonQuery();

                           //Conn.ConnectionString = ConfigurationManager.ConnectionStrings["connStringName"].ConnectionString;
                           //Comm.Connection = Conn;
                            Comm.CommandText = restore;
                            Comm.ExecuteNonQuery();
                            HDIC_Message.ShowInfoDialog(this, "恢复成功");

                        }
                        catch (Exception ex)
                        {
                            HDIC_Message.ShowInfoDialog(this, "恢复失败\n" + ex.ToString());
                        }
                        finally
                        {
                            Conn.Close();
                        }
                    }
                }
            }
        }
        #endregion

        private void tsb_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
