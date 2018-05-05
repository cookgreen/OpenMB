using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre_Procedural.MogreBites;
using Mogre;
using MOIS;

namespace AMOFGameEngine.Widgets
{
    public static class GameTrayManager
    {
        public static InputBox createInputBox(this SdkTrayManager trayMgr, TrayLocation trayLoc, string name, string caption,float width, float boxWidth, string text=null, bool onlyAcceptNum=false)
        {
            InputBox ib = new InputBox(name, caption, width, boxWidth, text, onlyAcceptNum);
            trayMgr.moveWidgetToTray(ib, trayLoc);
            ib.Text = text;
            //ib._assignListener(mListener);
            return ib;
        }

        public static ListView createListView(this SdkTrayManager trayMgr, TrayLocation trayLoc, string name, float height, float width, List<string> columnNames)
        {
            ListView lsv = new ListView(name, -1, -1, height, width, columnNames);
            //this.moveWidgetToTray(lsv,trayLoc);
            //lsv._assignListener(mListener);
            return lsv;
        }
    }
}
