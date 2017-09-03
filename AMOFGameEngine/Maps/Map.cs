using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Maps
{
    public abstract class Map
    {
        /// <summary>
        /// call when enter a map
        /// </summary>
        public virtual void Load() { }
        /// <summary>
        /// call when leave a map
        /// </summary>
        public virtual void Unload() { }
    }
}
