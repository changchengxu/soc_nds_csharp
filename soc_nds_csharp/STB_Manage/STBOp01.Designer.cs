namespace soc_nds_csharp.STB_Manage
{
    partial class STBOp01
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
            this.label1 = new System.Windows.Forms.Label();
            this.txt_pipID = new System.Windows.Forms.TextBox();
            this.btn1 = new System.Windows.Forms.Button();
            this.txt_currentPipID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_pipIDMax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "流水线号：";
            // 
            // txt_pipID
            // 
            this.txt_pipID.Location = new System.Drawing.Point(124, 39);
            this.txt_pipID.Name = "txt_pipID";
            this.txt_pipID.Size = new System.Drawing.Size(100, 21);
            this.txt_pipID.TabIndex = 1;
            // 
            // btn1
            // 
            this.btn1.Location = new System.Drawing.Point(52, 210);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(75, 23);
            this.btn1.TabIndex = 2;
            this.btn1.Text = " 提 交 ";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // txt_currentPipID
            // 
            this.txt_currentPipID.Location = new System.Drawing.Point(124, 84);
            this.txt_currentPipID.Name = "txt_currentPipID";
            this.txt_currentPipID.Size = new System.Drawing.Size(100, 21);
            this.txt_currentPipID.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "当前流水号：";
            // 
            // txt_pipIDMax
            // 
            this.txt_pipIDMax.Location = new System.Drawing.Point(124, 132);
            this.txt_pipIDMax.Name = "txt_pipIDMax";
            this.txt_pipIDMax.Size = new System.Drawing.Size(100, 21);
            this.txt_pipIDMax.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "流水线号最大值：";
            // 
            // btn2
            // 
            this.btn2.Location = new System.Drawing.Point(177, 210);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(75, 23);
            this.btn2.TabIndex = 7;
            this.btn2.Text = " 取 消 ";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // STBOp01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(298, 286);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.txt_pipIDMax);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_currentPipID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.txt_pipID);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "STBOp01";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "STBClient01";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.STBClient01_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_pipID;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.TextBox txt_currentPipID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_pipIDMax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn2;
    }
}