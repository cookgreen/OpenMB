using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;
using OpenMB.Utilities;
using OpenMB.Game.ItemTypes;
using OpenMB.Mods.XML;

namespace OpenMB.Game
{

    /// <summary>
    /// Item Type
    /// </summary>
    public enum ItemValidType
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
        IT_RIDEDRIVE,           //Anything that can ride or drive like horse or vehicle
        IT_CUSTOME,             //Customized Item Class

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
        IAO_SPIN,//Attach to the spin bone
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
        protected IItemType itemType;
        protected ItemHaveAttachOption itemAttachOptionWhenHave;
        protected ItemUseAttachOption itemAttachOptionWhenUse;
        private Character user;
        public event Action<int, int> OnWeaponAttack;

        #region Render
        private Actor itemActor;
        private ModItemDfnXML itemData;
        #endregion
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
            get { return mesh.Entity; }
        }
        public string ItemTypeID
        {
            get
            {
                return itemData.ID;
            }
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
        public GameObject LinkedGameObject { get; set; } //Use ItemType to check its type

        public Item(GameWorld world, IItemType itemType, ModItemDfnXML itemData, bool createNow = false) : base(-1, world)
        {
            this.itemType = itemType;
            this.itemData = itemData;

            this.itemType.Item = this;

            if (createNow)
            {
                create();
            }
        }

        public void SpawnIntoWorld()
        {
            itemType.SpawnIntoWorld();
        }

        public void SpawnIntoCharacter(Character character)//Spawn and attach into the character
        {
            itemType.SpawnIntoCharacter(world, character);
        }

        public void Attack(int victimId)
        {
            if (OnWeaponAttack != null)
            {
                OnWeaponAttack(ownerID, victimId);
            }
        }

        public void Spawn()
        {
            create();
        }

        protected override void create()
        {
            mesh.Entity = mesh.SceneManager.CreateEntity(Guid.NewGuid().ToString(),itemData.MeshName);
			mesh.EntityNode = mesh.SceneManager.RootSceneNode.CreateChildSceneNode();
			mesh.EntityNode.AttachObject(mesh.Entity);

            ActorDesc actorDesc = new ActorDesc();
            actorDesc.Density = 4;
            actorDesc.Body = null;
            actorDesc.Shapes.Add(physics.CreateTriangleMesh(new
                StaticMeshData(mesh.Entity.GetMesh())));
            itemActor = physicsScene.CreateActor(actorDesc);
        }

        protected override void create(GameWorld world)
        {
            base.create(world);
			mesh.Entity = mesh.SceneManager.CreateEntity(itemName, itemMeshName);
			mesh.EntityNode = mesh.SceneManager.RootSceneNode.CreateChildSceneNode();
			mesh.EntityNode.AttachObject(mesh.Entity);

            ActorDesc actorDesc = new ActorDesc();
            actorDesc.Density = 4;
            actorDesc.Body = null;
            actorDesc.Shapes.Add(physics.CreateTriangleMesh(new
                StaticMeshData(mesh.Entity.GetMesh())));
            itemActor = physicsScene.CreateActor(actorDesc);
        }

        public void Drop()
        {
            mesh.EntityNode.DetachObject(ItemEnt);
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

        public virtual void injectKeyDown(object arg)
        {

        }

        public virtual void injectKeyUp(MOIS.KeyEvent evt)
        {

        }

		public override MaterialPtr RenderInventoryPreview()
		{
			return itemType.RenderInventoryPreview(mesh.Entity);
		}
	}
}
