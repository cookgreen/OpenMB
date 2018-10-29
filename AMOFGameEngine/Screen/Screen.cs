using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOIS;

namespace AMOFGameEngine.Screen
{
    public class Screen : IScreen
    {
        public virtual string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual event Action OnScreenExit;

        public virtual void Exit()
        {
            OnScreenExit?.Invoke();
        }

        public virtual void Init(params object[] param)
        {
        }

        public virtual void InjectKeyPressed(KeyEvent arg)
        {
        }

        public virtual void InjectKeyReleased(KeyEvent arg)
        {
        }

        public virtual void InjectMouseMove(MouseEvent arg)
        {
            GameManager.Instance.mTrayMgr.injectMouseMove(arg);
        }
        public virtual void InjectMousePressed(MouseEvent arg, MouseButtonID id)
        {
            GameManager.Instance.mTrayMgr.injectMouseDown(arg, id);
        }
        public virtual void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
        {
            GameManager.Instance.mTrayMgr.injectMouseUp(arg, id);
        }

        public virtual void Run()
        {
        }

        public virtual void Update(float timeSinceLastFrame)
        {
        }
    }
}
