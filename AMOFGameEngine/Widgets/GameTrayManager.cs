using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre_Procedural.MogreBites;
using Mogre;
using MOIS;

namespace AMOFGameEngine.Widgets
{
    public class GameTrayManager : SdkTrayManager
    {
        public GameTrayManager(string name,RenderWindow win,Mouse mouse,SdkTrayListener listener) :base(name,win,mouse,listener)
        {

        }

        public InputBox createInputBox(TrayLocation trayLoc, string name, string caption,float width, float boxWidth, string text=null, bool onlyAcceptNum=false)
        {
            InputBox ib = new InputBox(name, caption, width, boxWidth, text, onlyAcceptNum);
            this.moveWidgetToTray(ib, trayLoc);
            ib.Text = text;
            ib._assignListener(mListener);
            return ib;
        }

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

    }
}
