using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Mod
{
    public class ModData
    {
        string modName;
        public string ModName
        {
            get { return modName; }
            set { modName = value; }
        }

        string modDesc;
        public string ModDesc
        {
            get { return modDesc; }
            set { modDesc = value; }
        }

        List<string> modResouces;
        public List<string> ModResouces
        {
            get { return modResouces; }
            set { modResouces = value; }
        }

        List<string> modMusic;
        public List<string> ModMusic
        {
            get { return modMusic; }
            set { modMusic = value; }
        }

    }
}
