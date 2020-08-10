using System;
using System.ComponentModel;
using Mogre;
using MMOC;
using OpenMB.Mods;
using OpenMB.Game;
using MOIS;

namespace OpenMB.States
{
	public class SinglePlayer : AppState
	{
		private GameWorld world;
		public SinglePlayer()
		{
		}

		public override void enter(ModData data = null)
		{
			world = new GameWorld(data);
			world.Init();
			world.Start();
		}

		bool mRoot_FrameStarted(FrameEvent evt)
		{
			return true;
		}

		public override bool pause()
		{
			return base.pause();
		}

		public override void update(double timeSinceLastFrame)
		{
			if (world == null)
			{
				return;
			}
			frameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
		}

		public override void exit()
		{
			modData = world.ModData;
			world.Destroy();
		}
	}
}
