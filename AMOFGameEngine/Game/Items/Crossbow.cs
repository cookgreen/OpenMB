using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class Crossbow : Item
    {
        public Crossbow(Camera cam, Scene physicsScene, int id, int ownerID = -1)
            : base(cam, physicsScene, id, ownerID)
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
