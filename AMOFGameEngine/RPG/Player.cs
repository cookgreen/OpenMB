using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.RPG
{
    /// <summary>
    /// Player Class
    /// </summary>
    public class Player : Character
    {
        public Player(Camera cam, Keyboard keyboard, Mouse mouse)
            : base(cam,keyboard,mouse,true)
        {
        }
    }
}
