using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.RPG
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
    }
}
