using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.RPG
{
    /// <summary>
    /// Side of a character
    /// </summary>
    public abstract class Side
    {
        private string sideName;
        private Dictionary<Side, int> relationship;

        public virtual Dictionary<Side, int> Relationship
        {
            get { return relationship; }
            set { relationship = value; }
        }
        public virtual string Name
        {
            get { return sideName; }
            set { sideName = value; }
        }
    }
}
