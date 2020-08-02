using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mogre_Procedural.MogreBites;
using OpenMB.UI;
using OpenMB.Mods.XML;
using OpenMB.Script;
using OpenMB.Game;
using OpenMB.UI.Widgets;

namespace OpenMB.Screen
{
	public class GameMenuScreen : Screen
	{
		private GameWorld world;
		private string menuID;
		private ModMenuDfnXml menuData;
		private PanelWidget menuMainPanel;
		private PanelWidget menuItemsPanel;
		private StaticText menuTitle;
		private List<ButtonWidget> menuButtons;
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
			menuButtons = new List<ButtonWidget>();
		}

		public override void Init(params object[] param)
		{
			world = param[0] as GameWorld;
			menuID = param[1].ToString();
		}

		public override void Run()
		{
			var modData = ScreenManager.Instance.ModData;
			var findedMenus = modData.MenuInfos.Where(o => o.ID == menuID);
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

				menuMainPanel = UIManager.Instance.CreatePanel("menuMainPanel");
				menuMainPanel.AddRow(UI.ValueType.Percent);
				menuMainPanel.AddCol(UI.ValueType.Percent);

				menuTitle = UIManager.Instance.CreateStaticText("menuStaticText", menuData.Title);
				menuTitle.MetricMode = Mogre.GuiMetricsMode.GMM_RELATIVE;
				menuTitle.Top = 0.05f;
				menuMainPanel.AddWidget(1, 1, menuTitle, AlignMode.Center);

				menuItemsPanel = UIManager.Instance.CreatePanel("menuItemsPanel", 0.5f, 0.5f, 0, 0);
				menuMainPanel.AddWidget(2, 1, menuItemsPanel, AlignMode.Left, AlignMode.Center, DockMode.Fill);

				foreach (var menu in menuData.Children)
				{
					menuItemsPanel.AddRow(UI.ValueType.Abosulte, 0.05f);
				}

				int row = 2;
				foreach (var menu in menuData.Children)
				{
					var button = UIManager.Instance.CreateButton(menu.id, menu.Text, 200);
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
			loader.ExecuteFunction(
				script, 
				"menuButtonClicked", 
				world,
				this,
				(sender as ButtonWidget).Name
			);
		}

		public override void Exit()
		{
			TimerManager.Instance.Resume();

			UIManager.Instance.DestroyAllWidgets();
		}
	}
}
