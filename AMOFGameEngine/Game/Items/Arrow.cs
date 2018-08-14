using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class Arrow : Cartridge
    {
        public Arrow(string name, string meshName, Camera cam, Scene physicsScene, int id, int ownerId = -1)
            : base(name, meshName, cam, physicsScene, id, ownerId)
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
                return 20;
            }
        }

        public override ItemType ItemType
        {
            get
            {
                return ItemType.IT_ARROW;
            }

            set
            {
                base.ItemType = value;
            }
        }
    }
}
