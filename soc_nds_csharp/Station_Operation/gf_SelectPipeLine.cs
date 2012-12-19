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

namespace soc_nds_csharp.Station_Operation
{
    public delegate void mainFormTrans(string mSign);// 传递给主窗体主窗体

    public partial class gf_SelectPipeLine : Form
    {

        public  event mainFormTrans eventSend;//传递给主窗体，当打开工位一序列化模块时是主窗体TreeView不能点击

        public gf_SelectPipeLine()
        {
            InitializeComponent();
            tableLayoutPanel1.BackColor = HDIC_Command.setColor();
            this.BackColor = HDIC_Command.setColor();
        }

        private void gf_SelectPipeLine_Load(object sender, EventArgs e)
        {
            try
            {
                cbo_LineID1.DataSource = HDIC_DB.GetList("select distinct STBOpLineNum from STBOp order by STBOpLineNum");
                cbo_LineID1.DisplayMember = "STBOpLineNum";
                cbo_LineID1.ValueMember = "STBOpLineNum";
                cbo_LineID1.SelectedIndex = -1;

                cbo_LineID2.DataSource = HDIC_DB.GetList("select distinct STBOpLineNum from STBOp order by STBOpLineNum");
                cbo_LineID2.DisplayMember = "STBOpLineNum";
                cbo_LineID2.ValueMember = "STBOpLineNum";
                cbo_LineID2.SelectedIndex = -1;
            }
            catch
            {
                HDIC_Message.ShowWarnDialog(this, "数据库打开失败,请检查服务器或者网络");
            }
        }

        private void tsb_Ok_Click(object sender, EventArgs e)
        {
            if (!checkPara())
            {
                return;
            }
            HDIC_Command.STBLineNum = Convert.ToInt32(cbo_LineID1.SelectedValue);

            gf_Serializer gf_serial = new gf_Serializer(this);
            //gf_serial.ShowDialog();
            gf_serial.TopLevel = true;
            gf_serial.TopMost = true;
            gf_serial.MdiParent = this.MdiParent;//父窗体是 父父窗体
            gf_serial.WindowState = FormWindowState.Maximized;
            gf_serial.MaximizeBox = false;

            eventFunc("off");

            gf_serial.Show();

        }

        public void eventFunc(string sign)//委托，父窗体和子窗体交互
        {
            eventSend(sign);
            if (sign == "off")//子窗体开启
            {
                this.Visible = false;
            }
            else if (sign == "open")//子窗体关闭
            {
                this.Visible = true;
            }
        }

        private bool checkPara()
        {
            if (cbo_LineID1.SelectedValue == null || cbo_LineID2.SelectedValue == null)
            {
                HDIC_Message.ShowErrorDialog(this, "请选择流水线");
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
