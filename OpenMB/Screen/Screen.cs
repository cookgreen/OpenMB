using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOIS;
using Mogre;
using Mogre_Procedural.MogreBites;
using OpenMB.Widgets;

namespace OpenMB.Screen
{
    public class Screen : IScreen
	{
        protected List<Widget> widgets;
		protected bool isExiting;
        protected UILayer layer;
		public virtual event Action OnScreenExit;
		public virtual event Action<string, string> OnScreenEventChanged;

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

        public List<Widget> UIWidgets { get { return widgets; } }

        public Screen()
		{
            widgets = new List<Widget>();
            UIManager.Instance.AddNewLayer();
            layer = UIManager.Instance.CurrentLayer;
		}

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
			var uiEvent = UIManager.Instance.InjectKeyPressed(arg);
			if (uiEvent != null)
			{
				OnScreenEventChanged?.Invoke(uiEvent.WidgetName, uiEvent.EventValue);
			}
		}

        public virtual void InjectKeyReleased(KeyEvent arg)
        {
			var uiEvent = UIManager.Instance.InjectKeyReleased(arg);
			if (uiEvent != null)
			{
				OnScreenEventChanged?.Invoke(uiEvent.WidgetName, uiEvent.EventValue);
			}
		}

        public virtual void InjectMouseMove(MouseEvent arg)
        {
			var uiEvent = UIManager.Instance.InjectMouseMove(arg);
			if (uiEvent != null)
			{
				OnScreenEventChanged?.Invoke(uiEvent.WidgetName, uiEvent.EventValue);
			}
		}
        public virtual void InjectMousePressed(MouseEvent arg, MouseButtonID id)
        {
			var uiEvent = UIManager.Instance.InjectMouseDown(arg, id);
			if (uiEvent != null)
			{
				OnScreenEventChanged?.Invoke(uiEvent.WidgetName, uiEvent.EventValue);
			}
		}
        public virtual void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
        {
			var uiEvent = UIManager.Instance.InjectMouseUp(arg, id);
			if (uiEvent != null)
			{
				OnScreenEventChanged?.Invoke(uiEvent.WidgetName, uiEvent.EventValue);
			}
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
