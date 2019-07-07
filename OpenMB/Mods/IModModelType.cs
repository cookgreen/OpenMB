using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods
{
    public interface IModModelType
    {
        string Name { get; }
        object Process(ModData mod, params object[] param);
    }
}
