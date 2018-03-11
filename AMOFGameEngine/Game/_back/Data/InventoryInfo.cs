using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Game.Objects;

namespace AMOFGameEngine.Game.Data
{
    public class InventoryInfo
    {
        Item headAmour;
        Item bodyAmour;
        Item handAmour;
        Item legAmour;
        Item[] weapons;
        List<Item> inventory;
        Item currentWeapon;

        public Item CurrentWeapon
        {
            get { return currentWeapon; }
            set { currentWeapon = value; }
        }
        public Item[] Weapons
        {
            get { return weapons; }
            set { weapons = value; }
        }
        public Item LegAmour
        {
            get { return legAmour; }
            set { legAmour = value; }
        }
        public Item HandAmour
        {
            get { return handAmour; }
            set { handAmour = value; }
        }
        public Item BodyAmour
        {
            get { return bodyAmour; }
            set { bodyAmour = value; }
        }
        public Item HeadAmour
        {
            get { return headAmour; }
            set { headAmour = value; }
        }

        public InventoryInfo()
        {
            weapons = new Item[4];
            inventory = new List<Item>(18);
        }

        public void AddItemToInventory(Item item)
        {
            item.Inventory = this;
            inventory.Add(item);
        }

        public void RemoveItemToInventory(Item item)
        {
            item.Inventory = null;
            inventory.Remove(item);
        }

        public Item FindItemInInventory(string name)
        {
            var result = from item in inventory
                         where item.ItemName == name
                         select item;
            if (result.Count() > 0)
            {
                return result.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public int CurrentWeaponIndex { get; set; }
    }
}
