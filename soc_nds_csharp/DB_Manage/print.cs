using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.DB;
using HDICSoft.Export;
using HDICSoft.Message;
using HDICSoft.Func;
using GoldPrinter;

namespace soc_nds_csharp.DB_Manage
{
    public partial class print : Form
    {
        HDIC_Export exportOBJ = new HDIC_Export();
        public print()
        {
            InitializeComponent();
            //dt = DBHelper.GetList("select * from SysMenuDisplay");
            //dt = DBHelper.GetList("select * from STBData");
        }

        private void print_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AllowUserToAddRows = false;

            this.panel1.Visible = false;
            //cbo_Time.Checked = false;
            dateTimePicker_start.Enabled = false;
            dateTimePicker_end.Enabled = false;

            this.Closed += new System.EventHandler(this.frmPrint_Closed);
        }

        #region 打印方法调用
        //打印浏览
        private void tsbPrintPreview_Click(object sender, EventArgs e)
        {
            Print(false);
        }
        //页面设置
        private void tsbPageSetup_Click(object sender, EventArgs e)
        {
            misGoldPrinter.PageSetup();

        }
        //打印
        private void tsbPrint_Click(object sender, EventArgs e)
        {
            Print(true);
        }


        //*****第一步*****：申明并实例化，可用带参构造函数指明默认横向显示/打印。
        //命名空间是using GoldPrinter;
        private GoldPrinter.MisGoldPrinter misGoldPrinter = new GoldPrinter.MisGoldPrinter(true);

        /// <summary>
        /// 打印方法
        /// </summary>
        /// <param name="p_PrintOrPreview"></param>
        private void Print(bool p_PrintOrPreview)
        {
            //*****第二步*****：直接赋值给打印对象(可选)
            misGoldPrinter.Title = "直播星生产线工位操作和数据库管理\nchangchengxue@gmail.com";							//主标题（C#用\n表示换行）	
            //misGoldPrinter.Caption = "　　——qq:765650886，有问题请联系CC.XUE";										//副标题
            misGoldPrinter.Top = "作者：薛长城|2012-10-25|版本：V1.0";										//抬头，一行三列的文字说明，用|分隔
            misGoldPrinter.Bottom = "打印直播星数据||打印日期：" + System.DateTime.Now.ToLongDateString();	//结尾，说明同抬头


            //*****第三步*****：打印重点，设置数据 源
            //可以是一维数组、二维数组、DataGrid(DataGridView和DataGrid不同，它需要转换成二维数组）、DataTable、ListView、MshFlexGrid、HtmlTable...，
            //总之，不支持的你自己写一个函数转换成二维数组赋给DataSource一切搞定
            string[,] array = HDIC_Func.ToStringArray(dataGridView1,true);
            misGoldPrinter.DataSource = array;	//DataGrid作为数据源

            Body body = new Body();
            body.ColsAlignString = "CRLL";						//由左中右第一个字母组成，对应于每列的对对齐方式

           #region 网格标题（多层表头，符合中国人习惯），可选
            //MultiHeader multiHeader = new MultiHeader(1, 4);			//两行6列的要合并分类说明的表头，如果只是单行表头且只要占用一行高度，用new MultiHeader(1,6)并用SetText赋值即可
            //multiHeader.SetText(0, 0, "ChipID");
            //multiHeader.SetText(0, 1, "CAID");
            //multiHeader.SetText(0, 2, "STBID");
            //multiHeader.SetText(0, 3, "生产时间");

            //multiHeader.ColsWidth = new int[] {240,210,200,200};			//指定每页列宽，因为是Body明细的列标题，所以明细的列宽和它设置一样。当然，你也可以不做明细的列标题
            //misGoldPrinter.MultiHeader = multiHeader;
           #endregion

            //misGoldPrinter.RowsPerPage = 20;				//可以指定每页打印的行数，默认自适应
            //misGoldPrinter.BackColor = Color.Red;	//背景色
            //((GoldPrinter.Top)(misGoldPrinter.Top)).IsDrawAllPage = false;		//可以设置是否每页都打印，页顶、页头默认第一页打印，而页脚与页底默认最后一页打印

            //misGoldPrinter.IsSubTotalPerPage = true;				//这是打印本页小计
            //misGoldPrinter.SubTotalColsList = (mcols - 2).ToString() + ";" + (mcols - 1).ToString();		//用分号分隔的要求小计的列				

            #region 装订线（可选）
            //Sewing sewing = new Sewing(20, SewingDirectionFlag.Left);	//左装订，边距为20个显示单位
            //misGoldPrinter.Sewing = sewing;
            //misGoldPrinter.IsSewingLine = true;							//打印装订线		
            #endregion	

            //打印的核心是Body，可以对它设置字体、列宽等等
            ((GoldPrinter.Body)(misGoldPrinter.Body)).IsAverageColsWidth = true;//指明平均列宽

            //*****第四步*****：打印或预览			
            if (p_PrintOrPreview)
            {
                misGoldPrinter.Print();											//打印
            }
            else
            {
                misGoldPrinter.Preview();										//预览
            }
        }

