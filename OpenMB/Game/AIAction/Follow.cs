using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game.AIAction
{
	public class Follow : Activity
	{
		private Character follower;
		private Character followed;
		private int keepDistance;
		public Follow(Character follower, Character followed, int keepDistance = 150)
		{
			this.follower = follower;
			this.followed = followed;
			this.keepDistance = keepDistance;
		}

		public override void Update(float deltaTime)
		{
			if (followed.IsDead)
			{
				State = ActionState.Cancel;
				return;
			}
			if (follower.CheckActivity<Move>() && follower.CurrentActivity.State == ActionState.Processing)
			{
				State = ActionState.Processing;
				return;
			}
			follower.QueueActivity(new Move(follower, followed.Position));
		}
	}
}
