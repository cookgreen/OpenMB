using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Game.Data;

namespace AMOFGameEngine.Game.Objects
{
    public enum WeaponType
    {
        WT_ONEHAND,
        WT_TWOHAND,
        WT_POLEARM,
        WT_BOW,
        WT_RIFLE
    }
    public class WeaponInfo
    {
        public string weaponName;
        public string weaponMeshName;
        public int weaponDamage;
        public WeaponType weaponType;
    }
    public abstract class Weapon : Item
    {
        public WeaponInfo info;
        public Weapon(string name, string mesh, int damage,WeaponType weaponType,Mogre.Camera cam) : base(cam)
        {
            itemType = ItemType.IT_WEAPON;
            info = new WeaponInfo()
            {
                weaponName = name,
                weaponDamage = damage,
                weaponMeshName = mesh,
                weaponType = weaponType
            };
        }

        public virtual void Attack(Character target)
        {
            target.UnderAttack(Owner);
        }
        public virtual void PlaySound(string effectName)
        { }
        public virtual void PlayPratice()
        { }
    }
}
