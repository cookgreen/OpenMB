using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
    public class ItemArmourFactory : ItemFactory
    {
        protected static new ItemArmourFactory instance;
        public static new ItemArmourFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemArmourFactory();
                }
                return instance;
            }
        }

        public Item Produce(
            int id,
            string name, 
            string meshName, 
            ItemType type, 
            ItemUseAttachOption itemAttachOptionWhenUse, 
            ItemHaveAttachOption itemHaveAttachOption,
            double armourNum,
            GameWorld world)
        {
            Armour item = new Armour(id, name, meshName, world);
            switch(type)
            {
                case ItemType.IT_HAND_ARMOUR:
                    item.HandArmourNum = armourNum;
                    break;
                case ItemType.IT_HEAD_ARMOUR:
                    item.HeadArmourNum = armourNum;
                    break;
                case ItemType.IT_FOOT_ARMOUR:
                    item.FootArmourNum = armourNum;
                    break;
                case ItemType.IT_BODY_ARMOUR:
                    item.BodyArmourNum = armourNum;
                    break;
            }
            return item;
        }
    }
}
