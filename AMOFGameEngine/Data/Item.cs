using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine
{
    /// <summary>
    /// Item Type
    /// </summary>
    public enum ItemType
    {
        IT_INVALID,//default value
        IT_ONEHAND,//One hand weapon like sword
        IT_TWOHAND,//Two hand weapon like great sword
        IT_POLEARM,//polearm weapon like pole-axe
        IT_BOW,//bow
        IT_CROSSBOW,//crossbow
        IT_MUSKET,//musket
        IT_HEAVYWEAPON//heavy weapon like machine gun, RPG Rocket
    }

    /// <summary>
    /// Item description
    /// </summary>
    public class Item
    {
        string itemID;
        string itemName;
        string itemMeshName;
        ItemType itemType;

        public Item()
        {
            this.itemID = "";
            this.itemName = "";
            this.itemMeshName = "";
            this.itemType = ItemType.IT_INVALID;
        }

        public Item(string itemName,string itemMeshName,ItemType itemType)
        {
            this.itemName = itemName;
            this.itemMeshName = itemMeshName;
            this.itemType = itemType;
        }

        public string ItemID
        {
            get { return itemID; }
        }
        public ItemType ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }
        public string ItemMeshName
        {
            get { return itemMeshName; }
            set { itemMeshName = value; }
        }
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
    }
}
