using Mogre;
using Mogre_Procedural.MogreBites;
using OpenMB.Core;
using OpenMB.Game;
using OpenMB.Mods.XML;
using OpenMB.Utilities;
using OpenMB.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenMB.UI.Widgets;

namespace OpenMB.Screen
{
	public class GameNotesScreen : Screen
	{
		private GameWorld world;
		private List<Widget> subWidgets;
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
			subWidgets = new List<Widget>();
		}

		public override void Run()
		{
			PanelWidget panel = UIManager.Instance.CreatePanel("OperationPanel", 1, 0.08f, 0, 0.92f, 1, 8);
			ButtonWidget btnGameLog = UIManager.Instance.CreateButton("btnGameLog", "Game Log", 150);
			ButtonWidget btnRecentMessage = UIManager.Instance.CreateButton("btnRecentMessage", "Recent", 100);
			ButtonWidget btnNotes = UIManager.Instance.CreateButton("btnNotes", "Notes", 100);
			ButtonWidget btnGameConcepts = UIManager.Instance.CreateButton("btnGameConcepts", "Concepts", 100);
			ButtonWidget btnCharacters = UIManager.Instance.CreateButton("btnCharacters", "Characters", 100);
			ButtonWidget btnLocations = UIManager.Instance.CreateButton("btnLocations", "Locations", 100);
			ButtonWidget btnFactions = UIManager.Instance.CreateButton("btnFactions", "Factions", 100);
			ButtonWidget btnReturn = UIManager.Instance.CreateButton("btnReturn", "Return", 100);
			btnGameLog.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnRecentMessage.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnNotes.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnGameConcepts.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnCharacters.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnLocations.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnFactions.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnReturn.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			panel.AddWidget(1, 1, btnGameLog, AlignMode.Center, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 2, btnRecentMessage, AlignMode.Center, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 3, btnNotes, AlignMode.Center, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 4, btnGameConcepts, AlignMode.Center, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 5, btnCharacters, AlignMode.Center, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 6, btnLocations, AlignMode.Center, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 7, btnFactions, AlignMode.Center, AlignMode.Center, DockMode.FillWidth);
			panel.AddWidget(1, 8, btnReturn, AlignMode.Center, AlignMode.Center, DockMode.FillWidth);
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
			ClearAllWidgets();

			PanelWidget panelFactionList = UIManager.Instance.CreatePanel("panelFactionList", 0.3f, 0.92f, 0.7f, 0);
			panelFactionList.ChangeRow(UI.ValueType.Abosulte, 0.05f);
			panelFactionList.AddRow(UI.ValueType.Percent);
			panelFactionList.Padding.PaddingLeft = 0.01f;
			panelFactionList.Padding.PaddingRight = 0.01f;

			StaticText txtFactionsTitle = UIManager.Instance.CreateStaticText("txtFactionsTitle", GameString.FromString("ui_game_notes_factions_title", "Factions").ToString());
			txtFactionsTitle.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			panelFactionList.AddWidget(1, 1, txtFactionsTitle, AlignMode.Center);
			PanelScrollableWidget panelFactions = UIManager.Instance.CreateScrollablePanel("panelFactions", 1, 1, 0, 0, 1, 1, false);
			panelFactionList.AddWidget(2, 1, panelFactions, AlignMode.Center, AlignMode.Center, DockMode.Fill);

			panelFactions.ChangeRow(UI.ValueType.Abosulte, 0.03f);
			panelFactions.AddRows(world.ModData.SideInfos.Count - 1, UI.ValueType.Abosulte, 0.03f);
			int curRow = 1;

			widgets.Add(txtFactionsTitle);
			widgets.Add(panelFactionList);
			widgets.Add(panelFactions);

			foreach (var sideInfo in world.ModData.SideInfos)
			{
				if(!GameSlotManager.Instance.SlotEqual(sideInfo.ID, "slot_faction_state", "inactive") &&
				   !GameSlotManager.Instance.SlotEqual(sideInfo.ID, "slot_faction_visibility", "hidden"))
				{
					if (curRow == 1)
					{
						BuildFactionDetails(sideInfo);
					}
					var btnFaction = new StaticTextButton("txtFaction_" + sideInfo.Name, GameString.FromString(sideInfo.ID, sideInfo.Name).ToString(), 
						(Color.FromArgb(36, 35, 191).ToColourValue()), 
						ColourValue.Black, true);
					panelFactions.AddWidget(curRow, 1, btnFaction);
					btnFaction.UserData = sideInfo;
					btnFaction.OnClick += (evtObj) =>
					{
						BuildFactionDetails((evtObj as Widget).UserData as ModSideDfnXML);
					};
					curRow++;
				}
			}
		}

