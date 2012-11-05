namespace soc_nds_csharp.Authority
{
    partial class gf_SysUser
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gf_SysUser));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_Add = new System.Windows.Forms.ToolStripButton();
            this.tsb_Edit = new System.Windows.Forms.ToolStripButton();
            this.tsb_Delete = new System.Windows.Forms.ToolStripButton();
            this.tsb_Exit = new System.Windows.Forms.ToolStripButton();
            this.dgv_SysUser = new System.Windows.Forms.DataGridView();
            this.panel_Edit = new System.Windows.Forms.Panel();
            this.grb_Edit = new System.Windows.Forms.GroupBox();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_EditHide = new System.Windows.Forms.Button();
            this.txt_Remark = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_UserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel_Query = new System.Windows.Forms.Panel();
            this.grb_Query = new System.Windows.Forms.GroupBox();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_QueryHidden = new System.Windows.Forms.Button();
            this.btn_Query = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_UserPwd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_UserPwdAgain = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbo_UserRole = new System.Windows.Forms.ComboBox();
            this.tsb_Query = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SysUser)).BeginInit();
            this.panel_Edit.SuspendLayout();
            this.grb_Edit.SuspendLayout();
            this.panel_Query.SuspendLayout();
            this.grb_Query.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_Add,
            this.toolStripSeparator1,
            this.tsb_Edit,
            this.toolStripSeparator2,
            this.tsb_Delete,
            this.toolStripSeparator3,
            this.tsb_Query,
            this.toolStripSeparator4,
            this.tsb_Exit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(873, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_Add
            // 
            this.tsb_Add.Image = global::soc_nds_csharp.Properties.Resources.save;
            this.tsb_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Add.Name = "tsb_Add";
            this.tsb_Add.Size = new System.Drawing.Size(51, 22);
            this.tsb_Add.Text = "添加";
            this.tsb_Add.Click += new System.EventHandler(this.tsb_Add_Click);
            // 
            // tsb_Edit
            // 
            this.tsb_Edit.Image = global::soc_nds_csharp.Properties.Resources.edit;
            this.tsb_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Edit.Name = "tsb_Edit";
            this.tsb_Edit.Size = new System.Drawing.Size(51, 22);
            this.tsb_Edit.Text = "修改";
            this.tsb_Edit.Click += new System.EventHandler(this.tsb_Edit_Click);
            // 
            // tsb_Delete
            // 
            this.tsb_Delete.Image = global::soc_nds_csharp.Properties.Resources.delete;
            this.tsb_Delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Delete.Name = "tsb_Delete";
            this.tsb_Delete.Size = new System.Drawing.Size(51, 22);
            this.tsb_Delete.Text = "删除";
            this.tsb_Delete.Click += new System.EventHandler(this.tsb_Delete_Click);
            // 
            // tsb_Exit
            // 
            this.tsb_Exit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Exit.Name = "tsb_Exit";
            this.tsb_Exit.Size = new System.Drawing.Size(35, 22);
            this.tsb_Exit.Text = "退出";
            this.tsb_Exit.Click += new System.EventHandler(this.tsb_Exit_Click);
            // 
            // dgv_SysUser
            // 
            this.dgv_SysUser.AllowUserToAddRows = false;
            this.dgv_SysUser.AllowUserToDeleteRows = false;
            this.dgv_SysUser.AllowUserToOrderColumns = true;
            this.dgv_SysUser.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_SysUser.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_SysUser.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_SysUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_SysUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_SysUser.Location = new System.Drawing.Point(0, 186);
            this.dgv_SysUser.Name = "dgv_SysUser";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_SysUser.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgv_SysUser.RowHeadersWidth = 15;
            this.dgv_SysUser.RowTemplate.Height = 23;
            this.dgv_SysUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_SysUser.Size = new System.Drawing.Size(873, 311);
            this.dgv_SysUser.TabIndex = 8;
            // 
            // panel_Edit
            // 
            this.panel_Edit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Edit.Controls.Add(this.grb_Edit);
            this.panel_Edit.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Edit.Location = new System.Drawing.Point(0, 93);
            this.panel_Edit.Name = "panel_Edit";
            this.panel_Edit.Size = new System.Drawing.Size(873, 93);
            this.panel_Edit.TabIndex = 7;
            // 
            // grb_Edit
            // 
            this.grb_Edit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grb_Edit.Controls.Add(this.cbo_UserRole);
            this.grb_Edit.Controls.Add(this.label6);
            this.grb_Edit.Controls.Add(this.txt_UserPwdAgain);
            this.grb_Edit.Controls.Add(this.label5);
            this.grb_Edit.Controls.Add(this.txt_UserPwd);
            this.grb_Edit.Controls.Add(this.label2);
            this.grb_Edit.Controls.Add(this.btn_Save);
            this.grb_Edit.Controls.Add(this.btn_EditHide);
            this.grb_Edit.Controls.Add(this.txt_Remark);
            this.grb_Edit.Controls.Add(this.label4);
            this.grb_Edit.Controls.Add(this.txt_UserName);
            this.grb_Edit.Controls.Add(this.label3);
            this.grb_Edit.Location = new System.Drawing.Point(9, 3);
            this.grb_Edit.Name = "grb_Edit";
            this.grb_Edit.Size = new System.Drawing.Size(850, 81);
            this.grb_Edit.TabIndex = 0;
            this.grb_Edit.TabStop = false;
            this.grb_Edit.Text = "编辑信息";
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.Color.Transparent;
            this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Save.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.Image")));
            this.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Save.Location = new System.Drawing.Point(675, 19);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(60, 21);
            this.btn_Save.TabIndex = 15;
            this.btn_Save.Text = "保存";
            this.btn_Save.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
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
            // txt_Remark
            // 
            this.txt_Remark.Location = new System.Drawing.Point(301, 52);
            this.txt_Remark.Name = "txt_Remark";
            this.txt_Remark.Size = new System.Drawing.Size(355, 21);
            this.txt_Remark.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(266, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "备注:";
            // 
            // txt_UserName
            // 
            this.txt_UserName.Location = new System.Drawing.Point(83, 19);
            this.txt_UserName.Name = "txt_UserName";
            this.txt_UserName.Size = new System.Drawing.Size(140, 21);
            this.txt_UserName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "用户名称:";
            // 
            // panel_Query
            // 
            this.panel_Query.Controls.Add(this.grb_Query);
            this.panel_Query.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Query.Location = new System.Drawing.Point(0, 25);
            this.panel_Query.Name = "panel_Query";
            this.panel_Query.Size = new System.Drawing.Size(873, 68);
            this.panel_Query.TabIndex = 6;
            // 
            // grb_Query
            // 
            this.grb_Query.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grb_Query.Controls.Add(this.txt_search);
            this.grb_Query.Controls.Add(this.btn_QueryHidden);
            this.grb_Query.Controls.Add(this.btn_Query);
            this.grb_Query.Controls.Add(this.label1);
            this.grb_Query.Location = new System.Drawing.Point(10, 3);
            this.grb_Query.Name = "grb_Query";
            this.grb_Query.Size = new System.Drawing.Size(852, 55);
            this.grb_Query.TabIndex = 0;
            this.grb_Query.TabStop = false;
            this.grb_Query.Text = "查询";
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(86, 20);
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
            // btn_Query
            // 
            this.btn_Query.BackColor = System.Drawing.Color.Transparent;
            this.btn_Query.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Query.Image = ((System.Drawing.Image)(resources.GetObject("btn_Query.Image")));
            this.btn_Query.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_Query.Location = new System.Drawing.Point(304, 19);
            this.btn_Query.Name = "btn_Query";
            this.btn_Query.Size = new System.Drawing.Size(60, 23);
            this.btn_Query.TabIndex = 12;
            this.btn_Query.Text = "查询";
            this.btn_Query.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Query.UseVisualStyleBackColor = false;
            this.btn_Query.Click += new System.EventHandler(this.btn_Query_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "用户名称：";
            // 
            // txt_UserPwd
            // 
            this.txt_UserPwd.Location = new System.Drawing.Point(301, 19);
            this.txt_UserPwd.Name = "txt_UserPwd";
            this.txt_UserPwd.PasswordChar = '*';
            this.txt_UserPwd.Size = new System.Drawing.Size(140, 21);
            this.txt_UserPwd.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "用户密码:";
            // 
            // txt_UserPwdAgain
            // 
            this.txt_UserPwdAgain.Location = new System.Drawing.Point(516, 19);
            this.txt_UserPwdAgain.Name = "txt_UserPwdAgain";
            this.txt_UserPwdAgain.PasswordChar = '*';
            this.txt_UserPwdAgain.Size = new System.Drawing.Size(140, 21);
            this.txt_UserPwdAgain.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(457, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "确认密码:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "用户权限:";
            // 
            // cbo_UserRole
            // 
            this.cbo_UserRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_UserRole.FormattingEnabled = true;
            this.cbo_UserRole.Location = new System.Drawing.Point(83, 52);
            this.cbo_UserRole.Name = "cbo_UserRole";
            this.cbo_UserRole.Size = new System.Drawing.Size(121, 20);
            this.cbo_UserRole.TabIndex = 22;
            // 
            // tsb_Query
            // 
            this.tsb_Query.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Query.Image")));
            this.tsb_Query.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Query.Name = "tsb_Query";
            this.tsb_Query.Size = new System.Drawing.Size(51, 22);
            this.tsb_Query.Text = "查询";
            this.tsb_Query.Click += new System.EventHandler(this.tsb_Query_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // gf_SysUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 497);
            this.Controls.Add(this.dgv_SysUser);
            this.Controls.Add(this.panel_Edit);
            this.Controls.Add(this.panel_Query);
            this.Controls.Add(this.toolStrip1);
            this.Name = "gf_SysUser";
            this.Text = "用户管理";
            this.Load += new System.EventHandler(this.gf_SysUser_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SysUser)).EndInit();
            this.panel_Edit.ResumeLayout(false);
            this.grb_Edit.ResumeLayout(false);
            this.grb_Edit.PerformLayout();
            this.panel_Query.ResumeLayout(false);
            this.grb_Query.ResumeLayout(false);
            this.grb_Query.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_Add;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_Edit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsb_Delete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsb_Exit;
        private System.Windows.Forms.DataGridView dgv_SysUser;
        private System.Windows.Forms.Panel panel_Edit;
        private System.Windows.Forms.GroupBox grb_Edit;
        private System.Windows.Forms.ComboBox cbo_UserRole;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_UserPwdAgain;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_UserPwd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_EditHide;
        private System.Windows.Forms.TextBox txt_Remark;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_UserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel_Query;
        private System.Windows.Forms.GroupBox grb_Query;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_QueryHidden;
        private System.Windows.Forms.Button btn_Query;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton tsb_Query;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;

    }
}