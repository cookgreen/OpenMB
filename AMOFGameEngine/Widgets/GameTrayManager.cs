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

        public InputBox createInputBox(TrayLocation trayLoc, string name, string caption,float boxWidth)
        {
            InputBox ib = new InputBox(name, caption, 0, boxWidth);
            this.moveWidgetToTray(ib, trayLoc);
            ib._assignListener(mListener);
            return ib;
        }
    }
}
