using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
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

        public Item[] Clothes
        {
            get { return clothes; }
            set { clothes = value; }
        }

        private Item rideDrive;

        public Item RideDrive
        {
            get
            {
                return rideDrive;
            }
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
            Backpack = new Inventory(30, owner);
        }

        public bool EquipNewWeapon(Item newWeapon)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i]==null)
                {
                    weapons[i] = newWeapon;
                    return true;
                }
            }
            return false;//No empty weapon slot
        }

        public bool EquipNewClothes(Item newClothes)
        {
            for (int i = 0; i < clothes.Length; i++)
            {
                if (clothes[i] == null)
                {
                    clothes[i] = newClothes;
                    return true;
                }
            }
            return false;//No empty clothes slot
        }

        public bool AddItemToBackpack(Item newItem)
        {
            if(!backpack.IsFull)
            {
                backpack.AddItemToInventory(newItem);
                return true;
            }
            else
            {
                return false;//Backpack is full
            }
        }

        public void EquipNewWeapon(Item newWeapon, int index)
        {
            if (index < 0 || index >= weapons.Length)
            {
                return;
            }
            weapons[index] = newWeapon;
        }

        public void EquipHeadArmour(Item newClothes)
        {
            clothes[0] = newClothes;
        }
        public void EquipBodyArmour(Item newClothes)
        {
            clothes[1] = newClothes;
        }
        public void EquipFootArmour(Item newClothes)
        {
            clothes[2] = newClothes;
        }
        public void EquipHandArmour(Item newClothes)
        {
            clothes[3] = newClothes;
        }
        public void EquipRideDrive(Item rideDrive)
        {
            this.rideDrive = rideDrive;
        }

        public void EquipNewItem(Item item)
        {
            if (item.ItemType == ItemType.IT_WEAPON)
            {
                if (!EquipNewWeapon(item))
                {
                    AddItemToBackpack(item);
                }
            }
            else if (item.ItemType == ItemType.IT_ARMOUR)
            {
                if (!EquipNewClothes(item))
                {
                    AddItemToBackpack(item);
                }
            }
            else
            {
                AddItemToBackpack(item);
            }
        }

        public void Update(float timeSinceLastFrame)
        {

        }
    }
}
