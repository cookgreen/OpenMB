using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class MoveInfo
    {
        private readonly float speed;

        public float Speed
        {
            get
            {
                return speed;
            }
        }

        public MoveInfo(float speed)
        {
            this.speed = speed;
        }
    }
}
