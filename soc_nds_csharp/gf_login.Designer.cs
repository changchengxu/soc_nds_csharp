namespace soc_nds_csharp
{
    partial class gf_login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gf_login));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_exit = new System.Windows.Forms.Button();
            this.btn_login = new System.Windows.Forms.Button();
            this.txt_username = new System.Windows.Forms.TextBox();
            this.txt_pwd = new System.Windows.Forms.TextBox();
            this.btn_network = new System.Windows.Forms.Button();
            this.gbx_NetWork = new System.Windows.Forms.GroupBox();
            this.lbl_IP = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbo_userRole = new System.Windows.Forms.ComboBox();
            this.gbx_NetWork.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(338, 272);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "  密  码：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(338, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "  用户名：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(478, 316);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(75, 23);
            this.btn_exit.TabIndex = 5;
            this.btn_exit.Text = " 退 出 ";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(340, 316);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(75, 23);
            this.btn_login.TabIndex = 4;
            this.btn_login.Text = " 登 录 ";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // txt_username
            // 
            this.txt_username.Location = new System.Drawing.Point(409, 229);
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(108, 21);
            this.txt_username.TabIndex = 2;
            this.txt_username.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_username_KeyDown);
            // 
            // txt_pwd
            // 
            this.txt_pwd.Location = new System.Drawing.Point(409, 265);
            this.txt_pwd.Name = "txt_pwd";
            this.txt_pwd.PasswordChar = '*';
            this.txt_pwd.Size = new System.Drawing.Size(108, 21);
            this.txt_pwd.TabIndex = 3;
            this.txt_pwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_pwd_KeyDown);
            // 
            // btn_network
            // 
            this.btn_network.Location = new System.Drawing.Point(693, 388);
            this.btn_network.Name = "btn_network";
            this.btn_network.Size = new System.Drawing.Size(75, 23);
            this.btn_network.TabIndex = 6;
            this.btn_network.Text = "网络设置";
            this.btn_network.UseVisualStyleBackColor = true;
            this.btn_network.Click += new System.EventHandler(this.btn_network_Click);
            // 
            // gbx_NetWork
            // 
            this.gbx_NetWork.BackColor = System.Drawing.Color.Transparent;
            this.gbx_NetWork.Controls.Add(this.lbl_IP);
            this.gbx_NetWork.Location = new System.Drawing.Point(333, 356);
            this.gbx_NetWork.Name = "gbx_NetWork";
            this.gbx_NetWork.Size = new System.Drawing.Size(227, 46);
            this.gbx_NetWork.TabIndex = 21;
            this.gbx_NetWork.TabStop = false;
            this.gbx_NetWork.Text = "登录服务器";
            // 
            // lbl_IP
            // 
            this.lbl_IP.AutoSize = true;
            this.lbl_IP.Location = new System.Drawing.Point(21, 21);
            this.lbl_IP.Name = "lbl_IP";
            this.lbl_IP.Size = new System.Drawing.Size(29, 12);
            this.lbl_IP.TabIndex = 0;
            this.lbl_IP.Text = "IP：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(338, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "用户类型：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbo_userRole
            // 
            this.cbo_userRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_userRole.FormattingEnabled = true;
            this.cbo_userRole.Location = new System.Drawing.Point(409, 193);
            this.cbo_userRole.Name = "cbo_userRole";
            this.cbo_userRole.Size = new System.Drawing.Size(108, 20);
            this.cbo_userRole.TabIndex = 1;
            this.cbo_userRole.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_userRole_KeyDown);
            // 
            // gf_login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(780, 456);
            this.Controls.Add(this.cbo_userRole);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gbx_NetWork);
            this.Controls.Add(this.btn_network);
            this.Controls.Add(this.txt_pwd);
            this.Controls.Add(this.txt_username);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.btn_login);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "gf_login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " 登 录 ";
            this.Load += new System.EventHandler(this.gf_login_Load);
            this.gbx_NetWork.ResumeLayout(false);
            this.gbx_NetWork.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.TextBox txt_username;
        private System.Windows.Forms.TextBox txt_pwd;
        private System.Windows.Forms.Button btn_network;
        private System.Windows.Forms.GroupBox gbx_NetWork;
        private System.Windows.Forms.Label lbl_IP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbo_userRole;


    }
}