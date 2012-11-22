namespace soc_nds_csharp.DB_Manage
{
    partial class gf_Barcode
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
            this.Print = new System.Windows.Forms.Button();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkGenerateLabel = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEncode = new System.Windows.Forms.Button();
            this.cbEncodeType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEncoded = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslEncodedType = new System.Windows.Forms.ToolStripStatusLabel();
            this.pic_barcode = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_barcode)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Print
            // 
            this.Print.Location = new System.Drawing.Point(176, 100);
            this.Print.Name = "Print";
            this.Print.Size = new System.Drawing.Size(53, 23);
            this.Print.TabIndex = 45;
            this.Print.Text = "Print";
            this.Print.UseVisualStyleBackColor = true;
            this.Print.Click += new System.EventHandler(this.Print_Click);
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(114, 300);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(91, 21);
            this.txtHeight.TabIndex = 44;
            this.txtHeight.Text = "100";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(111, 286);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 42;
            this.label6.Text = "Height";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.Print);
            this.groupBox1.Controls.Add(this.txtHeight);
            this.groupBox1.Controls.Add(this.txtWidth);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.chkGenerateLabel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnEncode);
            this.groupBox1.Controls.Add(this.cbEncodeType);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtData);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtEncoded);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 345);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Information";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(16, 300);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(90, 21);
            this.txtWidth.TabIndex = 43;
            this.txtWidth.Text = "300";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 286);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 41;
            this.label7.Text = "Width";
            // 
            // chkGenerateLabel
            // 
            this.chkGenerateLabel.AutoSize = true;
            this.chkGenerateLabel.Location = new System.Drawing.Point(115, 20);
            this.chkGenerateLabel.Name = "chkGenerateLabel";
            this.chkGenerateLabel.Size = new System.Drawing.Size(108, 16);
            this.chkGenerateLabel.TabIndex = 40;
            this.chkGenerateLabel.Text = "Generate label";
            this.chkGenerateLabel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 35;
            this.label3.Text = "Encoding";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(96, 101);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 21);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save As";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEncode
            // 
            this.btnEncode.Location = new System.Drawing.Point(15, 101);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(75, 21);
            this.btnEncode.TabIndex = 3;
            this.btnEncode.Text = "&Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // cbEncodeType
            // 
            this.cbEncodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEncodeType.FormattingEnabled = true;
            this.cbEncodeType.ItemHeight = 12;
            this.cbEncodeType.Items.AddRange(new object[] {
            "128",
            "128M",
            "EAN128",
            "25",
            "25C",
            "39",
            "39C",
            "93",
            "EAN13",
            "EAN13+2",
            "EAN13+5",
            "EAN8",
            "EAN8+2",
            "EAN8+5",
            "CODA",
            "POST",
            "UPCA",
            "UPCA+2",
            "UPCA+5",
            "UPCE",
            "UPCE+2",
            "UPCE+5"});
            this.cbEncodeType.Location = new System.Drawing.Point(16, 76);
            this.cbEncodeType.Name = "cbEncodeType";
            this.cbEncodeType.Size = new System.Drawing.Size(121, 20);
            this.cbEncodeType.TabIndex = 2;
            this.cbEncodeType.SelectedIndexChanged += new System.EventHandler(this.cbEncodeType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 33;
            this.label2.Text = "Encoded Value";
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(15, 38);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(188, 21);
            this.txtData.TabIndex = 1;
            this.txtData.Text = "400668800021";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 29;
            this.label1.Text = "Value to Encode";
            // 
            // txtEncoded
            // 
            this.txtEncoded.Location = new System.Drawing.Point(16, 143);
            this.txtEncoded.Multiline = true;
            this.txtEncoded.Name = "txtEncoded";
            this.txtEncoded.ReadOnly = true;
            this.txtEncoded.Size = new System.Drawing.Size(187, 89);
            this.txtEncoded.TabIndex = 14;
            this.txtEncoded.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslEncodedType});
            this.statusStrip1.Location = new System.Drawing.Point(0, 417);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(902, 22);
            this.statusStrip1.TabIndex = 42;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslEncodedType
            // 
            this.tsslEncodedType.Name = "tsslEncodedType";
            this.tsslEncodedType.Size = new System.Drawing.Size(118, 17);
            this.tsslEncodedType.Text = "toolStripStatusLabel1";
            // 
            // pic_barcode
            // 
            this.pic_barcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pic_barcode.BackColor = System.Drawing.Color.Transparent;
            this.pic_barcode.Location = new System.Drawing.Point(169, 157);
            this.pic_barcode.Name = "pic_barcode";
            this.pic_barcode.Size = new System.Drawing.Size(283, 65);
            this.pic_barcode.TabIndex = 13;
            this.pic_barcode.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.pic_barcode);
            this.groupBox2.Location = new System.Drawing.Point(259, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(628, 333);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Barcode Image";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.Image = global::soc_nds_csharp.Properties.Resources.HDICLogo2;
            this.pictureBox1.Location = new System.Drawing.Point(185, 64);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(246, 87);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // gf_Barcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 439);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Name = "gf_Barcode";
            this.Text = "gf_Barcode";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_barcode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Print;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkGenerateLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEncode;
        private System.Windows.Forms.ComboBox cbEncodeType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEncoded;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslEncodedType;
        private System.Windows.Forms.PictureBox pic_barcode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}