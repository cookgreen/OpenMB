using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Mogre;
using MOIS;
using AMOFGameEngine.Forms.Controller;
using AMOFGameEngine.Forms;

namespace AMOFGameEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmConfigureController controller = new frmConfigureController(new frmConfigure());
            Application.Run(controller.form);
        }
    }
}
