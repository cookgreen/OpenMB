using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Mod.Common;

namespace AMOFGameEngine.Mod.Test
{
    class TestModMenuState : ModState
    {
        public override void enter()
        {
            base.enter();
        }

        public override bool pause()
        {
            return base.pause();
        }

        public override void exit()
        {
            base.exit();
        }

        public override void resume()
        {
            base.resume();
        }

        public override void update(double timeSinceLastFrame)
        {
            base.update(timeSinceLastFrame);
        }
    }
}
