namespace soc_nds_csharp
{
    partial class gf_main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gf_main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dsafToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fsdafToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fdsaffdsafsdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.锁屏LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.taskPane1 = new XPExplorerBar.TaskPane();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taskPane1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dsafToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(998, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "&File  &Window";
            // 
            // dsafToolStripMenuItem
            // 
            this.dsafToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fsdafToolStripMenuItem,
            this.fdsaffdsafsdToolStripMenuItem});
            this.dsafToolStripMenuItem.Name = "dsafToolStripMenuItem";
            this.dsafToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dsafToolStripMenuItem.Text = "文件";
            // 
            // fsdafToolStripMenuItem
            // 
            this.fsdafToolStripMenuItem.Name = "fsdafToolStripMenuItem";
            this.fsdafToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.fsdafToolStripMenuItem.Text = "锁屏";
            // 
            // fdsaffdsafsdToolStripMenuItem
            // 
            this.fdsaffdsafsdToolStripMenuItem.Name = "fdsaffdsafsdToolStripMenuItem";
            this.fdsaffdsafsdToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.fdsaffdsafsdToolStripMenuItem.Text = "退出";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.锁屏LToolStripMenuItem,
            this.toolStripSeparator1,
            this.退出XToolStripMenuItem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // 锁屏LToolStripMenuItem
            // 
            this.锁屏LToolStripMenuItem.Name = "锁屏LToolStripMenuItem";
            this.锁屏LToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.锁屏LToolStripMenuItem.Text = "锁屏(&L)";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(110, 6);
            // 
            // 退出XToolStripMenuItem
            // 
            this.退出XToolStripMenuItem.Name = "退出XToolStripMenuItem";
            this.退出XToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.退出XToolStripMenuItem.Text = "退出(&X)";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::soc_nds_csharp.Properties.Resources.Level;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(998, 9);
            this.panel1.TabIndex = 23;
            // 
            // taskPane1
            // 
            this.taskPane1.AutoScroll = true;
            this.taskPane1.AutoScrollMargin = new System.Drawing.Size(12, 12);
            this.taskPane1.CustomSettings.GradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.taskPane1.CustomSettings.GradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.taskPane1.CustomSettings.Padding = new XPExplorerBar.Padding(1, 0, 0, 0);
            this.taskPane1.Dock = System.Windows.Forms.DockStyle.Left;
            this.taskPane1.Location = new System.Drawing.Point(0, 33);
            this.taskPane1.Name = "taskPane1";
            this.taskPane1.Size = new System.Drawing.Size(210, 519);
            this.taskPane1.TabIndex = 32;
            this.taskPane1.Text = "taskPane1";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::soc_nds_csharp.Properties.Resources.Vertical;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox2.Location = new System.Drawing.Point(210, 33);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(10, 519);
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            // 
            // gf_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Yellow;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(998, 552);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.taskPane1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "gf_main";
            this.Text = "直播星生产线数据库管理系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.gf_main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taskPane1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 锁屏LToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 退出XToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private XPExplorerBar.TaskPane taskPane1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolStripMenuItem dsafToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fsdafToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fdsaffdsafsdToolStripMenuItem;
    }
}

