using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre_Procedural.MogreBites;
using Mogre;
using AMOFGameEngine.Widgets;

namespace AMOFGameEngine.Screen
{
    public class InventoryScreen : IScreen
    {
        private List<Widget> elements;
        private OverlayContainer left;
        private OverlayContainer middle;
        private OverlayContainer right;
        private SceneNode node;
        private Entity ent;
        public event Action OnScreenExit;

        public InventoryScreen()
        {
            elements = new List<Widget>();
        }

        public void Exit()
        {
            OnScreenExit?.Invoke();
        }

        public void Init()
        {
            GameManager.Instance.mTrayMgr.destroyAllWidgets();
        }

        public void Run()
        {
            left = OverlayManager.Singleton.CreateOverlayElementFromTemplate("CharacterEquipment", "BorderPanel", "inventoryPanelLeftArea") as OverlayContainer;
            middle = OverlayManager.Singleton.CreateOverlayElementFromTemplate("CharacterPreview", "BorderPanel", "inventoryPanelMiddleArea") as OverlayContainer;
            right = OverlayManager.Singleton.CreateOverlayElementFromTemplate("CharacterBackpack", "BorderPanel", "inventoryPanelRightArea") as OverlayContainer;
            GameManager.Instance.mTrayMgr.getTraysLayer().Add2D(left);
            GameManager.Instance.mTrayMgr.getTraysLayer().Add2D(middle);
            GameManager.Instance.mTrayMgr.getTraysLayer().Add2D(right);
        }

        public void Update(float timeSinceLastFrame)
        {
        }
    }
}
