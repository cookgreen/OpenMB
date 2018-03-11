using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Game;

namespace AMOFGameEngine.Game.Traits
{
    /// <summary>
    /// Nodify the engine when state changed
    /// </summary>
    interface INodifyStateChanged
    {
        void StateChanged(int oldState, int newState);
    }
}
