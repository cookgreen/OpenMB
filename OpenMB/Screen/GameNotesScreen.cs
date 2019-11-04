using Mogre;
using Mogre_Procedural.MogreBites;
using OpenMB.Core;
using OpenMB.Game;
using OpenMB.Mods.XML;
using OpenMB.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Screen
{
	public class GameNotesScreen : Screen
	{
		private ModSideDfnXML currentSideInfo;
		private GameWorld world;
		private List<Widget> widgets;
		public override string Name
		{
			get
			{
				return "GameNotes";
			}
		}

		public override void Init(params object[] param)
		{
			world = param[0] as GameWorld;
			widgets = new List<Widget>();
		}

		public override void Run()
		{
			Panel panel = GameManager.Instance.trayMgr.createPanel("OperationPanel", 1, 0.08f, 0, 0.92f, 1, 8);
			Button btnGameLog = GameManager.Instance.trayMgr.createButton("btnGameLog", "Game Log", 150);
			Button btnRecentMessage = GameManager.Instance.trayMgr.createButton("btnRecentMessage", "Recent", 100);
			Button btnNotes = GameManager.Instance.trayMgr.createButton("btnNotes", "Notes", 100);
			Button btnGameConcepts = GameManager.Instance.trayMgr.createButton("btnGameConcepts", "Concepts", 100);
			Button btnCharacters = GameManager.Instance.trayMgr.createButton("btnCharacters", "Characters", 100);
			Button btnLocations = GameManager.Instance.trayMgr.createButton("btnLocations", "Locations", 100);
			Button btnFactions = GameManager.Instance.trayMgr.createButton("btnFactions", "Factions", 100);
			Button btnReturn = GameManager.Instance.trayMgr.createButton("btnReturn", "Return", 100);
			btnGameLog.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnRecentMessage.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnNotes.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnGameConcepts.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnCharacters.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnLocations.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnFactions.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnReturn.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			panel.AddWidget(1, 1, btnGameLog, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 2, btnRecentMessage, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 3, btnNotes, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 4, btnGameConcepts, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 5, btnCharacters, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 6, btnLocations, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 7, btnFactions, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 8, btnReturn, AlignMode.Center, DockMode.FillWidth);
			btnGameLog.OnClick += BtnGameLog_OnClick;
			btnRecentMessage.OnClick += BtnRecentMessage_OnClick;
			btnNotes.OnClick += BtnNotes_OnClick;
			btnGameConcepts.OnClick += BtnGameConcepts_OnClick;
			btnCharacters.OnClick += BtnCharacters_OnClick;
			btnLocations.OnClick += BtnLocations_OnClick;
			btnFactions.OnClick += BtnFactions_OnClick;
			btnReturn.OnClick += BtnReturn_OnClick;

			BtnFactions_OnClick(btnFactions);
		}

		private void BtnReturn_OnClick(object sender)
		{
			ScreenManager.Instance.ChangeScreenReturn();
		}

		private void BtnFactions_OnClick(object sender)
		{
			ClearWidgets();

			Panel panelFactionList = GameManager.Instance.trayMgr.createPanel("panelFactionList", 0.3f, 0.92f, 0.7f, 0);
			panelFactionList.ChangeRow(Widgets.ValueType.Abosulte, 0.05f);
			panelFactionList.AddRow(Widgets.ValueType.Percent);
			panelFactionList.Padding.PaddingLeft = 0.01f;
			panelFactionList.Padding.PaddingRight = 0.01f;

			StaticText txtFactionsTitle = GameManager.Instance.trayMgr.createStaticText("txtFactionsTitle", "Factions");
			txtFactionsTitle.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			panelFactionList.AddWidget(1, 1, txtFactionsTitle, AlignMode.Center);
			PanelScrollable panelFactions = GameManager.Instance.trayMgr.createScrollablePanel("panelFactions", 1, 1, 0, 0, 1, 1, false);
			panelFactionList.AddWidget(2, 1, panelFactions, AlignMode.Center, DockMode.Fill);

			panelFactions.ChangeRow(Widgets.ValueType.Abosulte, 0.03f);
			panelFactions.AddRows(world.ModData.SideInfos.Count - 1, Widgets.ValueType.Abosulte, 0.03f);
			int curRow = 1;
			foreach (var sideInfo in world.ModData.SideInfos)
			{
				if(!GameSlotManager.Instance.SlotEqual(sideInfo.ID, "slot_faction_state", "inactive"))
				{
					if (!GameSlotManager.Instance.SlotEqual(sideInfo.ID, "slot_faction_visibility", "hidden"))
					{
						if (currentSideInfo == null)
						{
							currentSideInfo = sideInfo;
						}
						var txtFaction = new StaticText("txtFaction_" + sideInfo.Name, sideInfo.Name, 100f, false, ColourValue.Black);
						txtFaction.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
						panelFactions.AddWidgetRelative(curRow, 1, txtFaction);
						curRow++;
					}
				}
			}
			
			PanelScrollable panelFactionDetails = GameManager.Instance.trayMgr.createScrollablePanel("panelFactionDetails", 0.7f, 0.92f);
			panelFactionDetails.ChangeRow(Widgets.ValueType.Abosulte, 0.03f);//next/prev
			panelFactionDetails.AddRow(Widgets.ValueType.Abosulte, 0.05f);//faction name
			panelFactionDetails.AddRow(Widgets.ValueType.Auto);//faction mesh
			panelFactionDetails.AddRow(Widgets.ValueType.Abosulte, 0.03f);//ruler
			panelFactionDetails.AddRow(Widgets.ValueType.Auto);//faction occupy lands
			panelFactionDetails.AddRow(Widgets.ValueType.Auto);//faction vassals
			panelFactionDetails.AddRow(Widgets.ValueType.Abosulte, 0.03f);//empty row
			panelFactionDetails.AddRow(Widgets.ValueType.Abosulte, 0.03f);//foreign relations
			panelFactionDetails.Padding.PaddingLeft = 0.01f;

			StaticTextRelative txtFactionName = new StaticTextRelative("txtFactionName", currentSideInfo.Name, 0, false, ColourValue.Black);
			txtFactionName.Width = txtFactionName.TextWidth;
			txtFactionName.Height = txtFactionName.TextHeight;
			panelFactionDetails.AddWidgetRelative(2, 1, txtFactionName, AlignMode.Center);

			PanelMaterial coatOfArmsPanel = new PanelMaterial("coatOfArmsPanel", currentSideInfo.COA);
			coatOfArmsPanel.Width = 0.3f;
			coatOfArmsPanel.Height = 0.3f;
			panelFactionDetails.AddWidgetRelative(3, 1, coatOfArmsPanel, AlignMode.Center, DockMode.Center);

			string chaID = GameSlotManager.Instance.GetSlot(currentSideInfo.ID, "slot_faction_leader");
			var chaData = world.ModData.CharacterInfos.Where(o => o.ID == chaID).FirstOrDefault();
			StaticTextRelative txtFactionRulerInfo = new StaticTextRelative("txtFactionRulerInfo", string.Format("{0} is ruled by {1}", currentSideInfo.Name,
				chaData == null ? "None" : chaData.Name), 0, false, ColourValue.Black);
			txtFactionRulerInfo.Width = txtFactionName.TextWidth;
			txtFactionRulerInfo.Height = txtFactionName.TextHeight;
			panelFactionDetails.AddWidgetRelative(4, 1, txtFactionRulerInfo);

			StaticTextRelative txtOccupiedLands = new StaticTextRelative("txtOccupiedLands", "It occupies none", 0, false, ColourValue.Black);
			txtOccupiedLands.Width = txtFactionName.TextWidth;
			txtOccupiedLands.Height = txtFactionName.TextHeight;
			panelFactionDetails.AddWidgetRelative(5, 1, txtOccupiedLands);

			StaticTextRelative txtVassalInfos = new StaticTextRelative("txtVassalInfos", "Its vassals are none", 0, false, ColourValue.Black);
			txtVassalInfos.Width = txtFactionName.TextWidth;
			txtVassalInfos.Height = txtFactionName.TextHeight;
			panelFactionDetails.AddWidgetRelative(6, 1, txtVassalInfos);

			StaticTextRelative txtForeignRelationship = new StaticTextRelative("txtForeignRelationship", "Foreign relations: ", 0, false, ColourValue.Black);
			txtForeignRelationship.Width = txtFactionName.TextWidth;
			txtForeignRelationship.Height = txtFactionName.TextHeight;
			panelFactionDetails.AddWidgetRelative(8, 1, txtForeignRelationship);

			widgets.Add(txtFactionsTitle);
			widgets.Add(panelFactionDetails);
			widgets.Add(panelFactionList);
			widgets.Add(panelFactions);
		}

		private void BtnLocations_OnClick(object sender)
		{
			ClearWidgets();
		}

		private void BtnCharacters_OnClick(object sender)
		{
			ClearWidgets();
		}

		private void BtnGameConcepts_OnClick(object sender)
		{
			ClearWidgets();
		}

		private void BtnNotes_OnClick(object sender)
		{
			ClearWidgets();
		}

		private void BtnRecentMessage_OnClick(object sender)
		{
			ClearWidgets();
		}

		private void BtnGameLog_OnClick(object sender)
		{
			ClearWidgets();
		}

		private void ClearWidgets()
		{
			for (int i = 0; i < widgets.Count; i++)
			{
				GameManager.Instance.trayMgr.destroyWidget(widgets[i]);
			}
			widgets.Clear();
		}

		public override void Exit()
		{
			GameManager.Instance.trayMgr.destroyAllWidgets();
		}
	}
}
