using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Game
{
    public abstract class GameObject
    {
        protected GameObjClass type;
        public GameObjClass ObjType
        {
            get
            {
                return type;
            }
        }
        protected uint uniqueId;
        public uint UniqueId
        {
            get
            {
                return uniqueId;
            }
        }
        protected Vector3 initPos;
        public Vector3 Position;

        public virtual void Update(float timeSinceLastFrame)
        {

        }
    }
}
