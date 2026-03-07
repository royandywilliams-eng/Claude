using System;
using System.Windows.Forms;

namespace NTQAAppLauncher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Use modern Windows 11 styled form
            Application.Run(new MainFormModern());
        }
    }
}
