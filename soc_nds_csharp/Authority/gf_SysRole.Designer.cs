namespace soc_nds_csharp.Authority
{
    partial class gf_SysRole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gf_SysRole));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAdd = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbModify = new System.Windows.Forms.ToolStripButton();
            this.tsbQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSelectOpt = new System.Windows.Forms.ToolStripButton();
            this.panel_Query = new System.Windows.Forms.Panel();
            this.grb_Query = new System.Windows.Forms.GroupBox();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_QueryHidden = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_Edit = new System.Windows.Forms.Panel();
            this.grb_Edit = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btn_EditHide = new System.Windows.Forms.Button();
            this.txtremark = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtroleName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgv_SysRole = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            this.panel_Query.SuspendLayout();
            this.grb_Query.SuspendLayout();
            this.panel_Edit.SuspendLayout();
            this.grb_Edit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SysRole)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAdd,
            this.tsbDelete,
            this.tsbModify,
            this.tsbQuery,
            this.toolStripSeparator1,
            this.tsbSelectOpt});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(909, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbAdd
            // 
            this.tsbAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsbAdd.Image")));
            this.tsbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAdd.Name = "tsbAdd";
            this.tsbAdd.Size = new System.Drawing.Size(51, 22);
            this.tsbAdd.Text = "添加";
            this.tsbAdd.Click += new System.EventHandler(this.tsbAdd_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(51, 22);
            this.tsbDelete.Text = "删除";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbModify
            // 
            this.tsbModify.Image = ((System.Drawing.Image)(resources.GetObject("tsbModify.Image")));
            this.tsbModify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbModify.Name = "tsbModify";
            this.tsbModify.Size = new System.Drawing.Size(51, 22);
            this.tsbModify.Text = "修改";
            this.tsbModify.Click += new System.EventHandler(this.tsbModify_Click);
            // 
            // tsbQuery
            // 
            this.tsbQuery.Image = ((System.Drawing.Image)(resources.GetObject("tsbQuery.Image")));
            this.tsbQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbQuery.Name = "tsbQuery";
            this.tsbQuery.Size = new System.Drawing.Size(51, 22);
            this.tsbQuery.Text = "查询";
            this.tsbQuery.Click += new System.EventHandler(this.tsbQuery_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSelectOpt
            // 
            this.tsbSelectOpt.Image = ((System.Drawing.Image)(resources.GetObject("tsbSelectOpt.Image")));
            this.tsbSelectOpt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSelectOpt.Name = "tsbSelectOpt";
            this.tsbSelectOpt.Size = new System.Drawing.Size(75, 22);
            this.tsbSelectOpt.Text = "选择权限";
            this.tsbSelectOpt.Click += new System.EventHandler(this.tsbSelectOpt_Click);
            // 
            // panel_Query
            // 
            this.panel_Query.Controls.Add(this.grb_Query);
            this.panel_Query.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Query.Location = new System.Drawing.Point(0, 25);
            this.panel_Query.Name = "panel_Query";
            this.panel_Query.Size = new System.Drawing.Size(909, 68);
            this.panel_Query.TabIndex = 3;
            // 
            // grb_Query
            // 
            this.grb_Query.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grb_Query.Controls.Add(this.txt_search);
            this.grb_Query.Controls.Add(this.btn_QueryHidden);
            this.grb_Query.Controls.Add(this.btnQuery);
            this.grb_Query.Controls.Add(this.label1);
            this.grb_Query.Location = new System.Drawing.Point(10, 3);
            this.grb_Query.Name = "grb_Query";
            this.grb_Query.Size = new System.Drawing.Size(888, 55);
            this.grb_Query.TabIndex = 0;
            this.grb_Query.TabStop = false;
            this.grb_Query.Text = "查询";
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(88, 20);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(191, 21);
            this.txt_search.TabIndex = 10;
            // 
            // btn_QueryHidden
            // 
            this.btn_QueryHidden.BackColor = System.Drawing.Color.Transparent;
            this.btn_QueryHidden.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_QueryHidden.Image = ((System.Drawing.Image)(resources.GetObject("btn_QueryHidden.Image")));
            this.btn_QueryHidden.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_QueryHidden.Location = new System.Drawing.Point(386, 19);
            this.btn_QueryHidden.Name = "btn_QueryHidden";
            this.btn_QueryHidden.Size = new System.Drawing.Size(60, 23);
            this.btn_QueryHidden.TabIndex = 14;
            this.btn_QueryHidden.Text = "隐藏";
            this.btn_QueryHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_QueryHidden.UseVisualStyleBackColor = false;
            this.btn_QueryHidden.Click += new System.EventHandler(this.btn_QueryHidden_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.BackColor = System.Drawing.Color.Transparent;
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnQuery.Image")));
            this.btnQuery.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnQuery.Location = new System.Drawing.Point(304, 19);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(60, 23);
            this.btnQuery.TabIndex = 12;
            this.btnQuery.Text = "查询";
            this.btnQuery.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnQuery.UseVisualStyleBackColor = false;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "角色名称：";
            // 
            // panel_Edit
            // 
            this.panel_Edit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Edit.Controls.Add(this.grb_Edit);
            this.panel_Edit.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Edit.Location = new System.Drawing.Point(0, 93);
            this.panel_Edit.Name = "panel_Edit";
            this.panel_Edit.Size = new System.Drawing.Size(909, 73);
            this.panel_Edit.TabIndex = 4;
            // 
            // grb_Edit
            // 
            this.grb_Edit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grb_Edit.Controls.Add(this.btnSave);
            this.grb_Edit.Controls.Add(this.btn_EditHide);
            this.grb_Edit.Controls.Add(this.txtremark);
            this.grb_Edit.Controls.Add(this.label4);
            this.grb_Edit.Controls.Add(this.txtroleName);
            this.grb_Edit.Controls.Add(this.label3);
            this.grb_Edit.Location = new System.Drawing.Point(9, 3);
            this.grb_Edit.Name = "grb_Edit";
            this.grb_Edit.Size = new System.Drawing.Size(886, 58);
            this.grb_Edit.TabIndex = 0;
            this.grb_Edit.TabStop = false;
            this.grb_Edit.Text = "编辑信息";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(675, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 21);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "保存";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btn_EditHide
            // 
            this.btn_EditHide.BackColor = System.Drawing.Color.Transparent;
            this.btn_EditHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_EditHide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_EditHide.Location = new System.Drawing.Point(776, 20);
            this.btn_EditHide.Name = "btn_EditHide";
            this.btn_EditHide.Size = new System.Drawing.Size(60, 21);
            this.btn_EditHide.TabIndex = 16;
            this.btn_EditHide.Text = " 隐 藏 ";
            this.btn_EditHide.UseVisualStyleBackColor = false;
            this.btn_EditHide.Click += new System.EventHandler(this.btn_EditHide_Click);
            // 
            // txtremark
            // 
            this.txtremark.Location = new System.Drawing.Point(280, 18);
            this.txtremark.Name = "txtremark";
            this.txtremark.Size = new System.Drawing.Size(388, 21);
            this.txtremark.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(245, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "备注";
            // 
            // txtroleName
            // 
            this.txtroleName.Location = new System.Drawing.Point(83, 19);
            this.txtroleName.Name = "txtroleName";
            this.txtroleName.Size = new System.Drawing.Size(140, 21);
            this.txtroleName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "角色名称";
            // 
            // dgv_SysRole
            // 
            this.dgv_SysRole.AllowUserToAddRows = false;
            this.dgv_SysRole.AllowUserToDeleteRows = false;
            this.dgv_SysRole.AllowUserToOrderColumns = true;
            this.dgv_SysRole.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_SysRole.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_SysRole.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_SysRole.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_SysRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_SysRole.Location = new System.Drawing.Point(0, 166);
            this.dgv_SysRole.Name = "dgv_SysRole";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_SysRole.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_SysRole.RowHeadersWidth = 15;
            this.dgv_SysRole.RowTemplate.Height = 23;
            this.dgv_SysRole.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_SysRole.Size = new System.Drawing.Size(909, 403);
            this.dgv_SysRole.TabIndex = 5;
            // 
            // gf_SysRole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 569);
            this.Controls.Add(this.dgv_SysRole);
            this.Controls.Add(this.panel_Edit);
            this.Controls.Add(this.panel_Query);
            this.Controls.Add(this.toolStrip1);
            this.Name = "gf_SysRole";
            this.Text = "角色权限";
            this.Load += new System.EventHandler(this.gf_SysRole_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel_Query.ResumeLayout(false);
            this.grb_Query.ResumeLayout(false);
            this.grb_Query.PerformLayout();
            this.panel_Edit.ResumeLayout(false);
            this.grb_Edit.ResumeLayout(false);
            this.grb_Edit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SysRole)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAdd;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbModify;
        private System.Windows.Forms.ToolStripButton tsbQuery;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbSelectOpt;
        private System.Windows.Forms.Panel panel_Query;
        private System.Windows.Forms.GroupBox grb_Query;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_QueryHidden;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel_Edit;
        private System.Windows.Forms.GroupBox grb_Edit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btn_EditHide;
        private System.Windows.Forms.TextBox txtremark;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtroleName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgv_SysRole;

    }
}