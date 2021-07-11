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
using OpenMB.Game.Controller;
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

	public enum CharacterFlag
	{
		CF_Mounted,
		CF_NPC,
	}

	/// <summary>
	/// Specific Characer in Game
	/// </summary>
	public class Character : DynamicGameObject
	{
		private DecisionSystem brain;
		private WeaponSystem weaponSystem;
		private EquipmentSystem equipmentSystem;
		private List<CharacterMessage> messageQueue;

		private Activity currentActivity;
		private string teamId;
		private CharacterController controller;

		private ModCharacterDfnXML xmlData;
		private ModCharacterSkinDfnXML skinXmlData;

		public ModCharacterDfnXML XmlData { get { return xmlData; } }
		public ModCharacterSkinDfnXML SkinXmlData { get { return skinXmlData; } }

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public string Name
		{
			get { return xmlData.Name; }
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

		public string TypeID
		{
			get
			{
				return xmlData.ID;
			}
		}

		public bool IsRider
		{
			get
			{
				return equipmentSystem.RideDrive != null;
			}
		}

		public Character(
			GameWorld world,
			ModCharacterDfnXML xmlData,
			ModCharacterSkinDfnXML skinXmlData,
			Mogre.Vector3 initPosition,
			bool isBot) : base(-1, world)
		{
			this.world = world;
			this.xmlData = xmlData;
			this.skinXmlData = skinXmlData;
			position = initPosition;
			Id = id;
			brain = new DecisionSystem(this);
			weaponSystem = new WeaponSystem(this, null);
			equipmentSystem = new EquipmentSystem(this);

			currentActivity = new Idle();
			moveInfo = new MoveInfo(CharacterController.RUN_SPEED);
			health = new HealthInfo(this);
			messageQueue = new List<CharacterMessage>();

			initEquipments();

			renderable = new CharacterController(world, this, isBot);
			controller = (CharacterController)renderable;
		}

		public void MoveTo(Mogre.Vector3 destPos)
		{
			controller.MoveTo(destPos);
		}

		private void initEquipments()
		{
			foreach (var item in xmlData.Equipments)
			{
				var itemInfo = world.ModData.ItemInfos.Where(o => o.ID == item).FirstOrDefault();
				if (itemInfo != null)
				{
					var itemType = world.ModData.ItemTypes.Where(o => o.Name == itemInfo.Type).FirstOrDefault();
					if (itemInfo != null && itemType != null)
					{
						var itm = new Item(world, itemType, itemInfo, false);
						switch (itemType.Name)
						{
							case "IT_RIDEDRIVE":
								equipmentSystem.EquipRideDrive(itm);
								break;
							case "IT_HEAD_ARMOUR":
								equipmentSystem.EquipHeadArmour(itm);
								break;
							case "IT_BODY_ARMOUR":
								equipmentSystem.EquipBodyArmour(itm);
								break;
							case "IT_FOOT_ARMOUR":
								equipmentSystem.EquipFootArmour(itm);
								break;
							case "IT_HAND_ARMOUR":
								equipmentSystem.EquipHandArmour(itm);
								break;
							default:
								equipmentSystem.AddItemToBackpack(itm);
								break;
						}
					}
				}
			}
		}

		public void AttchItem(Item target)
		{
			controller.AttachItem(ItemUseAttachOption.IAO_SPIN, target);
		}

		public bool GetControlled()
		{
			return controller.GetControlled();
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
			foreach (var enemyTeam in allEnemyTeams)
			{
				if (enemyTeam.Item1 == teamId)
				{
					if (!enemyTeamsWithMe.Contains(enemyTeam.Item2))
					{
						enemyTeamsWithMe.Add(enemyTeam.Item2);
					}
				}
				else if (enemyTeam.Item2 == teamId)
				{
					if (!enemyTeamsWithMe.Contains(enemyTeam.Item1))
					{
						enemyTeamsWithMe.Add(enemyTeam.Item1);
					}
				}
			}
			foreach (var enemyTeamWithMe in enemyTeamsWithMe)
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
			if (animation == null)
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
				switch (urgentMessage.Type)
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

		public override MaterialPtr RenderPreview()
		{
			return controller.RenderPreview();
		}

		public override void Destroy()
		{
			controller.EntityNode.RemoveAndDestroyAllChildren();
			controller.SceneManager.DestroySceneNode(controller.EntityNode);
		}
	}
}
