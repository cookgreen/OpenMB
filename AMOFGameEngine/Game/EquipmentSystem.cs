using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class EquipmentSystem
    {
        private Character owner;
        //Weapons
        private Item[] weapons;

        public Item[] Weapons
        {
            get { return weapons; }
            set { weapons = value; }
        }

        //Clothes
        private Item[] clothes;

        public Item[] Clotheses
        {
            get { return clothes; }
            set { clothes = value; }
        }

        //Backpack
        private Inventory backpack;

        public Inventory Backpack
        {
            get { return backpack; }
            set { backpack = value; }
        }

        public EquipmentSystem(Character owner)
        {
            this.owner = owner;
            weapons = new Item[4];
            clothes = new Item[4];
            Backpack = new Inventory(21, owner);
        }
        
        public void EquipNewWeapon(Item newWeapon, int index)
        {
            if (index < 0 || index >= weapons.Length)
            {
                return;
            }
            weapons[index] = newWeapon;
        }

        public void EquipClothes(Item newClothes, int index)
        {
            if (index < 0 || index >= clothes.Length)
            {
                return;
            }
            clothes[index] = newClothes;
        }

        public void EquipNewItem(Item item)
        {
            backpack.AddItemToInventory(item);
        }
    }
}
