//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows.Forms;
//using System.Data;
//using System.IO.Ports;
//using System.Net;
//using System.Text.RegularExpressions;
//using System.IO;
//using System.Security.Cryptography;

//namespace HDIC_Func
//{
//    class Func
//    {
//        /// <summary>
//        /// 获得程序运行路径
//        /// </summary>
//        /// <returns></returns>
//        public static string GetRunningPath()
//        {
//            string strDebugPath = Application.StartupPath;      // 获得执行路径            

//            if (!strDebugPath.EndsWith("\\"))
//            {
//                strDebugPath = strDebugPath + "\\";
//            }

//            return strDebugPath;
//        }

//        /// <summary>
//        /// 写日志信息
//        /// </summary>
//        /// <param name="LogMsg">日志信息</param>
//        public static void LogRecord(string LogMsg)
//        {
//            ////检查是否启用日志
//            //if (HaierSoft.Configuration.Configure.GetConfigString("enablelog").ToLower() != "true")
//            //{
//            //    return;
//            //}
//            StreamWriter tmpStreamWriter = null;                // 定义写文件流
//            string strPath = GetRunningPath() + "Log";

//            try
//            {
//                if (!Directory.Exists(strPath))
//                {
//                    Directory.CreateDirectory(strPath);
//                }

//                strPath = strPath + "\\HXECard_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

//                // 初始化文件流
//                tmpStreamWriter = new StreamWriter(strPath, true, Encoding.GetEncoding("GB2312"));
//                // 写入信息
//                tmpStreamWriter.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss:fff") + "]" + LogMsg);
//            }
//            catch
//            {
//            }
//            finally
//            {
//                try
//                {
//                    tmpStreamWriter.Close();
//                }
//                catch
//                { }
//            }
//        }

//        /// <summary>
//        /// 添加串口相关的下拉列表项
//        /// </summary>
//        /// <param name="comboBox">下拉列表</param>
//        public static void AddIRPortsToComboBox(ComboBox comboBox)
//        {
//            comboBox.Items.Clear();
//            comboBox.DisplayMember = "data";
//            comboBox.ValueMember = "key";

//            DataTable t = new DataTable();
//            t.Columns.Add(new DataColumn("key"));
//            t.Columns.Add(new DataColumn("data"));

//            DataRow r = t.NewRow();
//            r["key"] = -1;
//            r["data"] = string.Empty;
//            t.Rows.Add(r);
//            for (int i = 0; i < 8; i++)
//            {
//                r = t.NewRow();
//                r["key"] = i;
//                r["data"] = "MX" + i.ToString();
//                t.Rows.Add(r);
//            }
//            comboBox.DataSource = t;
//            comboBox.SelectedIndex = 0;
//            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
//        }

//        /// <summary>
//        /// 取得本机串口
//        /// </summary>
//        /// <returns></returns>
//        public static string[] GetHostCOM()
//        {
//            string[] sAllPort = null;
//            try
//            {
//                sAllPort = SerialPort.GetPortNames();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//            return sAllPort;
//        }

//        /// <summary>
//        /// 获取本地IP地址
//        /// </summary>
//        /// <returns></returns>
//        public static string GetHostIPAddress()
//        {
//            string strHostIPAddr;
//            strHostIPAddr = "";
//            System.Net.IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

//            //for (int i = 0; i < addressList.Length; i++)
//            //{
//            //    strHostIPAddr += addressList[i].ToString();
//            //}
//            strHostIPAddr += addressList[addressList.Length - 1].ToString();
//            return strHostIPAddr;
//        }

//        /// <summary>
//        /// 判断对象是否是整型数
//        /// </summary>
//        /// <param name="obj"></param>
//        /// <returns></returns>
//        public static bool CheckObjectIsInteger(object obj)
//        {
//            obj = obj.ToString().Trim();
//            if (string.IsNullOrEmpty(obj.ToString()))
//            {
//                return false;
//            }
//            else
//            {
//                return Regex.IsMatch(obj.ToString(), @"^\d{1,}$");
//            }

//        }

