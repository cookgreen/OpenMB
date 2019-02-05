using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class GameObject
    {
        protected MoveInfo moveInfo;

        public GameObject()
        {
            moveInfo = null;
        }

        public MoveInfo MoveInfo
        {
            get
            {
                return moveInfo;
            }
        }

        public virtual void Update(float timeSinceLastFrame)
        {
            
        }
    }
}
