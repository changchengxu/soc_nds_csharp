using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.Export;
using HDICSoft.Command;
using HDICSoft.Message;
using HDICSoft.DB;
using HDICSoft.Func;
using System.IO;
using System.Threading;

namespace soc_nds_csharp.Export_file
{
    public delegate void myDelegate();
    public partial class Export_file : Form
    {
        public Export_file()
        {
            InitializeComponent();
            tableLayoutPanel1.BackColor = HDIC_Command.setColor();
        }

        //导入数据
        
        private void btn_Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "打开";
            OpenFile.InitialDirectory = HDIC_Func.GetRunningPath()+"config" ;
            OpenFile.Filter = "导入返回文件参数(*.xml)|ReturnFile.xml";
            DialogResult drResult = OpenFile.ShowDialog();
            if (drResult == DialogResult.OK)
            {
                DataSet ds=new DataSet();
                try
                {
                    ds.ReadXml(OpenFile.FileName);
                }
                catch
                {
                    HDIC_Message.ShowWarnDialog(this, "导入的文件不正确，请检查该文件");
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txt_ManufacturerName.Text = ds.Tables[0].Rows[0]["manufacturer_name"].ToString().Trim();
                    txt_STBModel.Text=ds.Tables[0].Rows[0]["STB_model"].ToString().Trim();
                    txt_STBChip.Text=ds.Tables[0].Rows[0]["STB_chip"].ToString().Trim();
                    cbox_BroadCaster.Text=ds.Tables[0].Rows[0]["broadcater"].ToString().Trim();
                    txt_procBatch.Text=ds.Tables[0].Rows[0]["production_batch"].ToString().Trim();
                    txt_ManufacturerID.Text=ds.Tables[0].Rows[0]["manufacturer_id"].ToString().Trim();
                    txt_ModelID.Text=ds.Tables[0].Rows[0]["model_id"].ToString().Trim();
                    txt_HardwareID.Text=ds.Tables[0].Rows[0]["hardware_id"].ToString().Trim();
                }
            }
        }
        Thread td = null;//导入数据库的线程
      
        private void tsb_ExportData_Click(object sender, EventArgs e)
        {
            if (td != null && td.IsAlive)
            {
                HDIC_Message.ShowWarnDialog(this, "正在将数据导入到数据库，请等待……");
                return;
            }
            td = new Thread(new ThreadStart(DoWork));
            td.SetApartmentState(ApartmentState.STA);
            td.IsBackground = true;
            td.Start();

        
        }

        private void DoWork()
        {
            myDelegate d = new myDelegate(ExportData);
            this.Invoke(d);
        }

