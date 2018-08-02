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
        public Fist(Camera cam, Scene physicsScene, int id, int ownerID = -1)
            : base(cam, physicsScene, id, ownerID)
        {

        }

        public override int Range
        {
            get
            {
                return 5;
            }
        }

        public override int Damage
        {
            get
            {
                return 1;
            }
        }
    }
}
