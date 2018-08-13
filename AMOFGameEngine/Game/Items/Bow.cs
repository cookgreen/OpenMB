using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class Bow : Item
    {
        public Bow(Camera cam, Scene physicsScene, int id, int ownerID = -1)
            : base(cam, physicsScene, id, ownerID)
        {

        }

        public override int Range
        {
            get
            {
                return 1000;
            }
        }

        public override Type Ammo
        {
            get
            {
                return typeof(Arrow);
            }
        }

        public override ItemType ItemType
        {
            get
            {
                return ItemType.IT_BOW;
            }

            set
            {
                base.ItemType = value;
            }
        }

        public override double Damage
        {
            get
            {
                return 30;
            }
        }

        public override void OnAttackAnimation()
        {
            base.OnAttackAnimation();
        }

        public override void OnDrawItemAnimation()
        {
            base.OnDrawItemAnimation();
        }

        public override void OnHoldItemAnimation()
        {
            base.OnHoldItemAnimation();
        }
    }
}
