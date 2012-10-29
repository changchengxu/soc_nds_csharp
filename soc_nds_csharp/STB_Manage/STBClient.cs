using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.Security;
using HDICSoft.Message;

using HDICSoft.DB;
namespace soc_nds_csharp.STB_Manage
{
    public partial class STBClient : Form
    {
        public STBClient()
        {
            InitializeComponent();
        }

        private void STBClient_Load(object sender, EventArgs e)
        {
            dgv_STB.DataSource = HDIC_DB.GetList("select pipLineID as '流水线号',currentPipID as '当前流水号',piplineIDMax as '流水号最大值' from STBClient");
        }


        private STBClient01 stbObject;
        private void tsb_Newfrm_Click(object sender, EventArgs e)
        {
            stbObject = new STBClient01();
            stbObject.Text = "STBClient Add";
            stbObject.ShowDialog();
            this.STBClient_Load(sender, e);
        }

        private void tsb_Editfrm_Click(object sender, EventArgs e)
        {
            if (dgv_STB.SelectedRows.Count > 0)
            {
                int intTypeId = Convert.ToInt32(dgv_STB.SelectedRows[0].Cells["流水线号"].Value);
                //实例化编辑页面
                stbObject = new STBClient01(intTypeId);
                stbObject.Text = "STBClient Edit";
                // 显示人员编辑画面
                stbObject.ShowDialog();
                // 重新初始化该画面
                this.STBClient_Load(sender, e);
            }
            else
            {
                HDIC_Message.ShowWarnDialog(this, "请选择要编辑的信息。");
            }
        }

        private void tsb_Delete_Click(object sender, EventArgs e)
        {

            int intID;          //类别ID
            //是否确定删除
            if (HDIC_Message.ShowQuestionDialog(this, "是否确定删除所选信息？") == DialogResult.Cancel)
            {
                return;
            }
            if (dgv_STB.SelectedRows.Count > 0)
            {
                //获取要删除行的ID号
                intID = Convert.ToInt32(dgv_STB.SelectedRows[0].Cells["流水线号"].Value);
                int a = HDIC_DB.sqlDelete("delete from STBClient where pipLineID='" + intID + "'");
                if (a > 0)
                {
                    HDIC_Message.ShowInfoDialog(this, "人员类别信息删除成功！");
                    this.STBClient_Load(sender, e);

                }
                else
                {
                    HDIC_Message.ShowErrorDialog(this, "人员类别信息删除失败！");
                }
            }
            else
            {
                HDIC_Message.ShowWarnDialog(this, "请选择要删除的信息。");
            }
        }

    }
}
