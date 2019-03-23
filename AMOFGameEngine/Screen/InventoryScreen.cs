using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre_Procedural.MogreBites;
using Mogre;
using AMOFGameEngine.Widgets;
using AMOFGameEngine.Game;
using MOIS;

namespace AMOFGameEngine.Screen
{
    public class InventoryScreen : Screen
    {
        private List<OverlayElement> elements;
        private OverlayContainer equipmentPanel;
        private OverlayContainer previewPanel;
        private OverlayContainer backpackPanel;
        private int row;
        private int col;
        private const float INV_WIDTH = 0.12f;
        private Entity ent;
        private SceneNode sceneNode;
        private string meshName;
        private Overlay meshLayer;
        private string[] animNames;
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
        /// 1. meshName
        /// 2. animation name array
        ///     a. animation top name
        ///     b. animation base name
        /// </summary>
        /// <param name="param"></param>
        public override void Init(params object[] param)
        {
            if (param.Length > 0)
            {
                meshName = param[0].ToString();
                animNames = param[1] as string[];
            }
            GameManager.Instance.trayMgr.destroyAllWidgets();
        }

        public override void Run()
        {
            meshLayer = OverlayManager.Singleton.Create("CharacterPreview");
            meshLayer.ZOrder = 999;

            SceneManager scm = ScreenManager.Instance.Camera.SceneManager;
            ent = scm.CreateEntity(Guid.NewGuid().ToString(), meshName);
            sceneNode = scm.CreateSceneNode();
            sceneNode.Translate(new Mogre.Vector3(0, 0, 0));
            sceneNode.Rotate(Quaternion.IDENTITY);
            float lenght = ent.BoundingBox.Size.Length * 2;
            sceneNode.Translate(new Mogre.Vector3(-7f, 5f, -1.0f * lenght));
            ent.RenderQueueGroup = (byte)RenderQueueGroupID.RENDER_QUEUE_MAX;
            ent.Skeleton.BlendMode = SkeletonAnimationBlendMode.ANIMBLEND_CUMULATIVE;

            baseAnim = ent.GetAnimationState(animNames[1]);
            topAnim = ent.GetAnimationState(animNames[0]);
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
                    topValue +=  INV_WIDTH;
                    col_counter = 1;
                    row_counter++;
                    leftValue = 0.02f;
                }
                OverlayElement invSlotElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("InventorySlot", "BorderPanel", "INV_NO_"+i.ToString());
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

        public override void Update(float timeSinceLastFrame)
        {
            baseAnim.AddTime(timeSinceLastFrame);
            topAnim.AddTime(timeSinceLastFrame);
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
