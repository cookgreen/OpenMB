using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods.HitBalloons.Settings
{
    public class HasMultiplayerModSetting : IModSetting
    {
        public string Name {
            get
            {
                return "HasMultiplayer";
            }
        }

        public string Value { get; set; }

        public void Load(ModData mod)
        {
            mod.HasMultiplater = bool.Parse(Value);
        }
    }
}
