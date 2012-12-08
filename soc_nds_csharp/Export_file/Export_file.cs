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
using System.IO;

namespace soc_nds_csharp.Export_file
{
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
        }
        //#region 保存文件路径
        //string path; 
        //private void btn_SavePath_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog OpenFile = new OpenFileDialog();
        //    OpenFile.Title = "打开";
        //    OpenFile.InitialDirectory = @"桌面";
        //    OpenFile.Filter = "文本文件(*.txt|*.txt;*.doc)";
        //    DialogResult drResult = OpenFile.ShowDialog();
        //    if (drResult == DialogResult.OK)
        //        path = OpenFile.FileName;
        //    else
        //        return;

        //    txt_SavePath.Text = path;
        //}
        //#endregion

        //导出数据
        Stream myStream;
        StreamWriter sw;
        private void tsb_ExportData_Click(object sender, EventArgs e)
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
            //dr[4] = txt_BroadCaster.Text.Trim();
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
            string SaveFileName = "RetFile_DTHUMC_" + txt_ManufacturerName.Text.Trim() + "_" + txt_STBModel.Text.Trim() + "_" + Date+".txt";
            //前两行表示文件类型
            string content = "STB_vendor_to_DTHUMC\r\nver101\r\n";
            //第三行为生产的日期和时间
            content += dateTimePicker1.Value.ToString("ddd,dd MMM yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-us"))+"\r\n";
            //第四行用于描述机顶盒，必须包括"<STB model> with <STB chip> for <broadcaster>
            content += txt_STBModel.Text.Trim() + " with " + txt_STBChip.Text.Trim() + " for " + txt_BroadCaster.Text.Trim() + "\r\n";
            //第五行用于描述机顶盒制造商的生产批次
            content += txt_procBatch.Text.Trim() + "\r\n";
            //第六行为机顶盒平台标识，包括<Manufacturer ID>、<Model ID>和<Hardware ID>
            content += txt_ManufacturerID.Text.Trim() + txt_ModelID.Text.Trim() + txt_HardwareID.Text.Trim()+"\r\n";

           

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

#region 3
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "文本格式.txt|.txt";
            save.Title = "导出文件到";
            save.FileName=SaveFileName;
            if (save.ShowDialog() == DialogResult.OK)
            {
                 myStream = save.OpenFile();
                 //sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("GB2312"));
                 sw = new StreamWriter(myStream, System.Text.Encoding.ASCII); 
                try
                {
                    sw.Write(content);
                    //第六行之后的每一行列出了机顶盒的加密序列号＋空格＋芯片序列号+空格+智能卡号＋空格＋机顶盒序列号
                    DataTable dt = HDIC_DB.GetList("select * from STBData");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        content = dt.Rows[0]["CAID"].ToString().Trim() + " " + dt.Rows[0]["ChipID"].ToString().Trim()
                            + " " + dt.Rows[0]["SmartCardID"].ToString().Trim() + " " + dt.Rows[0]["STBID"].ToString().Trim() + 0x0A;

                        sw.Write(content);
                    }
                }
                catch
                {
                    sw.Close();
                    myStream.Close();
                }
            }
#endregion
        }

        //关闭本窗体，自动关闭返回文件创建
        private void tst_Exit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("导出数据成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            sw.Close();
            myStream.Close();

            this.Close();
        }

        private void Export_file_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
    }
}
