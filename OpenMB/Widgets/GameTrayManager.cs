using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre_Procedural.MogreBites;
using Mogre;
using MOIS;

namespace OpenMB.Widgets
{
    public static class GameTrayManager
    {
		public static InputBox createInputBox(this SdkTrayManager trayMgr, string name, string caption, float width, float boxWidth, string text = null, bool onlyAcceptNum = false)
		{
			InputBox ib = new InputBox(name, caption, width, boxWidth, text, onlyAcceptNum);
			trayMgr.moveWidgetToTray(ib, TrayLocation.TL_NONE);
			ib.Text = text;
			//ib._assignListener(mListener);
			return ib;
		}

		public static Panel createPanel(this SdkTrayManager trayMgr, string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1)
		{
			Panel panel = new Panel(name, width, height, left, top, row, col);
			trayMgr.moveWidgetToTray(panel, TrayLocation.TL_NONE);
			return panel;
		}

		public static PanelScrollable createScrollablePanel(this SdkTrayManager trayMgr, string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1)
		{
			PanelScrollable scrollablePanel = new PanelScrollable(name, width, height, left, top, row, col);
			trayMgr.moveWidgetToTray(scrollablePanel, TrayLocation.TL_NONE);
			return scrollablePanel;
		}

		public static PanelTemplate createTemplatePanel(this SdkTrayManager trayMgr, string name, string template, int width = 0, int height = 0, int top = 0, int left = 0)
		{
			PanelTemplate tmpPanel = new PanelTemplate(name, template, width, height, left, top);
			trayMgr.moveWidgetToTray(tmpPanel, TrayLocation.TL_NONE);
			return tmpPanel;
		}
	}
}
