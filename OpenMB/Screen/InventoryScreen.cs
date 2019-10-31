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
		private string characterID;
		private ModCharacterDfnXML characterData;
        private Entity ent;
        private SceneNode sceneNode;
        private Overlay meshLayer;
        private AnimationState baseAnim;
        private AnimationState topAnim;
		private Panel discordPanel;
		private Panel discordInventoryPanel;
		private Panel playerPanel;
		private Panel playerEquipPanel;
		private Panel playerPreviewPanel;
		private Panel backpackPanel;
		private Panel backpackInventoryPanel;

		public override event Action OnScreenExit;

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
				characterID = param[1].ToString();
            }
            GameManager.Instance.trayMgr.destroyAllWidgets();
        }

        public override void Run()
		{
			var moddata = ScreenManager.Instance.ModData;
			characterData = moddata.CharacterInfos.Where(o => o.ID == characterID).FirstOrDefault();
			if (characterData == null)
			{
				throw new Exception("Character Data can't be null!");
			}

			var skinData = moddata.SkinInfos.Where(o => o.skinID == characterData.SkinID).FirstOrDefault();
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

			ModSubAnimationDfnXml baseIdleAnim = null;
			ModSubAnimationDfnXml topIdleAnim = null;
			if (idleAnim.HasFlags(AnimationFlag.ANF_HAS_TOPBASE))
			{
				baseIdleAnim = idleAnim[AnimPlayType.BASE];
				topIdleAnim = idleAnim[AnimPlayType.TOP];
			}
			else
			{
				baseIdleAnim = idleAnim.SubAnimations.Where(o => o.PlayType == AnimPlayType.FULL).Random();
				topIdleAnim = idleAnim.SubAnimations.Where(o => o.PlayType == AnimPlayType.FULL).Random();
			}
			if (baseIdleAnim == null || topIdleAnim == null)
			{
				throw new Exception("Base Anim or Top Anim can't be null!");
			}

			meshLayer = OverlayManager.Singleton.Create("CharacterPreview");
			meshLayer.ZOrder = (ushort)(GameManager.Instance.trayMgr.getCursorContainer().ZOrder - 1);

			SceneManager scm = ScreenManager.Instance.Camera.SceneManager;
			ent = scm.CreateEntity(Guid.NewGuid().ToString(), characterData.MeshName);
			sceneNode = scm.CreateSceneNode();
			sceneNode.Translate(new Mogre.Vector3(0, 0, 0));
			sceneNode.Rotate(Quaternion.IDENTITY);
			float length = ent.BoundingBox.Size.Length * 2;
			sceneNode.Translate(new Mogre.Vector3(-2f, -6.3f, -1.0f * length));
			sceneNode.Scale(0.7f, 0.8f, 0.8f);
			ent.RenderQueueGroup = (byte)RenderQueueGroupID.RENDER_QUEUE_MAX;
			ent.Skeleton.BlendMode = SkeletonAnimationBlendMode.ANIMBLEND_CUMULATIVE;

			baseAnim = ent.GetAnimationState(baseIdleAnim.Name);
			topAnim = ent.GetAnimationState(topIdleAnim.Name);
			baseAnim.Enabled = true;
			topAnim.Enabled = true;
			baseAnim.Loop = true;
			topAnim.Loop = true;

			sceneNode.AttachObject(ent);
			meshLayer.Add3D(sceneNode);
			meshLayer.Show();

			discordPanel = GameManager.Instance.trayMgr.createPanel("discordPanel", 0.3f, 1, 0, 0, 3, 1);
			discordPanel.ChangeRow(Widgets.ValueType.Abosulte, 0.05f);
			var txtDiscord = GameManager.Instance.trayMgr.createStaticText("txtDiscord", "Discord");
			txtDiscord.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			discordInventoryPanel = GameManager.Instance.trayMgr.createPanel("discordInventoryPanel", 0.3f, 1, 0, 0, 9, 3);
			discordPanel.AddWidget(1, 1, txtDiscord, AlignMode.Center);
			discordPanel.AddWidget(2, 1, discordInventoryPanel);

			int currRow = 1;
			int currCol = 1;
			for (int i = 0; i < 9; i++)
			{
				var invSlot = new PanelTemplate("DiscordInvSlot_" + (i + 1).ToString(), "InventorySlot");
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
			playerPreviewPanel = GameManager.Instance.trayMgr.createPanel("playerPreviewPanel", 1, 1);

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
			txtOutfit.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			txtArms.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			playerEquipPanel.AddWidget(1, 2, txtOutfit, AlignMode.Center);
			playerEquipPanel.AddWidget(1, 3, txtArms, AlignMode.Center);
			for (int i = 0; i < 8; i++)
			{
				var equipSlot = new PanelTemplate("EquipSlot_" + (i + 1).ToString(), "InventorySlot");

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
				}
			}


			var txtPreviewHeadArmourTotal = GameManager.Instance.trayMgr.createStaticText("txtPreviewHeadArmourTotal", "Head Armour Total: 0");
			var txtPreviewBodyArmourTotal = GameManager.Instance.trayMgr.createStaticText("txtPreviewBodyArmourTotal", "Body Armour Total: 0");
			var txtPreviewLegArmourTotal = GameManager.Instance.trayMgr.createStaticText("txtPreviewLegArmourTotal", "Leg Armour Total: 0");
			var txtPreviewEncumbrance = GameManager.Instance.trayMgr.createStaticText("txtPreviewEncumbrance", "Encumbrance: 0");
			txtPreviewHeadArmourTotal.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			txtPreviewBodyArmourTotal.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			txtPreviewLegArmourTotal.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			txtPreviewEncumbrance.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			var btnReturn = GameManager.Instance.trayMgr.createButton("btnInventoryReturn", "Return", 200);
			btnReturn.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnReturn.OnClick += (sender) =>
			{
				ScreenManager.Instance.ChangeScreenReturn();
			};
			playerPreviewPanel.AddWidget(2, 2, txtPreviewHeadArmourTotal, AlignMode.Center);
			playerPreviewPanel.AddWidget(3, 2, txtPreviewBodyArmourTotal, AlignMode.Center);
			playerPreviewPanel.AddWidget(4, 2, txtPreviewLegArmourTotal, AlignMode.Center);
			playerPreviewPanel.AddWidget(5, 2, txtPreviewEncumbrance, AlignMode.Center);
			playerPreviewPanel.AddWidget(6, 2, btnReturn, AlignMode.Center, DockMode.FillWidth);

			backpackPanel = GameManager.Instance.trayMgr.createPanel("backpackPanel", 0.3f, 1, 0.7f, 0);
			backpackPanel.ChangeRow(Widgets.ValueType.Abosulte, 0.05f);
			backpackPanel.AddRow(Widgets.ValueType.Percent);
			backpackPanel.AddRow(Widgets.ValueType.Abosulte, 0.03f);

			var txtInvTitle = GameManager.Instance.trayMgr.createStaticText("txtInvTitle", "Inventory");
			txtInvTitle.WidgetMetricMode = GuiMetricsMode.GMM_RELATIVE;
			backpackInventoryPanel = GameManager.Instance.trayMgr.createPanel("backpackInventoryPanel", 1, 1, 0, 0, 10, 3);
			backpackPanel.AddWidget(1, 1, txtInvTitle, AlignMode.Center, DockMode.Fill);
			backpackPanel.AddWidget(2, 1, backpackInventoryPanel, AlignMode.Center, DockMode.Fill);
				
			int curRow = 1;
			int curCol = 1;
			for (int i = 0; i < 30; i++)
			{
				var invSlot = new PanelTemplate("InvSlot_" + (i + 1).ToString(), "InventorySlot");
				backpackInventoryPanel.AddWidgetRelative(curRow, curCol, invSlot, AlignMode.Center, DockMode.Fill);
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
			if (baseAnim != null)
			{
				baseAnim.AddTime(timeSinceLastFrame);
			}
			if (topAnim != null)
			{
				topAnim.AddTime(timeSinceLastFrame);
			}
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
            OverlayManager.Singleton.Destroy(meshLayer);

            SceneManager scm = ScreenManager.Instance.Camera.SceneManager;
            scm.DestroySceneNode(sceneNode);
            scm.DestroyEntity(ent);

            baseAnim.Dispose();
            topAnim.Dispose();

			GameManager.Instance.trayMgr.destroyAllWidgets();
            if (OnScreenExit != null)
            {
                OnScreenExit();
            }
        }
    }
}
