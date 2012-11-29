//************************************************************************
//版权信息：版权所有(C) 2012，HDICSoft Corporation
//公司名称：上海高清
//系统名称：soc_nds
//模块名称：
//作    者：薛长城
//创建时间：2012-10-28
//修改时间：
//修改内容：
//************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Data.Common;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using TSC;

namespace HDICSoft.Security
{
   public class MD5
    {
        /// <summary>
        /// MD5加密或者解密
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string Encrypt(string sourceString)
        {
            byte[] bytes = Encoding.Default.GetBytes(sourceString);
            bytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(bytes)).Replace("-", "");
        }

    }
}

namespace HDICSoft.DB
{
   public class HDIC_DB
    {

        //   private static readonly string connStringName = ConfigurationManager.AppSettings["connStringName"].ToString();
        private static readonly string connString = ConfigurationManager.ConnectionStrings["connStringName"].ConnectionString;
        private static readonly string providerName = ConfigurationManager.ConnectionStrings["connStringName"].ProviderName;
        public static DbProviderFactory provider = DbProviderFactories.GetFactory(providerName);

        #region Public Mothods

        public static int ExcuteNonQuery(string cmdText, params DbParameter[] cmdParms)
        {
            return ExcuteNonQuery(null, CommandType.Text, cmdText, cmdParms);
        }

        public static int ExcuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return ExcuteNonQuery(null, cmdType, cmdText, cmdParms);
        }

