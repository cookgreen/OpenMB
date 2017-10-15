using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using AMOFGameEngine.Widgets;

namespace AMOFGameEngine.UI
{
    public class GameListUI : GameUI
    {
        private string name;
        private ListView lsv;
        private PushButton btnHost;
        private PushButton btnJoin;
        private PushButton btnExit;
        private Overlay overlay;
        private OverlayContainer container;
        public GameListUI(string name, List<string> columns)
        {
            overlay = OverlayManager.Singleton.Create(name + "/overlay");
            container = OverlayManager.Singleton.CreateOverlayElement("BorderPanel", name + "/Container") as OverlayContainer;
            lsv = new ListView(name + "/listview", 0.03f, 0.1f, 0.8f, 0.9f, columns);
            //btnHost = new PushButton(name + "/btnHost", "Host Game", -250, 650, 40, 150);
            //btnJoin = new PushButton(name + "/btnJoin", "Join Game", -50, 650, 40, 150);
            //btnExit = new PushButton(name + "/btnExit", "Exit", 150, 650, 40, 150);
            container.AddChild(lsv.GetContainer());
            //container.AddChild(btnHost.GetContainer());
            //container.AddChild(btnExit.GetContainer());
            //container.AddChild(btnJoin.GetContainer());
            overlay.Add2D(container);
        }

        public void AppendItem(List<string> item)
        {
            lsv.AddItem(item);
        }

        public override void Show()
        {
            overlay.Show();
        }

        public override void Close()
        {
            //overlay.Remove2D(btnHost.GetContainer());
            //overlay.Remove2D(btnJoin.GetContainer());
            //overlay.Remove2D(btnExit.GetContainer());
            lsv.Dispose();
            overlay.Remove2D(lsv.GetContainer());
            overlay.Remove2D(container);
            OverlayManager.Singleton.DestroyOverlayElement(lsv.GetContainer());
            OverlayManager.Singleton.DestroyOverlayElement(container);
            OverlayManager.Singleton.Destroy(overlay);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
