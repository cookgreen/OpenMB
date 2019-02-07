using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;

namespace AMOFGameEngine.Game
{
    public class Cartridge : Item
    {
        public Cartridge(string name, string meshName, Camera cam, Scene physicsScene, int id, int ownerId)
            : base(name, meshName, ItemType.IT_AMMUNITION, 
                  ItemHaveAttachOption.IHAO_NO_VALUE,
                  ItemUseAttachOption.IAO_NO_VALUE,
                  physicsScene, cam)
        {

        }
    }
}
