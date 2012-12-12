namespace soc_nds_csharp.Station_Operation
{
    partial class gf_ReadChipID
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_ReadChipID = new System.Windows.Forms.Button();
            this.richtxt_info = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.17981F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.82019F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 179F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.17007F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.82993F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 149F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(804, 637);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_ReadChipID);
            this.panel1.Controls.Add(this.richtxt_info);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(185, 125);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(436, 359);
            this.panel1.TabIndex = 0;
            // 
            // btn_ReadChipID
            // 
            this.btn_ReadChipID.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ReadChipID.Location = new System.Drawing.Point(77, 207);
            this.btn_ReadChipID.Name = "btn_ReadChipID";
            this.btn_ReadChipID.Size = new System.Drawing.Size(298, 53);
            this.btn_ReadChipID.TabIndex = 3;
            this.btn_ReadChipID.Text = " 读 取 Chip ID";
            this.btn_ReadChipID.UseVisualStyleBackColor = true;
            this.btn_ReadChipID.Click += new System.EventHandler(this.btn_ReadChipID_Click);
            // 
            // richtxt_info
            // 
            this.richtxt_info.Location = new System.Drawing.Point(33, 116);
            this.richtxt_info.Name = "richtxt_info";
            this.richtxt_info.Size = new System.Drawing.Size(376, 75);
            this.richtxt_info.TabIndex = 2;
            this.richtxt_info.Text = "";
            // 
            // gf_ReadChipID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 637);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "gf_ReadChipID";
            this.Text = "读取 ChipID";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_ReadChipID;
        private System.Windows.Forms.RichTextBox richtxt_info;

    }
}