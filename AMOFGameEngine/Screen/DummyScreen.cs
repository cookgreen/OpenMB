using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Screen
{
    public class DummyScreen : IScreen
    {
        public event Action OnScreenExit;
        public event Action OnScreenRun;

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
    }
}
