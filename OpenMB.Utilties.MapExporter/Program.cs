using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenMB.FileFormats;
using System.IO;
using System.Windows.Forms;

namespace OpenMB.Utilties.MapExporter
{
    class Program
	{
		[STAThread]
		static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
