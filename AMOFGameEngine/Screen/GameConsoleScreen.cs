using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOIS;

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
            OnScreenExit?.Invoke();
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
