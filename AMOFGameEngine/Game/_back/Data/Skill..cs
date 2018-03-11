using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game.Data
{
    /// <summary>
    /// Skill Infomation
    /// </summary>
    public abstract class Skill
    {
        /// <summary>
        /// Skill Name
        /// </summary>
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Skill Description
        /// </summary>
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// Skill Handler
        /// </summary>
        public virtual void SkillHandle()
        {

        }
    }
}
