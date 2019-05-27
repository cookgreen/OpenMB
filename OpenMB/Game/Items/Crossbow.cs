using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
    public class Crossbow : Item
    {
        public Crossbow(int id, string desc, string meshName, GameWorld world) :
            base(id, desc, meshName, ItemType.IT_CROSSBOW, ItemHaveAttachOption.IHAO_BACK_FROM_LEFT_TO_RIGHT, ItemUseAttachOption.IAO_LEFT_HAND, world)
        {

        }

        public override Type Ammo
        {
            get
            {
                return typeof(Bolt);
            }
        }

        public override int Range
        {
            get
            {
                return 1200;
            }
        }

        public override double Damage
        {
            get
            {
                return 25;
            }
        }
    }
}
