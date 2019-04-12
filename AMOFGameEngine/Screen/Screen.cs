using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOIS;
using Mogre;

namespace AMOFGameEngine.Screen
{
    public class Screen : IScreen
    {
        protected bool isExiting;

        public virtual bool IsVisible
        {
            get
            {
                return false;
            }
        }

        public virtual string Name
        {
            get
            {
                return "";
            }
        }

        public virtual event Action OnScreenExit;

        public virtual bool CheckEnterScreen(Vector2 mousePos)
        {
            return false;
        }

        public virtual void Exit()
        {
            if (isExiting)
            {
                return;
            }

            isExiting = true;

            if (OnScreenExit!=null)
            {
                OnScreenExit();
            }
        }

        public virtual void Hide()
        {
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
            GameManager.Instance.trayMgr.injectMouseMove(arg);
        }
        public virtual void InjectMousePressed(MouseEvent arg, MouseButtonID id)
        {
            GameManager.Instance.trayMgr.injectMouseDown(arg, id);
        }
        public virtual void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
        {
            GameManager.Instance.trayMgr.injectMouseUp(arg, id);
        }

        public virtual void Run()
        {
        }

        public virtual void Show()
        {
        }

        public virtual void Update(float timeSinceLastFrame)
        {
        }
    }
}
