using System;
using System.Windows.Forms;
using FreeMeterRevival.Forms;

namespace FreeMeterRevival
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SplashForm frmSplash = new SplashForm();
            frmSplash.Show();
            Application.Run(new MainForm(frmSplash));
        }
    }
}
        //[STAThread]
        //// MAIN - if already running notify, otherwise run the main form
        //static void Main()
        //{
        //    //try
        //    {
        //        Process[] RunningProcesses = Process.GetProcessesByName("FreeMeterRevival");
        //        if (RunningProcesses.Length == 1)
        //        {
        //            Application.EnableVisualStyles();
        //            Application.Run(new MainForm());
        //        }
        //        else if (RunningProcesses.Length == 2)
        //        {
        //            if (RunningProcesses[0].StartTime > RunningProcesses[1].StartTime)
        //                RunningProcesses[1].Kill();
        //            else
        //                RunningProcesses[0].Kill();

        //            Application.EnableVisualStyles();
        //            Application.Run(new MainForm());
        //        }
        //        else
        //            MessageBox.Show("I'm Already Running!", "!");
				
        //    }
        //    /*catch (Exception ex)
        //    {
        //        if (!Debugger.IsAttached)
        //            (new Error(ex)).ShowDialog();
        //        else
        //            throw ex;
        //    }*/		
        //}