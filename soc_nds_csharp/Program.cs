﻿using System;
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
            Application.Run(new gf_main());
            //Application.Run(new STB_Manage.STBOp());
            //Application.Run(new DB_Manage.gf_Barcode());
            //Application.Run(new Station_Operation.gf_Serializer());
            //Application.Run(new Station_Operation.gf_SelectPipeLine());
            //Application.Run(new Station_Operation.gf_CheckSerializer());

            //Application.Run(new Export_file.Export_file());

        }
    }
}
