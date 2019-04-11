using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class Bolt : Item
    {
        public Bolt(GameWorld world, int id, int ownerID = -1)
            : base(world, id, ownerID)
        {

        }

        public override int AmmoCapcity
        {
            get
            {
                return 30;
            }
        }

        public override double Damage
        {
            get
            {
                return 25;
            }
        }

        public override ItemType ItemType
        {
            get
            {
                return ItemType.IT_BOLT;
            }

            set
            {
                base.ItemType = value;
            }
        }
    }
}
