using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game.AIAction
{
	public class Attack : Activity
	{
		private Character attacker;
		private Character victim;
		private string[] attackAnimNames;//0-Top animation, 1-Base animation
		public Attack(Character attacker, Character victim, string[] attackAnimNames)
		{
			this.attacker = attacker;
			this.victim = victim;
			this.attackAnimNames = attackAnimNames;
		}

		public override void Update(float deltaTime)
		{
			if (attacker.IsDead)
			{
				State = ActionState.Cancel;
				return;
			}
			if (!victim.IsDead && State == ActionState.Queued)
			{
				attacker.SetAnimation(attackAnimNames[0], attackAnimNames[1], true);
			}
			else
			{
				attacker.RestoreLastActivity();
				State = ActionState.Done;
			}
		}
	}
}
