using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.RPG.Objects
{
    /// <summary>
    /// Non-Player Character
    /// </summary>
    public class NPC : Character
    {
        public NPC(string npcId, Keyboard keyboard, Mouse mouse)
            : base(npcId, keyboard, mouse)
        {

        }
    }
}
