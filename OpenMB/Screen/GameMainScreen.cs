using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mogre;
using Mogre_Procedural.MogreBites;
using OpenMB.Game;
using OpenMB.Widgets;

namespace OpenMB.Screen
{
	public class GameMainScreen : Screen
	{
		private PanelWidget gameMainPanel;
		private ButtonWidget btnTerrain;
		private ButtonWidget btnCamp;
		private ButtonWidget btnReports;
		private ButtonWidget btnNotes;
		private ButtonWidget btnInventory;
		private ButtonWidget btnCharacter;
		private ButtonWidget btnParty;
		private StaticText txtCurrentDate;
		private StaticText txtCurrentTime;
		private object[] param;

		public override string Name
		{
			get
			{
				return "GameMain";
			}
		}

		public GameMainScreen()
		{
		}

		public override void Init(params object[] param)
		{
			this.param = param;
			TimerManager.Instance.Resume();
		}

		public override void Run()
		{
			gameMainPanel = GameManager.Instance.trayMgr.createPanel("gameMainPanel", 1.0f, 0.08f, 0.0f, 0.92f);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Abosulte, 0.1f);
			gameMainPanel.AddCol(Widgets.ValueType.Abosulte, 180);

			btnTerrain = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnTerrain", "Terrain", 150);
			btnTerrain.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnTerrain.Top = 0.025f;
			btnTerrain.OnClick += BtnTerrain_OnClick;
			gameMainPanel.AddWidget(1, 1, btnTerrain, AlignMode.Left, DockMode.FillWidth);
			if (!GameManager.Instance.IS_ENABLE_EDIT_MODE)
			{
				btnTerrain.Hide();
			}

			btnCamp = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnCamp", "Camp", 150);
			btnCamp.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnCamp.Top = 0.025f;
			btnCamp.OnClick += BtnCamp_OnClick;
			gameMainPanel.AddWidget(1, 2, btnCamp);

			btnReports = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnReports", "Reports", 150);
			btnReports.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnReports.OnClick += BtnReports_OnClick;
			btnReports.Top = 0.025f;
			gameMainPanel.AddWidget(1, 3, btnReports, AlignMode.Left, DockMode.FillWidth);

			btnNotes = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnNotes", "Notes", 150);
			btnNotes.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnNotes.Top = 0.025f;
			btnNotes.OnClick += BtnNotes_OnClick;
			gameMainPanel.AddWidget(1, 4, btnNotes);

			btnInventory = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnInventory", "Inventory", 150);
			btnInventory.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnInventory.Top = 0.025f;
			btnInventory.OnClick += BtnInventory_OnClick;
			gameMainPanel.AddWidget(1, 5, btnInventory, AlignMode.Left, DockMode.FillWidth);

			btnCharacter = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnCharacter", "Characters", 150);
			btnCharacter.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnCharacter.Top = 0.025f;
			btnCharacter.OnClick += BtnCharacter_OnClick;
			gameMainPanel.AddWidget(1, 6, btnCharacter);

			btnParty = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnParty", "Party", 150);
			btnParty.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnParty.Top = 0.025f;
			btnParty.OnClick += BtnParty_OnClick;
			gameMainPanel.AddWidget(1, 7, btnParty, AlignMode.Left, DockMode.FillWidth);

			txtCurrentDate = GameManager.Instance.trayMgr.createStaticText("gameDate", TimerManager.Instance.GetDate());
			txtCurrentTime = GameManager.Instance.trayMgr.createStaticText("gameTime", TimerManager.Instance.CurrentTime.ToString());
			txtCurrentDate.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			txtCurrentTime.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			txtCurrentDate.Top = 0.015f;
			txtCurrentTime.Top = 0.03f;
			gameMainPanel.AddWidget(1, 9, txtCurrentDate);
			gameMainPanel.AddWidget(1, 9, txtCurrentTime);
		}

		private void BtnParty_OnClick(object obj)
		{
			ScreenManager.Instance.ChangeScreen("GameParty", false, param[0], "pt_player");
		}

		private void BtnNotes_OnClick(object obj)
		{
			ScreenManager.Instance.ChangeScreen("GameNotes", false, param[0]);
		}

		private void BtnInventory_OnClick(object obj)
		{
			ScreenManager.Instance.ChangeScreen("Inventory", false, param[0], "cha_player");
		}

		private void BtnCharacter_OnClick(object obj)
		{
			ScreenManager.Instance.ChangeScreen("Character", false, param[0], "cha_player");
		}

		private void BtnReports_OnClick(object obj)
		{
			ScreenManager.Instance.ChangeScreen("GameMenu", false, param[0], "mnu_reports");
		}

		private void BtnCamp_OnClick(object obj)
		{
			ScreenManager.Instance.ChangeScreen("GameMenu", false, param[0], "mnu_camp");
		}

		private void BtnTerrain_OnClick(object obj)
		{
			ScreenManager.Instance.ChangeScreen("TerrainEditor", false, param[0]);
		}

		public override void Exit()
		{
			TimerManager.Instance.Pause();

			GameManager.Instance.trayMgr.DestroyAllWidgets();
		}

		public override void Update(float timeSinceLastFrame)
		{
			txtCurrentDate.SetText(TimerManager.Instance.GetDate());
			txtCurrentTime.SetText(TimerManager.Instance.CurrentTime.ToString());
		}
	}
}
