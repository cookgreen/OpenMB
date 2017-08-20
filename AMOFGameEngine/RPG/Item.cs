using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.RPG
{
    /// <summary>
    /// Item Type
    /// </summary>
    public enum ItemType
    {
        IT_INVALID,//default value
        IT_GOOD,
        IT_BOOK,
        IT_WEAPON,
        IT_BODYAMOUR,
        IT_HEADAMOUR,
        IT_LEGAMOUR,
        IT_HANDAMOUR
    }

    public enum ItemAttachOption
    {
        IAO_LEFT,
        IAO_RIGHT,
        IAO_FRONT,
        IAO_LEFTFLANK,
        IAO_RIGHTFLANK
    }

    /// <summary>
    /// Item class
    /// </summary>
    public class Item : RPGObject
    {
        protected string itemID;
        protected string itemName;
        protected string itemMeshName;
        protected ItemType itemType;
        protected ItemAttachOption itemAttachDir;

        Entity itemEnt;
        SceneNode itemNode;
        Camera cam;
        InventoryInfo inventory;

        public InventoryInfo Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }

        public Item(Camera cam)
        {
            this.itemID = "";
            this.itemName = "";
            this.itemMeshName = "";
            this.itemType = ItemType.IT_INVALID;
            this.cam = cam;
        }

        public Item(string itemName, string itemMeshName, ItemType itemType, Camera cam)
        {
            this.itemName = itemName;
            this.itemMeshName = itemMeshName;
            this.itemType = itemType;
            this.cam = cam;
        }

        public void Create()
        {
            itemEnt = cam.SceneManager.CreateEntity(itemName,itemMeshName);
        }

        public void Drop()
        {
            itemNode = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
            itemNode.AttachObject(itemEnt);
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
        public Entity ItemEnt
        {
            get { return itemEnt; }
        }
        public ItemAttachOption ItemAttachDir
        {
            get { return itemAttachDir; }
            set { itemAttachDir = value; }
        }
    }
}
