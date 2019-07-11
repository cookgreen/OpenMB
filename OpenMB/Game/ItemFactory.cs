using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.Mods.XML;
using OpenMB.Mods;

namespace OpenMB.Game
{
    public class ItemFactory
    {
        protected Camera cam;
        protected Scene physicsScene;
        protected ModData mod;
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

        public void Initization(Camera cam, Scene physicsScene, ModData mod)
        {
            this.cam = cam;
            this.physicsScene = physicsScene;
            this.mod = mod;
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

        public Item PreProduce(ModItemDfnXML findedItem)
        {
            var findedItemTypeXml = mod.FindItemType(findedItem.Type);
            if (findedItemTypeXml != null)
            {
                var findedItemTypes = mod.ItemTypes.Where(o => o.Name == findedItemTypeXml.Name);
                if (findedItemTypes.Count() > 0)
                {
                    Item itm = new Item(Guid.NewGuid().ToString(), findedItem.Name, findedItem.MaterialName, findedItemTypes.ElementAt(0), ItemHaveAttachOption.IHAO_NO_VALUE, ItemUseAttachOption.IAO_NO_VALUE);
                    return itm;
                }
            }
            return null;
        }

        public Item Produce(Mods.XML.ModItemDfnXML itemXml, GameWorld world)
        {
            return world.Map.CreateItem(itemXml.Desc, itemXml.MeshName, (ItemType)Enum.Parse(typeof(ItemType), itemXml.Type), itemXml.AttachOptionWhenUse,
                itemXml.AttachOptionWhenHave, double.Parse(itemXml.Damage), int.Parse(itemXml.Range), world, itemXml.AmmoCapcity, itemXml.AmourNum);
        }
    }
}
