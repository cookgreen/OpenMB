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
		public static InputBoxWidget createInputBox(this UIManager trayMgr, TrayLocation trayLocation, string name, string caption, float width, float boxWidth, string text = null, bool onlyAcceptNum = false)
		{
			InputBoxWidget ib = new InputBoxWidget(name, caption, width, boxWidth, text, onlyAcceptNum);
			trayMgr.moveWidgetToTray(ib, trayLocation);
			ib.Text = text;
			//ib._assignListener(mListener);
			return ib;
		}

		public static PanelWidget createPanel(this UIManager trayMgr, string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1)
		{
			PanelWidget panel = new PanelWidget(name, width, height, left, top, row, col);
			trayMgr.moveWidgetToTray(panel, TrayLocation.TL_NONE);
			return panel;
		}

		public static PanelScrollableWidget createScrollablePanel(this UIManager trayMgr, string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1, bool hasBorder = true)
		{
			PanelScrollableWidget scrollablePanel = new PanelScrollableWidget(name, width, height, left, top, row, col, hasBorder);
			trayMgr.moveWidgetToTray(scrollablePanel, TrayLocation.TL_NONE);
			return scrollablePanel;
		}

		public static PanelMaterialWidget createMaterialPanel(this UIManager trayMgr, string name, string texture, float width = 0, float height = 0, float left = 0, float top = 0)
		{
			PanelMaterialWidget materialPanel = new PanelMaterialWidget(name, texture, width, height, left, top);
			trayMgr.moveWidgetToTray(materialPanel, TrayLocation.TL_NONE);
			return materialPanel;
		}

		public static PanelTemplateWidget createTemplatePanel(this UIManager trayMgr, string name, string template, int width = 0, int height = 0, int top = 0, int left = 0)
		{
			PanelTemplateWidget tmpPanel = new PanelTemplateWidget(name, template, width, height, left, top);
			trayMgr.moveWidgetToTray(tmpPanel, TrayLocation.TL_NONE);
			return tmpPanel;
		}
	}
}
