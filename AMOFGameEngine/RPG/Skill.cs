using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.RPG
{
    /// <summary>
    /// Skill Class
    /// </summary>
    public abstract class Skill
    {
        private string skillName;
        private string skillDsp;

        public string Description
        {
            get { return skillDsp; }
            set { skillDsp = value; }
        }
        public string Name
        {
            get { return skillName; }
            set { skillName = value; }
        }

        public virtual void SkillHandle()
        {

        }
    }
}
