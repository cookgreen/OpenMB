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
            if(OnScreenExit!=null)
            {
                OnScreenExit();
            }
        }

        public override void Init(params object[] param)
        {
        }

        public override void Run()
        {
            if(OnScreenRun!=null)
            {
                OnScreenRun();
            }
        }

        public override void Update(float timeSinceLastFrame)
        {
        }
    }
}