//        /// <summary>
//        /// 验证对象是否是字节数组
//        /// </summary>
//        /// <param name="objValue"></param>
//        /// <returns></returns>
//        public static bool CheckObjIsByteArray(object objValue)
//        {
//            if (objValue is byte[])
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// 验证身份证号码的格式
//        /// </summary>
//        /// <param name="IDNo"></param>
//        /// <returns></returns>
//        public static bool IsValidIdentityNo(string IdentityNo)
//        {
//            IdentityNo = IdentityNo.Trim();
//            if (string.IsNullOrEmpty(IdentityNo))
//            {
//                return false;
//            }
//            else
//            {
//                return Regex.IsMatch(IdentityNo, @"^(\d{14}(\d{1}|\d{4}|\d{3}[xy]))$");
//            }
//        }

//        /// <summary>
//        /// 防注入方法
//        /// </summary>
//        /// <param name="text"></param>
//        /// <returns></returns>
//        public static string InputText(string text)
//        {
//            text = text.Trim();
//            if (string.IsNullOrEmpty(text))
//                return string.Empty;
//            // 替换两个或两个以上的空格为一个空格
//            text = Regex.Replace(text, "[\\s]{2,}", " ");
//            // 替换<br>为转义字符“\n”
//            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");
//            // 替换"&nbsp;"为空格
//            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");
//            // 替换特殊字符
//            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);
//            // 替换单引号
//            text = text.Replace("'", "''");
//            return text;
//        }

//        public static DataTable DT { get; set; }
//        /// <summary>
//        /// 得到dataTable每列最大的宽度
//        /// </summary>
//        /// <returns></returns>
//        public static Dictionary<int, int> GetMaxWidth()
//        {
//            Dictionary<int, int> max = new Dictionary<int, int>();
//            for (int i = 0; i < DT.Columns.Count; i++)
//            {
//                int width = 0;
//                foreach (DataRow dr in DT.Rows)
//                {
//                    if (dr[i].ToString().Length > width)
//                    {
//                        width = dr[i].ToString().Length;
//                    }
//                }
//                max.Add(i, width);
//            }
//            return max;
//        }
//        /// <summary>
//        /// DataGridView转换为二维数组 
//        /// </summary>
//        /// <param name="dataGridView"></param>
//        /// <param name="includeColumnText"></param>
//        /// <returns></returns>
//        public static string[,] ToStringArray(DataGridView dataGridView, bool includeColumnText)
//        {

//            string[,] arrReturn = null;
//            int rowsCount = dataGridView.Rows.Count;
//            int colsCount = dataGridView.Columns.Count;
//            if (rowsCount > 0)
//            {
//                //最后一行是供输入的行时，不用读数据。 
//                if (dataGridView.Rows[rowsCount - 1].IsNewRow)
//                {
//                    rowsCount--;
//                }
//            }
//            int i = 0;
//            //包括列标题 
//            if (includeColumnText)
//            {
//                rowsCount++;
//                //arrReturn = new string[rowsCount, colsCount];
//                arrReturn = new string[rowsCount, colsCount-1];
//                for (i = 1; i < colsCount; i++)
//                {
//                    //arrReturn[0, i] = dataGridView.Columns[i].HeaderText;
//                    arrReturn[0, i-1] = dataGridView.Columns[i].HeaderText;
//                }
//                i = 1;
//            }
//            else
//            {
//                arrReturn = new string[rowsCount, colsCount];
//            }
//            //读取单元格数据 
//            int rowIndex = 0;
//            for (; i < rowsCount; i++, rowIndex++)
//            {
//                for (int j = 1; j < colsCount; j++)
//                {
//                    //arrReturn[i, j] = dataGridView.Rows[rowIndex].Cells[j].Value.ToString();
//                    arrReturn[i, j-1] = dataGridView.Rows[rowIndex].Cells[j].Value.ToString();
//                }
//            }
//            return arrReturn;
//        }

