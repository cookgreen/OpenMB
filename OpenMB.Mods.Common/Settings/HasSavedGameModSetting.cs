using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods.Common.Settings
{
    public class HasSavedGameModSetting : IModSetting
    {
        public string Name
        {
            get
            {
                return "HasSavedGame";
            }
        }

        public string Value { get; set; }

        public void Load(ModData mod)
        {
            mod.HasSavedGame = bool.Parse(Value);
        }
    }
}
