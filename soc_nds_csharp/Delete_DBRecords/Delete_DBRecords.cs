using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.DB;
using HDICSoft.Message;
using HDICSoft.Command;

namespace soc_nds_csharp.Delete_DBRecords
{
    public partial class Delete_DBRecords : Form
    {
        public Delete_DBRecords()
        {
            InitializeComponent();
            tableLayoutPanel1.BackColor = HDIC_Command.setColor();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (HDIC_Message.ShowQuestionDialog(this, "确定要清除CA_ID:" + txt_CAID.Text.Trim() + " 吗？") == DialogResult.OK)
            {
                try
                {
                    if (HDIC_DB.sqlDelete("delete STBData where CAID='" + txt_CAID.Text.Trim() + "'") > 0)
                    {
                        HDIC_Message.ShowInfoDialog(this, "删除成功");
                    }
                    else
                    {
                        HDIC_Message.ShowInfoDialog(this, "删除失败,");
                    }
                }
                catch (System.Exception ex)
                {
                    HDIC_Message.ShowInfoDialog(this, "删除失败,请检查服务器是否已经打开：\r\n"+ex.ToString());
                }

                txt_CAID.Text = "";
            }
        }
    }
}
