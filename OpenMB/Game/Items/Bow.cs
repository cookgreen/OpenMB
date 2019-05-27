using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
    public class Bow : Item
    {
        public Bow(int id, string desc, string meshName, GameWorld world) :
            base(id, desc, meshName, ItemType.IT_ONE_HAND_WEAPON, ItemHaveAttachOption.IHAO_BACK_FROM_LEFT_TO_RIGHT, ItemUseAttachOption.IAO_LEFT_HAND, world)
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
