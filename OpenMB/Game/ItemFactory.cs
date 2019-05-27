using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
    public class ItemFactory
    {
        protected Camera cam;
        protected Scene physicsScene;
        protected static ItemFactory instance;
        public static ItemFactory Instance
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
            int id,
            string desc, string meshName, ItemType type,
            ItemUseAttachOption itemUseAttachOption,
            ItemHaveAttachOption itemHaveAttachOption,
            double damage, int range,
            GameWorld world, int ammoCapcity = -1,
            double amourNum = -1)
        {
            Item item = null;
            switch (type)
            {
                case ItemType.IT_BOW | ItemType.IT_CROSSBOW | ItemType.IT_RIFLE | ItemType.IT_PISTOL|
                     ItemType.IT_ONE_HAND_WEAPON | ItemType.IT_TWO_HAND_WEAPON| ItemType.IT_POLEARM |
                     ItemType.IT_RPG_MISSILE | ItemType.IT_SUBMACHINE_GUN| ItemType.IT_THROWN:
                     item = ItemWeaponFactory.Instance.Produce(
                         id,
                         desc, meshName, type,
                         itemUseAttachOption,
                         itemHaveAttachOption,
                         damage, range,
                         world);
                     break;
                case ItemType.IT_HAND_ARMOUR| ItemType.IT_HEAD_ARMOUR| ItemType.IT_BODY_ARMOUR| 
                     ItemType.IT_FOOT_ARMOUR:
                     item = ItemArmourFactory.Instance.Produce(
                         id,
                         desc, meshName, type, 
                         itemUseAttachOption,
                         itemHaveAttachOption,
                         amourNum,
                         world);
                     break;
                case ItemType.IT_ARROW | ItemType.IT_BOLT | ItemType.IT_BULLET:
                     item = ItemAmmoFactory.Produce(
                         desc, meshName, type, 
                         itemUseAttachOption, 
                         itemHaveAttachOption, 
                         damage, ammoCapcity);
                     break;
                case ItemType.IT_GOOD:
                    break;
                case ItemType.IT_BOOK:
                    break;
            }
            
            return item;
        }

        public Item Produce(Mods.XML.ModItemDfnXML itemXml, GameWorld world)
        {
            return world.Map.Produce(itemXml.Desc, itemXml.MeshName, itemXml.Type, itemXml.AttachOptionWhenUse,
                itemXml.AttachOptionWhenHave, double.Parse(itemXml.Damage), int.Parse(itemXml.Range), world, itemXml.AmmoCapcity, itemXml.AmourNum);
        }
    }
}
