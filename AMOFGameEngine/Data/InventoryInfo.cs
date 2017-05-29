using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Data
{
    public class InventoryInfo
    {
        Item headAmour;
        Item bodyAmour;
        Item handAmour;
        Item legAmour;
        Item[] weapons;
        List<Item> inventory;

        public List<Item> Inventory
        {
            get { return inventory; }
            set { inventory = value; }
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
            inventory = new List<Item>();
        }
    }
}
