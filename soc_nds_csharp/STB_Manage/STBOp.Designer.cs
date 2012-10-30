namespace soc_nds_csharp.STB_Manage
{
    partial class STBOp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(STBOp));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_Newfrm = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_Editfrm = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_Delete = new System.Windows.Forms.ToolStripButton();
            this.dgv_STB = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_STB)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_Newfrm,
            this.toolStripSeparator1,
            this.tsb_Editfrm,
            this.toolStripSeparator2,
            this.tsb_Delete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(675, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_Newfrm
            // 
            this.tsb_Newfrm.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Newfrm.Image")));
            this.tsb_Newfrm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Newfrm.Name = "tsb_Newfrm";
            this.tsb_Newfrm.Size = new System.Drawing.Size(51, 22);
            this.tsb_Newfrm.Text = "添加";
            this.tsb_Newfrm.Click += new System.EventHandler(this.tsb_Newfrm_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_Editfrm
            // 
            this.tsb_Editfrm.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Editfrm.Image")));
            this.tsb_Editfrm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Editfrm.Name = "tsb_Editfrm";
            this.tsb_Editfrm.Size = new System.Drawing.Size(51, 22);
            this.tsb_Editfrm.Text = "修改";
            this.tsb_Editfrm.Click += new System.EventHandler(this.tsb_Editfrm_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_Delete
            // 
            this.tsb_Delete.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Delete.Image")));
            this.tsb_Delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Delete.Name = "tsb_Delete";
            this.tsb_Delete.Size = new System.Drawing.Size(51, 22);
            this.tsb_Delete.Text = "删除";
            this.tsb_Delete.Click += new System.EventHandler(this.tsb_Delete_Click);
            // 
            // dgv_STB
            // 
            this.dgv_STB.AllowUserToAddRows = false;
            this.dgv_STB.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_STB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_STB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_STB.Location = new System.Drawing.Point(0, 25);
            this.dgv_STB.Name = "dgv_STB";
            this.dgv_STB.ReadOnly = true;
            this.dgv_STB.RowTemplate.Height = 23;
            this.dgv_STB.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_STB.Size = new System.Drawing.Size(675, 391);
            this.dgv_STB.TabIndex = 1;
            // 
            // STBOp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 416);
            this.Controls.Add(this.dgv_STB);
            this.Controls.Add(this.toolStrip1);
            this.Name = "STBOp";
            this.Text = "STBOp";
            this.Load += new System.EventHandler(this.STBClient_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_STB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_Newfrm;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_Editfrm;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsb_Delete;
        private System.Windows.Forms.DataGridView dgv_STB;
    }
}