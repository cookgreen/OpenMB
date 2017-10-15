using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using AMOFGameEngine.RPG.Data;

namespace AMOFGameEngine.RPG.Object
{

    public class ItemInfo
    {
        private string itemName;
        private string itemMeshName;
        private ItemType itemType;

        public string Mesh
        {
            get { return itemMeshName; }
            set { itemMeshName = value; }
        }
        public string Name
        {
            get { return itemName; }
            set { itemName = value; }
        }
    }

    /// <summary>
    /// Item class
    /// </summary>
    public abstract class Item : RPGObject
    {
        protected string itemID;
        protected string itemName;
        protected string itemMeshName;
        protected ItemType itemType;
        protected ItemAttachOption itemAttachDir;
        private Character owner;

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

        public Character Owner
        {
            get { return owner; }
            set { owner = value; }
        }
    }
}
