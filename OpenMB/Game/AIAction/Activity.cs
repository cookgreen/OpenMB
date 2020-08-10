using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game.AIAction
{
	public enum ActionState { Queued, Done, Cancel, Processing }
	//An Action in world
	public abstract class Activity : IUpdate
	{
		private Activity parentActivity;
		private Activity nextActivity;
		public ActionState State;
		public abstract void Update(float deltaTime);
		public Activity ParentActivity
		{
			get
			{
				return parentActivity;
			}
			set
			{
				parentActivity = this;
			}
		}
		public Activity NextActivity
		{
			get
			{
				return nextActivity;
			}
			set
			{
				nextActivity = value;
			}
		}

		public void Enqueue(Activity newActivity)
		{
			parentActivity = this;
			NextActivity = newActivity;
		}

		public void Dequeue()
		{
			if (parentActivity != null)
			{
				parentActivity.NextActivity = NextActivity;
			}
			parentActivity = null;
		}
	}
}
