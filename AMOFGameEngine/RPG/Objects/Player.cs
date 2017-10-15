using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.RPG.Object
{
    /// <summary>
    /// Player Class
    /// </summary>
    public class Player : Character
    {
        public Player(string playId,Camera cam, Keyboard keyboard, Mouse mouse)
            : base(playId,cam, keyboard, mouse, true)
        {
        }
    }
}
