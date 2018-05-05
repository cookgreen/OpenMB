using System;
using System.ComponentModel;
using Mogre;
using MMOC;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Game;
using MOIS;

namespace AMOFGameEngine.States
{
    public class SinglePlayer : AppState
    {
        private GameWorld world;
        public SinglePlayer()
        {
        }

        public override void enter(Mods.ModData data = null)
        {
            world = new GameWorld(data);
            world.Init();
            world.ChangeScene("Cubescene.xml");
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
            world.Update((float)timeSinceLastFrame);
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
        }

        public override void exit()
        {
            m_Data = world.ModData;
            world.Destroy();
        }
    }
}
