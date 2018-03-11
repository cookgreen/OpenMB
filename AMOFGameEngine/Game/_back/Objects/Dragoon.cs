using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Game.Controller;
using AMOFGameEngine.Game.Objects;
using AMOFGameEngine.Game.Traits;

namespace AMOFGameEngine.Game.Objects
{
    public class Dragoon : MoveableObject, IMountable
    {
        private DragoonController controller;

        public void Mount(Character rider)
        {
            //we assign this dragoon to the rider
            //then we will attach the rider model into dragoon model
            //finnaly we will handle the movement orders
            //the movement orders will effect the dragoon not the character
        }

        public void Unmount()
        {
            //we detach the dragoon from the rider
            //then we will detach the model
            //finally give controller back to character
        }
    }
}
