using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods
{
    public interface IGameMapLoader
    {
        event Action LoadMapFinished;
        string Name { get; }
        void LoadAsync(string mapFile);
    }
}
