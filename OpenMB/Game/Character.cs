using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Mogre;
using MOIS;
using OpenMB.Sound;
using Mogre.PhysX;
using org.critterai.nav;
using OpenMB.Game.AIAction;
using OpenMB.Mods.XML;

namespace OpenMB.Game
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
        private string displayName;
        private string meshName;
        private DecisionSystem brain;
        private WeaponSystem weaponSystem;
        private EquipmentSystem equipmentSystem;
        private List<CharacterMessage> messageQueue;

        private Activity currentActivity;
        private ModCharacterSkinDfnXML skin;
        private bool isBot;
        private string teamId;
        private CharacterController controller;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return displayName; }
        }

        public string MeshName
        {
            get
            {
                return meshName;
            }
        }

        public string TeamId
        {
            get
            {
                return teamId;
            }
			set
			{
				teamId = value;
			}
        }

        public bool IsDead
        {
            get
            {
                return Health.HP <= 0;
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

		public Character(
			GameWorld world,
			ModCharacterDfnXML chaData,
			ModCharacterSkinDfnXML chaSkin,
			Mogre.Vector3 initPosition,
			bool isBot) : base(-1, world)
		{
			this.world = world;
			this.isBot = isBot;
            displayName = chaData.Name;
            meshName = chaData.MeshName;
            skin = chaSkin;
            Id = id;
			position = initPosition;
			brain = new DecisionSystem(this);
			weaponSystem = new WeaponSystem(this, new Fist(world, -1, id));
			equipmentSystem = new EquipmentSystem(this);

			currentActivity = new Idle();
			moveInfo = new MoveInfo(CharacterController.RUN_SPEED);
			health = new HealthInfo(this);
			messageQueue = new List<CharacterMessage>();

			controller = new CharacterController(world, chaData, chaSkin, position, isBot);
		}

		public void AttchItem(Item target)
        {
            controller.AttachItem(ItemUseAttachOption.IAO_SPIN, target);
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
            health.Update(timeSinceLastFrame);
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

        public void EquipWeapon(Item item)
        {
            if (equipmentSystem.EquipNewWeapon(item))
            {
                controller.AttachItem(item.ItemAttachOption, item);
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
            var agent = world.CurrentMap.GetAgentById(agentId);
            if (agent != null)
            {
                agent.ReceiveMessage(new CharacterMessage(level, type));
            }
        }

        public void InjectMouseClick(MouseEvent arg, MouseButtonID id)
        {
            controller.injectMouseDown(arg, id);
        }

        public void InjectMouseMove(MouseEvent arg)
        {
            controller.injectMouseMove(arg);
        }

        public void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
        {
            controller.injectMouseReleased(arg, id);
        }

        public void InjectKeyPressed(KeyEvent arg)
        {
            if (equipmentSystem.RideDrive != null)
            {
                equipmentSystem.RideDrive.injectKeyDown(arg);
            }
            else
            {
                controller.injectKeyDown(arg);
            }
        }

        public void InjectKeyUp(KeyEvent evt)
        {
            if (equipmentSystem.RideDrive != null)
            {
                equipmentSystem.RideDrive.injectKeyUp(evt);
            }
            else
            {
                controller.injectKeyUp(evt);
            }
        }

        public string GetIdleTopAnim()
        {
            return controller.GetAnimationNameByType(ChaAnimType.CAT_IDLE_TOP);
        }

        public string GetIdleBaseAnim()
        {
            return controller.GetAnimationNameByType(ChaAnimType.CAT_IDLE_BASE);
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

        public override void Dispose()
        {
            controller.Dispose();
        }
    }
}
