using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L_Titrator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();

            //string thisName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            //System.Diagnostics.Process[] matched = System.Diagnostics.Process.GetProcessesByName(thisName);
            //if (matched.Length > 1)
            //{
            //    ATIK.MsgFrm_NotifyOnly msg = new ATIK.MsgFrm_NotifyOnly($"{thisName} is already running.");
            //    msg.ShowDialog();

            //    Application.Exit();
            //}

            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Frm_Main());
        }
    }
}
