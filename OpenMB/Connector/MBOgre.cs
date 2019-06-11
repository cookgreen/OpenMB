using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.FileFormats;
using Mogre;
using OpenMB.Map;

namespace OpenMB.Connector
{
    public class MBOgre
    {
        public static MeshPtr ConvertToOgreMesh(GameMap map, MBOgreBrf mbBrfFile, string meshName)
        {
            var findMeshes = mbBrfFile.Brf.Meshes.Where(o => o.Name == meshName);
            if (findMeshes.Count() > 0)
            {
                MBBrfMesh mesh = findMeshes.ElementAt(0);
                //Convert Vertex and Faces to Ogre Mesh Format

                map.CreateMesh(meshName);
            }
            return null;
        }
    }
}
