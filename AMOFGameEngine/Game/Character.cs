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
using AMOFGameEngine.Game.Action;
using AMOFGameEngine.Mods.XML;

namespace AMOFGameEngine.Game
{
    public enum CharacterState
    {
        Idle,
        Seek,
        Follow,
        Wander,
        Attack,
        Flee
    }
    /// <summary>
    /// Specific Characer in Game
    /// </summary>
    public class Character : GameObject
    {
        private string meshName;
        private DecisionSystem brain;
        private WeaponSystem weaponSystem;
        private EquipmentSystem equipmentSystem;
        private List<CharacterMessage> messageQueue;
        private Activity currentActivity;
        private ModCharacterSkinDfnXML skin;
        private bool isBot;

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

        public string MeshName
        {
            get
            {
                return meshName;
            }
        }

        //Team, usually used to identity whether is enemy
        private string teamId;

        //Controller
        private CharacterController controller;

        //Hitpoint
        private int hitpoint;

        public int Hitpoint
        {
            get { return hitpoint; }
            set { hitpoint = value; }
        }

        public string TeamId
        {
            get
            {
                return teamId;
            }
        }

        public void EquipWeapon(Item item)
        {
            if (equipmentSystem.EquipNewWeapon(item))
            {
                controller.AttachItem(item.ItemAttachOption, item);
            }
        }

        public bool IsDead
        {
            get
            {
                return Hitpoint <= 0;
            }
        }

        public WeaponSystem WeaponSystem
        {
            get
            {
                return weaponSystem;
            }
        }

        public EquipmentSystem EquipmentSystem
        {
            get
            {
                return equipmentSystem;
            }
        }

        public Activity CurrentActivity
        {
            get
            {
                return currentActivity;
            }
        }

        public SceneNode SceneNode
        {
            get
            {
                return controller.BodyNode;
            }
        }

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
        /// <param name="isBot">Is Bot or not</param>
        public Character(
            GameWorld world,
            int id,
            string teamId,
            string meshName,
            Mogre.Vector3 initPosition,
            ModCharacterSkinDfnXML skin,
            bool isBot) : base(id, world)
        {
            this.world = world;
            this.meshName = meshName;
            this.skin = skin;
            this.isBot = isBot;
            Id = id;
            position = initPosition;
            Hitpoint = 100;
            brain = new DecisionSystem(this);
            weaponSystem = new WeaponSystem(this, new Fist(world, -1, id));
            equipmentSystem = new EquipmentSystem(this);

            currentActivity = new Idle();
            moveInfo = new MoveInfo(CharacterController.RUN_SPEED);

            messageQueue = new List<CharacterMessage>();

            create();
        }

        protected override void create()
        {
            controller = new CharacterController(world.Camera, world.Map.NavmeshQuery, world.Map.PhysicsScene, meshName, skin, isBot, position);
        }

        public bool GetControlled()
        {
            return controller.GetControlled();
        }

        public void WearHat(Item item)
        {
            if (item != null && item.ItemType == ItemType.IT_HEAD_ARMOUR)
            {
                equipmentSystem.EquipClothes(item, 0);
                controller.AttachEntityToChara("head", item.ItemEnt);
            }
        }

        public void WearClothes(Item item)
        {
            if (item != null && item.ItemType == ItemType.IT_BODY_ARMOUR)
            {
                equipmentSystem.EquipClothes(item, 1);
                controller.AttachEntityToChara("back", item.ItemEnt);
            }
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
            position = controller.Position;
            brain.Update(timeSinceLastFrame);
            controller.update(timeSinceLastFrame);
            weaponSystem.Update(timeSinceLastFrame);
            equipmentSystem.Update(timeSinceLastFrame);
            UpdateActivity(timeSinceLastFrame);
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

        public void SetBodyPos(float x, float y, float z)
        {
            controller.SetBodyPos(x, y, z);
        }

        public void SetTopAnimation(string animName, bool v)
        {
            CharacterAnimation animation = controller.GetAnimationByName(animName);
            if (animation==null)
            {
                return;
            }
            controller.SetTopAnimation(animation);
        }

        public void SetBaseAnimation(string animName, bool v)
        {
            CharacterAnimation animation = controller.GetAnimationByName(animName);
            if (animation == null)
            {
                return;
            }
            controller.SetBaseAnimation(animation, v);
        }

        public void SetAnimation(string topAnimName, string baseAnimName, bool v)
        {
            SetTopAnimation(topAnimName, v);
            SetBaseAnimation(baseAnimName, v);
        }

        public void QueueActivity(Activity newActivity)
        {
            currentActivity.Enqueue(newActivity);
            currentActivity = currentActivity.NextActivity;
        }

        public void UpdateActivity(float deltaTime)
        {
            Activity tempActivity = currentActivity;
            while (tempActivity != null)
            {
                tempActivity.Update(deltaTime);
                var nextActivity = tempActivity.NextActivity;
                if (tempActivity.State == ActionState.Cancel || tempActivity.State == ActionState.Done)
                {
                    tempActivity.Dequeue();
                }
                tempActivity = nextActivity;
            }
        }

        public void HandleMessage()
        {
            var urgentMessages = messageQueue.Where(o => o.Level == MessageLevel.heigh || o.Level == MessageLevel.veryhigh);
            for (int i = 0; i < urgentMessages.Count(); i++)
            {
                var urgentMessage = urgentMessages.ElementAt(i);
                switch(urgentMessage.Type)
                {
                    case MessageType.enemy_spotted:
                        brain.ReGroupAndAttackWhenReady();
                        break;
                    case MessageType.need_backup:
                        brain.TryReforceAllies();
                        break;
                }
            }
        }

        public void RestoreLastActivity()
        {
            currentActivity = currentActivity.ParentActivity;
        }

        public bool CheckActivity<T>() where T : Activity
        {
            return currentActivity.GetType() is T;
        }

        public void ReceiveMessage(CharacterMessage message)
        {
            messageQueue.Add(message);
        }

        public void SendMessage(MessageLevel level, MessageType type, int agentId)
        {
            var agent = world.Map.GetAgentById(agentId);
            if (agent != null)
            {
                agent.ReceiveMessage(new CharacterMessage(level, type));
            }
        }

        public void InjectMouseMove(MouseEvent evt)
        {
            controller.injectMouseMove(evt);
        }

        public void InjectKeyPressed(KeyEvent arg)
        {
            controller.injectKeyDown(arg);
        }

        public void InjectKeyUp(KeyEvent evt)
        {
            controller.injectKeyUp(evt);
        }

        public string GetIdleTopAnim()
        {
            return controller.GetAnimationNameByType(CharacterAnimationType.CAT_IDLE_TOP);
        }

        public string GetIdleBaseAnim()
        {
            return controller.GetAnimationNameByType(CharacterAnimationType.CAT_IDLE_BASE);
        }

        public void AttachCamera(Camera camera)
        {
            controller.setupCamera(camera);
        }

        public Camera DetachCamera()
        {
            return controller.removeCamera();
        }

        public void UpdateCamera(float deltaTime)
        {
            controller.updateCamera(deltaTime);
        }
    }
}
