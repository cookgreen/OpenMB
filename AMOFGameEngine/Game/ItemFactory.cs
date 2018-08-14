using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class ItemFactory
    {
        protected Camera cam;
        protected Scene physicsScene;
        protected ItemFactory instance;
        public ItemFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemFactory();
                }
                return instance;
            }
        }

        public void Initization(Camera cam, Scene physicsScene)
        {
            this.cam = cam;
            this.physicsScene = physicsScene;
        }

        public Item Produce(
            string name, string meshName, ItemType type,
            double damage, int range, int ammoCapcity = -1,
            double amourNum = -1)
        {
            Item item = null;
            switch (type)
            {
                case ItemType.IT_BOW | ItemType.IT_CROSSBOW | ItemType.IT_RIFLE | ItemType.IT_PISTOL|
                     ItemType.IT_ONE_HAND_WEAPON | ItemType.IT_TWO_HAND_WEAPON| ItemType.IT_POLEARM |
                     ItemType.IT_RPG_MISSILE | ItemType.IT_SUBMACHINE_GUN| ItemType.IT_THROWN:
                     item = ItemWeaponFactory.Instance.Produce(name, meshName, type, damage, range);
                     break;
                case ItemType.IT_HAND_ARMOUR| ItemType.IT_HEAD_ARMOUR| ItemType.IT_BODY_ARMOUR| 
                     ItemType.IT_FOOT_ARMOUR:
                     item = ItemArmourFactory.Instance.Produce(name, meshName, type, amourNum);
                     break;
                case ItemType.IT_ARROW | ItemType.IT_BOLT | ItemType.IT_BULLET:
                     ItemAmmoFactory.Produce(name, meshName, type, damage, ammoCapcity);
                     break;
                case ItemType.IT_GOOD:
                    break;
                case ItemType.IT_BOOK:
                    break;
            }
            
            return item;
        }
    }
}
