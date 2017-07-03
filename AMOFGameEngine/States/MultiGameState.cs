using System;
using Mogre;

namespace AMOFGameEngine.States
{
    public class MultiGameState : AppState
    {
        SceneManager scm;

        public MultiGameState()
        {
            scm = null;
        }

        public override void enter(Mods.ModData e = null)
        {
            base.enter(e);
        }

        public override bool pause()
        {
            return base.pause();
        }

        public override void update(double timeSinceLastFrame)
        {
            base.update(timeSinceLastFrame);
        }

        public override void exit()
        {
            base.exit();
        }
    }
}
