using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.FileFormats;
using Mogre;
using OpenMB.Map;

namespace OpenMB.Connector
{
    public class MBOgre : IConnector
    {
        private Dictionary<string, MaterialPtr> materials;
        private Dictionary<string, TexturePtr> textures;
        private Dictionary<string, MeshPtr> meshes;
        private static int cp, cv;
        private static MBOgre instance;
        public static MBOgre Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MBOgre();
                }
                return instance;
            }
        }

        public MBOgre()
        {
            materials = new Dictionary<string, MaterialPtr>();
            textures = new Dictionary<string, TexturePtr>();
            meshes = new Dictionary<string, MeshPtr>();
        }

        public MBOgreManifest LoadBrfFile(MBOgreBrf mbBrfFile)
        {
            MBOgreManifest manifest = new MBOgreManifest();
            if (mbBrfFile == null)
            {
                return null;
            }
            return manifest;
        }

        public TexturePtr LoadBrfTextureIntoOgre(SceneManager sceneManager, MBBrfTexture brfTexture)
        {
            if (brfTexture == null)
            {
                return null;
            }
            if (textures.ContainsKey(brfTexture.name))
            {
                return textures[brfTexture.name];
            }
            return null;

        }

        public MaterialPtr LoadBrfMaterialIntoOgre(SceneManager sceneManager, MBBrfMaterial brfMaterial)
        {
            if (brfMaterial == null)
            {
                return null;
            }
            if (materials.ContainsKey(brfMaterial.name))
            {
                return materials[brfMaterial.name];
            }
            return null;
        }

        public MeshPtr LoadStaticBrfMeshIntoOgre(SceneManager sceneManager, MBBrfMesh brfMesh, int frame = 0)
        {
            //Convert Vertex and Faces to Ogre Mesh Format
            if (brfMesh == null)
            {
                return null;
            }
            if (meshes.ContainsKey(brfMesh.Name))
            {
                return meshes[brfMesh.Name];
            }
            ManualObject mo = sceneManager.CreateManualObject(brfMesh.Name + "-" + Guid.NewGuid().ToString());
            mo.Begin(brfMesh.Material);

            int np = 0, nv = 0;
            for (int i = 0; i < brfMesh.Frames[frame].pos.Count; i++)
            {
                mo.Position(
                    brfMesh.Frames[frame].pos[i].x,
                    brfMesh.Frames[frame].pos[i].y,
                    brfMesh.Frames[frame].pos[i].z
                );
                np++;
            }

            for (int i = 0; i < brfMesh.Frames[frame].norm.Count; i++)
            {
                mo.Normal(
                    -brfMesh.Frames[frame].norm[i].x,
                    brfMesh.Frames[frame].norm[i].y,
                    brfMesh.Frames[frame].norm[i].z
                );
                mo.TextureCoord(
                    brfMesh.Vertex[i].ta.X,
                    brfMesh.Vertex[i].ta.Y
                );
                nv++;
            }

            for (int i = 0; i < brfMesh.Faces.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mo.Triangle(
                        (uint)(brfMesh.Vertex[brfMesh.Faces[i].index[j]].index + 1 + cp),
                        (uint)(brfMesh.Faces[i].index[j] + 1 + cv),
                        (uint)(brfMesh.Faces[i].index[j] + 1 + cv)
                    );
                }
            }

            mo.End();

            cp += np;
            cv += nv;

            return mo.ConvertToMesh(brfMesh.Name + "-" + Guid.NewGuid().ToString());
        }

        public MeshPtr LoadWorldMap(string worldMapID, SceneManager sceneManager, MBWorldMap worldMap)
        {
            //Convert Vertex and Faces to Ogre Mesh Format
            if (worldMap == null)
            {
                return null;
            }
            if (meshes.ContainsKey(worldMapID))
            {
                return meshes[worldMapID];
            }
            ManualObject mo = sceneManager.CreateManualObject("WORLDMAP-MANUAL-OBJECT-" + worldMapID);
            mo.Begin("");

            for (int i = 0; i < worldMap.Faces.Count; i++)
            {
                mo.Colour(worldMap.Color[worldMap.Faces[i].TerrainType]);

                for (int j = 0; j < 3; j++)
                {
                    int vindex = -1;
                    if (j == 0)
                    {
                        vindex = worldMap.Faces[i].indexFirst;
                    }
                    else if (j == 1)
                    {
                        vindex = worldMap.Faces[i].indexSecond;
                    }
                    else if (j == 2)
                    {
                        vindex = worldMap.Faces[i].indexThird;
                    }
                    mo.Position(
                        worldMap.Vertics[vindex].x,
                        worldMap.Vertics[vindex].y,
                        worldMap.Vertics[vindex].z
                    );
                }
                mo.Triangle(
                    (uint)(worldMap.Faces[i].indexFirst),
                    (uint)(worldMap.Faces[i].indexSecond),
                    (uint)(worldMap.Faces[i].indexThird)
                );
            }

            mo.End();

            var mesh = mo.ConvertToMesh("WORLDMAP-" + worldMapID);
            meshes.Add(worldMapID, mesh);
            return mesh;
        }
    }
}
