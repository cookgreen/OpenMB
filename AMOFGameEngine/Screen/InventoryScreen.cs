using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre_Procedural.MogreBites;
using Mogre;
using AMOFGameEngine.Widgets;
using AMOFGameEngine.Game;

namespace AMOFGameEngine.Screen
{
    public class InventoryScreen : IScreen
    {
        private List<OverlayElement> elements;
        private OverlayContainer equipmentPanel;
        private OverlayContainer previewPanel;
        private OverlayContainer backpackPanel;
        private SceneNode node;
        private Entity ent;
        private int row;
        private int col;
        private const float INV_WIDTH = 0.12f;
        private Character character;
        public event Action OnScreenExit;
        public string Name
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

        public void Exit()
        {
            Control.nukeOverlayElement(equipmentPanel);
            Control.nukeOverlayElement(previewPanel);
            Control.nukeOverlayElement(backpackPanel);
            SceneManager scm = GameManager.Instance.mRoot.GetSceneManager("GameSceneManager");
            scm.DestroyEntity(ent);
            OnScreenExit?.Invoke();
        }

        public void Init(params object[] param)
        {
            GameManager.Instance.mTrayMgr.destroyAllWidgets();
        }

        public void Run()
        {
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

            Overlay charaOverlay = new Overlay("PreviewOverlay");
            SceneManager scm = GameManager.Instance.mRoot.GetSceneManager("GameSceneManager");
            ent = scm.CreateEntity("sinbad", "sinbad.mesh");
            ent.SetRenderQueueGroupAndPriority((byte)RenderQueueGroupID.RENDER_QUEUE_OVERLAY, 900);
            node = new SceneNode(scm, "sinbadNode");
            node.Scale(0.1f, 0.1f, 0.1f);
            node.Translate(0, 0, 0);
            node.AttachObject(ent);
            charaOverlay.Add3D(node);

            for (uint i = 0; i < ent.NumSubEntities; ++i)
            {
                SubEntity subEnt = ent.GetSubEntity(i);
                if (subEnt != null)
                {
                    Technique tech = subEnt.GetMaterial().GetBestTechnique();
                    if (tech != null)
                    {
                        for (ushort iPass = 0; iPass < tech.NumPasses; ++iPass)
                        {
                            Pass pass = tech.GetPass(iPass);
                            if (pass != null)
                            {
                                pass.DepthCheckEnabled = false;
                            }
                        }
                    }
                }
            }
            charaOverlay.ZOrder = (ushort)(GameManager.Instance.mTrayMgr.getTraysLayer().ZOrder + 100);
            charaOverlay.Show();
            GameManager.Instance.mTrayMgr.getTraysLayer().Add2D(equipmentPanel);
            GameManager.Instance.mTrayMgr.getTraysLayer().Add2D(previewPanel);
            GameManager.Instance.mTrayMgr.getTraysLayer().Add2D(backpackPanel);
        }

        public void Update(float timeSinceLastFrame)
        {
        }
    }
}
