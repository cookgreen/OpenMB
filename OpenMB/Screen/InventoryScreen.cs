using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre_Procedural.MogreBites;
using Mogre;
using OpenMB.Game;
using OpenMB.Mods.XML;
using OpenMB.Widgets;
using OpenMB.Utilities;
using MOIS;

namespace OpenMB.Screen
{
    public class InventoryScreen : Screen
    {
		private GameWorld world;
		private GameObject gameObject;
		private string chaID;
		private ModCharacterDfnXML chaData;
		private PanelWidget discordPanel;
		private PanelWidget discordInventoryPanel;
		private PanelWidget playerPanel;
		private PanelWidget playerEquipPanel;
		private PanelWidget playerPreviewPanel;
		private PanelWidget backpackPanel;
		private PanelScrollableWidget backpackInventoryPanel;
		private Overlay meshLayer;

        public override string Name
        {
            get
            {
                return "Inventory";
            }
        }

        public InventoryScreen()
        {
        }

        /// <summary>
        /// Init parameters:
        /// character ID
        /// </summary>
        /// <param name="param"></param>
        public override void Init(params object[] param)
        {
            if (param.Length > 0)
            {
				world = param[0] as GameWorld;
				chaID = param[1].ToString();
            }
            GameManager.Instance.trayMgr.DestroyAllWidgets();
        }

