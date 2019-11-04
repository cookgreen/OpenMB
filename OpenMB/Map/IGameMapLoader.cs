using Mogre;
using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Map
{
    public interface IGameMapLoader
    {
        event Action LoadMapStarted;
        event Action LoadMapFinished;
        string Name { get; }
        string LoadedMapName { get; }
        void LoadAsync(IGameMap gameMap, string mapFile);
    }
}
