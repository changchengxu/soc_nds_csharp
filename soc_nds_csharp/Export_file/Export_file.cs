using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.Export;

namespace soc_nds_csharp.Export_file
{
    public partial class Export_file : Form
    {
        public Export_file()
        {
            InitializeComponent();
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
        private void tsb_ExportData_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("生产商名字", typeof(string)));
            dt.Columns.Add(new DataColumn(" STB Model", typeof(string)));
            dt.Columns.Add(new DataColumn("生产日期", typeof(string)));
            dt.Columns.Add(new DataColumn("STB Chip", typeof(string)));
            dt.Columns.Add(new DataColumn("BroadCaster", typeof(string)));
            dt.Columns.Add(new DataColumn("生产批次", typeof(string)));
            dt.Columns.Add(new DataColumn("ManufacturerID", typeof(string)));
            dt.Columns.Add(new DataColumn("ModelID", typeof(string)));
            dt.Columns.Add(new DataColumn("HardwareID", typeof(string)));

            DataRow dr = dt.NewRow();
            dr[0] = txt_name.Text.Trim();
            dr[1] = txt_STBModel.Text.Trim();
            dr[2] = dateTimePicker1.Value;
            dr[3] = txt_STBChip.Text.Trim();
            dr[4] = txt_BroadCaster.Text.Trim();
            dr[5] = txt_procBatch.Text.Trim();
            dr[6] = txt_ManufacturerID.Text.Trim();
            dr[7] = txt_ModelID.Text.Trim();
            dr[8] = txt_HardwareID.Text.Trim();
            dt.Rows.Add(dr);
            HDIC_Export.outPutDataSet(dt, "导出数据");
        }

        private void tst_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
