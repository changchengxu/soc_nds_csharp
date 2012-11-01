using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace soc_nds_csharp.Station_Operation
{
    public partial class gf_CheckSerializer : Form
    {
        public gf_CheckSerializer()
        {
            InitializeComponent();
        }

        private void gf_CheckSerializer_Load(object sender, EventArgs e)
        {
            init();
            btn_begin.Focus();
        }

        private void init()
        {
            richTextBox1.Enabled = false;
            richTextBox2.Enabled = false;
            richTextBox3.Enabled = false;
        }

        private void btn_begin_Click(object sender, EventArgs e)
        {
            // richtxt_Connect
            this.richTextBox3.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox3.ForeColor = System.Drawing.Color.ForestGreen;
            this.richTextBox3.Text = "正在建立连接,请稍后... ...";

        }

        private void btn_begin_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.KeyCode == Keys.Enter)
            {
                btn_begin_Click(sender, e);
            }
        }
    }
}
