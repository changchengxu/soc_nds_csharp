using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace soc_nds_csharp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new gf_main());
           Application.Run(new DB_Manage.print());

        }
    }
}
