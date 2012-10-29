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
        //构造全局变量
        private int intGloID;

        public STBClient01()
        {
            InitializeComponent();
            intGloID = -1;
        }

        public STBClient01(int intID)
        {
            InitializeComponent();
            intGloID = intID;
        }
    }
}
