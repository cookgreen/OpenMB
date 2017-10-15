using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.RPG.Object
{
    /// <summary>
    /// Non-Player Character
    /// </summary>
    public class NPC : Character
    {
        public NPC(string npcId, Camera cam, Keyboard keyboard, Mouse mouse)
            : base(npcId,cam, keyboard, mouse, false)
        {

        }
    }
}
