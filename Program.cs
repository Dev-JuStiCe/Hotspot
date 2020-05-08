using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show("This program requires to be runned as Administrator.", "Warning");
                //Form1.textBox5.AppendText("This program requires to be runned as Administrator.");
                Environment.Exit(0);
                /*
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This program requires to be runned as Administrator.");
                Console.ResetColor();
                Thread.Sleep(10000000);
                */
                //return;
            }

        }
    }
}
