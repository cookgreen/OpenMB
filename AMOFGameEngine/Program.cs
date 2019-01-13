using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Mogre;
using MOIS;
using AMOFGameEngine.Core;
using AMOFGameEngine.Forms.Controller;
using AMOFGameEngine.Forms;

namespace AMOFGameEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Core.Game game = new Core.Game(args);
            game.Run();
        }
    }
}
