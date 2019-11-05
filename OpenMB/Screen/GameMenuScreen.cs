using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mogre_Procedural.MogreBites;
using OpenMB.Widgets;
using OpenMB.Mods.XML;
using OpenMB.Script;
using OpenMB.Game;

namespace OpenMB.Screen
{
	public class GameMenuScreen : Screen
	{
		private GameWorld world;
		private string menuID;
		private ModMenuDfnXml menuData;
		private Panel menuMainPanel;
		private Panel menuItemsPanel;
		private StaticText menuTitle;
		private List<Button> menuButtons;
		private ScriptLoader loader = new ScriptLoader();
		private ScriptFile script = new ScriptFile();

		public override string Name
		{
			get
			{
				return "GameMenu";
			}
		}

		public GameMenuScreen()
		{
			menuButtons = new List<Button>();
		}

		public override void Init(params object[] param)
		{
			world = param[0] as GameWorld;
			menuID = param[1].ToString();
		}

		public override void Run()
		{
			var modData = ScreenManager.Instance.ModData;
			var findedMenus = modData.MenuInfos.Where(o => o.id == menuID);
			if (findedMenus.Count() > 0)
			{
				menuData = findedMenus.First();
				if(world.GlobalVariableTable.ContainsKey("menuData"))
				{
					world.GlobalVariableTable["menuData"] = menuData;
				}
				else
				{
					world.GlobalVariableTable.Add("menuData", menuData);
				}

				script = new ScriptFile(menuData.Logic);
				loader.ExecuteFunction(script, "menuInit", world);
				menuData = world.GlobalVariableTable["menuData"] as ModMenuDfnXml;

				menuMainPanel = GameManager.Instance.trayMgr.createPanel("menuMainPanel");
				menuMainPanel.AddRow(Widgets.ValueType.Percent);
				menuMainPanel.AddCol(Widgets.ValueType.Percent);

				menuTitle = GameManager.Instance.trayMgr.createStaticText("menuStaticText", menuData.Title);
				menuTitle.MetricMode = Mogre.GuiMetricsMode.GMM_RELATIVE;
				menuTitle.Top = 0.05f;
				menuMainPanel.AddWidget(1, 1, menuTitle, AlignMode.Center);

				menuItemsPanel = GameManager.Instance.trayMgr.createPanel("menuItemsPanel", 0.5f, 0.5f, 0, 0);
				menuMainPanel.AddWidget(2, 1, menuItemsPanel, AlignMode.Left, DockMode.Fill);

				foreach (var menu in menuData.Children)
				{
					menuItemsPanel.AddRow(Widgets.ValueType.Abosulte, 0.05f);
				}

				int row = 2;
				foreach (var menu in menuData.Children)
				{
					var button = GameManager.Instance.trayMgr.createButton(menu.id, menu.Text, 200);
					button.MetricMode = Mogre.GuiMetricsMode.GMM_RELATIVE;
					menuItemsPanel.AddWidget(row, 1, button, AlignMode.Center);
					menuButtons.Add(button);
					button.OnClick += Button_OnClick;
					row++;
				}
			}
		}

		private void Button_OnClick(object sender)
		{
			loader.ExecuteFunction(script, "menuButtonClicked", world, (sender as Button).getName());
		}

		public override void Exit()
		{
			TimerManager.Instance.Resume();

			GameManager.Instance.trayMgr.destroyAllWidgets();
		}
	}
}
