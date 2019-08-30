using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace OpenMB.Mods.Common.Loaders
{
    public class MapLoaderPk3 : IGameMapLoader
    {
        public string Name
        {
            get
            {
                return "PK3";
            }
        }

        public event Action LoadMapFinished;

        public void LoadAsync(string mapFile)
        {
            throw new NotImplementedException();
        }
    }
}
