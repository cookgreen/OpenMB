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

        public Item Produce(int id, string name, string meshName, ItemType type, double damage, int range)
        {
            Item item = null;
            switch(type)
            {
                case ItemType.IT_BOW:
                    item = new Bow(cam, physicsScene, id);
                    break;
                case ItemType.IT_CROSSBOW:
                    item = new Crossbow(cam, physicsScene, id);
                    break;
                case ItemType.IT_ONE_HAND_WEAPON:
                    item = new OneHandWeapon(cam, physicsScene, id);
                    break;
                case ItemType.IT_TWO_HAND_WEAPON:
                    break;
                case ItemType.IT_POLEARM:
                    break;
                case ItemType.IT_RIFLE:
                    item = new Rifle(cam, physicsScene, id);
                    break;
                case ItemType.IT_PISTOL:
                    item = new Pistol(cam, physicsScene, id);
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
