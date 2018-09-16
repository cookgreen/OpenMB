using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Screen
{
    public class GameConsoleScreen : IScreen
    {
        public event Action OnScreenExit;
        public string Name
        {
            get
            {
                return "Console";
            }
        }

        public GameConsoleScreen()
        {
        }

        public void Exit()
        {

        }

        public void Init(params object[] param)
        {

        }

        public void Run()
        {

        }

        public void Update(float timeSinceLastFrame)
        {

        }
    }
}
