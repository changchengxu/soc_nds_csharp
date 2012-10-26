using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace HDIC_EXPORT
{
    class exportData
    {
        //做数据库处理的时候，经常需要将数据导出，一般都是导出到EXCEL，网上很多方法都是用EXCEL组件，自己感觉效率比较低，于是重新用流处理的方式导出数据，在数据量大的情况下，速度不知道快了多少，非常快，而且导出格式可以是EXCEL，WORD，TXT等，自由设定，不多说，贴出代码，直接传入对应参数即可！

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
                int[] padCol = new int[dgv.Columns.Count+1]; //用于获取每个数据字段位置
                Stream myStream = save.OpenFile();
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("GB2312"));
                try
                {
                    int[] temp = new int[dgv.Columns.Count];
                    for (int index = 0; index < dgv.Columns.Count; index++)
                    {
                        temp[index] = GetMaxWidth(dgv, index);
                    }
                    for (int indexCol = 1; indexCol < temp.Length; indexCol++)
                    {
                        padCol[indexCol] +=padCol[indexCol-1]+ temp[indexCol - 1]+2;
                    }
                        //写标题
                        sw.WriteLine(txtTitle);

                    //写数据字段
                    string tempTitle = "";
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        if (save.FilterIndex == 4)//如果是表格xls格式
                        {
                            if (i > 0)
                            {
                                tempTitle += "\t";
                            }
                            tempTitle += dgv.Columns[i].Name;
                        }
                        else//如果是doc、txt等格式
                        {
                            if (i == 0)
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
                        for (int k = 0; k < dgv.Columns.Count; k++)
                        {
                            if (save.FilterIndex == 4)//如果是表格xls格式
                            {
                                if (k > 0)
                                {
                                    tempStr += "\t";
                                }
                                tempStr += "'" + dgv.Rows[j].Cells[k].Value.ToString().Trim();
                            }
                            else //如果是doc、txt等格式
                            {
                                if (k == 0)
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
