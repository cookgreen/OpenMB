using OpenMB.Game.AIAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
	/// <summary>
	/// Single Character's brain
	/// </summary>
	public class DecisionSystem
	{
		private Character owner;
		private CharacterState ownerState;
		private Character enemy;
		private List<Character> enemies;
		public DecisionSystem(Character owner)
		{
			this.owner = owner;
			enemies = new List<Character>();
			enemy = null;
		}

		public void Update(float deltaTime)
		{
			AssessSituation();
			//FSM
			switch (ownerState)
			{
				case CharacterState.Idle://Do nothing
					break;
				case CharacterState.Seek://Search the enemy
					break;
				case CharacterState.Follow://Follow the target
					var followed = FindSuitableCharacterToFollow();
					owner.QueueActivity(new Follow(owner, followed));
					break;
				case CharacterState.Wander://Walk Randomly
					break;
				case CharacterState.Attack://Destroy the enemy
					owner.QueueActivity(new Attack(owner, enemy, owner.WeaponSystem.CurrentWeapon.Animations));
					break;
				case CharacterState.Flee://Retreat!
					break;
			}
		}

		private void AssessSituation()
		{
			int grace = 100;
			//Number of the enemies
			var enemies = FindAllEneimes();
			//Number of the allies
			var allies = FindAllies();
			//Weapons equipment
			var equipmentSystem = owner.EquipmentSystem;
			//Attack? Defense? Flee?
			if (enemies == null || allies == null)
			{
				return;
			}
			if (enemies.Count > allies.Count)
			{
				float ratio = enemies.Count / allies.Count;
				if (ratio > 1 && ratio <= 2)
				{
					grace -= 10;//Attack
				}
				else if (ratio > 2 && ratio <= 4)
				{
					grace -= 20;//Defense
				}
				else
				{
					grace -= 50;//Flee
				}
			}
			if (grace > 80 && grace <= 100)
			{
				ownerState = CharacterState.Attack;
			}
			else if (grace > 60 && grace <= 80)
			{
				ownerState = CharacterState.Seek;
			}
			else if (grace > 40 && grace <= 60)
			{
				ownerState = CharacterState.Flee;
			}
		}

		public void Active()
		{
			ownerState = CharacterState.Seek;
		}

		public void Deactive()
		{
			ownerState = CharacterState.Idle;
		}

		private List<Character> FindAllEneimes()
		{
			return null;
		}

		private Character FindClostestEnemy()
		{
			return null;
		}

		private List<Character> FindAllies()
		{
			return null;
		}

		private Character FindClostestAllies()
		{
			return null;
		}

		private Character FindSuitableCharacterToFollow()
		{
			return null;
		}

		public void TryReforceAllies()
		{
			//let's just move to the allies
		}

		public void ReGroupAndAttackWhenReady()
		{
			//find the rally point and all agents move to this point

		}
	}
}
