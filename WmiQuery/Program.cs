/* 
 * DBO-Tools collection.
 * WMI Query tool.
 * Simple application to execute WQL/WMI queries.
 * 
 * Program entry-point.
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WmiQuery
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormMain frm = new FormMain(args);
            Application.Run(frm);
        }
    }
}
