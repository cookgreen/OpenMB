using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Mogre;
using MOIS;
using AMOFGameEngine.Sound;
using Mogre.PhysX;
using org.critterai.nav;

namespace AMOFGameEngine.Game
{
    public enum CharacterState
    {
        Idle,
        Seek,
        Attack,
        Flee
    }
    /// <summary>
    /// Specific Characer in Game
    /// </summary>
    public class Character : GameObject
    {
        //Unique Id
        private int id;
        private CharacterState currentState;
        private Character currentEnemy;
        private Item currentWieldWeapon;
        private Dictionary<string, CharacterController.AnimID> animations;
        public event Action<int, int, double> OnCharacterUseWeaponAttack;
        public event Action<int> OnCharacterDie;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        //Name
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //Team, usually used to identity whether is enemy
        private string teamId;

        //Controller
        private CharacterController controller;

        public CharacterController Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        //Hitpoint
        private int hitpoint;

        public int Hitpoint
        {
            get { return hitpoint; }
            set { hitpoint = value; }
        }

        //Weapons
        private Item[] weapons;

        public Item[] Weapons
        {
            get { return weapons; }
            set { weapons = value; }
        }

        internal void SetBodyPos(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        //Clothes
        private Item[] clothes;

        public Item[] Clothes
        {
            get { return clothes; }
            set { clothes = value; }
        }

        //Backpack
        private Inventory backpack;

        public Inventory Backpack
        {
            get { return backpack; }
            set { backpack = value; }
        }

        public string TeamId
        {
            get
            {
                return teamId;
            }
        }

        public Mogre.Vector3 Position
        {
            get
            {
                return controller.Position;
            }
        }

        //Environment
        private GameWorld world;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="world">Environment</param>
        /// <param name="cam">Camera</param>
        /// <param name="id">Unique Id</param>
        /// <param name="teamId">Team Id</param>
        /// <param name="name">Name</param>
        /// <param name="meshName">Mesh Name</param>
        /// <param name="initPosition">Init Position</param>
        /// <param name="controlled">Is Bot or not</param>
        public Character(GameWorld world, 
                         Camera cam, 
                         int id,
                         string teamId,
                         string name,
                         string meshName,
                         Mogre.Vector3 initPosition,
                         bool controlled)
        {
            this.world = world;
            Id = id;
            Name = string.Empty;
            Hitpoint = 100;
            Weapons = new Item[5];
            Clothes = new Item[5];
            Backpack = new Inventory(21, this);
            controller = new CharacterController(cam,world.GetCurrentMap().NavmeshQuery,world.GetCurrentMap().PhysicsScene, name + id.ToString(), meshName, controlled);//初始化控制器
            controller.Position = initPosition;
            currentEnemy = null;
            currentWieldWeapon = new Fist(cam, world.GetCurrentMap().PhysicsScene, -1, id);
            currentWieldWeapon.OnWeaponAttack += CurrentWieldWeapon_OnWeaponAttack;
            currentState = CharacterState.Seek;

            moveInfo = new MoveInfo(CharacterController.RUN_SPEED);

            /* TODO: Use Xml file to define the animation dynamically */
            animations = new Dictionary<string, CharacterController.AnimID>();
            animations.Add("ANIM_DANCE", CharacterController.AnimID.ANIM_DANCE);
            animations.Add("ANIM_DRAW_SWORDS", CharacterController.AnimID.ANIM_DRAW_SWORDS);
            animations.Add("ANIM_HANDS_CLOSED", CharacterController.AnimID.ANIM_HANDS_CLOSED);
            animations.Add("ANIM_HANDS_RELAXED", CharacterController.AnimID.ANIM_HANDS_RELAXED);
            animations.Add("ANIM_IDLE_BASE", CharacterController.AnimID.ANIM_IDLE_BASE);
            animations.Add("ANIM_IDLE_TOP", CharacterController.AnimID.ANIM_IDLE_TOP);
            animations.Add("ANIM_JUMP_END", CharacterController.AnimID.ANIM_JUMP_END);
            animations.Add("ANIM_JUMP_LOOP", CharacterController.AnimID.ANIM_JUMP_LOOP);
            animations.Add("ANIM_JUMP_START", CharacterController.AnimID.ANIM_JUMP_START);
            animations.Add("ANIM_NONE", CharacterController.AnimID.ANIM_NONE);
            animations.Add("ANIM_RUN_BASE", CharacterController.AnimID.ANIM_RUN_BASE);
            animations.Add("ANIM_RUN_TOP", CharacterController.AnimID.ANIM_RUN_TOP);
            animations.Add("ANIM_SLICE_HORIZONTAL", CharacterController.AnimID.ANIM_SLICE_HORIZONTAL);
            animations.Add("ANIM_SLICE_VERTICAL", CharacterController.AnimID.ANIM_SLICE_VERTICAL);
        }

        private void CurrentWieldWeapon_OnWeaponAttack(int arg1, int arg2)
        {
            if (OnCharacterUseWeaponAttack != null)
            {
                OnCharacterUseWeaponAttack(arg1, arg2, currentWieldWeapon.Damage);
            }
        }

        public bool GetControlled()
        {
            return controller.GetControlled();
        }

        public void WearHat(Item item)
        {
            if (item != null && item.ItemType == ItemType.IT_HEAD_ARMOUR)
            {
                Clothes[0] = item;
                controller.AttachEntityToChara("head", item.ItemEnt);
            }
        }

        public void WearClothes(Item item)
        {
            if (item != null && item.ItemType == ItemType.IT_BODY_ARMOUR)
            {
                Clothes[1] = item;
                controller.AttachEntityToChara("back", item.ItemEnt);
            }
        }

        public void AddItemToBackpack(Item item)
        {
            Backpack.AddItemToInventory(item);
        }

        /// <summary>
        /// Find characters against this character
        /// </summary>
        /// <returns>Enemies</returns>
        public List<Character> FindEnemies()
        {
            List<Character> enemies = new List<Character>();
            List<string> enemyTeamsWithMe = new List<string>();
            var allEnemyTeams = world.GetTeamRelationshipByCondition(o => o.Item3 == -1);
            foreach(var enemyTeam in allEnemyTeams)
            {
                if(enemyTeam.Item1 == teamId)
                {
                    if (!enemyTeamsWithMe.Contains(enemyTeam.Item2))
                    {
                        enemyTeamsWithMe.Add(enemyTeam.Item2);
                    }
                }
                else if(enemyTeam.Item2 == teamId)
                {
                    if (!enemyTeamsWithMe.Contains(enemyTeam.Item1))
                    {
                        enemyTeamsWithMe.Add(enemyTeam.Item1);
                    }
                }
            }
            foreach(var enemyTeamWithMe in enemyTeamsWithMe)
            {
                enemies.AddRange(world.GetCharactersByCondition(o => o.TeamId == enemyTeamWithMe));
            }
            return enemies;
        }

        /// <summary>
        /// Change Character to a new team
        /// </summary>
        /// <param name="newTeamId">New Team Id</param>
        public void Turn(string newTeamId)
        {
            teamId = newTeamId;
        }

        public override void Update(float timeSinceLastFrame)
        {
            //FSM
            switch(currentState)
            {
                case CharacterState.Idle:
                    currentState = CharacterState.Seek;
                    break;
                case CharacterState.Seek:
                    List<Character> enemies = FindEnemies();
                    if (enemies.Count > 0)
                    {
                        currentEnemy = enemies[0];
                        currentState = CharacterState.Attack;
                    }
                    break;
                case CharacterState.Attack:
                    if (currentEnemy != null)
                    {
                        world.QueueAction(new Move(this, currentEnemy.Position));
                        if((currentEnemy.controller.Position-controller.Position).Length <= currentWieldWeapon.Range)//Enemy in range
                        {
                            Attack();
                        }
                        else
                        {
                            Run();
                        }
                    }
                    else
                    {
                        currentState = CharacterState.Seek;
                    }
                    break;
                case CharacterState.Flee:
                    break;
            }
            controller.update(timeSinceLastFrame);
            if (Hitpoint < 0)
            {
                //R.I.P
                if (OnCharacterDie != null)
                {
                    OnCharacterDie(id);
                }
            }
            else
            {
                //CheckEnvironment();
            }
        }

        private void Wander()
        {
            //Move Randomly in the scene
            controller.Wander();
        }

        private void Run()
        {
            controller.Run();
        }

        private void Attack()
        {
            controller.Attack();
        }

        public void RotateBody(Quaternion quat)
        {
            controller.RotateBody(quat);
        }

        public void TranslateBody(Mogre.Vector3 vector3)
        {
            controller.TranslateBody(vector3);
        }

        public Mogre.Quaternion GetBodyOrientation()
        {
            return controller.GetBodyOrientation();
        }

        public void YawBody(Degree degree)
        {
            controller.YawBody(degree);
        }

        public void SetTopAnimation(string animName, bool v)
        {
            if (!animations.ContainsKey(animName))
            {
                return;
            }
            controller.SetTopAnimation(animations[animName]);
        }

        public void SetBaseAnimation(string animName, bool v)
        {
            if (!animations.ContainsKey(animName))
            {
                return;
            }
            controller.SetBaseAnimation(animations[animName]);
        }

        public void SetAnimation(string topAnimName, string baseAnimName, bool v)
        {
            SetTopAnimation(topAnimName, v);
            SetBaseAnimation(baseAnimName, v);
        }
    }
}
