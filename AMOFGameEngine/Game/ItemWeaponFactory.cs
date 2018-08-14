using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class ItemWeaponFactory : ItemFactory
    {
        protected static new ItemWeaponFactory instance;
        public static new ItemWeaponFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemWeaponFactory();
                }
                return instance;
            }
        }

        public Item Produce(string name, string meshName, ItemType type, double damage, int range)
        {
            Item item = new Item(name, meshName, type, physicsScene, cam);
            switch(type)
            {
                case ItemType.IT_BOW:
                    break;
                case ItemType.IT_CROSSBOW:
                    break;
                case ItemType.IT_ONE_HAND_WEAPON:
                    break;
                case ItemType.IT_TWO_HAND_WEAPON:
                    break;
                case ItemType.IT_POLEARM:
                    break;
                case ItemType.IT_RIFLE:
                    break;
                case ItemType.IT_PISTOL:
                    break;
                case ItemType.IT_SUBMACHINE_GUN:
                    break;
                case ItemType.IT_LAUNCHER:
                    break;
            }
            item.Damage = damage;
            item.Range = range;
            return item;
        }
    }
}
