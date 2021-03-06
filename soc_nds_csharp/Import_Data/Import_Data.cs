﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HDICSoft.Message;
using HDICSoft.DB;
using HDICSoft.Command;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;

namespace soc_nds_csharp.Import_Data
{
    public partial class Import_Data : Form
    {
        DataTable DtContent=new DataTable();

        public Import_Data()
        {
            InitializeComponent();
            tableLayoutPanel1.BackColor = HDIC_Command.setColor();

            DtContent.Columns.Add("ChipID", System.Type.GetType("System.String"));
            DtContent.Columns.Add("ChipInfo", System.Type.GetType("System.String"));

        }

        #region 自己修改了网上的导出txt数据到SQL中(一行行，暂时没用)
        ///// <summary> 
        ///// 通用函数,读文本文件 
        ///// </summary> 
        ///// <param name="fileName">读入的文本文件名称</param> 
        //public  void ReadTextFromFileName(string fileName)
        //{
        //    string strRecord = "";

        //    //读入文本文件时,一定要指定文件的编码格式.其中:default为文本文件本来的编码格式 
        //    //如果是简体中文的文本文件,也可以这样设置编码格式: System.Text.Encoding.GetEncode("gb2312") 
        //    //Encoding.GetEncode("gb2312")为简体中文编码格式,Encoding.GetEncode("big5")为繁体中文编码格式. 
        //    StreamReader reader = new StreamReader(fileName, System.Text.Encoding.Default);

        //    //i is the really row 
        //    //j is the row of writed to database 
        //    int i, j;
        //    i = 0;//行
        //    j = 0;//列
        //    try
        //    {
        //        string[] source;
        //        string[] result ;
        //        string split = " ";
        //        while (reader.Peek() >= 0)
        //        {
        //            strRecord = reader.ReadLine();
        //            source = System.Text.RegularExpressions.Regex.Split(strRecord,split);
        //            result = new string[source.Length];
        //            for (int index = 0; index < source.Length; index++)
        //            {
        //                result[index] = StringConvertByteArray(source[index]);
        //                    j++;
        //            }
        //            i++;
        //            try
        //            {
        //                DataRow dr = DtContent.NewRow();
        //                dr[0] = result[0];
        //                dr[1] = result[1];
        //                DtContent.Rows.Add(dr);
        //               // HDIC_DB.ExcuteNonQuery(@"insert into ChipData(ChipID,ChipInfo) values ('" + result[0] + "','" + result[1] + "')", null);
        //            }
        //            catch (System.Exception ex)
        //            {
        //                HDIC_Message.ShowWarnDialog(this, "数据库打开失败,请检查服务器或者网络");
        //            }
                   
        //        }
        //        HDIC_Message.ShowInfoDialog(this, "数据导入成功:"+DtContent.Rows.Count.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        HDIC_Message.ShowErrorDialog(this, "文件:" + fileName + "导入失败.错误在" + i.ToString() + "行 " + j.ToString() +"列,原因是: " + ex.Message);
        //        throw new Exception(ex.Message);
        //    }
        //          //相关资源的消除 
        //    finally
        //    {
        //        reader.Close();
        //    }
        //}

        ///// <summary> 
        ///// 处理定长文本文件的函数,将字符串转换成byte[]数组 
        ///// </summary> 
        ///// <param name="aRecord"></param> 
        //private  string StringConvertByteArray(string aRecord)
        //{
        //    //解决文本文件一行中可能存在中文的情况,将string类型转换为byte[]来达到 
        //    //正确处理文本文件的目的 
        //    byte[] repRecord = System.Text.Encoding.Default.GetBytes(aRecord);
        //   return GetString(repRecord);
        //}

        ////private static void    
        ///// <summary> 
        ///// 处理长度固定的文本文,读取到每个字段的值 
        ///// </summary> 
        ///// <param name="aStr">文本文件的每行文本转换的Byte数组</param> 
        ///// <returns>返回的字符串,对应于具体的字段值</returns> 
        //private static string GetString(byte[] aStr)
        //{
        //    byte[] tempStr = new byte[aStr.Length];
        //    for (int i = 0; i < aStr.Length; i++)
        //    {
        //        tempStr[i] = (byte)aStr.GetValue(i);
        //    }

        //    return System.Text.Encoding.Default.GetString(tempStr);
        //}

        #endregion

        Thread td = null;//导入数据库的线程
        /// <summary>
        /// import data to SQL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_add_Click(object sender, EventArgs e)
        {

            if (td != null && td.IsAlive)
            {
                HDIC_Message.ShowWarnDialog(null, "正在将数据导入到数据库，请等待……");
                return;
            }

            if (txt_path.Text.Trim() == "")
            {
                HDIC_Message.ShowErrorDialog(null, "请选择需要上传数据的文件");
                return;
            }

            td = new Thread(new ThreadStart(ImportData));
            td.SetApartmentState(ApartmentState.STA);
            td.IsBackground = true;
            td.Start();

           

            // ReadTextFromFileName(path);
        }


        private void ImportData()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringName"].ConnectionString))
            //using (SqlConnection conn = new SqlConnection("Data Source=192.168.16.9;Initial Catalog=STBInfo;Persist Security Info=False;User ID=sa; pwd =sa;"))

            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    string tt = ConfigurationManager.ConnectionStrings["connStringName"].ConnectionString;
                    cmd.Connection = conn;
                    cmd.CommandTimeout = 2000;//长城设置，15分钟即超时
                    cmd.CommandText = "BULK INSERT ChipData FROM '" + path + "' WITH (FIELDTERMINATOR = ' ',KEEPIDENTITY,ROWTERMINATOR='\r\n',BATCHSIZE = 100000)";

                    try
                    {
                        
                        Stopwatch timer = new Stopwatch();
                        timer.Start();
                        Int32 InsertCount=cmd.ExecuteNonQuery();
                        timer.Stop();
                        HDIC_Message.ShowInfoDialog(null, "数据导入成功,用时：" + timer.ElapsedMilliseconds.ToString() + "毫秒\r\n"+"导入数据个数:"+InsertCount+"条");
                        conn.Close();
                       
                    }
                    catch (System.Exception ex)
                    {
                        conn.Close();
                        HDIC_Message.ShowInfoDialog(null, "导入数据库失败，原因：\r\n" + ex.ToString());
                    }
                }
            }
        }

        string path; 
        private void btn_open_Click(object sender, EventArgs e)
        {
            if (td != null && td.IsAlive)
            {
                HDIC_Message.ShowWarnDialog(null, "正在将数据导入到数据库，请等待……");
                return;
            }

            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "打开";
            OpenFile.InitialDirectory = @"桌面";
            OpenFile.Filter = "文本文件(*.txt,*.pers)|*.txt;*.pers|文档文件（*.doc)|*.doc;*.docx)";
            DialogResult drResult = OpenFile.ShowDialog();
            if (drResult == DialogResult.OK)
                path = OpenFile.FileName;
            else
                return;

            txt_path.Text = path;
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            if (td != null && td.IsAlive)
            {
                HDIC_Message.ShowWarnDialog(null, "正在将数据导入到数据库，请等待……");
                return;
            }
            this.Close();
        }

        private void Import_Data_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (td != null && td.IsAlive)
            {
                HDIC_Message.ShowWarnDialog(null, "正在将数据导入到数据库，请等待……");
                e.Cancel = true;
                return;
            }
            e.Cancel = false;
        }
    }
}
