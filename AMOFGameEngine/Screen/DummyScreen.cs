using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOIS;

namespace AMOFGameEngine.Screen
{
    public class DummyScreen : IScreen
    {
        public event Action OnScreenExit;
        public event Action OnScreenRun;
        public string Name
        {
            get
            {
                return "Script";
            }
        }

        public void Exit()
        {
            OnScreenExit?.Invoke();
        }

        public void Init(params object[] param)
        {
        }

        public void Run()
        {
            OnScreenRun?.Invoke();
        }

        public void Update(float timeSinceLastFrame)
        {
            throw new NotImplementedException();
        }

        public void InjectMouseMove(MouseEvent arg)
        {
        }

        public void InjectMousePressed(MouseEvent arg, MouseButtonID id)
        {
        }

        public void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
        {
        }

        public void InjectKeyPressed(KeyEvent arg)
        {
        }

        public void InjectKeyReleased(KeyEvent arg)
        {
        }
    }
}
