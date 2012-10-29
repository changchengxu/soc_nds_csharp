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
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Text.RegularExpressions;


namespace HDICSoft.Security
{
   class MD5
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
    class HDIC_DB
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
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        cmd.Dispose();
                        throw new Exception("数据操作错误"+ex.ToString());
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
                throw new Exception("数据操作错误"+ex.ToString());
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
                        throw new Exception("数据操作错误"+ex.ToString());
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
                        throw new Exception("数据操作错误"+ex.ToString());
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

        public static DataTable GetList(string sqlString)
        {
            DataTable dt = ExcuteDataSet(sqlString, null).Tables[0];
            return dt;
        }

        public static int sqlDelete(string sqlStr)
        {
            int i = ExcuteNonQuery(sqlStr, null);
            return i;
        }
    }
}

namespace HDICSoft.Message
{
    class HDIC_Message
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
        /// 显示信息对话框
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
        /// 显示问答对话框
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
   class HDIC_Export
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

        public void outPutDataSet(DataSet ds, string txtTitle)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "表格.xls|.xls|文档.doc|.doc|RLC格式.rlc|.rlc|文本格式.txt|.txt";
            save.Title = "导出文件到";

            if (save.ShowDialog() == DialogResult.OK)
            {
                Stream myStream = save.OpenFile();
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("GB2312"));
                try
                {
                    //写标题
                    sw.WriteLine(txtTitle);
                    //写数据字段
                    string tempTitle = "";
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        if (i > 0)
                        {
                            tempTitle += "\t";
                        }
                        tempTitle += ds.Tables[0].Columns[i].ColumnName;
                    }
                    sw.WriteLine(tempTitle);

                    //循环写内容
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        string tempStr = "";
                        for (int k = 0; k < ds.Tables[0].Columns.Count; k++)
                        {
                            if (k > 0)
                            { tempStr += "\t"; }

                            if (save.FilterIndex == 4)
                            {
                                tempStr += "'" + ds.Tables[0].Rows[j][k].ToString();
                            }
                            else
                            {
                                tempStr += ds.Tables[0].Rows[j][k].ToString();
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
        /// <param name="txtTitle">导出数据标题</param>

        public void outPutDataGridViewData(DataGridView dgv, string txtTitle, string filter)
        {
            DataSet myds = (DataSet)dgv.DataSource;
            this.outPutDataSet(myds, txtTitle);
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
    class HDIC_Func
    {
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
        /// DataGridView转换为二维数组 
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="includeColumnText"></param>
        /// <returns></returns>
        public static string[,] ToStringArray(DataGridView dataGridView, bool includeColumnText)
        {

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
    }
}

