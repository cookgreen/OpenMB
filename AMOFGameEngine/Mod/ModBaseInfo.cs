using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Mod
{
    /// <summary>
    /// Display on the ModChooser
    /// </summary>
    public class ModBaseInfo
    {
        string modName;
        string modDesc;
        string modThumb;

        public string ModThumb
        {
            get { return modThumb; }
            set { modThumb = value; }
        }
        public string ModDesc
        {
            get { return modDesc; }
            set { modDesc = value; }
        }
        public string ModName
        {
            get { return modName; }
            set { modName = value; }
        }
    }
}
