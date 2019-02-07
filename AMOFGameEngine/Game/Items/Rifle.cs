using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;

namespace AMOFGameEngine.Game
{
    public class Rifle : Item
    {

        public Rifle(string desc, string meshName, Scene physicsScene, Camera cam) :
            base(desc, meshName, ItemType.IT_RIFLE, ItemHaveAttachOption.IHAO_BACK_FROM_LEFT_TO_RIGHT, ItemUseAttachOption.IAO_LEFT_HAND, physicsScene, cam)
        {
        }
    }
}
