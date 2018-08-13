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
        IT_INVALID,             //default value
        IT_GOOD,                //Good
        IT_BOOK,                //Book
        IT_AMMUNITION,          //Ammo
        IT_HEAD_ARMOUR,         //Head Armour
        IT_BODY_ARMOUR,         //Body Armour
        IT_FOOT_ARMOUR,         //Foot Armour
        IT_HAND_ARMOUR,         //Hand Armour
        IT_ONE_HAND_WEAPON,     //One Hand Melee Weapon
        IT_TWO_HAND_WEAPON,     //Two Hand Melee Weapon
        IT_POLEARM,             //Polearm Melee Weapon
        IT_BOW,                 //Bow
        IT_CROSSBOW,            //Crossbow
        IT_THROWN,              //Throw
        IT_RIFLE,               //Rifle
        IT_PISTOL,              //Pistol
        IT_SUBMACHINE_GUN,      //Submachine Gun
        IT_LIGHT_MACHINE_GUN,   //Light Machine Gun
        IT_LAUNCHER,            //RPG Launcher
        IT_ARROW,               //Arrow
        IT_BOLT,                //Bolt
        IT_RPG_MISSILE,         //Missile for RPG Launcher
        IT_BULLET               //Bullet for Gun
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
        protected int ownerID;
        protected string itemName;
        protected string itemMeshName;
        protected ItemType itemType;
        protected ItemAttachOption itemAttachOption;
        private Character user;
        public event Action<int, int> OnWeaponAttack;

        #region Render
        private Entity itemEnt;
        private SceneNode itemNode;
        private Camera cam;
        private Actor itemActor;
        private Physics physics;
        private Scene physicsScene;
        #endregion

        public int ItemID
        {
            get { return itemID; }
        }
        public virtual ItemType ItemType
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

        public virtual int Range { get; }
        public virtual double Damage { get; }

        public virtual Type Ammo { get; }
        public virtual int AmmoCapcity { get; }

        public Item(Camera cam, Scene physicsScene, int id, int ownerID = -1)
        {
            this.itemID = id;
            this.itemName = "";
            this.itemMeshName = "";
            this.itemType = ItemType.IT_INVALID;
            this.cam = cam;
            this.physicsScene = physicsScene;
            this.physics = physicsScene.Physics;
            this.ownerID = ownerID;
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

        public void Attack(int victimId)
        {
            if (OnWeaponAttack != null)
            {
                OnWeaponAttack(ownerID, victimId);
            }
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

        /// <summary>
        /// Use Item to attack animation
        /// </summary>
        public virtual void OnAttackAnimation()
        {
            
        }

        /// <summary>
        /// Play animation when the item be draw from the backpack
        /// </summary>
        public virtual void OnDrawItemAnimation()
        {

        }

        /// <summary>
        /// Play animation when hold this item in character's hand
        /// </summary>
        public virtual void OnHoldItemAnimation()
        {

        }

        /// <summary>
        /// Play animation when reload the item
        /// </summary>
        public virtual void OnItemReloadAnimation()
        {

        }
    }
}
