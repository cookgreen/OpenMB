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
        public static Panel createPanel(this SdkTrayManager trayMgr, TrayLocation trayLoc, string name)
        {
            Panel panel = new Panel(name);
            trayMgr.moveWidgetToTray(panel, trayLoc);
            return panel;
        }
        public static Panel createPanel(this SdkTrayManager trayMgr, TrayLocation trayLoc, string name, float width, float height)
        {
            Panel panel = new Panel(name, width, height);
            trayMgr.moveWidgetToTray(panel, trayLoc);
            return panel;
        }
        public static Panel createPanel(this SdkTrayManager trayMgr, TrayLocation trayLoc, string name, float width, float height, float left, float top)
        {
            Panel panel = new Panel(name, width, height, left, top);
            trayMgr.moveWidgetToTray(panel, trayLoc);
            return panel;
        }
    }
}
