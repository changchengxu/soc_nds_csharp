using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace soc_nds_csharp.Station_Operation
{
    public partial class gf_Serializer : Form
    {
        public gf_Serializer()
        {
            InitializeComponent();
        }

        private void gf_Serializer_Load(object sender, EventArgs e)
        {
            init();
            btn_begin.Focus();
        }


        private void init()
        {
            richtxt_Connect.Enabled = false;
            richtxt_info.Enabled = false;
            richtxt_SerialID.Enabled = false;
            richtxt_CAID.Enabled = false;
            richtxt_ChipID.Enabled = false;
            richtxt_STBID.Enabled = false;
            richtxt_STBPla1.Enabled = false; richtxt_STBPla2.Enabled = false; richtxt_STBPla3.Enabled = false; 

        }

        private void btn_begin_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.KeyCode == Keys.Enter)
            {
                btn_begin_Click(null, e);
            }
        }

        private void btn_begin_Click(object sender, EventArgs e)
        {
            // richtxt_Connect
            this.richtxt_Connect.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richtxt_Connect.ForeColor = System.Drawing.Color.ForestGreen;
            this.richtxt_Connect.Text = "正在尝试连接,请稍后... ...";

        }

    }
}
