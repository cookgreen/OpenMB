using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Mogre;
using MOIS;
using OpenMB.Core;
using OpenMB.Forms.Controller;
using OpenMB.Forms;

namespace OpenMB
{ 
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            GameContainerApp game = new GameContainerApp(args);
            game.Run();
        }
    }
}
