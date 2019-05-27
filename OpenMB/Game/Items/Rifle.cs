using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;

namespace OpenMB.Game
{
    public class Rifle : Item
    {

        public Rifle(int id, string desc, string meshName, GameWorld world) :
            base(id, desc, meshName, ItemType.IT_RIFLE, ItemHaveAttachOption.IHAO_BACK_FROM_LEFT_TO_RIGHT, ItemUseAttachOption.IAO_LEFT_HAND, world)
        {
        }
    }
}
