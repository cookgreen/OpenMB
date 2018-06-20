using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;
using AMOFGameEngine.Utilities;

namespace AMOFGameEngine.Game
{

    /// <summary>
    /// Item Type
    /// </summary>
    public enum ItemType
    {
        IT_INVALID,     //default value
        IT_GOOD,        //Good
        IT_BOOK,        //Book
        IT_AMMUNITION,  //Ammo
        IT_HEAD_ARMOUR, //头防
        IT_BODY_ARMOUR, //身防
        IT_FOOT_ARMOUR, //腿防
        IT_HAND_ARMOUR,  //手防
        IT_ONE_HAND_WEAPON, //单手
        IT_TWO_HAND_WEAPON, //双手
        IT_POLEARM,         //长杆
        IT_BOW,              //弓
        IT_CROSSBOW,          //弩
        IT_THROWN,            //投掷
        IT_RIFLE,             //步枪
        IT_PISTOL,            //手枪
        IT_HEAVYWEAPON        //重武器
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
    public abstract class Item : GameObject
    {
        protected int itemID;
        protected string itemName;
        protected string itemMeshName;
        protected ItemType itemType;
        protected ItemAttachOption itemAttachOption;
        private Character user;

        Entity itemEnt;
        SceneNode itemNode;
        Camera cam;
        Actor itemActor;
        Physics physics;
        Scene physicsScene;

        public int ItemID
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
        public ItemAttachOption ItemAttachOption
        {
            get { return itemAttachOption; }
            set { itemAttachOption = value; }
        }

        public Character User
        {
            get { return user; }
            set { user = value; }
        }

        public Item(Camera cam, Scene physicsScene, int id)
        {
            this.itemID = id;
            this.itemName = "";
            this.itemMeshName = "";
            this.itemType = ItemType.IT_INVALID;
            this.cam = cam;
            this.physicsScene = physicsScene;
            this.physics = physicsScene.Physics;
            Create();
        }

        public Item(string itemName, string itemMeshName, ItemType itemType, Scene physicsScene, Camera cam)
        {
            this.itemName = itemName;
            this.itemMeshName = itemMeshName;
            this.itemType = itemType;
            this.cam = cam;
            this.physicsScene = physicsScene;
            this.physics = physicsScene.Physics;
            Create();
        }

        private void Create()
        {
            itemEnt = cam.SceneManager.CreateEntity(itemName,itemMeshName);
            itemNode = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
            itemNode.AttachObject(itemEnt);

            ActorDesc actorDesc = new ActorDesc();
            actorDesc.Density = 4;
            actorDesc.Body = null;
            actorDesc.Shapes.Add(physics.CreateTriangleMesh(new
                StaticMeshData(itemEnt.GetMesh())));
            itemActor = physicsScene.CreateActor(actorDesc);
        }

        public void Drop()
        {
            itemNode.DetachObject(ItemEnt);
        }
    }
}
