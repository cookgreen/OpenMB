using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.RPG
{
    /// <summary>
    /// Basic class of all rpg elements, such as scene-prop, character, 
    /// </summary>
    public abstract class RPGObject
    {
        private uint uniqueId;
        protected uint UniqueId
        {
            get { return uniqueId; }
        }

        protected Mogre.Vector3 position;
        public Mogre.Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected Mogre.Vector3 initPos;
        public Mogre.Vector3 InitPos
        {
            get { return initPos; }
            set { initPos = value; }
        }

        public virtual void Update(float deltaTime)
        {
        }
    }
}
