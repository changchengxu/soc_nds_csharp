using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.DB;
using HDICSoft.Message;

namespace soc_nds_csharp.Delete_DBRecords
{
    public partial class Delete_DBRecords : Form
    {
        public Delete_DBRecords()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (HDIC_Message.ShowQuestionDialog(this, "确定要清除CA_ID:" + txt_CAID.Text.Trim() + " 吗？") == DialogResult.OK)
            {

            }
        }
    }
}
