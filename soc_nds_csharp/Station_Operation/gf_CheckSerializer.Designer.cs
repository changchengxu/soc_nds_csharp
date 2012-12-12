namespace soc_nds_csharp.Station_Operation
{
    partial class gf_CheckSerializer
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_SmartCardID = new System.Windows.Forms.TextBox();
            this.btn_begin = new System.Windows.Forms.Button();
            this.richtxt_Tips = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_CAID = new System.Windows.Forms.TextBox();
            this.richtxt_LincenseAdo = new System.Windows.Forms.RichTextBox();
            this.richtxt_LincenseBoard = new System.Windows.Forms.RichTextBox();
            this.txt_STBID = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.97947F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.02053F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.608696F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.3913F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(919, 734);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_STBID);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txt_SmartCardID);
            this.panel1.Controls.Add(this.btn_begin);
            this.panel1.Controls.Add(this.richtxt_Tips);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txt_CAID);
            this.panel1.Controls.Add(this.richtxt_LincenseAdo);
            this.panel1.Controls.Add(this.richtxt_LincenseBoard);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(199, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(526, 613);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(83, 353);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "扫描机顶盒STBID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 387);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "扫描智能卡号 SmartCardID:";
            // 
            // txt_SmartCardID
            // 
            this.txt_SmartCardID.Location = new System.Drawing.Point(201, 384);
            this.txt_SmartCardID.Name = "txt_SmartCardID";
            this.txt_SmartCardID.Size = new System.Drawing.Size(278, 21);
            this.txt_SmartCardID.TabIndex = 13;
            this.txt_SmartCardID.TextChanged += new System.EventHandler(this.txt_SmartCardID_TextChanged);
            // 
            // btn_begin
            // 
            this.btn_begin.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_begin.Location = new System.Drawing.Point(150, 530);
            this.btn_begin.Name = "btn_begin";
            this.btn_begin.Size = new System.Drawing.Size(222, 46);
            this.btn_begin.TabIndex = 8;
            this.btn_begin.Text = " 开 始 ";
            this.btn_begin.UseVisualStyleBackColor = true;
            this.btn_begin.Click += new System.EventHandler(this.btn_begin_Click);
            this.btn_begin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_begin_KeyDown);
            // 
            // richtxt_Tips
            // 
            this.richtxt_Tips.Location = new System.Drawing.Point(5, 425);
            this.richtxt_Tips.Name = "richtxt_Tips";
            this.richtxt_Tips.Size = new System.Drawing.Size(513, 93);
            this.richtxt_Tips.TabIndex = 11;
            this.richtxt_Tips.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 318);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "扫描机顶盒标签上CAID:";
            // 
            // txt_CAID
            // 
            this.txt_CAID.Location = new System.Drawing.Point(202, 313);
            this.txt_CAID.Name = "txt_CAID";
            this.txt_CAID.Size = new System.Drawing.Size(278, 21);
            this.txt_CAID.TabIndex = 10;
            this.txt_CAID.TextChanged += new System.EventHandler(this.txt_CAID_TextChanged);
            // 
            // richtxt_LincenseAdo
            // 
            this.richtxt_LincenseAdo.Location = new System.Drawing.Point(5, 163);
            this.richtxt_LincenseAdo.Name = "richtxt_LincenseAdo";
            this.richtxt_LincenseAdo.Size = new System.Drawing.Size(513, 131);
            this.richtxt_LincenseAdo.TabIndex = 7;
            this.richtxt_LincenseAdo.Text = "";
            // 
            // richtxt_LincenseBoard
            // 
            this.richtxt_LincenseBoard.Location = new System.Drawing.Point(5, 24);
            this.richtxt_LincenseBoard.Name = "richtxt_LincenseBoard";
            this.richtxt_LincenseBoard.Size = new System.Drawing.Size(513, 131);
            this.richtxt_LincenseBoard.TabIndex = 6;
            this.richtxt_LincenseBoard.Text = "";
            // 
            // txt_STBID
            // 
            this.txt_STBID.Location = new System.Drawing.Point(201, 349);
            this.txt_STBID.Name = "txt_STBID";
            this.txt_STBID.Size = new System.Drawing.Size(278, 21);
            this.txt_STBID.TabIndex = 17;
            this.txt_STBID.TextChanged += new System.EventHandler(this.txt_STBID_TextChanged);
            // 
            // gf_CheckSerializer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 734);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "gf_CheckSerializer";
            this.Text = "校验序列化和高级安全";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.gf_CheckSerializer_FormClosing);
            this.Load += new System.EventHandler(this.gf_CheckSerializer_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_begin;
        private System.Windows.Forms.RichTextBox richtxt_Tips;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_CAID;
        private System.Windows.Forms.RichTextBox richtxt_LincenseAdo;
        private System.Windows.Forms.RichTextBox richtxt_LincenseBoard;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_SmartCardID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_STBID;

    }
}