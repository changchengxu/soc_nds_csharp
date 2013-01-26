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
using HDICPrinter;

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
        ////private static readonly string connString = ConfigurationManager.ConnectionStrings["connStringName"].ConnectionString;
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
                        cmd.CommandTimeout = 4;//长城设置，25s即超时
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
            //conn.ConnectionString = connString;
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connStringName"].ConnectionString;

            try
            {
                #region  长城添加，测试当前网络是否正常
                string MIp = ConfigurationManager.ConnectionStrings["connStringName"].ConnectionString;
                MIp = MIp.Substring(0, MIp.IndexOf(';'));
                MIp=MIp.Substring(MIp.IndexOf('=')+1);
                if (!HDIC_DB.TestConnection(MIp, 1433, 3000))
                {
                    //throw new Exception();
                    HDICSoft.Message.HDIC_Message.ShowWarnDialog(null, "数据库连接失败,请检查服务器或者网络是否正常.");
                    return;
                }

                #endregion
               
                if (conn.State != ConnectionState.Open)
                    conn.Open();
            }
            //catch (System.Exception ex)
            //{
            //    HDICSoft.Message.HDIC_Message.ShowWarnDialog(null, "数据库连接失败,请检查服务器或者网络是否正常.");
            //}
            catch 
            {
                HDICSoft.Message.HDIC_Message.ShowWarnDialog(null, "数据库连接失败,请检查服务器或者网络是否正常.");
                //throw new Exception();
            }

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

       /// <summary>
       /// 测试服务器通信是否正常
       /// </summary>
       /// <param name="host">服务器IP地址</param>
       /// <param name="port">服务器端口号， sql一般是1433</param>
       /// <param name="millisecondsTimeout">超时</param>
       /// <returns></returns>
        public static bool TestConnection(string host, int port, int millisecondsTimeout)
        {
            var client = new System.Net.Sockets.TcpClient();
            try
            {
                var ar = client.BeginConnect(host, port, null, null);
                ar.AsyncWaitHandle.WaitOne(millisecondsTimeout);
                return client.Connected;
            }
            catch(Exception ex)
            {
                MessageBox.Show("在检测网络是否正常时发生错误.当前参数是\r\nhost:"+host+"port:"+port+"timeout:"+millisecondsTimeout+"\r\n错误原因是\r\n"+ex.ToString());
            }
              
            finally
            {
                client.Close();
            }
            return false;
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
        /// TSC 344打印机
        /// </summary>
        /// <param name="OutPutPort">电脑端的输出口</param>
        /// <param name="width">卷标宽度</param>
        /// <param name="height">卷标高度</param>
        /// <param name="printSpeed">打印速度</param>
        /// <param name="density">打印浓度</param>
        /// <param name="sensor">感应器类别</param>
        /// <param name="vertical">垂直间距</param>
        /// <param name="offset">偏移距离</param>
        /// <param name="X">条形码X方向起始点</param>
        /// <param name="Y">条形码Y方向起始点</param>
        /// <param name="FontType">文字类型</param>
        /// <param name="FontRotation">文字旋转角度</param>
        /// <param name="EncodeType">编码类型</param>
        /// <param name="BarcodeHeight">条码高度</param>
        /// <param name="PrintCode">打印码文</param>
        /// <param name="CodeInterval">自定义码文与标签间隔</param>
        /// <param name="BarCodeInterval">条形码之间的间隔</param>
        /// <param name="FontMagnify1">设定文字X方向放大倍率,1-8</param>
        /// <param name="FontMagnify2">设定文字X方向放大倍率,1-8</param>
        /// <param name="content1">打印STB ID</param>
        /// <param name="content2">打印CA ID</param>
        /// <param name="content3">打印 SmartCardID</param>
        /// <param name="rotate">设定条形码旋转角度</param>
        /// <param name="BarNarrow">条形码窄bar 比例因子</param>
        /// <param name="BarWide">条形码宽bar 比例因子</param>
        /// <param name="flag">0：不打印SmartCardID；1打印</param>
        /// <param name="PrintLabelSetNum">打印式数</param>
        /// <param name="PrintLabelCopeNum">打印份数</param>
        public static void TSCPrinter(string OutPutPort, string width, string height, string printSpeed, string density, string sensor, string vertical, string offset, string X, string Y, string FontType, string FontRotation, string EncodeType, string BarcodeHeight, string PrintCode, string CodeInterval, string FontMagnify1, string FontMagnify2, string BarCodeInterval, string content1, string content2, string content3, string rotate, string BarNarrow, string BarWide, int flag, string PrintLabelSetNum, string PrintLabelCopeNum)
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

            try
            {
                TSCLIB_DLL.openport(OutPutPort);

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
                TSCLIB_DLL.setup(width, height, printSpeed, density,sensor, vertical, offset);  //Setup the media size and sensor type info
                TSCLIB_DLL.clearbuffer();
                //打印STB ID
                TSCLIB_DLL.barcode(X, Y, EncodeType, BarcodeHeight, PrintCode, rotate, BarNarrow,BarWide, content1);
                //Drawing printer font,打印STBID的码文
                TSCLIB_DLL.printerfont(X, (Convert.ToInt32(Y) + Convert.ToInt32(CodeInterval)).ToString(), FontType, FontRotation, FontMagnify1, FontMagnify2, "STB ID:" + content1);
                //打印分隔符
                //TSCLIB_DLL.printerfont(X, (((Convert.ToInt32(Y) + Convert.ToInt32(CodeInterval)) + (Convert.ToInt32(Y) + Convert.ToInt32(BarCodeInterval))) / 2).ToString(), FontType, FontRotation, FontMagnify1, FontMagnify2, "-".PadRight(("STB ID:" + content1).Length, '-'));
               
                //打印CA ID
                TSCLIB_DLL.barcode(X, (Convert.ToInt32(Y) + Convert.ToInt32(BarCodeInterval)).ToString(), EncodeType, BarcodeHeight, PrintCode, rotate, BarNarrow, BarWide, content2);
                //Drawing printer font,打印CAID的码文
                TSCLIB_DLL.printerfont(X, (Convert.ToInt32(Y) + (Convert.ToInt32(CodeInterval) + Convert.ToInt32(BarCodeInterval))).ToString(), FontType, FontRotation, FontMagnify1, FontMagnify2, "CA ID:" + content2);
                if (flag == 0)//1，表示不用打印智能卡号（户户通打印八份） ；0表示打印智能卡号（村村通打印七份）
                {
                    //打印分隔符
                    //TSCLIB_DLL.printerfont(X, (((Convert.ToInt32(Y) + (Convert.ToInt32(CodeInterval) + Convert.ToInt32(BarCodeInterval))) + (Convert.ToInt32(Y) + 2 * Convert.ToInt32(BarCodeInterval))) / 2).ToString(), FontType, FontRotation, FontMagnify1, FontMagnify2, "-".PadRight(("CA ID:" + content2).Length, '-'));

                    //打印SmartCardID
                    TSCLIB_DLL.barcode(X, (Convert.ToInt32(Y) + 2 * Convert.ToInt32(BarCodeInterval)).ToString(), EncodeType, BarcodeHeight, PrintCode, rotate, BarNarrow, BarWide, content3);
                    //Drawing printer font,打印SmartCardID的码文
                    TSCLIB_DLL.printerfont(X, (Convert.ToInt32(Y) + (Convert.ToInt32(CodeInterval) + 2 * Convert.ToInt32(BarCodeInterval))).ToString(), FontType, FontRotation, FontMagnify1, FontMagnify2, "SC ID:" + content3);
                    TSCLIB_DLL.printlabel(PrintLabelSetNum, PrintLabelCopeNum);//开始打印村村通
                }
                else
                {
                    TSCLIB_DLL.printlabel(PrintLabelSetNum, (Convert.ToInt32(PrintLabelCopeNum) + 1).ToString());//开始打印户户通
                }
                TSCLIB_DLL.closeport();
                //TSCLIB_DLL.printerfont("200", "250", "3", "0", "1", "1", "ShangHai HDIC");        //Drawing printer font
                //TSCLIB_DLL.windowsfont(200, 300, 24, 0, 2, 0, "ARIAL", "长城测试");  //Draw windows font

                //TSCLIB_DLL.windowsfont(250, 350, 160, 0, 2, 0, "Times new Roman", "邱晓淯");  //Draw windows font
                //TSCLIB_DLL.downloadpcx("UL.PCX", "UL.PCX");                                         //Download PCX file into printer
                //TSCLIB_DLL.sendcommand("PUTPCX 100,400,\"UL.PCX\"");                                //Drawing PCX graphic
                //TSCLIB_DLL.printlabel("1", "1");                                                    //Print labels
               
            #endregion
        }

       /// <summary>
        /// 博思得打印机
       /// </summary>
        /// <param name="OutPutPort">打印机端口</param>
        /// <param name="printSpeed">打印速度</param>
        /// <param name="density">打印黑度</param>
        /// <param name="height">标签高度</param>
        /// <param name="interval">定位间隔</param>
        /// <param name="width">标签宽度</param>
        /// <param name="X">条形码X坐标</param>
        /// <param name="Y">条形码Y坐标</param>
        /// <param name="BarRotation">条码打印方向</param>
        /// <param name="EncodeType">条码类型</param>
        /// <param name="BarNarrow">条形码窄单元的宽度（以点dots为单位）</param>
        /// <param name="BarWide">条形码宽单元的宽度（以点dots为单位）</param>
       /// <param name="BarcodeHeight">条形码高度</param>
       /// <param name="PrintCode">打印码文</param>
        /// <param name="BarCodeInterval">条形码之间的间距</param>
        /// <param name="content1">打印STB ID</param>
        /// <param name="content2">打印CA ID</param>
        /// <param name="content3">打印SmartCardID</param>
        /// <param name="CodeInterval">自定义码文与标签间隔</param>
        /// <param name="FontHeight">字型高度</param>
        /// <param name="FontWidth">字型宽度</param>
        /// <param name="FontType">字型名称</param>
        /// <param name="FontAlign">字体旋转角度以及对齐方式</param>
        /// <param name="FontWeight">字体粗细</param>
        /// <param name="Flag">0：不打印SmartCardID；1打印</param>
        /// <param name="PrintLabelSetNum">打印标签</param>
        /// <param name="PrintLabelCopeNum">复制标签份数</param>
        public static void POSTEKPrinter(string OutPutPort, string printSpeed, string density, string height, string interval, string width, string X, string Y, string BarRotation, string EncodeType, string BarNarrow, string BarWide, string BarcodeHeight, string PrintCode,string BarCodeInterval, string content1, string content2, string content3, string CodeInterval,string FontHeight,string FontWidth,string FontType, string FontAlign, string FontWeight, Int32 Flag, string PrintLabelSetNum, string PrintLabelCopeNum)
        {
            #region 1
            //PrintLab.OpenPort("POSTEK C168 200s");//打开打印机端口
            //PrintLab.PTK_ClearBuffer();//清空缓冲区
            //PrintLab.PTK_SetPrintSpeed(3);//设置打印速度
            //PrintLab.PTK_SetDarkness(8);//设置打印黑度
            //PrintLab.PTK_SetLabelHeight(600, 200);//设置标签的高度和定位间隙\黑线\穿孔的高度
            //PrintLab.PTK_SetLabelWidth(800);//设置标签的宽度
            //for (int i = 1; i <= 5; i++)
            //{
            //    PrintLab.PTK_DrawTextTrueTypeW(200, 300, 40, 40, "宋体", 1, 400, false, true, true, "1", "12456789");//打印一行 TrueType Font文字
            //    PrintLab.PTK_DrawBarcode(100, 20, 0, "1", 3, 3, 80, 'N', "12345");//打印一个条码
            //    PrintLab.PTK_SetPagePrintCount(1, 1);//命令打印机执行打印工作
            //}
            //PrintLab.PTK_WritePrinter();
            //PrintLab.ClosePort();//关闭打印机端口
            #endregion

            char MPrintCode;
            if (PrintCode == "N")
            {
                MPrintCode='N';
            }
            else
            {
                MPrintCode='B';
            }

            #region 2
            PrintLab.OpenPort(OutPutPort);//打开打印机端口
            PrintLab.PTK_ClearBuffer();//清空缓冲区
            PrintLab.PTK_SetPrintSpeed(Convert.ToUInt32(printSpeed));//设置打印速度
            PrintLab.PTK_SetDarkness(Convert.ToUInt32(density));//设置打印黑度
            PrintLab.PTK_SetLabelHeight(Convert.ToUInt32(height), Convert.ToUInt32(interval));//设置标签的高度和定位间隙\黑线\穿孔的高度（以dots为单位200DPI:1点＝1/8mm 300DPI:1点=1/12mm）
            PrintLab.PTK_SetLabelWidth(Convert.ToUInt32(width));//设置标签的宽度

            //打印STBID条形码
            PrintLab.PTK_DrawBarcode(Convert.ToUInt32(X), Convert.ToUInt32(Y), Convert.ToUInt32(BarRotation), EncodeType, Convert.ToUInt32(BarNarrow), Convert.ToUInt32(BarWide), Convert.ToUInt32(BarcodeHeight), MPrintCode, content1);//打印一个条码
            //打印码文
            PrintLab.PTK_DrawTextTrueTypeW(Convert.ToInt32(X), Convert.ToInt32(Y) + Convert.ToInt32(CodeInterval), Convert.ToInt32(FontHeight), Convert.ToInt32(FontWidth),FontType, Convert.ToInt32(FontAlign), Convert.ToInt32(FontWeight), false, true, true, "1", "STB ID"+content1);//打印一行 TrueType Font文字

            //打印CAID条形码
            PrintLab.PTK_DrawBarcode(Convert.ToUInt32(X), Convert.ToUInt32(Y), Convert.ToUInt32(BarRotation), EncodeType, Convert.ToUInt32(BarNarrow), Convert.ToUInt32(BarWide), Convert.ToUInt32(BarcodeHeight), MPrintCode, content2);//打印一个条码
            //打印码文
            PrintLab.PTK_DrawTextTrueTypeW(Convert.ToInt32(X), Convert.ToInt32(Y) + Convert.ToInt32(CodeInterval)+Convert.ToInt32(BarCodeInterval), Convert.ToInt32(FontHeight), Convert.ToInt32(FontWidth), FontType, Convert.ToInt32(FontAlign), Convert.ToInt32(FontWeight), false, true, true, "1", "CA ID"+content2);//打印一行 TrueType Font文字

            if (Flag == 0)//1，表示不用打印智能卡号（户户通打印八份） ；0表示打印智能卡号（村村通打印七份）
            {
                //打印SC ID条形码
                PrintLab.PTK_DrawBarcode(Convert.ToUInt32(X), Convert.ToUInt32(Y), Convert.ToUInt32(BarRotation), EncodeType, Convert.ToUInt32(BarNarrow), Convert.ToUInt32(BarWide), Convert.ToUInt32(BarcodeHeight), MPrintCode, content3);//打印一个条码
                //打印码文
                PrintLab.PTK_DrawTextTrueTypeW(Convert.ToInt32(X), Convert.ToInt32(Y) + Convert.ToInt32(CodeInterval)+2*Convert.ToInt32(BarCodeInterval), Convert.ToInt32(FontHeight), Convert.ToInt32(FontWidth), FontType, Convert.ToInt32(FontAlign), Convert.ToInt32(FontWeight), false, true, true, "1", "SC ID"+content3);//打印一行 TrueType Font文字
                PrintLab.PTK_SetPagePrintCount(Convert.ToUInt32(PrintLabelSetNum), Convert.ToUInt32(PrintLabelCopeNum));//命令打印机执行打印 村村通 工作
            }
            else
            {
                PrintLab.PTK_SetPagePrintCount(Convert.ToUInt32(PrintLabelSetNum), Convert.ToUInt32(PrintLabelCopeNum)+1);//命令打印机执行打印 户户通 工作
            }
        
            PrintLab.PTK_WritePrinter();
            PrintLab.ClosePort();//关闭打印机端口
            #endregion
        }

       /// <summary>
        /// 立像(ARGOX)打印机
       /// </summary>
        /// <param name="nEnable">設定除錯環境,1 -> 除錯環境致能, 0 -> 除錯環境關閉</param>
        /// <param name="Arogox_hor">設定開始列印點,hor;水平邊界基點。單位 dot</param>
        /// <param name="Arogox_ver">設定開始列印點,ver: 垂直邊界基點。單位 dot</param>
        /// <param name="Arogox_object"> 印表機功能選項</param>
        /// <param name="darkness"> 設定熱感頭列印熱度，範圍：0～15，內定是 8</param>
        /// <param name="IsImmediate">用來立即將資料傳送出或暫時寫入暫存區中（1為立即傳送, 只傳pbuf中的內容至輸出埠，0為將資料加到暫存區後, 等呼叫B_Print_Out()時才傳送）</param>
        /// <param name="Arogox_pbuf">用來立即將資料傳送出或暫時寫入暫存區中（要傳送的資料指標）</param>
        /// <param name="X">X 座標</param>
        /// <param name="Y">Y 座標</param>
        /// <param name="ori">列印方向定位，'0'是 0°，'1'是90°、'2'是180°，'3'是270°</param>
        /// <param name="EncodeType">条码类型</param>
        /// <param name="BarNarrow">条形码窄单元的宽度</param>
        /// <param name="BarWide">条形码宽单元的宽度</param>
        /// <param name="BarcodeHeight">條碼高度</param>
        /// <param name="PrintCode">打印码文</param>
        /// <param name="BarCodeInterval">条形码之间的间距</param>
        /// <param name="content1">打印STB ID</param>
        /// <param name="content2">打印CA ID</param>
        /// <param name="content3">打印SmartCardID</param>
        /// <param name="CodeInterval">自定义码文与标签间隔</param>
        /// <param name="font">選擇字形，1～5 選擇常佇字形</param>
        /// <param name="hor_factor">水平放大比例，範圍：1～24。</param>
        /// <param name="ver_factor">垂直放大比例，範圍：1～24</param>
        /// <param name="mode">反白功能，'N'普通文字，'R'反白文字。</param>
        /// <param name="Flag">0：不打印SmartCardID；1打印</param>
        /// <param name="PrintLabelCopeNum">复制标签份数</param>
        public static void ArogoxPrinter(string OutPutPort,string nEnable, string Arogox_hor, string Arogox_ver, string Arogox_object, string darkness, string IsImmediate, string Arogox_pbuf, string X, string Y, string ori, string EncodeType, string BarNarrow, string BarWide, string BarcodeHeight, string PrintCode, string BarCodeInterval, string content1, string content2, string content3, string CodeInterval, string font, string hor_factor, string ver_factor, string mode, Int32 Flag, string PrintLabelCopeNum)
        {
            #region 1
            //// sample setting.
            //Argox_Dll.B_Set_DebugDialog(1);//設定除錯環境,1 -> 除錯環境致能, 0 -> 除錯環境關閉
            //Argox_Dll.B_Set_Originpoint(0, 0);//設定開始列印點,hor;水平邊界基點,ver: 垂直邊界基點。單位 dot.
            ////設定轉印模式、啟動 Cutter 或 Peel
            //// 1 -> 開啟熱轉，關閉 Cutter 和 Peel。
            ////2 -> 開啟熱感，關閉 Cutter 和 Peel。
            ////3 -> 開啟熱感和 Cutter，關閉 Peel。
            ////4 -> 開啟熱感和 Peel，關閉 Cutter。
            ////5 -> 開啟熱轉和 Cutter，關閉 Peel。
            ////6 -> 開啟熱轉和 Peel，關閉 Cutter。     
            //Argox_Dll.B_Select_Option(2);
            //Argox_Dll.B_Set_Darkness(8);
            //Argox_Dll.B_Del_Pcx("*");// delete all picture.
            //Argox_Dll.B_WriteData(0, encAscII.GetBytes(Argox_Dll.sznop2), Argox_Dll.sznop2.Length);
            //Argox_Dll.B_WriteData(1, encAscII.GetBytes(Argox_Dll.sznop1), Argox_Dll.sznop1.Length);

            ////draw box.
            //Argox_Dll.B_Draw_Box(20, 20, 4, 760, 560);
            //Argox_Dll.B_Draw_Line('O', 400, 20, 4, 540);

            ////print text, true type text.
            //Argox_Dll.B_Prn_Text(30, 40, 0, 2, 1, 1, 'N', "PPLB Lib Example");
            //Argox_Dll.B_Prn_Text_TrueType(30, 100, 30, "Arial", 1, 400, 0, 0, 0, "AA", "TrueType Font");//save in printer.
            //Argox_Dll.B_Prn_Text_TrueType_W(30, 160, 20, 20, "Times New Roman", 1, 400, 0, 0, 0, "AB", "TT_W: 多字元測試");
            //Argox_Dll.B_Prn_Text_TrueType_Uni(30, 220, 30, "Times New Roman", 1, 400, 0, 0, 0, "AC", Encoding.Unicode.GetBytes("TT_Uni: 多字元測試"), 1);//UTF-16
            //encUnicode.GetBytes("\xFEFF", 0, 1, pbuf, 0);//UTF-16.//pbuf[0]=0xFF,pbuf[1]=0xFE;
            //encUnicode.GetBytes("TT_UniB: 多字元測試", 0, 14, pbuf, 2);//copy mutil byte.
            //encUnicode.GetBytes("\x0000", 0, 1, pbuf, 30);//null.//pbuf[30]=0x00,pbuf[31]=0x00;
            //Argox_Dll.B_Prn_Text_TrueType_UniB(30, 280, 30, "Times New Roman", 1, 400, 0, 0, 0, "AD", pbuf, 0);//Byte Order Mark.

            ////barcode.
            //Argox_Dll.B_Prn_Barcode(420, 100, 0, "3", 2, 3, 40, 'B', "1234<+1>");//have a counter
            //Argox_Dll.B_Bar2d_QR(420, 200, 1, 3, 'M', 'A', 0, 0, 0, "QR CODE");

            ////picture.
            //Argox_Dll.B_Get_Graphic_ColorBMP(420, 280, "bb.bmp");// Color bmp file.
            //Argox_Dll.B_Get_Graphic_ColorBMPEx(420, 320, 200, 150, 2, "bb1", "bb.bmp");//180 angle.
            //IntPtr himage = Argox_Dll.LoadImage(IntPtr.Zero, "bb.bmp", Argox_Dll.IMAGE_BITMAP, 0, 0, Argox_Dll.LR_LOADFROMFILE);
            //Argox_Dll.B_Get_Graphic_ColorBMP_HBitmap(630, 280, 250, 80, 1, "bb2", himage);//90 angle.
            //if (IntPtr.Zero != himage)
            //    Argox_Dll.DeleteObject(himage);

            //// output.
            //Argox_Dll.B_Print_Out(2);// copy 2.

            //// close port.
            //Argox_Dll.B_ClosePrn();
            #endregion

            #region 判断当前打印机是否打开
            //Test code start
                // open port.
                int nLen, ret, sw;
                byte[] pbuf = new byte[128];
                string ver, strmsg;
                System.Text.Encoding encAscII = System.Text.Encoding.ASCII;
                System.Text.Encoding encUnicode = System.Text.Encoding.Unicode;

                // dll version.
                ver = Argox_Dll.B_Get_DLL_Version(0);

                // search port.
                nLen = Argox_Dll.B_GetUSBBufferLen() + 1;
                strmsg = "DLL ";
                strmsg += ver;
                strmsg += "\r\n";
                if (nLen > 1)
                {
                    byte[] buf1, buf2;
                    int len1 = 128, len2 = 128;
                    buf1 = new byte[len1];
                    buf2 = new byte[len2];
                    Argox_Dll.B_EnumUSB(pbuf);
                    Argox_Dll.B_GetUSBDeviceInfo(1, buf1, out len1, buf2, out len2);
                    sw = 1;
                    if (1 == sw)
                    {
                        ret = Argox_Dll.B_CreatePrn(12, encAscII.GetString(buf2, 0, len2));// open usb.
                    }
                    else
                    {
                        ret = Argox_Dll.B_CreateUSBPort(1);// must call B_GetUSBBufferLen() function fisrt.
                    }
                    if (0 != ret)
                    {
                        strmsg += "Open USB fail!";
                    }
                    else
                    {
                        strmsg += "Open USB:\r\nDevice name: ";
                        strmsg += encAscII.GetString(buf1, 0, len1);
                        strmsg += "\r\nDevice path: ";
                        strmsg += encAscII.GetString(buf2, 0, len2);
                        //sw = 2;
                        if (2 == sw)
                        {
                            //Immediate Error Report.
                            Argox_Dll.B_WriteData(1, encAscII.GetBytes("^ee\r\n"), 5);//^ee
                            ret = Argox_Dll.B_ReadData(pbuf, 4, 1000);
                        }
                    }
                }
                else
                {
                    System.IO.Directory.CreateDirectory(Argox_Dll.szSavePath);
                    ret = Argox_Dll.B_CreatePrn(0, Argox_Dll.szSaveFile);// open file.
                    strmsg += "Open ";
                    strmsg += Argox_Dll.szSaveFile;
                    if (0 != ret)
                    {
                        strmsg += " file fail!";
                    }
                    else
                    {
                        strmsg += " file succeed!";
                    }
                }
                MessageBox.Show(strmsg);
                if (0 != ret)
                    return;
#endregion
            #region 2

            //////////Argox_Dll.B_CreatePrn(12, OutPutPort);

            // sample setting.
            Argox_Dll.B_Set_DebugDialog(Convert.ToInt32(nEnable));//設定除錯環境,1 -> 除錯環境致能, 0 -> 除錯環境關閉
            Argox_Dll.B_Set_Originpoint(Convert.ToInt32(Arogox_hor), Convert.ToInt32(Arogox_ver));//設定開始列印點,hor;水平邊界基點,ver: 垂直邊界基點。單位 dot.
            //object;設定轉印模式、啟動 Cutter 或 Peel
            // 1 -> 開啟熱轉，關閉 Cutter 和 Peel。
            //2 -> 開啟熱感，關閉 Cutter 和 Peel。
            //3 -> 開啟熱感和 Cutter，關閉 Peel。
            //4 -> 開啟熱感和 Peel，關閉 Cutter。
            //5 -> 開啟熱轉和 Cutter，關閉 Peel。
            //6 -> 開啟熱轉和 Peel，關閉 Cutter。     
            Argox_Dll.B_Select_Option(Convert.ToInt32(Arogox_object));
            Argox_Dll.B_Set_Darkness(Convert.ToInt32(darkness));// 設定熱感頭列印熱度
            Argox_Dll.B_Del_Pcx("*");// delete all picture.
            //用來立即將資料傳送出或暫時寫入暫存區中
            //  IsImmediate;
            //  1為立即傳送, 只傳pbuf中的內容至輸出埠; 
            //  0為將資料加到暫存區後, 等呼叫B_Print_Out()時才傳送。
            //pbuf;
            //  要傳送的資料指標。
            //length;
            //  pbuf的資料長度。
            Argox_Dll.B_WriteData(Convert.ToInt32(IsImmediate), encAscII.GetBytes(Arogox_pbuf), Arogox_pbuf.Length);//用來立即將資料傳送出或暫時寫入暫存區中

            /////////////////////////////////////////////////////////////////////////////
            char mPrintCode = ' ';
            if (PrintCode == "N")
            {
                mPrintCode = 'N';
            }
            else if (PrintCode == "B")
            {
                mPrintCode = 'B';
            }
            #region 参数说明
            //barcode.
            //          x;
            //  X 座標。
            //y;
            //  Y 座標。
            //ori;
            //  列印方向定位，'0'是 0°，'1'是90°、'2'是180°，'3'是270°
            //       type;
            //  條碼型式
            //條碼型式、如下表：
            //+========================================================================+
            //| type|          條碼總類           | type|           條碼總類           |
            //+========================================================================+
            //|  0  | Code 128 UCC (shipping cont-|  E80| EAN-8                        |  
            //|     | ainer code)                 +-----+------------------------------+
            //+-----+-----------------------------+  E82| EAN-8 2 digit add-on         |
            //|  1  | Code 128 subset A,B and C   +-----+------------------------------+
            //+-----+-----------------------------+  E85| EAN-8 5 digit add-on         |
            //|  1E | UCC/EAN                     +-----+------------------------------+
            //+-----+-----------------------------+  K  | Codabar                      |
            //|  2  | Interleaved 2 of 5          +-----+------------------------------+
            //+-----+-----------------------------+  P  | Postnet                      |
            //|  2C | Interleaved 2 of 5 with che-+-----+------------------------------+
            //|     | ck sum digit                |  UA0| UPC-A                        |
            //+-----+-----------------------------+-----+------------------------------+
            //|  2D | Interleaved 2 of 5 with hum-|  UA2| UPC-A 2 digit add-on         |
            //|     | an readable check digit     +-----+------------------------------+
            //+-----+-----------------------------+  UA5| UPC-A 5 digit add-on         |
            //|  2G | German Postcode             +-----+------------------------------+
            //+-----+-----------------------------+  UE0| UPC-E                        |
            //|  2M | Matrix 2 of 5               +-----+------------------------------+
            //+-----+-----------------------------+  UE2| UPC-E 2 digit add-on         |
            //|  2U | UPC Interleaved 2 of 5      +-----+------------------------------+
            //+-----+-----------------------------+  UE5| UPC-E 5 digit add-on         |
            //|  3  | Code 3 of 9                 +-----+------------------------------+
            //+-----+-----------------------------+  R14|RSS-14                        |
            //|  3C | Code 3 of 9 with check sum  +-----+------------------------------+
            //|     | digit                       |  RL |RSS Limited                   |
            //+-----+-----------------------------+-----+------------------------------+
            //|  9  | Code 93                     |  RS |RSS Stacked                   | 
            //+-----+-----------------------------+-----+------------------------------+
            //|  E30| EAN-13                      |  RT |RSS Truncated                 |
            //+-----+-----------------------------+-----+------------------------------+
            //|  E32| EAN-13 2 digit add-on       |  RSO|RSS Stacked Omnidirectiona    |
            //+-----+-----------------------------+-----+------------------------------+
            //|  E35| EAN-13 5 digit add-on       |  REX|RSS Expanded                  |
            //+-----+-----------------------------+-----+------------------------------+
            //       n;
            //  NARROW bar 寬度在最小單位，
            //  若是RSS條碼，則此參數為條碼寬度的倍數，範圍為1~10，預設值為1。
            //w;
            //  WIDE bar 寬度在最小單位，
            //  若是RSS條碼，則此參數為每列條碼可包含的最大資料區塊數量，只在使用RSS Expanded條碼時
            //  有效，在其它類型的RSS條碼中則為無效參數，範圍為2~22，預設值為22，且此值必需為偶數。
            //height;
            //  條碼高度。
            //  RSS條碼的標準的最小高度限制:
            //  R14 -> 33 pixels
            //  RL  -> 10 pixels
            //  RS  -> 13 pixels
            //  RT  -> 13 pixels
            //  RSO -> 69 pixels
            //  REX -> 34 pixels
            //PrintCode;
            //  當 human 是'N'時，則不列印文字，當 human 是'B'時，則列印可讀文字。
            //data;
            //  資料字串，於資料末端加上<Operation Number>，則可有跳號功能，
            //  Operation: + or - 記號，Number: 0 ~ 32768 數值。
            //  若在RSS需要加上2D(composite symbol)的資料時, 需加上'|'符號, 如1234567890123|123.
            //  RSS條碼的最大數值限制:
            //  R14 -> 9999999999999
            //  RL  -> 1999999999999
            //  RS  -> 9999999999999
            //  RT  -> 9999999999999
            //  RSO -> 9999999999999
            //  REX -> 74 digits
            #endregion

            //打印STB ID
            Argox_Dll.B_Prn_Barcode(Convert.ToInt32(X), Convert.ToInt32(Y), Convert.ToInt32(ori), EncodeType, Convert.ToInt32(BarNarrow), Convert.ToInt32(BarWide), Convert.ToInt32(BarcodeHeight), mPrintCode, content1 + "<+1>");//have a counter

            #region 参数说明
            //print text, true type text.
             //印出一行文字和加上跳號功能
             //  X 座標。
             //  Y 座標。
             //ori;
             //  列印方向定位，'0'是 0°，'1'是90°、'2'是180°，'3'是270°
             //font;
             //  選擇字形，1～5 選擇常佇字形。
             //hor_factor;
             //  水平放大比例，範圍：1～24。
             //ver_factor;
             //  垂直放大比例，範圍：1～24。
             //mode;
             //  反白功能，'N'普通文字，'R'反白文字。
             //data;
             //  資料字串，當尾端資料加上 <Operation Number> 格式為加上跳號功能，
            //  Operation: + or - 記號，Number: 0 ~ 32768 數值。
            #endregion

            char mMode;
            if (mode == "N")
            {
                mMode='N';
            }
            else
            {
                mMode='Y';
            }
            //  打印 STB ID 码文
            Argox_Dll.B_Prn_Text( Convert.ToInt32(X), Convert.ToInt32(Y)+Convert.ToInt32(CodeInterval),   Convert.ToInt32(ori),   Convert.ToInt32(font),   Convert.ToInt32(hor_factor),   Convert.ToInt32(ver_factor), mMode, "STB ID"+ content1);
            
            //打印 CA ID 
            Argox_Dll.B_Prn_Barcode(Convert.ToInt32(X), Convert.ToInt32(Y) + Convert.ToInt32(BarCodeInterval), Convert.ToInt32(ori), EncodeType, Convert.ToInt32(BarNarrow), Convert.ToInt32(BarWide), Convert.ToInt32(BarcodeHeight), mPrintCode, content2 + "<+1>");//have a counter
            //打印CA ID 码文
            Argox_Dll.B_Prn_Text(Convert.ToInt32(X), Convert.ToInt32(Y) + Convert.ToInt32(CodeInterval)+Convert.ToInt32(BarCodeInterval), Convert.ToInt32(ori), Convert.ToInt32(font), Convert.ToInt32(hor_factor), Convert.ToInt32(ver_factor), mMode,"CA ID"+ content2);

            if (Flag == 0)//1，表示不用打印智能卡号（户户通打印八份） ；0表示打印智能卡号（村村通打印七份）
            {
                //打印 SC ID 
                Argox_Dll.B_Prn_Barcode(Convert.ToInt32(X), Convert.ToInt32(Y) + 2 * Convert.ToInt32(BarCodeInterval), Convert.ToInt32(ori), EncodeType, Convert.ToInt32(BarNarrow), Convert.ToInt32(BarWide), Convert.ToInt32(BarcodeHeight), mPrintCode, content3 + "<+1>");//have a counter
                //打印 SC ID 码文
                Argox_Dll.B_Prn_Text(Convert.ToInt32(X), Convert.ToInt32(Y) + Convert.ToInt32(CodeInterval) + 2 * Convert.ToInt32(BarCodeInterval), Convert.ToInt32(ori), Convert.ToInt32(font), Convert.ToInt32(hor_factor), Convert.ToInt32(ver_factor), mMode, "SC ID"+content3);
                //列印所有資料
                //列印的份數
                Argox_Dll.B_Print_Out(Convert.ToInt32(PrintLabelCopeNum));// copy 2.
            }
            else
            {
                Argox_Dll.B_Print_Out(Convert.ToInt32(PrintLabelCopeNum)+1);// copy 2.
            }
            // output.
            //列印所有資料
            //列印的份數
            Argox_Dll.B_Print_Out(Convert.ToInt32(PrintLabelCopeNum));// copy 2.

            // close port.
            //關閉 Printer 工作
            Argox_Dll.B_ClosePrn();
            #endregion

        }

       /// <summary>
       /// 斑马打印机
       /// </summary>
       /// <param name="content1"></param>
       /// <param name="content2"></param>
       /// <param name="content3"></param>
       public static void ZebraPrinter(string content1,string content2,string content3)
        {
            #region 1
            ////实例化LPT端口操作类
            //Zebra Mylpt = new Zebra();
            //Mylpt.Open();
            //try
            //{

            //    //这里使用的是EAN-13码（^BE=EAN-13码）
            //    //N=不旋转，空的=条码高度（默认由BY来设置），Y=打印注释行，Y=打印校验位
            //    //^BY=条码字段默认参数设置2=窄条的宽度【点阵】，2.0=宽条与窄条的比例（默认=3.0有效范围——2.0到3.0【增量0.1】），95=条码的高度   
            //    //^FD+"字符串"+^FS  打印内容  
            //    //^A0B(A零B)表示字体的设置,数字为字符的高度和宽度                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
            //    Mylpt.Write("^XA~TA000~JSN^LT0^MMT^MNW^MTT^PON^PMN^LH0,0^JMA^PR4,4^MD30^JUS^LRN^CI0^XZ^XA^LL1199^FT572,415^A0B,48,72^FH\\^FD" +
            //        this.textBox6.Text.Trim() + "kg" + "^FS^FT578,920^A0B,48,57^FH\\^FD" +
            //        this.textBox5.Text.Trim() + "^FS^FT462,520^A0B,41,72^FH\\^FD" +
            //        this.textBox4.Text.Trim() + "%" + "^FS^FT465,775^A0B,46,72^FH\\^FD" +
            //        this.textBox3.Text.Trim() + "%" + "^FS^FT360,840^A0B,50,81^FH\\^FD" +
            //        this.textBox2.Text.Trim() + "^FS^FT254,920^A0B,75,124^FD" +
            //        this.textBox1.Text.Trim() + "^FS^BY3,2.1,95^FT339,165^BEN,,Y,Y^FD6948351700016^FS^PQ1,0,1,Y^XZ");
            //    Mylpt.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("打开LPT端口时发生错误，原因" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            #endregion
           //^PQ（打印数量）指令有几个打印操作。它控制打印标签数量，打印机暂停打印标签数量，每个序列号复制数量。
            //^PQ50，10，1，Y：打印总数50张标签每个序列号只打一张。打印每组数量是10，但在每组间不暂停。
            #region 2
            //实例化LPT端口操作类
            Zebra Mylpt = new Zebra();
            Mylpt.Open();
            try
            {

                //这里使用的是EAN-13码（^BE=EAN-13码）
                //N=不旋转，空的=条码高度（默认由BY来设置），Y=打印注释行，Y=打印校验位
                //^BY=条码字段默认参数设置2=窄条的宽度【点阵】，2.0=宽条与窄条的比例（默认=3.0有效范围——2.0到3.0【增量0.1】），95=条码的高度   
                //^FD+"字符串"+^FS  打印内容  
                //^A0B(A零B)表示字体的设置,数字为字符的高度和宽度                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
                Mylpt.Write("^XA~TA000~JSN^LT0^MMT^MNW^MTT^PON^PMN^LH0,0^JMA^PR4,4^MD30^JUS^LRN^CI0^XZ^XA^LL1199^FT572,415^A0B,48,72^FH\\^FD" +
                   "STB ID"+content1+ "^FS^FT578,920^A0B,48,57^FH\\^FD" +
                   "CA ID"+content2+ "^FS^FT462,520^A0B,41,72^FH\\^FD" +
                   "SC ID"+content3 + "%" + "^FS^FT465,775^A0B,46,72^FH\\^FS^BY3,2.1,95^FT339,165^BEN,,Y,Y^FD6948351700016^FS^PQ1,0,1,Y^XZ");
                Mylpt.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开LPT端口时发生错误，原因" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

           //^XA 条码打印指令开始
           //^MD30 设置色带颜色的深度，取值范围从-30到30
           //^LH60,10设置条码纸的边距
           //^FO20,10 设置条码左上角的位置
           //^ACN,18,10 设置字体
           //^BY1.4,3,50 设置条码样式。1.4是条码的缩放级别，3是条码中粗细柱的比例，50是条码高度
           //^BCN,,Y,N 打印code128的指令
           //^FD123^FS 设置要打印的内容 ^FD是要打印的条码内容 ^FS表示换行
           //^AON  字体方向
           //^XZ 条码打印指令结束
           //^MCY 清除以前的标签
           //^LRN 不反向打印（N表示不反向）
           //^FWN 字体方向为正常（N表示正常)
           //^CFD 采用的字符字体
           //^PR 打印速度
           //^MNY 纸类型类Y表示非连续纸
           //^MTT纸种类T表示热传印纸
           //^MMT 打印模式T表示撕下
           //^MD 标签深度（值在-30到30之间）
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

                strPath = strPath + "\\STBLog_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

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
        public static void AddIRPortsToComboBox(ComboBox comboBox,string[] portNames)
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
            for (int i = 0; i < portNames.Length; i++)
            {
                r = t.NewRow();
                r["key"] = i;
                r["data"] = portNames[i];
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

       /// <summary>
       /// 字符串转换字节数组（长度除以2）
       /// </summary>
       /// <param name="str"></param>
       /// <param name="buffer"></param>
       /// <returns></returns>
        public static bool CStringToByte(string str, ref Byte[] buffer)
        {
            int i, len;

            if (str.Trim().Length == 0) return false;
            len = (int)str.Length;
            if ((len % 2) != 0) return false;
            for (i = 0; i < len; i += 2)
            {
                if (str[i] >= '0' && str[i] <= '9')
                {
                    buffer[i / 2] = (Byte)((Byte)str[i] - 0x30);
                }
                else if (str[i] >= 'A' && str[i] <= 'F')
                {
                    buffer[i / 2] = (Byte)((Byte)str[i] - 'A' + 0xa);
                }
                else if (str[i] >= 'a' && str[i] <= 'f')
                {
                    buffer[i / 2] = (Byte)((Byte)str[i] - 'a' + 0xa);
                }
                else
                {
                    buffer[i / 2] = 0;
                }
                buffer[i / 2] = (Byte)((buffer[i / 2]) << 4);
                if (str[i + 1] >= '0' && str[i + 1] <= '9')
                {
                    buffer[i / 2] = (Byte)((str[i + 1] - 0x30) + buffer[i / 2]);
                }
                else if (str[i + 1] >= 'A' && str[i + 1] <= 'F')
                {
                    buffer[i / 2] = (Byte)((str[i + 1] - 'A' + 0xa) + buffer[i / 2]);
                }
                else if (str[i] >= 'a' && str[i + 1] <= 'f')
                {
                    buffer[i / 2] = (Byte)((str[i + 1] - 'a' + 0xa) + buffer[i / 2]);
                }
            }
            //length = i / 2;
            return true;
        }
        //private  byte[] HexStringToByteArray(string s)//和上面CStringToByte功能一样
        //{
        //    s = s.Replace(" ", "");
        //    byte[] buffer = new byte[s.Length / 2];
        //    for (int i = 0; i < s.Length; i += 2)
        //        buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
        //    return buffer;
        //}
       /// <summary>
        /// 字节数组转换字符串
       /// </summary>
       /// <param name="bytes"></param>
       /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        #region 密码加密（异或算法）暂时没有用到
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">待加密的明文字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptStr(string str, string key)
        {
            byte[] bStr = (new UnicodeEncoding()).GetBytes(str);
            byte[] bKey = (new UnicodeEncoding()).GetBytes(key);

            for (int i = 0; i < bStr.Length; i++)
            {
                for (int j = 0; j < bKey.Length; j++)
                {
                    bStr[i] = Convert.ToByte(bStr[i] ^ bKey[j]);
                }
            }
            return (new UnicodeEncoding()).GetString(bStr).TrimEnd('\0');
        }

        private static string EncryptString(string str, string key)
        {
            //检验加密解密，加密解密失败，返回 null
            string s1 = EncryptStr(str, key);
            string s2 = EncryptStr(s1, key);
            if (s2 != str)
                return null;
            else
                return s1;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str">待解密的密文字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的明文</returns>
        public static string DecryptString(string str, string key)
        {
            return EncryptStr(str, key);
        }

        #endregion

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

       /// <summary>
        /// STBLineNum
       /// </summary>
        private static Int32 _STBlinenum = 0;
        public static Int32 STBLineNum
        {
            get { return _STBlinenum; }
            set { _STBlinenum = value; }
        }

       ///// <summary>
       // ///STBType 数值0代表户户通，1代表村村通
       ///// </summary>
       // private static Int32 _STBType = 0;
       // public static Int32 STBType
       // {
       //     get { return _STBType; }
       //     set { _STBType = value; }
       // }

       /// <summary>
        /// 获取connectionStrings中数据库连接信息
       /// </summary>
        private static string _connectionStr;
        public static string ConnectionStr
        {
            get { return _connectionStr; }
            set { _connectionStr = value; }
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