        public override void Run()
		{
			var moddata = ScreenManager.Instance.ModData;
			chaData = moddata.CharacterInfos.Where(o => o.ID == chaID).FirstOrDefault();
			if (chaData == null)
			{
				throw new Exception("Character Data can't be null!");
			}

			var skinData = moddata.SkinInfos.Where(o => o.ID == chaData.Skin).FirstOrDefault();
			if (skinData == null)
			{
				throw new Exception("Character Skin Data can't be null!");
			}

			var idleSkinAnim = skinData[ChaAnimType.CAT_IDLE];
			if (idleSkinAnim == null)
			{
				throw new Exception("Idle Skin Anim Data can't be null!");
			}

			var idleAnim = moddata.AnimationInfos.Where(o => o.ID == idleSkinAnim.AnimID).FirstOrDefault();
			if (idleAnim == null)
			{
				throw new Exception("Idle Anim Data can't be null!");
			}

			discordPanel = GameManager.Instance.trayMgr.createPanel("discordPanel", 0.3f, 1);
			discordPanel.Padding.PaddingLeft = 0.01f;
			discordPanel.Padding.PaddingRight = 0.01f;
			discordPanel.ChangeRow(Widgets.ValueType.Abosulte, 0.05f);
			discordPanel.AddRow(Widgets.ValueType.Percent);
			var txtDiscord = GameManager.Instance.trayMgr.createStaticText("txtDiscord", "Discord");
			txtDiscord.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			discordInventoryPanel = GameManager.Instance.trayMgr.createPanel("discordInventoryPanel", 0.3f, 1, 0, 0, 9, 3);
			discordPanel.AddWidget(1, 1, txtDiscord, AlignMode.Center);
			discordPanel.AddWidget(2, 1, discordInventoryPanel, AlignMode.Center, DockMode.Fill);

			int currRow = 1;
			int currCol = 1;
			for (int i = 0; i < 9; i++)
			{
				var invSlot = new PanelTemplateWidget("DiscordInvSlot_" + (i + 1).ToString(), "InventorySlot");
				discordInventoryPanel.AddWidgetRelative(currRow, currCol, invSlot, AlignMode.Center, DockMode.Fill);
				if ((i + 1) % 3 == 0)
				{
					currRow++;
					currCol = 1;
				}
				else
				{
					currCol++;
				}
			}


			playerPanel = GameManager.Instance.trayMgr.createPanel("playerPanel", 0.4f, 1, 0.3f);
			playerPanel.ChangeRow(Widgets.ValueType.Abosulte, 0.6f);
			playerPanel.AddRow(Widgets.ValueType.Abosulte, 0.4f);

			playerEquipPanel = GameManager.Instance.trayMgr.createPanel("playerEquipPanel", 1, 1);
			playerEquipPanel.Padding.PaddingLeft = 0.01f;
			playerEquipPanel.Padding.PaddingRight = 0.01f;
			playerPreviewPanel = GameManager.Instance.trayMgr.createPanel("playerPreviewPanel", 1, 1);
			playerPreviewPanel.Padding.PaddingLeft = 0.01f;
			playerPreviewPanel.Padding.PaddingRight = 0.01f;
			playerPreviewPanel.ChangeCol(Widgets.ValueType.Abosulte, 0.6f);
			playerPreviewPanel.AddCol(Widgets.ValueType.Abosulte, 0.4f);
			playerPreviewPanel.AddRow(Widgets.ValueType.Abosulte, 0.05f);
			playerPreviewPanel.AddRow(Widgets.ValueType.Abosulte, 0.05f);
			playerPreviewPanel.AddRow(Widgets.ValueType.Abosulte, 0.05f);
			playerPreviewPanel.AddRow(Widgets.ValueType.Abosulte, 0.05f);
			playerPreviewPanel.AddRow(Widgets.ValueType.Abosulte, 0.1f);

			playerEquipPanel.ChangeRow(Widgets.ValueType.Abosulte, 0.05f);
			playerEquipPanel.AddRow(Widgets.ValueType.Percent);
			playerEquipPanel.AddRow(Widgets.ValueType.Percent);
			playerEquipPanel.AddRow(Widgets.ValueType.Percent);
			playerEquipPanel.AddRow(Widgets.ValueType.Percent);
			playerEquipPanel.AddCol(Widgets.ValueType.Percent);
			playerEquipPanel.AddCol(Widgets.ValueType.Percent);

			playerPanel.AddWidget(1, 1, playerEquipPanel, AlignMode.Left, DockMode.Fill);
			playerPanel.AddWidget(2, 1, playerPreviewPanel, AlignMode.Left, DockMode.Fill);

			var txtOutfit = GameManager.Instance.trayMgr.createStaticText("txtOutfit", "Outfit");
			var txtArms = GameManager.Instance.trayMgr.createStaticText("txtArms", "Arms");
			txtOutfit.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			txtArms.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			playerEquipPanel.AddWidget(1, 2, txtOutfit, AlignMode.Center);
			playerEquipPanel.AddWidget(1, 3, txtArms, AlignMode.Center);
			for (int i = 0; i < 9; i++)
			{
				var equipSlot = new PanelTemplateWidget("EquipSlot_" + (i + 1).ToString(), "InventorySlot");
				switch (i)
				{
					case 0:
						playerEquipPanel.AddWidgetRelative(2, 2, equipSlot, AlignMode.Center, DockMode.Fill);
						break;
					case 1:
						playerEquipPanel.AddWidgetRelative(3, 2, equipSlot, AlignMode.Center, DockMode.Fill);
						break;
					case 2:
						playerEquipPanel.AddWidgetRelative(4, 2, equipSlot, AlignMode.Center, DockMode.Fill);
						break;
					case 3:
						playerEquipPanel.AddWidgetRelative(5, 1, equipSlot, AlignMode.Center, DockMode.Fill);
						break;
					case 4:
						playerEquipPanel.AddWidgetRelative(2, 3, equipSlot, AlignMode.Center, DockMode.Fill);
						break;
					case 5:
						playerEquipPanel.AddWidgetRelative(3, 3, equipSlot, AlignMode.Center, DockMode.Fill);
						break;
					case 6:
						playerEquipPanel.AddWidgetRelative(4, 3, equipSlot, AlignMode.Center, DockMode.Fill);
						break;
					case 7:
						playerEquipPanel.AddWidgetRelative(3, 1, equipSlot, AlignMode.Center, DockMode.Fill);
						break;
					case 8:
						playerEquipPanel.AddWidgetRelative(5, 3, equipSlot, AlignMode.Center, DockMode.Fill);
						break;
				}
			}

			meshLayer = OverlayManager.Singleton.Create("CharacterPreview");
			meshLayer.ZOrder = (ushort)(GameManager.Instance.trayMgr.GetCursorContainer().ZOrder - 1);
			gameObject = new Character(world, chaData, skinData, new Mogre.Vector3(), true);
			float length = gameObject.Mesh.Entity.BoundingBox.Size.Length * 2;
			gameObject.Mesh.Entity.RenderQueueGroup = (byte)RenderQueueGroupID.RENDER_QUEUE_MAX;
			gameObject.Mesh.EntityNode.Translate(new Mogre.Vector3(-2f, -6.3f, -1.0f * length));
			gameObject.Mesh.EntityNode.Scale(0.7f, 0.8f, 0.8f);
			meshLayer.Add3D(gameObject.MeshNode);
			meshLayer.Show();

			var txtPreviewHeadArmourTotal = GameManager.Instance.trayMgr.createStaticText("txtPreviewHeadArmourTotal", "Head Armour Total: 0");
			var txtPreviewBodyArmourTotal = GameManager.Instance.trayMgr.createStaticText("txtPreviewBodyArmourTotal", "Body Armour Total: 0");
			var txtPreviewLegArmourTotal = GameManager.Instance.trayMgr.createStaticText("txtPreviewLegArmourTotal", "Leg Armour Total: 0");
			var txtPreviewEncumbrance = GameManager.Instance.trayMgr.createStaticText("txtPreviewEncumbrance", "Encumbrance: 0");
			txtPreviewHeadArmourTotal.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			txtPreviewBodyArmourTotal.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			txtPreviewLegArmourTotal.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			txtPreviewEncumbrance.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			var btnReturn = GameManager.Instance.trayMgr.createButton("btnInventoryReturn", "Return", 200);
			btnReturn.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnReturn.OnClick += (sender) =>
			{
				ScreenManager.Instance.ChangeScreenReturn();
			};
			playerPreviewPanel.AddWidget(2, 2, txtPreviewHeadArmourTotal, AlignMode.Center, DockMode.FillWidth);
			playerPreviewPanel.AddWidget(3, 2, txtPreviewBodyArmourTotal, AlignMode.Center, DockMode.FillWidth);
			playerPreviewPanel.AddWidget(4, 2, txtPreviewLegArmourTotal, AlignMode.Center, DockMode.FillWidth);
			playerPreviewPanel.AddWidget(5, 2, txtPreviewEncumbrance, AlignMode.Center, DockMode.FillWidth);
			playerPreviewPanel.AddWidget(6, 2, btnReturn, AlignMode.Center, DockMode.FillWidth);

			backpackPanel = GameManager.Instance.trayMgr.createPanel("backpackPanel", 0.3f, 1, 0.7f, 0);
			backpackPanel.Padding.PaddingRight = 0.01f;
			backpackPanel.Padding.PaddingLeft = 0.01f;
			backpackPanel.ChangeRow(Widgets.ValueType.Abosulte, 0.05f);
			backpackPanel.AddRow(Widgets.ValueType.Percent);
			backpackPanel.AddRow(Widgets.ValueType.Abosulte, 0.03f);

			var txtInvTitle = GameManager.Instance.trayMgr.createStaticText("txtInvTitle", "Inventory");
			txtInvTitle.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			backpackInventoryPanel = GameManager.Instance.trayMgr.createScrollablePanel("backpackInventoryPanel", 1, 1, 0, 0, 20, 3);
			backpackPanel.AddWidget(1, 1, txtInvTitle, AlignMode.Center, DockMode.Fill);
			backpackPanel.AddWidget(2, 1, backpackInventoryPanel, AlignMode.Center, DockMode.Fill);
				
			int curRow = 1;
			int curCol = 1;
			for (int i = 0; i < 60; i++)
			{
				var invSlot = new PanelTemplateWidget("InvSlot_" + (i + 1).ToString(), "InventorySlot");
				invSlot.Height = 0.1f;
				backpackInventoryPanel.ChangeRow(Widgets.ValueType.Abosulte, invSlot.Height, curRow);
				backpackInventoryPanel.AddWidget(curRow, curCol, invSlot, AlignMode.Center, DockMode.Fill);
				if ((i + 1) % 3 == 0)
				{
					curRow++;
					curCol = 1;
				}
				else
				{
					curCol++;
				}
			}
		}

        public override void Update(float timeSinceLastFrame)
        {
        }

        public override void InjectKeyPressed(KeyEvent arg)
        {
            base.InjectKeyPressed(arg);
            if (arg.key == KeyCode.KC_ESCAPE)
            {
                Exit();
            }
        }

        public override void InjectKeyReleased(KeyEvent arg)
        {
            base.InjectKeyReleased(arg);
            if (arg.key == KeyCode.KC_ESCAPE)
            {
                Exit();
            }
        }

        public override void Exit()
		{
			gameObject.Destroy();
			OverlayManager.Singleton.Destroy(meshLayer);
			GameManager.Instance.trayMgr.DestroyAllWidgets();
        }
    }
}
