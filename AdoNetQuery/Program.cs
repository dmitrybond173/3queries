/* 
 * DBO-Tools collection.
 * ADO.NET Query tool.
 * Simple application to execute SQL queries via ADO.NET interface.
 * 
 * Program entry-point.
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AdoNetQuery
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