        Stream myStream;
        StreamWriter sw;
        private void ExportData()
        {
            #region 1
            //DataTable dt = new DataTable();
            //dt.Columns.Add(new DataColumn("生产商名字", typeof(string)));
            //dt.Columns.Add(new DataColumn("STB Model", typeof(string)));
            //dt.Columns.Add(new DataColumn("生产日期", typeof(string)));
            //dt.Columns.Add(new DataColumn("STB Chip", typeof(string)));
            //dt.Columns.Add(new DataColumn("BroadCaster", typeof(string)));
            //dt.Columns.Add(new DataColumn("生产批次", typeof(string)));
            //dt.Columns.Add(new DataColumn("ManufacturerID", typeof(string)));
            //dt.Columns.Add(new DataColumn("ModelID", typeof(string)));
            //dt.Columns.Add(new DataColumn("HardwareID", typeof(string)));

            //DataRow dr = dt.NewRow();
            //dr[0] = txt_name.Text.Trim();
            //dr[1] = txt_STBModel.Text.Trim();
            //dr[2] = dateTimePicker1.Value;
            //dr[3] = txt_STBChip.Text.Trim();
            //dr[4] = cbox__BroadCaster.Text.Trim();
            //dr[5] = txt_procBatch.Text.Trim();
            //dr[6] = txt_ManufacturerID.Text.Trim();
            //dr[7] = txt_ModelID.Text.Trim();
            //dr[8] = txt_HardwareID.Text.Trim();
            //dt.Rows.Add(dr);
            //HDIC_Export.outPutDataSet(dt, "导出数据");
            #endregion

            //RetFile_DTHUMC_<manufacturer_name>_<STB model> <Date DDMMMYYYY>
            //RetFile_DTHUMC_STBman_MBB265_23Aug2010
            //由于是中文，所以“en_us”转换后月份才会是英文
            string Date = dateTimePicker1.Value.ToString("ddMMMyyyy", new System.Globalization.CultureInfo("en-us"));
            string SaveFileName = "RetFile_" + cbox_BroadCaster.Text.Trim() + "_" + txt_ManufacturerName.Text.Trim() + "_" + txt_STBModel.Text.Trim() + "_" + Date;
            //前两行表示文件类型
            string ContentTitle = "STB_vendor_to_" + cbox_BroadCaster.Text.Trim() + "\r\nver101\r\n";
            //第三行为生产的日期和时间
            //ContentTitle += dateTimePicker1.Value.ToString("ddd,dd MMM yyyy HH:mm:ss zzz", new System.Globalization.CultureInfo("en-us")) + "\r\n";
            string temp = System.DateTimeOffset.Now.ToString("ddd,dd MMM yyyy HH:mm:ss zzz", new System.Globalization.CultureInfo("en-us"));
            int pos = temp.LastIndexOf(":");
            ContentTitle += temp.Substring(0, pos) + temp.Substring(pos + 1, 2);

            //第四行用于描述机顶盒，必须包括"<STB model> with <STB chip> for <broadcaster>
            ContentTitle += txt_STBModel.Text.Trim() + " with " + txt_STBChip.Text.Trim() + " for " + cbox_BroadCaster.Text.Trim() + "\r\n";
            //第五行用于描述机顶盒制造商的生产批次
            ContentTitle += txt_procBatch.Text.Trim() + "\r\n";
            //第六行为机顶盒平台标识，包括<Manufacturer ID>、<Model ID>和<Hardware ID>
            ContentTitle += txt_ManufacturerID.Text.Trim() + txt_ModelID.Text.Trim() + txt_HardwareID.Text.Trim() + "\r\n";


            #region 2
            //            if (!File.Exists("D:\\" + SaveFileName))
            //            {
            //                File.CreateText("D:\\" + SaveFileName);
            //            }
            //            FileStream fs = new FileStream(SaveFileName, FileMode.Append);     //建立文件流
            //            try
            //            {
            //                //StreamWriter sw = new StreamWriter(fs,System.Text.Encoding.GetEncoding("gb2312"));
            //                //sw.WriteLine(content);
            //                byte[] mybyte = System.Text.Encoding.ASCII.GetBytes(content);
            //                int size = (int)mybyte.Length;     //记录字符长度   
            //                fs.Write(mybyte, 0, size);       //写入文件流   
            //                fs.Flush();         //强制清空缓冲区,并把内容写到文件data.txt文件   

            //            }
            //            catch (System.Exception ex)
            //            {
            //                HDIC_Message.ShowWarnDialog(this, "写入返回文件失败\n" + ex.ToString());
            //            }
            //            finally
            //            {
            //                fs.Close();           //关闭文件流   
            //            }
            #endregion

            string ChipIndex = "";


            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "文本格式.txt|.txt";
            save.Title = "导出文件到";
            save.FileName = SaveFileName + ".txt";
            if (save.ShowDialog() == DialogResult.OK)
            {
                #region 临时用
                //myStream = save.OpenFile();
                //sw = new StreamWriter(myStream, System.Text.Encoding.ASCII);

                //for (int i = 0; i < 10000000; i++)
                //{
                //    ContentTitle = String.Format("{0:d10}", i) + " " + String.Format("{0:D99}", i + 2) + "\r\n";

                //    sw.Write(ContentTitle);
                //}
                //sw.Close();
                //myStream.Close();
                //HDIC_Message.ShowInfoDialog(null, "ok");
                //return;
                #endregion


                DataTable dt = HDIC_DB.GetList("select * from STBData where Flag =0 and  CONVERT(varchar(12) , ProductDate, 111 )='" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "'");
                if (dt.Rows.Count < 100000)
                {
                    myStream = save.OpenFile();
                    //sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("GB2312"));
                    sw = new StreamWriter(myStream, System.Text.Encoding.ASCII);
                    try
                    {
                        sw.Write(ContentTitle);

                        string content = "";
                        //第六行之后的每一行列出了机顶盒的加密序列号＋空格＋芯片序列号+空格+智能卡号＋空格＋机顶盒序列号
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (cbox_BroadCaster.Text.Trim() == "DTHUMC")//村村通
                            {
                                content = dt.Rows[i]["CAID"].ToString().Trim() + " " + dt.Rows[i]["ChipID"].ToString().Trim()
                               + " " + dt.Rows[i]["SmartCardID"].ToString().Trim() + " " + dt.Rows[i]["STBID"].ToString().Trim() + "\r\n";

                            }
                            else if (cbox_BroadCaster.Text.Trim() == "SARFT")//户户通
                            {
                                content = dt.Rows[i]["CAID"].ToString().Trim() + " " + dt.Rows[i]["ChipID"].ToString().Trim()
                               + " " + dt.Rows[i]["STBID"].ToString().Trim() + "\r\n";

                            }

                            sw.Write(content);
                            ChipIndex += dt.Rows[i]["ChipID"].ToString().Trim() + ",";
                        }
                    }
                    catch (Exception ex)
                    {
                        HDIC_Message.ShowWarnDialog(this, "创建文件时发生错误，原因是：" + ex.ToString());
                        return;
                    }
                    sw.Close();
                    myStream.Close();
                }

                #region 数据大于十万条，需要创建两个文件
                else
                {
                    FileStream fsFile = new FileStream(SaveFileName + "_1", FileMode.Open);

                    sw = new StreamWriter(fsFile, System.Text.Encoding.ASCII);
                    try
                    {
                        sw.Write(ContentTitle);

                        string content = "";
                        //第六行之后的每一行列出了机顶盒的加密序列号＋空格＋芯片序列号+空格+智能卡号＋空格＋机顶盒序列号
                        for (int i = 0; i < 100000; i++)
                        {
                            if (cbox_BroadCaster.Text.Trim() == "DTHUMC")//村村通
                            {
                                content = dt.Rows[i]["CAID"].ToString().Trim() + " " + dt.Rows[i]["ChipID"].ToString().Trim()
                               + " " + dt.Rows[i]["SmartCardID"].ToString().Trim() + " " + dt.Rows[i]["STBID"].ToString().Trim() + "\r\n";

                            }
                            else if (cbox_BroadCaster.Text.Trim() == "SARFT")//户户通
                            {
                                content = dt.Rows[i]["CAID"].ToString().Trim() + " " + dt.Rows[i]["ChipID"].ToString().Trim()
                               + " " + dt.Rows[i]["STBID"].ToString().Trim() + "\r\n";

                            }
                            sw.Write(content);
                            ChipIndex += dt.Rows[i]["ChipID"].ToString().Trim() + ",";
                        }
                    }
                    catch (Exception ex)
                    {
                        HDIC_Message.ShowWarnDialog(this, "创建文件时发生错误，原因是：" + ex.ToString());
                        return;
                    }
                    sw.Close();
                    //////////////////////////////////////////////////////////////////////////
                    fsFile = new FileStream(SaveFileName + "_2", FileMode.Open);

                    sw = new StreamWriter(fsFile, System.Text.Encoding.ASCII);
                    try
                    {
                        sw.Write(ContentTitle);

                        string content = "";
                        //第六行之后的每一行列出了机顶盒的加密序列号＋空格＋芯片序列号+空格+智能卡号＋空格＋机顶盒序列号
                        for (int i = 100000; i < dt.Rows.Count; i++)
                        {
                            if (cbox_BroadCaster.Text.Trim() == "DTHUMC")//村村通
                            {
                                content = dt.Rows[i]["CAID"].ToString().Trim() + " " + dt.Rows[i]["ChipID"].ToString().Trim()
                               + " " + dt.Rows[i]["SmartCardID"].ToString().Trim() + " " + dt.Rows[i]["STBID"].ToString().Trim() + "\r\n";

                            }
                            else if (cbox_BroadCaster.Text.Trim() == "SARFT")//户户通
                            {
                                content = dt.Rows[i]["CAID"].ToString().Trim() + " " + dt.Rows[i]["ChipID"].ToString().Trim()
                               + " " + dt.Rows[i]["STBID"].ToString().Trim() + "\r\n";

                            }
                            sw.Write(content);
                            ChipIndex += dt.Rows[i]["ChipID"].ToString().Trim() + ",";
                        }
                    }
                    catch (Exception ex)
                    {
                        HDIC_Message.ShowWarnDialog(this, "创建文件时发生错误，原因是：" + ex.ToString());
                        return;
                    }
                    sw.Close();
                    myStream.Close();
                }
                #endregion


                //修改数据库
                if (ChipIndex.Trim() != "")
                {
                    ChipIndex = ChipIndex.Substring(0, ChipIndex.LastIndexOf(','));
                    if (HDIC_DB.ExcuteNonQuery("update STBData set Flag=1 where ChipID in (" + ChipIndex + ")", null) <= 0)
                    {
                        HDIC_Message.ShowWarnDialog(this, "数据库更新失败");
                        return;
                    }
                }

                HDIC_Message.ShowWarnDialog(this, "导出数据成功！");
            }
        }

        //关闭本窗体，自动关闭返回文件创建
        private void tst_Exit_Click(object sender, EventArgs e)
        {
            if (td != null && td.IsAlive)
            {
                HDIC_Message.ShowWarnDialog(this, "正在将数据导入到数据库，请等待……");
                return;
            }
            this.Close();
        }

        private void Export_file_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (td != null && td.IsAlive)
            {
                HDIC_Message.ShowWarnDialog(this, "正在将数据导入到数据库，请等待……");
                return;
            }
            e.Cancel = false;
        }
    }
}
