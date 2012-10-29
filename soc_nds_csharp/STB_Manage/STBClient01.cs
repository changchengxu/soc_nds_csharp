using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace soc_nds_csharp.STB_Manage
{
    public partial class STBClient01 : Form
    {
        #region //in order to move the form
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        #endregion

        //构造全局变量
        private int intGloID;

        public STBClient01()
        {
            InitializeComponent();
            intGloID = -1;
            btn1.Text = " 添 加 ";
        }

        public STBClient01(int intID)
        {
            InitializeComponent();
            intGloID = intID;
            btn1.Text = " 修 改 ";
        }

        private void STBClient01_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left & this.WindowState == FormWindowState.Normal)
            {
                // 移动窗体
                this.Capture = false;
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
