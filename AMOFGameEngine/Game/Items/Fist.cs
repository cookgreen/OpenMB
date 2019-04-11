using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    /// <summary>
    /// Character's default weapon
    /// </summary>
    public class Fist : Item
    {
        public Fist(GameWorld world, int id, int ownerID = -1)
            : base(world, id, ownerID)
        {

        }

        public override int Range
        {
            get
            {
                return 2;
            }
        }

        public override double Damage
        {
            get
            {
                return 1;
            }
        }
    }
}
