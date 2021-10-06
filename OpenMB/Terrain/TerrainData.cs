using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Terrain
{
    public class TerrainTileData
    {
        public float Height { get; set; }   
    }

    public class TerrainData
    {
        private int width;
        private int height;
        private int tileSizeX;
        private int tileSizeY;

        private TerrainTileData[,] tileData;
        
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        public TerrainData(int width, int height, int tileSizeX, int tileSizeY)
        {
            this.width = width;
            this.height = height;
            tileData = new TerrainTileData[tileSizeX, tileSizeY];
        }

        public TerrainData(string terrainCode)
        {
            parseTerrainCode(terrainCode);
        }

        public TerrainData(byte[,] highMapPicData)
        {
            parseTerrainHeightMap(highMapPicData);
        }

        private void parseTerrainCode(string terrainCode)
        {
        }

        private void parseTerrainHeightMap(byte[,] highMapPicData)
        {
        }
    }
}
