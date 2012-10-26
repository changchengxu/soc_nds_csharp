using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace soc_nds_csharp.DB_Manage
{
    public partial class Query_condition : Form
    {
        AnchorStyles anchors;
        const int OFFSET = 2;
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_TOPMOST = 8;
                base.CreateParams.Parent = GetDesktopWindow();
                base.CreateParams.ExStyle |= WS_EX_TOPMOST;
                return base.CreateParams;
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_MOVING = 534;
            switch (m.Msg)
            {
                case WM_MOVING:
                    int left = Marshal.ReadInt32(m.LParam, 0);
                    int top = Marshal.ReadInt32(m.LParam, 4);
                    int right = Marshal.ReadInt32(m.LParam, 8);
                    int bottom = Marshal.ReadInt32(m.LParam, 12);
                    left = Math.Min(Math.Max(0, left), Screen.PrimaryScreen.Bounds.Width - Width);
                    top = Math.Min(Math.Max(0, top), Screen.PrimaryScreen.Bounds.Height - Height);
                    right = Math.Min(Math.Max(Width, right), Screen.PrimaryScreen.Bounds.Width);
                    bottom = Math.Min(Math.Max(Height, bottom), Screen.PrimaryScreen.Bounds.Height);
                    Marshal.WriteInt32(m.LParam, 0, left);
                    Marshal.WriteInt32(m.LParam, 4, top);
                    Marshal.WriteInt32(m.LParam, 8, right);
                    Marshal.WriteInt32(m.LParam, 12, bottom);
                    anchors = AnchorStyles.None;
                    if (left <= OFFSET)
                    {
                        anchors |= AnchorStyles.Left;
                    }
                    if (top <= OFFSET)
                    {
                        anchors |= AnchorStyles.Top;
                    }
                    if (bottom >= Screen.PrimaryScreen.Bounds.Height - OFFSET)
                    {
                        anchors |= AnchorStyles.Bottom;
                    }
                    if (right >= Screen.PrimaryScreen.Bounds.Width - OFFSET)
                    {
                        anchors |= AnchorStyles.Right;
                    }
                    timer1.Enabled = anchors != AnchorStyles.None;
                    break;
            }
            base.WndProc(ref m);
        }



        public Query_condition()
        {
            InitializeComponent();
            timer1.Enabled = false;
            timer1.Interval = 200;
            TopMost = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
    }
}
