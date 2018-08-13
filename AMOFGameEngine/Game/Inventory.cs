using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    /// <summary>
    /// Inventory
    /// </summary>
    public class Inventory
    {
        private int capicity;
        private List<Item> items;
        private Character owner;

        public int Capicity
        {
            get { return capicity; }
        }

        public Inventory(int initCapicity, Character owner)
        {
            capicity = initCapicity;
            items = new List<Item>(capicity);
            this.owner = owner;
        }

        public void AddItemToInventory(Item item)
        {
            if (items.Count < capicity)
            {
                items.Add(item);
            }
        }

        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }

        public void ChangeCapicity(int newCapcity)
        {
            if (newCapcity > capicity)
            {
                capicity = newCapcity;
            }
        }

        public List<Item> GetAllItems()
        {
            return items;
        }
    }
}
