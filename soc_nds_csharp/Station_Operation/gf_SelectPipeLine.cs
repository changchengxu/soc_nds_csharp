using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.DB;
using HDICSoft.Message;

namespace soc_nds_csharp.Station_Operation
{
    public partial class gf_SelectPipeLine : Form
    {
        public gf_SelectPipeLine()
        {
            InitializeComponent();
        }

        private void gf_SelectPipeLine_Load(object sender, EventArgs e)
        {
            cbo_LineID1.DataSource = HDIC_DB.GetList("select pipLineID from STBOp order by pipLineID");
            cbo_LineID1.DisplayMember = "pipLineID";
            cbo_LineID1.ValueMember = "pipLineID";
            cbo_LineID1.SelectedIndex = -1;

            cbo_LineID2.DataSource = HDIC_DB.GetList("select pipLineID from STBOp order by pipLineID");
            cbo_LineID2.DisplayMember = "pipLineID";
            cbo_LineID2.ValueMember = "pipLineID";
            cbo_LineID2.SelectedIndex = -1;
        }

        private void tsb_Ok_Click(object sender, EventArgs e)
        {
            if (!checkPara())
            {
                return;
            }
            gf_Serializer gf_serial = new gf_Serializer();
            gf_serial.ShowDialog();
        }

        private bool checkPara()
        {
            if (cbo_LineID1.SelectedValue == null || cbo_LineID2.SelectedValue == null)
            {
                HDIC_Message.ShowErrorDialog(this, "请将信息填写完整");
                return false;
            }
            else if (cbo_LineID1.SelectedValue.ToString().Trim() != cbo_LineID2.SelectedValue.ToString().Trim())
            {
                HDIC_Message.ShowErrorDialog(this, "两次选择的流水号不一致");
                return false;
            }
            return true;

        }

        private void tsb_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
