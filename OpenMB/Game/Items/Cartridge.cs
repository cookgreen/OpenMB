using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;

namespace OpenMB.Game
{
    public class Cartridge : Item
    {
        public Cartridge(string name, string meshName, int id, GameWorld world, int ownerId)
            : base(id, name, meshName, ItemType.IT_AMMUNITION, 
                  ItemHaveAttachOption.IHAO_NO_VALUE,
                  ItemUseAttachOption.IAO_NO_VALUE,
                  world)
        {

        }
    }
}
