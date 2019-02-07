using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class ItemAmmoFactory : ItemFactory
    {
        private static new ItemAmmoFactory instance;
        public static new ItemAmmoFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemAmmoFactory();
                }
                return instance;
            }
        } 
        public static Item Produce(
            string name, string meshName, ItemType type, 
            ItemUseAttachOption itemAttachOptionWhenUse, 
            ItemHaveAttachOption itemAttachOptionWhenHave,
            double damage, int ammoCapcity)
        {
            return null;
        }
    }
}
