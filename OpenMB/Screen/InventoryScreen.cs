using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre_Procedural.MogreBites;
using Mogre;
using OpenMB.Mods.XML;
using OpenMB.Widgets;
using OpenMB.Game;
using MOIS;

namespace OpenMB.Screen
{
    public class InventoryScreen : Screen
    {
		private GameWorld world;
		private string characterID;
		private ModCharacterDfnXML characterData;
        private List<OverlayElement> elements;
        private OverlayContainer equipmentPanel;
        private OverlayContainer previewPanel;
        private OverlayContainer backpackPanel;
        private int row;
        private int col;
        private const float INV_WIDTH = 0.12f;
        private Entity ent;
        private SceneNode sceneNode;
        private Overlay meshLayer;
        private AnimationState baseAnim;
        private AnimationState topAnim;
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
            elements = new List<OverlayElement>();
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

			var idleSkinAnim = skinData[CharacterAnimationType.CAT_IDLE];
			if (idleSkinAnim == null)
			{
				throw new Exception("Idle Skin Anim Data can't be null!");
			}

			var idleAnim = moddata.AnimationInfos.Where(o => o.ID == idleSkinAnim.AnimID).FirstOrDefault();
			if (idleAnim == null)
			{
				throw new Exception("Idle Anim Data can't be null!");
			}

			if (idleAnim.HasFlags(AnimationFlag.ANF_HAS_TOPBASE) &&
				idleAnim.HasFlags(AnimationFlag.ANF_SKELETON_ANIM) &&
				idleAnim.Resource == skinData.Skeleton)
			{
				var baseIdleAnim = idleAnim[CharacterAnimationPlayType.BASE];
				var topIdleAnim = idleAnim[CharacterAnimationPlayType.TOP];
				if (baseIdleAnim == null || topIdleAnim == null)
				{
					throw new Exception("Base Anim or Top Anim can't be null!");
				}
				meshLayer = OverlayManager.Singleton.Create("CharacterPreview");
				meshLayer.ZOrder = 999;

				SceneManager scm = ScreenManager.Instance.Camera.SceneManager;
				ent = scm.CreateEntity(Guid.NewGuid().ToString(), characterData.MeshName);
				sceneNode = scm.CreateSceneNode();
				sceneNode.Translate(new Mogre.Vector3(0, 0, 0));
				sceneNode.Rotate(Quaternion.IDENTITY);
				float lenght = ent.BoundingBox.Size.Length * 2;
				sceneNode.Translate(new Mogre.Vector3(-7f, 5f, -1.0f * lenght));
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

				equipmentPanel = OverlayManager.Singleton.CreateOverlayElementFromTemplate("CharacterEquipment", "BorderPanel", "inventoryPanelLeftArea") as OverlayContainer;
				previewPanel = OverlayManager.Singleton.CreateOverlayElementFromTemplate("CharacterPreview", "BorderPanel", "inventoryPanelMiddleArea") as OverlayContainer;
				backpackPanel = OverlayManager.Singleton.CreateOverlayElementFromTemplate("CharacterBackpack", "BorderPanel", "inventoryPanelRightArea") as OverlayContainer;
				row = (int)System.Math.Round((backpackPanel.Height - 0.04) / INV_WIDTH);
				col = (int)System.Math.Round((backpackPanel.Width - 0.04) / INV_WIDTH);

				int row_counter = 1;
				int col_counter = 1;
				float topValue = 0.0f;
				float leftValue = 0.0f;

				for (int i = 0; i < 35; i++)
				{
					if (i == 0)
					{
						topValue = 0.02f;
						leftValue = 0.02f;
					}
					else if (col_counter <= col)
					{
						leftValue += INV_WIDTH;
					}
					else
					{
						topValue += INV_WIDTH;
						col_counter = 1;
						row_counter++;
						leftValue = 0.02f;
					}
					OverlayElement invSlotElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("InventorySlot", "BorderPanel", "INV_NO_" + i.ToString());
					invSlotElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
					invSlotElement.Left = leftValue;
					invSlotElement.Top = topValue;
					invSlotElement.Width = INV_WIDTH;
					invSlotElement.Height = INV_WIDTH;
					if (row_counter <= row)
					{
						backpackPanel.AddChild(invSlotElement);
					}
					else
					{
						invSlotElement.Hide();
						backpackPanel.AddChild(invSlotElement);
					}
					elements.Add(invSlotElement);
					col_counter++;
				}
				GameManager.Instance.trayMgr.getTraysLayer().Add2D(equipmentPanel);
				GameManager.Instance.trayMgr.getTraysLayer().Add2D(previewPanel);
				GameManager.Instance.trayMgr.getTraysLayer().Add2D(backpackPanel);
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

            Control.nukeOverlayElement(equipmentPanel);
            Control.nukeOverlayElement(previewPanel);
            Control.nukeOverlayElement(backpackPanel);
            if (OnScreenExit != null)
            {
                OnScreenExit();
            }
        }
    }
}
