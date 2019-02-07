using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;

namespace AMOFGameEngine.Game.Items
{
    public class Sword : OneHandWeapon
    {
        public Sword(Camera cam, Scene physicsScene, int id, int ownerID = -1) : base(cam, physicsScene, id, ownerID)
        {
            animations = new string[] {

            };
        }

        private string[] animations;

        public override string[] Animations
        {
            get
            {
                return base.Animations;
            }

            set
            {
                base.Animations = value;
            }
        }
    }
}
