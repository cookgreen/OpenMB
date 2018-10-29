using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOIS;

namespace AMOFGameEngine.Screen
{
    public class GameConsoleScreen : Screen
    {
        public override event Action OnScreenExit;
        public override string Name
        {
            get
            {
                return "Console";
            }
        }

        public GameConsoleScreen()
        {
        }

        public override void Exit()
        {
            OnScreenExit?.Invoke();
        }

        public override void Init(params object[] param)
        {

        }

        public override void Run()
        {

        }

        public override void Update(float timeSinceLastFrame)
        {

        }
    }
}