//        //#region 下面加密和解密复制考勤机 长城暂时屏蔽
//        ///// <summary>
//        ///// 对字符串进行DES加密
//        ///// </summary>
//        ///// <param name="sourceString">待加密的字符串</param>
//        ///// <returns>加密后的BASE64编码的字符串</returns>
//        //public string Encrypt(string sourceString)
//        //{
//        //    byte[] btKey = Encoding.Default.GetBytes(key);
//        //    byte[] btIV = Encoding.Default.GetBytes(iv);
//        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
//        //    using (MemoryStream ms = new MemoryStream())
//        //    {
//        //        byte[] inData = Encoding.Default.GetBytes(sourceString);
//        //        try
//        //        {
//        //            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
//        //            {
//        //                cs.Write(inData, 0, inData.Length);
//        //                cs.FlushFinalBlock();
//        //            }

//        //            return Convert.ToBase64String(ms.ToArray());
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            //MessageBox.Show("加密失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//        //            MessageBox.Show(ex.Message);
//        //            return "";
//        //        }
//        //    }
//        //}

//        ///// <summary>
//        ///// 对DES加密后的字符串进行解密
//        ///// </summary>
//        ///// <param name="encryptedString">待解密的字符串</param>
//        ///// <returns>解密后的字符串</returns>
//        //public string Decrypt(string encryptedString)
//        //{
//        //    byte[] btKey = Encoding.Default.GetBytes(key);
//        //    byte[] btIV = Encoding.Default.GetBytes(iv);
//        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();

//        //    using (MemoryStream ms = new MemoryStream())
//        //    {
//        //        byte[] inData = Convert.FromBase64String(encryptedString);
//        //        try
//        //        {
//        //            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
//        //            {
//        //                cs.Write(inData, 0, inData.Length);
//        //                cs.FlushFinalBlock();
//        //            }

//        //            return Encoding.Default.GetString(ms.ToArray());
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            MessageBox.Show("解密失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//        //            MessageBox.Show(ex.Message);
//        //            return "";
//        //        }
//        //    }
//        //}
//        ///// <summary>
//        ///// 对数据库连接字符串进行DES加密
//        ///// </summary>
//        ///// <param name="sourceString">待加密的字符串</param>
//        ///// <returns>加密后的BASE64编码的字符串</returns>
//        //public string DBjiami(string sourceString)
//        //{
//        //    byte[] btKey = Encoding.Default.GetBytes("ruanjianke");
//        //    byte[] btIV = Encoding.Default.GetBytes("RUANJIANKE");
//        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
//        //    using (MemoryStream ms = new MemoryStream())
//        //    {
//        //        byte[] inData = Encoding.Default.GetBytes(sourceString);
//        //        try
//        //        {
//        //            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
//        //            {
//        //                cs.Write(inData, 0, inData.Length);
//        //                cs.FlushFinalBlock();
//        //            }

//        //            return Convert.ToBase64String(ms.ToArray());
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            //MessageBox.Show("加密失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//        //            MessageBox.Show(ex.Message);
//        //            return "";
//        //        }
//        //    }
//        //}

//        ///// <summary>
//        ///// 对DES加密后的数据库连接字符串进行解密
//        ///// </summary>
//        ///// <param name="encryptedString">待解密的字符串</param>
//        ///// <returns>解密后的字符串</returns>
//        //public string DBjiemi(string encryptedString)
//        //{
//        //    byte[] btKey = Encoding.Default.GetBytes("ruanjianke");
//        //    byte[] btIV = Encoding.Default.GetBytes("RUANJIANKE");
//        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();

//        //    using (MemoryStream ms = new MemoryStream())
//        //    {
//        //        byte[] inData = Convert.FromBase64String(encryptedString);
//        //        try
//        //        {
//        //            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
//        //            {
//        //                cs.Write(inData, 0, inData.Length);
//        //                cs.FlushFinalBlock();
//        //            }

//        //            return Encoding.Default.GetString(ms.ToArray());
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            MessageBox.Show("解密失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//        //            MessageBox.Show(ex.Message);
//        //            return "";
//        //        }
//        //    }
//        //}
//        //#endregion
//    }
//}
