using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.RPG.Objects
{
    /// <summary>
    /// Player Class
    /// </summary>
    public class Player : Character
    {
        public Player(string playId, Keyboard keyboard, Mouse mouse)
            : base(playId,keyboard,mouse)
        {
        }
    }
}
