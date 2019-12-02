﻿using System;
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
			UIManager.Instance.InjectKeyPressed(arg);
		}

        public virtual void InjectKeyReleased(KeyEvent arg)
        {
			UIManager.Instance.InjectKeyReleased(arg);
        }

        public virtual void InjectMouseMove(MouseEvent arg)
        {
            UIManager.Instance.InjectMouseMove(arg);
        }
        public virtual void InjectMousePressed(MouseEvent arg, MouseButtonID id)
        {
            UIManager.Instance.InjectMouseDown(arg, id);
        }
        public virtual void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
        {
            UIManager.Instance.InjectMouseUp(arg, id);
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
