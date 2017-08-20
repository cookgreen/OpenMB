using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.RPG
{
    /// <summary>
    /// Non-Player Character
    /// </summary>
    public class NPC : Character
    {
        public NPC(Camera cam, Keyboard keyboard, Mouse mouse)
            : base(cam, keyboard, mouse, false)
        {

        }
    }
}
