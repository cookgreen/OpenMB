using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;

namespace AMOFGameEngine.Game
{
    class Pistol : Item
    {
        private Camera cam;
        private Scene physicsScene;

        public Pistol(string desc, string meshName, Scene physicsScene, Camera cam) :
            base(desc, meshName, ItemType.IT_PISTOL, ItemHaveAttachOption.IHAO_BACK_FROM_LEFT_TO_RIGHT, ItemUseAttachOption.IAO_LEFT_HAND, physicsScene, cam)
        {
            this.cam = cam;
            this.physicsScene = physicsScene;
        }
    }
}
