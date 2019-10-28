using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mogre;
using Mogre_Procedural.MogreBites;
using OpenMB.Widgets;

namespace OpenMB.Screen
{
	public class GameMainScreen : Screen
	{
		private Panel gameMainPanel;
		private Button btnTerrain;
		private Button btnCamp;
		private Button btnReports;
		private Button btnNotes;
		private Button btnInventory;
		private Button btnCharacter;
		private Button btnParty;
		private StaticText txtCurrentDate;
		private StaticText txtCurrentTime;

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
			base.Init(param);
		}

		public override void Run()
		{
			gameMainPanel = GameManager.Instance.trayMgr.createPanel("gameMainPanel", 1.0f, 0.0f, 0.0f, 0.92f);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Percent);
			gameMainPanel.AddCol(Widgets.ValueType.Abosulte, 200);

			btnTerrain = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnTerrain", "Terrain", 150);
			btnTerrain.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			btnTerrain.Top = 0.025f;
			btnTerrain.OnClick += BtnTerrain_OnClick;
			gameMainPanel.AddWidget(1, 1, btnTerrain);

			btnCamp = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnCamp", "Camp", 150);
			btnCamp.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			btnCamp.Top = 0.025f;
			btnCamp.OnClick += BtnCamp_OnClick;
			gameMainPanel.AddWidget(1, 2, btnCamp);

			btnReports = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnReports", "Reports", 150);
			btnReports.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			btnReports.OnClick += BtnReports_OnClick;
			btnReports.Top = 0.025f;
			gameMainPanel.AddWidget(1, 3, btnReports);

			btnNotes = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnNotes", "Notes", 150);
			btnNotes.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			btnNotes.Top = 0.025f;
			btnNotes.OnClick += BtnNotes_OnClick;
			gameMainPanel.AddWidget(1, 4, btnNotes);

			btnInventory = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnInventory", "Inventory", 150);
			btnInventory.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			btnInventory.Top = 0.025f;
			btnInventory.OnClick += BtnInventory_OnClick;
			gameMainPanel.AddWidget(1, 5, btnInventory);

			btnCharacter = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnCharacter", "Characters", 150);
			btnCharacter.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			btnCharacter.Top = 0.025f;
			btnCharacter.OnClick += BtnCharacter_OnClick;
			gameMainPanel.AddWidget(1, 6, btnCharacter);

			btnParty = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_NONE, "btnParty", "Party", 150);
			btnParty.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			btnParty.Top = 0.025f;
			btnParty.OnClick += BtnParty_OnClick;
			gameMainPanel.AddWidget(1, 7, btnParty);
		}

		private void BtnParty_OnClick(object obj)
		{
		}

		private void BtnNotes_OnClick(object obj)
		{
		}

		private void BtnInventory_OnClick(object obj)
		{
		}

		private void BtnCharacter_OnClick(object obj)
		{
		}

		private void BtnReports_OnClick(object obj)
		{
		}

		private void BtnCamp_OnClick(object obj)
		{
		}

		private void BtnTerrain_OnClick(object obj)
		{

		}
	}
}