        //关闭窗体
        private void frmPrint_Closed(object sender, System.EventArgs e)
        {
            //*****第五步*****：用完释放
            misGoldPrinter.Dispose();
            misGoldPrinter = null;
        }

        #endregion

        #region Delete Button
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            string rowSelect = "";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                {
                    rowSelect += Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString().Trim()) + ",";
                }
            }

            if (HDIC_Message.ShowQuestionDialog(this,"Are you sure you want to delete the selected row " + (rowSelect.Split(',').Length - 1)) == DialogResult.No)
            {
                return;
            }

            if (rowSelect != "")
            {
                rowSelect = rowSelect.Substring(0, rowSelect.LastIndexOf(','));
                string sqlstr = "delete from STBData where ChipID in (" + rowSelect + ")";
                try
                {
                    HDIC_DB.sqlDelete(sqlstr);
                    HDIC_Message.ShowInfoDialog(this, "deleted successfully");
                }
                catch (System.Exception ex)
                {
                    HDIC_Message.ShowErrorDialog(this,"delete failed because of" + ex.ToString());
                }

                btn_Query_Click(sender, e);
            }

        }
        #endregion

        #region Query Button
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }

        private void cbo_Time_Click(object sender, EventArgs e)
        {
            if (cbo_Time.Checked)
            {
                dateTimePicker_start.Enabled = true;
                dateTimePicker_end.Enabled = true;
                dateTimePicker_end.Value = DateTime.Now.AddMonths(1);
            }
            else
            {
                dateTimePicker_start.Enabled = false;
                dateTimePicker_end.Enabled = false;
            }
        }

        private void btn_Query_Click(object sender, EventArgs e)
        {
            string sqlstr = "select * from STBData where 1=1";
            if (cbo_Time.Checked)
            {
                sqlstr += " and ProductDate between '"+dateTimePicker_start.Value+"' and '"+dateTimePicker_end.Value+"'";
            }
            if (txt_CAID.Text.Trim() != "")
            {
                sqlstr += " and CAID='"+txt_CAID.Text.Trim()+"'";
            }
            if (txt_ChipID.Text.Trim() != "")
            {
                sqlstr += " and ChipID='" + txt_ChipID.Text.Trim() + "'";
            }

            dataGridView1.DataSource=null;
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();

            DataGridViewCheckBoxColumn cb_check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            cb_check.HeaderText = "Select";
            cb_check.Name = "cb_check";
            dataGridView1.Columns.Add(cb_check);

            dataGridView1.DataSource = HDIC_DB.GetList(sqlstr);

        }

        private void btn_hide_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = false;
        }

        #endregion

        #region Export file
        private void tsbExport_Click(object sender, EventArgs e)
        {
            exportOBJ.outPutDataGridViewData(dataGridView1, "直播星数据库管理软件");
        }
        #endregion

        #region automatically add a number
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,e.RowBounds.Location.Y
                                                 ,dataGridView1.RowHeadersWidth - 4,e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView1.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        #endregion

    }

}