        public static int ExcuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            using (DbConnection conn = provider.CreateConnection())
            {
                using (DbCommand cmd = provider.CreateCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
                        //return cmd.ExecuteNonQuery();
                        int count = 0;               //长城添加
                        count = cmd.ExecuteNonQuery(); //长城添加
                        if (trans != null) { trans.Commit(); }           //长城添加 
                        return count;                //长城添加
                    }
                    catch (Exception ex)
                    {
                        if (trans != null) { trans.Rollback(); }//长城添加 
                        conn.Close();
                        cmd.Dispose();
                        throw new Exception("数据操作错误" + ex.ToString());
                    }
                }
            }
        }

        public static DbDataReader ExecuteReader(string cmdText, params DbParameter[] cmdParms)
        {
            return ExecuteReader(null, CommandType.Text, cmdText, cmdParms);
        }

        public static DbDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return ExecuteReader(null, cmdType, cmdText, cmdParms);
        }

        public static DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            //using (DbConnection conn = provider.CreateConnection())
            //{
            //    using (DbCommand cmd = provider.CreateCommand())
            //    {
            DbConnection conn = provider.CreateConnection();
            DbCommand cmd = provider.CreateCommand();
            try
            {
                PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                throw new Exception("数据操作错误" + ex.ToString());
            }
            //    }
            //}
        }

        public static object ExecuteScalar(string cmdText, params DbParameter[] cmdParms)
        {
            return ExecuteScalar(null, CommandType.Text, cmdText, cmdParms);
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return ExecuteScalar(null, cmdType, cmdText, cmdParms);
        }

        public static object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            using (DbConnection conn = provider.CreateConnection())
            {
                using (DbCommand cmd = provider.CreateCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
                        return cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        cmd.Dispose();
                        throw new Exception("数据操作错误" + ex.ToString());
                    }
                }
            }
        }

        public static DataSet ExcuteDataSet(string cmdText, params DbParameter[] cmdParms)
        {
            return ExcuteDataSet(null, CommandType.Text, cmdText, cmdParms);
        }
        public static DataSet ExcuteDataSet(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return ExcuteDataSet(null, cmdType, cmdText, cmdParms);
        }
        public static DataSet ExcuteDataSet(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            using (DbConnection conn = provider.CreateConnection())
            {
                using (DbCommand cmd = provider.CreateCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
                        using (DbDataAdapter da = provider.CreateDataAdapter())
                        {
                            da.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                da.Fill(ds);
                                return ds;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        cmd.Dispose();
                        throw new Exception("数据操作错误" + ex.ToString());
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

        /// <summary>
        /// get DataTable
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static DataTable GetList(string sqlString)
        {
            DataTable dt = ExcuteDataSet(sqlString, null).Tables[0];
            return dt;
        }

        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static int sqlDelete(string sqlStr)
        {
            int i = ExcuteNonQuery(sqlStr, null);
            return i;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static string sqlQuery(string sqlStr)
        {
            object i = ExecuteScalar(sqlStr, null);
            if (i == null)
            {
                return "0";
            }
            return i.ToString();
        }
    }
}

namespace HDICSoft.Message
{
   public class HDIC_Message
    {
        /// <summary>
        /// 显示警告对话框
        /// </summary>
        /// <param name="CurrentForm">当前窗体</param>
        /// <param name="DialogMessage">警告信息</param>
        /// <returns></returns>
        public static DialogResult ShowWarnDialog(Form CurrentForm, string DialogMessage)
        {
            DialogResult WormDialog = MessageBox.Show(CurrentForm,
                DialogMessage,
                "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return WormDialog;
        }

        /// <summary>
        /// 显示错误对话框
        /// </summary>
        /// <param name="CurrentForm">当前窗体</param>
        /// <param name="DialogMessage">错误信息</param>
        /// <returns></returns>
        public static DialogResult ShowErrorDialog(Form CurrentForm, string DialogMessage)
        {
            DialogResult ErrorDialog = MessageBox.Show(CurrentForm,
                DialogMessage,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return ErrorDialog;
        }

        /// <summary>
        /// 显示信息对话框OK
        /// </summary>
        /// <param name="CurrentForm">当前窗体</param>
        /// <param name="DialogMessage">信息</param>
        /// <returns></returns>
        public static DialogResult ShowInfoDialog(Form CurrentForm, string DialogMessage)
        {
            DialogResult InfoDialog = MessageBox.Show(CurrentForm,
                DialogMessage,
                "Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return InfoDialog;
        }

        /// <summary>
        /// 显示问答对话框OKCancel
        /// </summary>
        /// <param name="CurrentForm">当前窗体</param>
        /// <param name="DialogMessage">提示信息</param>
        /// <returns></returns>
        public static DialogResult ShowQuestionDialog(Form CurrentForm, string DialogMessage)
        {
            DialogResult QuestionDialog = MessageBox.Show(CurrentForm,
                DialogMessage,
                "Question",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            return QuestionDialog;
        }
    }
}

namespace HDICSoft.Export
{
   public class HDIC_Export
    {
        /// <summary>
        /// 从listbox中导出数据
        /// </summary>
        /// <param name="lbox">listbox控件</param>
        /// <param name="txtTitle">导出数据的标题</param>
        /// <param name="filter">导出数据的格式(拓展名)</param>
        public void outPutListBoxData(ListBox lbox, string txtTitle, string filter)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "user files(" + filter + ")|" + filter;
            save.Title = "导出文件到";

            if (save.ShowDialog() == DialogResult.OK)
            {
                Stream myStream = save.OpenFile();
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("GB2312"));
                try
                {
                    //写标题
                    sw.WriteLine(txtTitle);
                    //循环写内容
                    for (int i = 0; i < lbox.Items.Count; i++)
                    {
                        string tempStr = "";
                        tempStr += lbox.Items[i].ToString();
                        tempStr += "\t";
                        sw.WriteLine(tempStr);
                    }
                    MessageBox.Show("导出数据成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sw.Close();
                    myStream.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                finally
                {
                    sw.Close();
                    myStream.Close();
                }
            }
        }

        /// <summary>
        /// 导出DataSet数据
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="txtTitle">导出数据的标题</param>

        public static void outPutDataSet(DataTable dt, string txtTitle)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "文本格式.txt|.txt|表格.xls|.xls|文档.doc|.doc|RLC格式.rlc|.rlc";
            save.Title = "导出文件到";

            if (save.ShowDialog() == DialogResult.OK)
            {
                string padStr = "";//用于填充空格
                Stream myStream = save.OpenFile();
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("GB2312"));
                try
                {
                    int[] temp = new int[dt.Columns.Count];
                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        int max = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            max = max > Encoding.Default.GetBytes(dt.Rows[i][index].ToString().Trim()).Length ? max : Encoding.Default.GetBytes(dt.Rows[i][index].ToString().Trim()).Length;
                        }
                        temp[index] = max;
                    }

                    //写标题
                    sw.WriteLine(txtTitle);
                    //写数据字段
                    string tempTitle = "";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (save.FilterIndex == 4)//如果是表格xls格式
                        {
                            if (i > 0)
                            {
                                tempTitle += "\t";
                            }
                            tempTitle += dt.Columns[i].ColumnName;
                        }
                        else//如果是doc、txt等格式
                        {
                            if (i == 0)
                            {
                                tempTitle += dt.Columns[i].ColumnName;
                            }
                            else
                            {
                                if (temp[i - 1] - Encoding.Default.GetBytes(dt.Columns[i - 1].ColumnName).Length < 0)
                                {
                                    tempTitle += padStr.PadRight(2, ' ');

                                }
                                else
                                {
                                    tempTitle += padStr.PadRight(temp[i - 1] - Encoding.Default.GetBytes(dt.Columns[i - 1].ColumnName).Length + 2, ' ');
                                }

                                tempTitle += dt.Columns[i].ColumnName;
                            }

                        }
                    }
                    sw.WriteLine(tempTitle);

                    //循环写内容
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        string tempStr = "";
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            if (save.FilterIndex == 4)//如果是表格xls格式
                            {
                                if (k > 0)
                                {
                                    tempStr += "\t";
                                }
                                tempStr += "'" + dt.Rows[j][k].ToString().Trim();
                            }
                            else //如果是doc、txt等格式
                            {
                                if (k == 0)
                                {
                                    tempStr += dt.Rows[j][k].ToString();
                                }
                                else
                                {
                                    if (temp[k - 1] - Encoding.Default.GetBytes(dt.Columns[k - 1].ColumnName).Length < 0)
                                    {
                                        tempStr += padStr.PadRight(Encoding.Default.GetBytes(dt.Columns[k - 1].ColumnName).Length - Encoding.Default.GetBytes(dt.Rows[j][k - 1].ToString().Trim()).Length + 2, ' ');
                                    }
                                    else
                                    {
                                        tempStr += padStr.PadRight(temp[k - 1] - Encoding.Default.GetBytes(dt.Rows[j][k - 1].ToString().Trim()).Length + 2, ' ');

                                    }
                                    tempStr += dt.Rows[j][k].ToString().Trim();
                                }
                            }

                        }
                        sw.WriteLine(tempStr);
                    }
                    MessageBox.Show("导出数据成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sw.Close();
                    myStream.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                finally
                {
                    sw.Close();
                    myStream.Close();
                }
            }
        }

        /// <summary>
        /// 导出DataGridView数据
        /// </summary>
        /// <param name="dgv">DataGridView控件</param>        

        public void outPutDataGridViewData(DataGridView dgv, string txtTitle)
        {
            SaveFileDialog save = new SaveFileDialog();
            //save.Filter = "user files(" + filter + ")|" + filter";
            save.Filter = "文本格式.txt|.txt|表格.xls|.xls|文档.doc|.doc|RLC格式.rlc|.rlc";
            save.Title = "导出文件到";

            if (save.ShowDialog() == DialogResult.OK)
            {
                string padStr = "";//用于填充空格
                Stream myStream = save.OpenFile();
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("GB2312"));
                try
                {
                    int[] temp = new int[dgv.Columns.Count];
                    //for (int index = 0; index < dgv.Columns.Count; index++)
                    for (int index = 1; index < dgv.Columns.Count; index++)
                    {
                        temp[index] = GetMaxWidth(dgv, index);
                    }
                    //写标题
                    sw.WriteLine(txtTitle);

                    //写数据字段
                    string tempTitle = "";
                    //for (int i = 0; i < dgv.Columns.Count; i++)
                    for (int i = 1; i < dgv.Columns.Count; i++)
                    {
                        if (save.FilterIndex == 4)//如果是表格xls格式
                        {
                            //if (i > 0)
                            if (i > 1)
                            {
                                tempTitle += "\t";
                            }
                            tempTitle += dgv.Columns[i].Name;
                        }
                        else//如果是doc、txt等格式
                        {
                            //if (i == 0)
                            if (i == 1)
                            {
                                tempTitle += dgv.Columns[i].Name;
                            }
                            else
                            {
                                if (temp[i - 1] - Encoding.Default.GetBytes(dgv.Columns[i - 1].Name).Length < 0)
                                {
                                    tempTitle += padStr.PadRight(2, ' ');

                                }
                                else
                                {
                                    tempTitle += padStr.PadRight(temp[i - 1] - Encoding.Default.GetBytes(dgv.Columns[i - 1].Name).Length + 2, ' ');
                                }

                                tempTitle += dgv.Columns[i].Name;
                            }

                        }
                    }
                    sw.WriteLine(tempTitle);

                    //循环写内容
                    for (int j = 0; j < dgv.Rows.Count; j++)
                    {
                        string tempStr = "";
                        //for (int k = 0; k < dgv.Columns.Count; k++)
                        for (int k = 1; k < dgv.Columns.Count; k++)
                        {
                            if (save.FilterIndex == 4)//如果是表格xls格式
                            {
                                //if (k > 0)
                                if (k > 1)
                                {
                                    tempStr += "\t";
                                }
                                tempStr += "'" + dgv.Rows[j].Cells[k].Value.ToString().Trim();
                            }
                            else //如果是doc、txt等格式
                            {
                                //if (k == 0)
                                if (k == 1)
                                {
                                    tempStr += dgv.Rows[j].Cells[k].Value.ToString();
                                }
                                else
                                {
                                    if (temp[k - 1] - Encoding.Default.GetBytes(dgv.Columns[k - 1].Name).Length < 0)
                                    {
                                        tempStr += padStr.PadRight(Encoding.Default.GetBytes(dgv.Columns[k - 1].Name).Length - Encoding.Default.GetBytes(dgv.Rows[j].Cells[k - 1].Value.ToString().Trim()).Length + 2, ' ');
                                    }
                                    else
                                    {
                                        tempStr += padStr.PadRight(temp[k - 1] - Encoding.Default.GetBytes(dgv.Rows[j].Cells[k - 1].Value.ToString().Trim()).Length + 2, ' ');

                                    }
                                    tempStr += dgv.Rows[j].Cells[k].Value.ToString().Trim();
                                }
                            }
                        }
                        sw.WriteLine(tempStr);
                    }
                    MessageBox.Show("导出数据成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sw.Close();
                    myStream.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                finally
                {
                    sw.Close();
                    myStream.Close();
                }
            }
        }

        /// <summary>
        /// 得到DataGridView每列最大的宽度
        /// </summary>
        /// <returns></returns>
        private int GetMaxWidth(DataGridView dgv, int columnIndex)
        {
            int max = 0;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                max = max > Encoding.Default.GetBytes(dgv.Rows[i].Cells[columnIndex].Value.ToString().Trim()).Length ? max : Encoding.Default.GetBytes(dgv.Rows[i].Cells[columnIndex].Value.ToString().Trim()).Length;
            }
            return max;
        }
    }
}

namespace HDICSoft.Func
{
   public class HDIC_Func
    {
        /// <summary>
        /// 设置app.config文件
        /// </summary>
        /// <param name="AppKey"></param>
        /// <param name="AppValue"></param>
        public static void SetValue(string AppKey, string AppValue)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Application.StartupPath + @"\soc_nds_csharp.exe.config");
            XmlNode xNode;
            XmlElement xElem1;
            XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//connectionStrings");
            xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@name='" + AppKey + "']");
            if (xElem1 != null)
            {
                xElem1.SetAttribute("connectionString", AppValue);
            }
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("name", AppKey);
                xElem2.SetAttribute("connectionString", AppValue);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(Application.StartupPath + @"\soc_nds_csharp.exe.config");
        }

        /// <summary>
        /// 获得程序运行路径
        /// </summary>
        /// <returns></returns>
        public static string GetRunningPath()
        {
            string strDebugPath = Application.StartupPath;      // 获得执行路径            

            if (!strDebugPath.EndsWith("\\"))
            {
                strDebugPath = strDebugPath + "\\";
            }

            return strDebugPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">卷标宽度</param>
        /// <param name="height">卷标高度</param>
        /// <param name="printSpeed">打印速度</param>
        /// <param name="density">打印浓度</param>
        /// <param name="X">条形码X方向起始点</param>
        /// <param name="Y">条形码Y方向起始点</param>
        /// <param name="EncodeType">编码类型</param>
        /// <param name="GenerateLabel">打印码文</param>
        /// <param name="content1">打印STB ID</param>
        /// <param name="content2">打印CA ID</param>
        /// <param name="content3">打印 SmartCardID</param>
        /// <param name="rotate">设定条形码旋转角度</param>
        /// <param name="bar">条形码宽窄bar 比例因子</param>
        /// <param name="flag">0：不打印SmartCardID；1打印</param>
        public static void TSCPrinter(string width, string height, string printSpeed, string density, string X, string Y, string EncodeType, string GenerateLabel, string content1, string content2, string content3, string rotate, string bar, int flag)
        {
            #region 1
            //TSCLIB_DLL.openport("TSC TTP-344M Plus");                                           //Open specified printer driver
            //TSCLIB_DLL.setup("100", "63.5", "4", "8", "0", "0", "0");                           //Setup the media size and sensor type info
            //TSCLIB_DLL.clearbuffer();                                                           //Clear image buffer

            //TSCLIB_DLL.barcode("100", "100", "128", "100", "1", "0", "2", "2", this.txtData.Text.Trim()); //Drawing barcode
            //TSCLIB_DLL.printerfont("100", "250", "3", "0", "1", "1", "Print Font Test");        //Drawing printer font
            //TSCLIB_DLL.windowsfont(100, 300, 24, 0, 0, 0, "ARIAL", "Windows Arial Font Test");  //Draw windows font

            //TSCLIB_DLL.downloadpcx("UL.PCX", "UL.PCX");                                         //Download PCX file into printer
            //TSCLIB_DLL.sendcommand("PUTPCX 100,400,\"UL.PCX\"");                                //Drawing PCX graphic

            //TSCLIB_DLL.printlabel("1", "1");                                                    //Print labels
            //TSCLIB_DLL.closeport();                                                             //Close specified printer driver 
            #endregion

            #region 2
            TSCLIB_DLL.openport("TSC TTP-344M Plus");
            TSCLIB_DLL.setup(width, height, printSpeed, density, "0", "0", "0");                           //Setup the media size and sensor type info
            TSCLIB_DLL.clearbuffer();

            TSCLIB_DLL.barcode(X, Y, EncodeType, "100", GenerateLabel, rotate, bar, "2", content1);//打印STB ID
            TSCLIB_DLL.barcode(X, (Convert.ToInt32(Y) + 140).ToString(), EncodeType, "100", GenerateLabel, rotate, bar, "2", content2);//打印STB ID
            if (flag == 0)
            {
                TSCLIB_DLL.printlabel("1", "3");  //户户通打印八份
            }
            if (flag == 1)//0，表示不用打印智能卡号 ；1表示打印智能卡号
            {
                TSCLIB_DLL.barcode(X, (Convert.ToInt32(Y) + 280).ToString(), EncodeType, "100", GenerateLabel, rotate, bar, "2", content3);//打印SmartCardID
                TSCLIB_DLL.printlabel("1", "2");  //村村通打印七份
            }

            //TSCLIB_DLL.printerfont("200", "250", "3", "0", "1", "1", "ShangHai HDIC");        //Drawing printer font
            //TSCLIB_DLL.windowsfont(200, 300, 24, 0, 2, 0, "ARIAL", "长城测试");  //Draw windows font

            //TSCLIB_DLL.windowsfont(250, 350, 160, 0, 2, 0, "Times new Roman", "邱晓淯");  //Draw windows font
            //TSCLIB_DLL.downloadpcx("UL.PCX", "UL.PCX");                                         //Download PCX file into printer
            //TSCLIB_DLL.sendcommand("PUTPCX 100,400,\"UL.PCX\"");                                //Drawing PCX graphic
            TSCLIB_DLL.printlabel("1", "1");                                                    //Print labels
            TSCLIB_DLL.closeport();
            #endregion
        }

        /// <param name="xmlPath">xml字符串</param>
        /// <returns>DataSet</returns>
        public static DataSet XMLToDataSet(string dir)
        {
            String fileExtension = System.IO.Path.GetExtension(dir);
            if (fileExtension != ".xml" || !System.IO.File.Exists(dir))
            {
                MessageBox.Show("加载的xml文件格式错误");
            }
            DataSet ds = new DataSet();
            ds.ReadXml(dir);
            return ds;
        }


        /// <summary>
        /// 写日志信息
        /// </summary>
        /// <param name="LogMsg">日志信息</param>
        public static void LogRecord(string LogMsg)
        {
            ////检查是否启用日志
            //if (HaierSoft.Configuration.Configure.GetConfigString("enablelog").ToLower() != "true")
            //{
            //    return;
            //}
            StreamWriter tmpStreamWriter = null;                // 定义写文件流
            string strPath = GetRunningPath() + "Log";

            try
            {
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                strPath = strPath + "\\HXECard_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                // 初始化文件流
                tmpStreamWriter = new StreamWriter(strPath, true, Encoding.GetEncoding("GB2312"));
                // 写入信息
                tmpStreamWriter.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss:fff") + "]" + LogMsg);
            }
            catch
            {
            }
            finally
            {
                try
                {
                    tmpStreamWriter.Close();
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 添加串口相关的下拉列表项
        /// </summary>
        /// <param name="comboBox">下拉列表</param>
        public static void AddIRPortsToComboBox(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.DisplayMember = "data";
            comboBox.ValueMember = "key";

            DataTable t = new DataTable();
            t.Columns.Add(new DataColumn("key"));
            t.Columns.Add(new DataColumn("data"));

            DataRow r = t.NewRow();
            r["key"] = -1;
            r["data"] = string.Empty;
            t.Rows.Add(r);
            for (int i = 0; i < 8; i++)
            {
                r = t.NewRow();
                r["key"] = i;
                r["data"] = "MX" + i.ToString();
                t.Rows.Add(r);
            }
            comboBox.DataSource = t;
            comboBox.SelectedIndex = 0;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// 取得本机串口
        /// </summary>
        /// <returns></returns>
        public static string[] GetHostCOM()
        {
            string[] sAllPort = null;
            try
            {
                sAllPort = SerialPort.GetPortNames();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sAllPort;
        }

        /// <summary>
        /// 获取本地IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetHostIPAddress()
        {
            string strHostIPAddr;
            strHostIPAddr = "";
            System.Net.IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

            //for (int i = 0; i < addressList.Length; i++)
            //{
            //    strHostIPAddr += addressList[i].ToString();
            //}
            strHostIPAddr += addressList[addressList.Length - 1].ToString();
            return strHostIPAddr;
        }

        /// <summary>
        /// 判断对象是否是整型数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool CheckObjectIsInteger(object obj)
        {
            obj = obj.ToString().Trim();
            if (string.IsNullOrEmpty(obj.ToString()))
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(obj.ToString(), @"^\d{1,}$");
            }

        }

        /// <summary>
        /// 验证对象是否是字节数组
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static bool CheckObjIsByteArray(object objValue)
        {
            if (objValue is byte[])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证身份证号码的格式
        /// </summary>
        /// <param name="IDNo"></param>
        /// <returns></returns>
        public static bool IsValidIdentityNo(string IdentityNo)
        {
            IdentityNo = IdentityNo.Trim();
            if (string.IsNullOrEmpty(IdentityNo))
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(IdentityNo, @"^(\d{14}(\d{1}|\d{4}|\d{3}[xy]))$");
            }
        }

        /// <summary>
        /// 防注入方法
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string InputText(string text)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            // 替换两个或两个以上的空格为一个空格
            text = Regex.Replace(text, "[\\s]{2,}", " ");
            // 替换<br>为转义字符“\n”
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");
            // 替换"&nbsp;"为空格
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");
            // 替换特殊字符
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);
            // 替换单引号
            text = text.Replace("'", "''");
            return text;
        }

        public static DataTable DT { get; set; }
        /// <summary>
        /// 得到dataTable每列最大的宽度
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, int> GetMaxWidth()
        {
            Dictionary<int, int> max = new Dictionary<int, int>();
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                int width = 0;
                foreach (DataRow dr in DT.Rows)
                {
                    if (dr[i].ToString().Length > width)
                    {
                        width = dr[i].ToString().Length;
                    }
                }
                max.Add(i, width);
            }
            return max;
        }

        /// <summary>
        /// 生成指定个数的随机数
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        private string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(10);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }

        /// <summary>
        /// 生成20位的主键数，如AB200807021438250001
        /// </summary>
        /// <param name="str">主键字符串的前两位</param>
        /// <param name="str">例如调用接口：PKey.CreateKeyStr("RO");返回RO打头的20位字符</param>
        /// <returns></returns>
        public static string CreateKeyStr(string str)
        {
            StringBuilder strKey = new StringBuilder();
            Random rd = new Random(System.DateTime.Now.Millisecond);
            string strDate;
            string strRd;

            str = str.ToUpper().Replace(" ", "") + "AA";
            str = str.Substring(0, 2);
            strKey.Append(str);
            strDate = DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HH:mm:ss").Replace(":", "");
            strKey.Append(strDate);
            strRd = "0000" + rd.Next(1, 10000);
            strRd = strRd.Substring(strRd.Length - 4, 4);
            strKey.Append(strRd);
            return strKey.ToString();
        }

        /// <summary>
        /// DataGridView转换为二维数组 
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="includeColumnText"></param>
        /// <returns></returns>
        public static string[,] ToStringArray(DataGridView dataGridView, bool includeColumnText)
        {
            if (dataGridView.Rows.Count <= 0)
            {
                return null;
            }
            string[,] arrReturn = null;
            int rowsCount = dataGridView.Rows.Count;
            int colsCount = dataGridView.Columns.Count;
            if (rowsCount > 0)
            {
                //最后一行是供输入的行时，不用读数据。 
                if (dataGridView.Rows[rowsCount - 1].IsNewRow)
                {
                    rowsCount--;
                }
            }
            int i = 0;
            //包括列标题 
            if (includeColumnText)
            {
                rowsCount++;
                //arrReturn = new string[rowsCount, colsCount];
                arrReturn = new string[rowsCount, colsCount - 1];
                for (i = 1; i < colsCount; i++)
                {
                    //arrReturn[0, i] = dataGridView.Columns[i].HeaderText;
                    arrReturn[0, i - 1] = dataGridView.Columns[i].HeaderText;
                }
                i = 1;
            }
            else
            {
                arrReturn = new string[rowsCount, colsCount];
            }
            //读取单元格数据 
            int rowIndex = 0;
            for (; i < rowsCount; i++, rowIndex++)
            {
                for (int j = 1; j < colsCount; j++)
                {
                    //arrReturn[i, j] = dataGridView.Rows[rowIndex].Cells[j].Value.ToString();
                    arrReturn[i, j - 1] = dataGridView.Rows[rowIndex].Cells[j].Value.ToString();
                }
            }
            return arrReturn;
        }

        /// <summary>
        /// 系统重启
        /// </summary>
        public static void Reset()
        {
            Application.ExitThread();
            System.Threading.Thread thtmp = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(run));
            object appName = Application.ExecutablePath;
            //System.Threading.Thread.Sleep(2000);
            thtmp.Start(appName);
        }
        private static void run(Object obj)
        {
            System.Diagnostics.Process ps = new System.Diagnostics.Process();
            ps.StartInfo.FileName = obj.ToString();
            ps.Start();
        }


        //#region 下面加密和解密复制考勤机 长城暂时屏蔽
        ///// <summary>
        ///// 对字符串进行DES加密
        ///// </summary>
        ///// <param name="sourceString">待加密的字符串</param>
        ///// <returns>加密后的BASE64编码的字符串</returns>
        //public string Encrypt(string sourceString)
        //{
        //    byte[] btKey = Encoding.Default.GetBytes(key);
        //    byte[] btIV = Encoding.Default.GetBytes(iv);
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        byte[] inData = Encoding.Default.GetBytes(sourceString);
        //        try
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
        //            {
        //                cs.Write(inData, 0, inData.Length);
        //                cs.FlushFinalBlock();
        //            }

        //            return Convert.ToBase64String(ms.ToArray());
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //            return "";
        //        }
        //    }
        //}

        ///// <summary>
        ///// 对DES加密后的字符串进行解密
        ///// </summary>
        ///// <param name="encryptedString">待解密的字符串</param>
        ///// <returns>解密后的字符串</returns>
        //public string Decrypt(string encryptedString)
        //{
        //    byte[] btKey = Encoding.Default.GetBytes(key);
        //    byte[] btIV = Encoding.Default.GetBytes(iv);
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        byte[] inData = Convert.FromBase64String(encryptedString);
        //        try
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
        //            {
        //                cs.Write(inData, 0, inData.Length);
        //                cs.FlushFinalBlock();
        //            }

        //            return Encoding.Default.GetString(ms.ToArray());
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("解密失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            MessageBox.Show(ex.Message);
        //            return "";
        //        }
        //    }
        //}
        ///// <summary>
        ///// 对数据库连接字符串进行DES加密
        ///// </summary>
        ///// <param name="sourceString">待加密的字符串</param>
        ///// <returns>加密后的BASE64编码的字符串</returns>
        //public string DBjiami(string sourceString)
        //{
        //    byte[] btKey = Encoding.Default.GetBytes("ruanjianke");
        //    byte[] btIV = Encoding.Default.GetBytes("RUANJIANKE");
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        byte[] inData = Encoding.Default.GetBytes(sourceString);
        //        try
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
        //            {
        //                cs.Write(inData, 0, inData.Length);
        //                cs.FlushFinalBlock();
        //            }

        //            return Convert.ToBase64String(ms.ToArray());
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //            return "";
        //        }
        //    }
        //}

        ///// <summary>
        ///// 对DES加密后的数据库连接字符串进行解密
        ///// </summary>
        ///// <param name="encryptedString">待解密的字符串</param>
        ///// <returns>解密后的字符串</returns>
        //public string DBjiemi(string encryptedString)
        //{
        //    byte[] btKey = Encoding.Default.GetBytes("ruanjianke");
        //    byte[] btIV = Encoding.Default.GetBytes("RUANJIANKE");
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        byte[] inData = Convert.FromBase64String(encryptedString);
        //        try
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
        //            {
        //                cs.Write(inData, 0, inData.Length);
        //                cs.FlushFinalBlock();
        //            }

        //            return Encoding.Default.GetString(ms.ToArray());
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //            return "";
        //        }
        //    }
        //}
        //#endregion
    }
}

