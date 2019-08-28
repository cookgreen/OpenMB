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
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Core.Game game = new Core.Game(args);
            game.Run();
        }
    }
}
