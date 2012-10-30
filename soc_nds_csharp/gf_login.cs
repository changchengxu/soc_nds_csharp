using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using HDICSoft.DB;
using HDICSoft.Message;

namespace soc_nds_csharp
{
    public partial class gf_login : Form
    {
        public gf_login()
        {
            InitializeComponent();
           //txt_username.Region = new Region(GetRoundRectPath(new RectangleF(0, 0, this.txt_username.Width, this.txt_username.Height), 10f));
           //txt_pwd.Region = new Region(GetRoundRectPath(new RectangleF(0, 0, this.txt_username.Width, this.txt_username.Height), 10f));

        }

        #region 文本框圆角
        public GraphicsPath GetRoundRectPath(RectangleF rect, float radius)
        {
            return GetRoundRectPath(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public GraphicsPath GetRoundRectPath(float X, float Y, float width, float height, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(X + radius, Y, (X + width) - (radius * 2f), Y);
            path.AddArc((X + width) - (radius * 2f), Y, radius * 2f, radius * 2f, 270f, 90f);
            path.AddLine((float)(X + width), (float)(Y + radius), (float)(X + width), (float)((Y + height) - (radius * 2f)));
            path.AddArc((float)((X + width) - (radius * 2f)), (float)((Y + height) - (radius * 2f)), (float)(radius * 2f), (float)(radius * 2f), 0f, 90f);
            path.AddLine((float)((X + width) - (radius * 2f)), (float)(Y + height), (float)(X + radius), (float)(Y + height));
            path.AddArc(X, (Y + height) - (radius * 2f), radius * 2f, radius * 2f, 90f, 90f);
            path.AddLine(X, (Y + height) - (radius * 2f), X, Y + radius);
            path.AddArc(X, Y, radius * 2f, radius * 2f, 180f, 90f);
            path.CloseFigure();
            return path;
        }

        #endregion

        private void btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void gf_login_Load(object sender, EventArgs e)
        {
            h
        }
    }
}