namespace HDICSoft.Command
{
   public class HDIC_Command
    {
        /// <summary>
        /// 设置背景颜色
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Color setColor()
        {
            return System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
        }

        /// <summary>
        /// userName
        /// </summary>
        private static string _usename = "";
        public static string UseName
        {
            get { return _usename; }
            set { _usename = value; }
        }

        /// <summary>
        /// userNo
        /// </summary>
        private static string _useno = "";
        public static string userNo
        {
            get { return _useno; }
            set { _useno = value; }
        }

        /// <summary>
        /// roleNo
        /// </summary>
        private static string _roleno = "";
        public static string roleNo
        {
            get { return _roleno; }
            set { _roleno = value; }
        }

        //#region 条形码打印机变量
        //        /// <summary>
        //        /// 卷标宽度
        //        /// </summary>
        //        private static string tscwidth = "";
        //        public static string tscWidth
        //        {
        //            get { return tscwidth; }
        //            set { tscwidth = value; }
        //        }
        //        /// <summary>
        //        /// 卷标高度
        //        /// </summary>
        //        private static string tscheight = "";
        //        public static string tscHeight
        //        {
        //            get { return tscheight; }
        //            set { tscheight = value; }
        //        }
        //        /// <summary>
        //        /// 打印速度
        //        /// </summary>
        //        private static string tscprintspeed = "";
        //            public static string tscPrintSpeed
        //            {
        //                get { return tscprintspeed; }
        //                set { tscprintspeed = value; }
        //            }
        //        /// <summary>
        //        /// 打印浓度
        //        /// </summary>
        //            private static string tscdensity = "";
        //        public static string tscDensity
        //            {
        //                get { return tscdensity; }
        //                set { tscdensity = value; }
        //            }
        //        /// <summary>
        //        /// 条形码X方向起始点
        //        /// </summary>
        //        private static string tscx = "";
        //        public static string tscX
        //        {
        //            get { return tscx; }
        //            set { tscx = value; }
        //        }
        //        /// <summary>
        //        /// 条形码Y方向起始点
        //        /// </summary>
        //        private static string tscy = "";
        //        public static string tscY
        //        {
        //            get { return tscy; }
        //            set { tscy = value; }
        //        }
        //        /// <summary>
        //        /// 编码类型
        //        /// </summary>
        //        private static string tscencodetype = "";
        //            public static string tscEncodeType
        //            {
        //                get { return tscencodetype; }
        //                set { tscencodetype = value; }
        //            }
        //        /// <summary>
        //        /// 打印码文
        //        /// </summary>
        //        private static string tscgeneratelabel="";
        //        public static string  tscGeneratelabel
        //        {
        //            get { return tscgeneratelabel; }
        //            set { tscgeneratelabel = value; }
        //        }
        //        /// <summary>
        //        /// 设置条形码旋转角度
        //        /// </summary>
        //        private static string tscrotate = "";
        //        public static string tscRotate
        //        {
        //            get { return tscrotate; }
        //            set { tscrotate = value; }
        //        }
        //        /// <summary>
        //        /// 条形码宽窄bar 比例因子
        //        /// </summary>
        //        private static string tscbar = "";
        //           public  static string tscBar
        //            {
        //                get { return tscbar; }
        //                set { tscbar = value; }
        //            }
        //        /// <summary>
        //        /// 0 不打印智能卡号，1 打印
        //        /// </summary>
        //           private static int tscflag=0;
        //        public static int tscFlag
        //           {
        //               get { return tscflag; }
        //               set { tscflag = value; }
        //           }
        //#endregion
    }
}
