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
    }
}
