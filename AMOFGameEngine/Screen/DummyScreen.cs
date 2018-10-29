using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOIS;

namespace AMOFGameEngine.Screen
{
    public class DummyScreen : Screen
    {
        public override event Action OnScreenExit;
        public event Action OnScreenRun;
        public override string Name
        {
            get
            {
                return "Script";
            }
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
            OnScreenRun?.Invoke();
        }

        public override void Update(float timeSinceLastFrame)
        {
        }
    }
}
