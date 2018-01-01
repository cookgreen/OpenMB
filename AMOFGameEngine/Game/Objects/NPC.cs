using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using AMOFGameEngine.Game.World;

namespace AMOFGameEngine.Game.Objects
{
    /// <summary>
    /// Non-Player Character
    /// </summary>
    public class NPC : Character
    {
        public NPC(string npcId, GameWorld world, Keyboard keyboard, Mouse mouse)
            : base(npcId, keyboard, mouse)
        {

        }
    }
}