		private void BuildFactionDetails(ModSideDfnXML sideInfo)
		{
			ClearSubWidgets();

			PanelScrollableWidget panelFactionDetails = UIManager.Instance.CreateScrollablePanel("panelFactionDetails", 0.7f, 0.92f);
			panelFactionDetails.ChangeRow(UI.ValueType.Abosulte, 0.03f);//next/prev
			panelFactionDetails.AddRow(UI.ValueType.Abosulte, 0.05f);//faction name
			panelFactionDetails.AddRow(UI.ValueType.Auto);//faction mesh
			panelFactionDetails.AddRow(UI.ValueType.Abosulte, 0.03f);//ruler
			panelFactionDetails.AddRow(UI.ValueType.Auto);//faction occupy lands
			panelFactionDetails.AddRow(UI.ValueType.Auto);//faction vassals
			panelFactionDetails.AddRow(UI.ValueType.Abosulte, 0.03f);//empty row
			panelFactionDetails.AddRow(UI.ValueType.Abosulte, 0.03f);//foreign relations
			panelFactionDetails.Padding.PaddingLeft = 0.01f;

			StaticTextRelativeWidget txtFactionName = new StaticTextRelativeWidget("txtFactionName", GameString.FromString(sideInfo.ID, sideInfo.Name).ToString(), 0, false, ColourValue.Black, 150);
			panelFactionDetails.AddWidget(2, 1, txtFactionName, AlignMode.Center);

			PanelMaterialWidget coatOfArmsPanel = new PanelMaterialWidget("coatOfArmsPanel", sideInfo.COA);
			coatOfArmsPanel.Width = 0.3f;
			coatOfArmsPanel.Height = 0.3f;
			panelFactionDetails.AddWidget(3, 1, coatOfArmsPanel, AlignMode.Center, AlignMode.Center, DockMode.Center);

			string chaID = GameSlotManager.Instance.GetSlot(sideInfo.ID, "slot_faction_leader");
			var chaData = world.ModData.CharacterInfos.Where(o => o.ID == chaID).FirstOrDefault();

			GameRegisterManager.Instance.SetRegisterValue("reg0", GameString.FromString(sideInfo.ID, sideInfo.Name).ToString());
			GameRegisterManager.Instance.SetRegisterValue("reg1", GameString.FromString(chaData.ID, chaData.Name).ToString());
			StaticTextRelativeWidget txtFactionRulerInfo = new StaticTextRelativeWidget("txtFactionRulerInfo", GameString.FromString("@{reg0} is ruled by {reg1}").ToString(),0, false, ColourValue.Black);
			txtFactionRulerInfo.Width = txtFactionName.TextWidth;
			txtFactionRulerInfo.Height = txtFactionName.TextHeight;
			panelFactionDetails.AddWidget(4, 1, txtFactionRulerInfo);

			StaticTextRelativeWidget txtOccupiedLands = new StaticTextRelativeWidget("txtOccupiedLands", GameString.FromString("@It occupies none").ToString(), 0, false, ColourValue.Black);
			panelFactionDetails.AddWidget(5, 1, txtOccupiedLands);

			StaticTextRelativeWidget txtVassalInfos = new StaticTextRelativeWidget("txtVassalInfos", GameString.FromString("@Its vassals are none").ToString(), 0, false, ColourValue.Black);
			panelFactionDetails.AddWidget(6, 1, txtVassalInfos);

			StaticTextRelativeWidget txtForeignRelationship = new StaticTextRelativeWidget("txtForeignRelationship", GameString.FromString("@Foreign relations:").ToString(), 0, false, ColourValue.Black);
			panelFactionDetails.AddWidget(8, 1, txtForeignRelationship);

			subWidgets.Add(panelFactionDetails);
		}

		private void BtnLocations_OnClick(object sender)
		{
			ClearAllWidgets();
		}

		private void BtnCharacters_OnClick(object sender)
		{
			ClearAllWidgets();
		}

		private void BtnGameConcepts_OnClick(object sender)
		{
			ClearAllWidgets();
		}

		private void BtnNotes_OnClick(object sender)
		{
			ClearAllWidgets();
		}

		private void BtnRecentMessage_OnClick(object sender)
		{
			ClearAllWidgets();
		}

		private void BtnGameLog_OnClick(object sender)
		{
			ClearAllWidgets();
		}

		private void ClearWidgets()
		{
			for (int i = 0; i < widgets.Count; i++)
			{
				UIManager.Instance.DestroyWidget(widgets[i]);
			}
			widgets.Clear();
		}

		private void ClearSubWidgets()
		{
			for (int i = 0; i < subWidgets.Count; i++)
			{
				UIManager.Instance.DestroyWidget(subWidgets[i]);
			}
			subWidgets.Clear();
		}

		private void ClearAllWidgets()
		{
			ClearWidgets();
			ClearSubWidgets();
		}

		public override void Exit()
		{
			UIManager.Instance.DestroyAllWidgets();
		}
	}
}
