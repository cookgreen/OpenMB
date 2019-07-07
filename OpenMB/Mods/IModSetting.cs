using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods
{
    public interface IModSetting
    {
        string Name { get; }
        string Value { get; set; }
        void Load(ModData mod);
    }
}
