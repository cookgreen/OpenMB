using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods.Interfaces
{
    public interface IModFlag
    {
        string Name { get; }
        void Enable(string value, params object[] param);
    }
}
