using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UCSEditor
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            EditorSetting setting = EditorSetting.Read();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(setting));
        }
    }
}
