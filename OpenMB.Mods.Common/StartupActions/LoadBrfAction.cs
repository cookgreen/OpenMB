using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods.Common.StartupActions
{
    class LoadBrfAction : IModSetting
    {
        public string Name
        {
            get
            {
                return "load_brf";
            }
        }

        public string Value
        {
            get;
            set;
        }

        public void Load(ModData currentMod)
        {

        }
    }
}
