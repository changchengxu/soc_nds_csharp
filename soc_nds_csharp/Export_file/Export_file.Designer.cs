﻿namespace soc_nds_csharp.Export_file
{
    partial class Export_file
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Export_file));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.btn_Import = new System.Windows.Forms.Button();
            this.txt_STBModel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_STBChip = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_BroadCaster = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_procBatch = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_ManufacturerID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_ModelID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_HardwareID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_ExportData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tst_Exit = new System.Windows.Forms.ToolStripButton();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.74568F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.25432F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 141F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.78873F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.21127F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(764, 589);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.txt_HardwareID);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txt_ModelID);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txt_ManufacturerID);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txt_procBatch);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txt_BroadCaster);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txt_STBChip);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txt_STBModel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btn_Import);
            this.panel1.Controls.Add(this.txt_name);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(163, 78);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(456, 429);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "生产商名字:";
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(129, 38);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(153, 21);
            this.txt_name.TabIndex = 1;
            // 
            // btn_Import
            // 
            this.btn_Import.Location = new System.Drawing.Point(288, 36);
            this.btn_Import.Name = "btn_Import";
            this.btn_Import.Size = new System.Drawing.Size(75, 23);
            this.btn_Import.TabIndex = 2;
            this.btn_Import.Text = " 导 入 ";
            this.btn_Import.UseVisualStyleBackColor = true;
            this.btn_Import.Click += new System.EventHandler(this.btn_Import_Click);
            // 
            // txt_STBModel
            // 
            this.txt_STBModel.Location = new System.Drawing.Point(129, 81);
            this.txt_STBModel.Name = "txt_STBModel";
            this.txt_STBModel.Size = new System.Drawing.Size(237, 21);
            this.txt_STBModel.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = " STB Model:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "生产日期:";
            // 
            // txt_STBChip
            // 
            this.txt_STBChip.Location = new System.Drawing.Point(129, 160);
            this.txt_STBChip.Name = "txt_STBChip";
            this.txt_STBChip.Size = new System.Drawing.Size(237, 21);
            this.txt_STBChip.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "STB Chip:";
            // 
            // txt_BroadCaster
            // 
            this.txt_BroadCaster.Location = new System.Drawing.Point(129, 199);
            this.txt_BroadCaster.Name = "txt_BroadCaster";
            this.txt_BroadCaster.Size = new System.Drawing.Size(237, 21);
            this.txt_BroadCaster.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "BroadCaster:";
            // 
            // txt_procBatch
            // 
            this.txt_procBatch.Location = new System.Drawing.Point(129, 242);
            this.txt_procBatch.Name = "txt_procBatch";
            this.txt_procBatch.Size = new System.Drawing.Size(237, 21);
            this.txt_procBatch.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "生产批次:";
            // 
            // txt_ManufacturerID
            // 
            this.txt_ManufacturerID.Location = new System.Drawing.Point(129, 279);
            this.txt_ManufacturerID.Name = "txt_ManufacturerID";
            this.txt_ManufacturerID.Size = new System.Drawing.Size(237, 21);
            this.txt_ManufacturerID.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 284);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "ManufacturerID:";
            // 
            // txt_ModelID
            // 
            this.txt_ModelID.Location = new System.Drawing.Point(129, 327);
            this.txt_ModelID.Name = "txt_ModelID";
            this.txt_ModelID.Size = new System.Drawing.Size(237, 21);
            this.txt_ModelID.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(49, 332);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 15;
            this.label8.Text = "ModelID:";
            // 
            // txt_HardwareID
            // 
            this.txt_HardwareID.Location = new System.Drawing.Point(129, 362);
            this.txt_HardwareID.Name = "txt_HardwareID";
            this.txt_HardwareID.Size = new System.Drawing.Size(237, 21);
            this.txt_HardwareID.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(31, 367);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "HardwareID:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_ExportData,
            this.toolStripSeparator1,
            this.tst_Exit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(764, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_ExportData
            // 
            this.tsb_ExportData.Image = ((System.Drawing.Image)(resources.GetObject("tsb_ExportData.Image")));
            this.tsb_ExportData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_ExportData.Name = "tsb_ExportData";
            this.tsb_ExportData.Size = new System.Drawing.Size(75, 22);
            this.tsb_ExportData.Text = "数据导出";
            this.tsb_ExportData.Click += new System.EventHandler(this.tsb_ExportData_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tst_Exit
            // 
            this.tst_Exit.Image = ((System.Drawing.Image)(resources.GetObject("tst_Exit.Image")));
            this.tst_Exit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tst_Exit.Name = "tst_Exit";
            this.tst_Exit.Size = new System.Drawing.Size(51, 22);
            this.tst_Exit.Text = "退出";
            this.tst_Exit.Click += new System.EventHandler(this.tst_Exit_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(129, 118);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(237, 21);
            this.dateTimePicker1.TabIndex = 22;
            // 
            // Export_file
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 589);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Export_file";
            this.Text = "导出返回文件";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_Import;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox txt_HardwareID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_ModelID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_ManufacturerID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_procBatch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_BroadCaster;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_STBChip;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_STBModel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_ExportData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tst_Exit;
    }
}