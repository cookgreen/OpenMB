using Mogre;
using OpenMB.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenMB.FileFormats
{
    public class MBBrf
    {
        private string name;
        private int version;
        private uint meshNum;
        private List<MBBrfMesh> meshes;
        private uint textureNum;
        private List<MBBrfTexture> textures;
        private uint shaderNum;
        private List<MBBrfShader> shaders;
        private uint materialNum;
        private List<MBBrfMaterial> materials;
        private string path;
        private int globalVersion;

        public List<MBBrfMesh> Meshes
        {
            get
            {
                return meshes;
            }

            set
            {
                meshes = value;
            }
        }

        public List<MBBrfTexture> Textures
        {
            get
            {
                return textures;
            }
        }

        public List<MBBrfShader> Shaders
        {
            get
            {
                return shaders;
            }
        }

        public List<MBBrfMaterial> Materials
        {
            get
            {
                return materials;
            }
        }

        public MBBrf(string name, string path)
        {
            this.name = name;
            this.path = path;
            meshes = new List<MBBrfMesh>();
            textures = new List<MBBrfTexture>();
            shaders = new List<MBBrfShader>();
            materials = new List<MBBrfMaterial>();

            Load();
        }

        public MBBrf(string name, DataStreamPtr stream)
        {
            this.name = name;
            this.path = "OGRE_RESOURCE_PATH";
            meshes = new List<MBBrfMesh>();
            textures = new List<MBBrfTexture>();
            shaders = new List<MBBrfShader>();
            materials = new List<MBBrfMaterial>();

            Load(stream);
        }

        private void Load()
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
            {
                globalVersion = 0;
                while (true)
                {
                    string str = MBUtil.LoadString(reader);
                    if (str == "end" || reader.BaseStream.Length == reader.BaseStream.Position)
                    {
                        break;
                    }
                    else if (str == "rfver ")
                    {
                        version = (int)reader.ReadUInt32();
                        globalVersion = version;
                    }
                    else if (str == "mesh")
                    {
                        meshNum = reader.ReadUInt32();
                        for (int i = 0; i < meshNum; i++)
                        {
                            MBBrfMesh brfMesh = new MBBrfMesh();
                            brfMesh.globalVersion = globalVersion;
                            brfMesh.Load(reader);
                            meshes.Add(brfMesh);
                        }
                    }
                    else if (str == "texture")
                    {
                        textureNum = reader.ReadUInt32();
                        for (int i = 0; i < textureNum; i++)
                        {
                            MBBrfTexture texture = new MBBrfTexture();
                            texture.Load(reader);
                            textures.Add(texture);
                        }
                    }
                    else if (str == "shader")
                    {
                        shaderNum = reader.ReadUInt32();
                        for (int i = 0; i < shaderNum; i++)
                        {
                            MBBrfShader shader = new MBBrfShader();
                            shader.Load(reader);
                            shaders.Add(shader);
                        }
                    }
                    else if (str == "material")
                    {
                        materialNum = reader.ReadUInt32();
                        for (int i = 0; i < materialNum; i++)
                        {
                            MBBrfMaterial material = new MBBrfMaterial();
                            material.Load(reader);
                            materials.Add(material);
                        }
                    }
                }
            }
        }

        private unsafe void Load(DataStreamPtr reader)
        {
            globalVersion = 0;
            while (true)
            {
                string str = MBOgreUtil.LoadString(reader);
                if (str == "end" || reader.Eof())
                {
                    break;
                }
                else if (str == "rfver ")
                {
                    version = MBOgreUtil.LoadInt32(reader);
                    globalVersion = version;
                }
                else if (str == "mesh")
                {
                    meshNum = MBOgreUtil.LoadUInt32(reader);
                    for (int i = 0; i < meshNum; i++)
                    {
                        MBBrfMesh brfMesh = new MBBrfMesh();
                        brfMesh.globalVersion = globalVersion;
                        brfMesh.Load(reader);
                        meshes.Add(brfMesh);
                    }
                }
                else if (str == "texture")
                {
                    textureNum = MBOgreUtil.LoadUInt32(reader);
                    for (int i = 0; i < textureNum; i++)
                    {
                        MBBrfTexture texture = new MBBrfTexture();
                        texture.Load(reader);
                        textures.Add(texture);
                    }
                }
                else if (str == "shader")
                {
                    shaderNum = MBOgreUtil.LoadUInt32(reader);
                    for (int i = 0; i < shaderNum; i++)
                    {
                        MBBrfShader shader = new MBBrfShader();
                        shader.Load(reader);
                        shaders.Add(shader);
                    }
                }
                else if (str == "material")
                {
                    materialNum = MBOgreUtil.LoadUInt32(reader);
                    for (int i = 0; i < materialNum; i++)
                    {
                        MBBrfMaterial material = new MBBrfMaterial();
                        material.Load(reader);
                        materials.Add(material);
                    }
                }
            }
        }
    }
}
