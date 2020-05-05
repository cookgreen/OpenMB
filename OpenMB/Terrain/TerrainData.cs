using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Terrain
{
    public class TerrainData
    {
        private TerrainGroup terrainGroup;
        private TerrainGlobalOptions terrainGlobalOptions;

        public TerrainGroup TerrainGroup { get { return terrainGroup; } }
        public TerrainGlobalOptions TerrainGlobalOptions { get { return terrainGlobalOptions; } }
        public TerrainData(TerrainGroup terrainGroup, TerrainGlobalOptions terrainGlobalOptions)
        {
            this.terrainGroup = terrainGroup;
            this.terrainGlobalOptions = terrainGlobalOptions;
        }
    }
}
