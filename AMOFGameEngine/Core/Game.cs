using AMOFGameEngine.Forms;
using AMOFGameEngine.Forms.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AMOFGameEngine.Core
{
    public class Game
    {
        private Argument gameArgument;
        public Game(string[] args)
        {
            gameArgument = new Argument(args);
            if (args != null)
            {
                foreach (string arg in args)
                {
                    string newArg = arg.Trim().Replace(" ", null);//Remove Space
                    string[] tokens = newArg.Split('=');
                    if (tokens.Length == 2)
                    {
                        gameArgument.AddArg(tokens[0], tokens[1]);
                    }
                }
            }
        }

        public void Run()
        {
            string modArg = gameArgument.GetArgValue("Engine.Mod");

            string showConfigArg = gameArgument.GetArgValue("Engine.ShowConfig");
            if (string.IsNullOrEmpty(showConfigArg) || showConfigArg == "yes")
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                frmConfigureController controller = new frmConfigureController(new frmConfigure());
                controller.form.ShowDialog();
            }
            else
            {
                GameApp app = new GameApp(null, modArg);
                app.Run();
            }
        }
    }
}
