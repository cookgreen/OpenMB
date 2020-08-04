using OpenMB.UI;
using OpenMB.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Screen
{
    public class MultiplayerServerBrowserScreen : Screen
    {
        public override string Name { get { return "MultiplayerServerBrowser"; } }

        public MultiplayerServerBrowserScreen()
        {
        }

        public override void Run()
        {
            var columns = new List<string>();
            columns.Add("Name");
            columns.Add("Module");
            columns.Add("Map");
            columns.Add("Mode");
            columns.Add("Player");
            SimpleListViewWidget listViewWidget = new SimpleListViewWidget("lsvServerList", columns, 0.7f, 0.8f, 0.15f, 0.1f);
            UIManager.Instance.Append(listViewWidget);
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_RIGHT, "btnJoin", "Join", 150);
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_RIGHT, "btnHost", "Host", 150);
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_RIGHT, "btnExit", "Exit", 150);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
