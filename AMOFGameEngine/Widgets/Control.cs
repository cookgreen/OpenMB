using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.Widgets
{
    public abstract class Control : IDisposable
    {
        public Control()
        {
            GameManager.Instance.mMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mMouse_MouseReleased);
            GameManager.Instance.mMouse.MousePressed += new MouseListener.MousePressedHandler(mMouse_MousePressed);
            GameManager.Instance.mMouse.MouseMoved += new MouseListener.MouseMovedHandler(mMouse_MouseMoved);
            GameManager.Instance.mKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(mKeyboard_KeyReleased);
            GameManager.Instance.mKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(mKeyboard_KeyPressed);
        }

        bool mMouse_MouseReleased(MouseEvent arg, MouseButtonID id)
        {
            cursorReleased(new Vector2(arg.state.X.abs, arg.state.Y.abs));
            return true;
        }

        bool mMouse_MousePressed(MouseEvent arg, MouseButtonID id)
        {
            cursorPressed(new Vector2(arg.state.X.abs, arg.state.Y.abs));
            return true;
        }

        bool mMouse_MouseMoved(MouseEvent arg)
        {
            return true;
        }

        bool mKeyboard_KeyReleased(KeyEvent arg)
        {
            keyReleased(arg.text);
            return true;
        }

        bool mKeyboard_KeyPressed(KeyEvent arg)
        {
            keyPressed(arg.text);
            return true;
        }

        public abstract void cursorMoved(Vector2 cursorPos);

        public abstract void cursorPressed(Vector2 cursorPos);

        public abstract void cursorReleased(Vector2 cursorPos);

        public abstract void keyPressed(uint text);

        public abstract void keyReleased(uint text);

        public abstract void Dispose();

        public static void nukeOverlayElement(OverlayElement element)
        {
            Mogre.OverlayContainer container = element as Mogre.OverlayContainer;
            if (container != null)
            {
                List<Mogre.OverlayElement> toDelete = new List<Mogre.OverlayElement>();

                Mogre.OverlayContainer.ChildIterator children = container.GetChildIterator();
                while (children.MoveNext())
                {
                    toDelete.Add(children.Current);
                }

                for (int i = 0; i < toDelete.Count; i++)
                {
                    nukeOverlayElement(toDelete[i]);
                }
            }
            if (element != null)
            {
                Mogre.OverlayContainer parent = element.Parent;
                if (parent != null)
                    parent.RemoveChild(element.Name);
                Mogre.OverlayManager.Singleton.DestroyOverlayElement(element);
            }
        }

        public static bool isPositionInElement(OverlayElement element, Vector2 position)
        {
            if (position.x >= (element._getLeft() * OverlayManager.Singleton.ViewportWidth) &&
                position.y >= (element._getDerivedTop() * OverlayManager.Singleton.ViewportHeight) &&
                position.x <= ((element._getLeft() + element.Width )* OverlayManager.Singleton.ViewportWidth) &&
                position.y <= ((element._getDerivedTop()) + element.Height) * OverlayManager.Singleton.ViewportHeight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
