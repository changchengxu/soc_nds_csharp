using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.Command;

namespace soc_nds_csharp.Station_Operation
{
    public partial class gf_RemoteFlashProtect : Form
    {
        public gf_RemoteFlashProtect()
        {
            InitializeComponent();
            this.BackColor = HDIC_Command.setColor();
        }

        private void btn_RemoteFlash_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.ForeColor = System.Drawing.Color.ForestGreen;
            this.richTextBox1.Text = "正在尝试连接,请稍后... ...";

        }
    }
}
