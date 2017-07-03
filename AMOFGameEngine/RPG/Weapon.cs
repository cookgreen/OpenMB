using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.RPG
{
    public enum WeaponType
    {
        WT_ONEHAND,
        WT_TWOHAND,
        WT_POLEAM,
        WT_CROSSBOW,
        WT_BOW,
        WT_MUSKET,
        WT_RIFLE
    }
    class Weapon : Item
    {
        WeaponType weaponType;

        public WeaponType WeaponType
        {
            get { return weaponType; }
            set { weaponType = value; }
        }

        public Weapon(Camera cam) : base(cam)
        {
            itemType = ItemType.IT_WEAPON;
            weaponType = WeaponType.WT_ONEHAND;
        }
    }
}
