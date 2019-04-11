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
        IT_AMMUNITION,          //Ammo
        IT_GOOD,                //Good
        IT_BOOK,                //Book
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
        IT_BULLET,              //Bullet for Gun

        IT_WEAPON = IT_ONE_HAND_WEAPON | IT_TWO_HAND_WEAPON | IT_POLEARM | IT_BOW | IT_CROSSBOW | IT_THROWN | IT_RIFLE | IT_PISTOL | IT_SUBMACHINE_GUN | IT_LIGHT_MACHINE_GUN | IT_LAUNCHER,
        IT_ARMOUR = IT_HEAD_ARMOUR | IT_BODY_ARMOUR | IT_FOOT_ARMOUR | IT_HAND_ARMOUR
    }
    /// <summary>
    /// How did the item attach to the character when the character use this item?
    /// </summary>
    public enum ItemUseAttachOption
    {
        IAO_NO_VALUE,
        IAO_LEFT_HAND,//Attach to the left hand
        IAO_RIGHT_HAND,//Attach to the right hand
        IAO_LEFT_FOOT,//Attach to the left foot
        IAO_RIGHT_FOOT,//Attach to the right foot
        IAO_BODY,//Use this for body amour
        IAO_HEAD,//Use this for head amour
    }

    public enum ItemHaveAttachOption
    {
        IHAO_NO_VALUE,
        IHAO_INVISIBLE,
        IHAO_BACK_FROM_RIGHT_TO_LEFT,
        IHAO_BACK_FROM_LEFT_TO_RIGHT,
        IHAO_BACK_VERTICAL,
        IHAO_BACK_HORIZONL
    }

    /// <summary>
    /// Item class
    /// </summary>
    public class Item : GameObject
    {
        protected int itemID;
        protected int ownerID;
        protected string itemName;
        protected string itemMeshName;
        protected ItemType itemType;
        protected ItemHaveAttachOption itemAttachOptionWhenHave;
        protected ItemUseAttachOption itemAttachOptionWhenUse;
        protected List<Cartridge> cartridges;
        private Character user;
        public event Action<int, int> OnWeaponAttack;

        #region Render
        private Entity itemEnt;
        private SceneNode itemNode;
        private Actor itemActor;
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
        public ItemUseAttachOption ItemAttachOption
        {
            get { return itemAttachOptionWhenUse; }
            set { itemAttachOptionWhenUse = value; }
        }

        public Character User
        {
            get { return user; }
            set { user = value; }
        }

        public virtual int Range { get; set; }
        public virtual double Damage { get; set; }

        public virtual Type Ammo { get; set; }
        public virtual int AmmoCapcity { get; set; }
        public virtual string[] Animations { get; set; }

        public Item(GameWorld world, int id, int ownerID = -1) : base(id, world)
        {
            this.itemID = id;
            this.itemName = "";
            this.itemMeshName = "";
            this.itemType = ItemType.IT_INVALID;
            this.ownerID = ownerID;
        }

        public Item(
            int id,
            string itemName, string itemMeshName, ItemType itemType, 
            ItemHaveAttachOption itemAttachOptionWhenHave,
            ItemUseAttachOption itemAttachOptionWhenUse,
            GameWorld world) : base(id, world)
        {
            this.itemName = itemName;
            this.itemMeshName = itemMeshName;
            this.itemType = itemType;
            this.itemAttachOptionWhenUse = itemAttachOptionWhenUse;
            this.itemAttachOptionWhenHave = itemAttachOptionWhenHave;
            create();
        }

        public void Attack(int victimId)
        {
            if (OnWeaponAttack != null)
            {
                OnWeaponAttack(ownerID, victimId);
            }
        }

        protected override void create()
        {
            itemEnt = camera.SceneManager.CreateEntity(itemName,itemMeshName);
            itemNode = camera.SceneManager.RootSceneNode.CreateChildSceneNode();
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
