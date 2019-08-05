#region
/*
 Original written by Swyter in Cartographer with Lua Language
 https://bitbucket.org/Swyter/cartographer
 */
#endregion
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.FileFormats
{
    public enum MBWorldMapTerrainType
    {
        rt_bridge = 7,
        rt_deep_water = 15,
        rt_desert = 5,
        rt_desert_forest = 13,
        rt_forest = 11,
        rt_mountain = 1,
        rt_mountain_forest = 9,
        rt_plain = 3,
        rt_river = 8,
        rt_snow = 4,
        rt_snow_forest = 12,
        rt_steppe = 2,
        rt_steppe_forest = 10,
        rt_water = 0,
    }
    
    [XmlRoot("Vertex")]
    public class Pont3FSerializable
    {
        [XmlAttribute]
        public float x;
        [XmlAttribute]
        public float y;
        [XmlAttribute]
        public float z;
    }

    [XmlRoot("Face")]
    public class MBWorldMapFace
    {
        [XmlAttribute]
        public MBWorldMapTerrainType TerrainType;
        [XmlAttribute]
        public int indexFirst;
        [XmlAttribute]
        public int indexSecond;
        [XmlAttribute]
        public int indexThird;
    }

    [XmlRoot("MapData")]
    public class MBWorldMap
    {
        [XmlElement("Vertics")]
        public List<Point3F> Vertics;
        [XmlElement("Faces")]
        public List<MBWorldMapFace> Faces;
        [XmlAttribute]
        public int VertexNum;
        [XmlAttribute]
        public int FaceNum;

        /// <summary>
        /// Color
        /// </summary>
        [XmlIgnore]
        public Dictionary<MBWorldMapTerrainType, Mogre.ColourValue> Color;

        /// <summary>
        /// Face Normal
        /// </summary>
        [XmlIgnore]
        public List<float[]> fcn;

        /// <summary>
        /// Face Area
        /// </summary>
        [XmlIgnore]
        public List<float> cfa;

        /// <summary>
        /// Vertex Normal
        /// </summary>
        [XmlIgnore]
        public List<float[]> vtn;

        public MBWorldMap()
        {
            Vertics = new List<Point3F>();
            Faces = new List<MBWorldMapFace>();
            Color = new Dictionary<MBWorldMapTerrainType, Mogre.ColourValue>()
            {
                { MBWorldMapTerrainType.rt_bridge, new Mogre.ColourValue(1, 0, 0) },
                { MBWorldMapTerrainType.rt_deep_water, new Mogre.ColourValue(0, 0, 0.2f) },
                { MBWorldMapTerrainType.rt_desert, new Mogre.ColourValue(0.7f, 0.8f, 0.6f) },
                { MBWorldMapTerrainType.rt_desert_forest, new Mogre.ColourValue(0.6f, 0.7f, 0.5f) },
                { MBWorldMapTerrainType.rt_forest, new Mogre.ColourValue() },
                { MBWorldMapTerrainType.rt_mountain, new Mogre.ColourValue(0.5f, 0.6f, 0.4f) },
                { MBWorldMapTerrainType.rt_mountain_forest, new Mogre.ColourValue(0.4f, 0.7f, 0.3f) },
                { MBWorldMapTerrainType.rt_plain, new Mogre.ColourValue(0, 1, 0.2f) },
                { MBWorldMapTerrainType.rt_river, new Mogre.ColourValue(0, 0, 0.5f) },
                { MBWorldMapTerrainType.rt_snow, new Mogre.ColourValue(1, 1, 1) } ,
                { MBWorldMapTerrainType.rt_snow_forest, new Mogre.ColourValue() },
                { MBWorldMapTerrainType.rt_steppe, new Mogre.ColourValue(0, 1, 0.5f) },
                { MBWorldMapTerrainType.rt_steppe_forest, new Mogre.ColourValue(0.4f, 0.5f, 0.4f) },
                { MBWorldMapTerrainType.rt_water, new Mogre.ColourValue(0, 0, 0.3f) },
            };
        }

        public static MBWorldMap ParseXml(string mapXmlFile)
        {
            MBWorldMap worldMap = new MBWorldMap();
            Mods.ModXmlLoader loader = new Mods.ModXmlLoader(mapXmlFile);
            loader.Load<MBWorldMap>(out worldMap);
            return worldMap;
        }

        public void ParseTxt(string mapTxt)
        {
            using (StreamReader reader = new StreamReader(new FileStream(mapTxt, FileMode.Open, FileAccess.Read)))
            {
                string str = reader.ReadLine();
                if (int.TryParse(str, out VertexNum))
                {
                    for (int i = 0; i < VertexNum; i++)
                    {
                        str = reader.ReadLine();
                        var tokens = str.Split(' ');
                        Point3F vertex = new Point3F();
                        vertex.x = float.Parse(tokens[0]);
                        vertex.y = float.Parse(tokens[1]);
                        vertex.z = float.Parse(tokens[2]);
                        Vertics.Add(vertex);
                    }

                    str = reader.ReadLine();
                    FaceNum = int.Parse(str);
                    for (int i = 0; i < FaceNum; i++)
                    {
                        str = reader.ReadLine();
                        var tokens = str.Split(' ');
                        MBWorldMapFace face = new MBWorldMapFace();
                        face.TerrainType = (MBWorldMapTerrainType)int.Parse(tokens[2]);
                        face.indexFirst = int.Parse(tokens[7]);
                        face.indexSecond = int.Parse(tokens[9]);
                        face.indexThird = int.Parse(tokens[11]);
                        Faces.Add(face);
                    }

                    if (AreTheAxisReversed())
                    {
                        for (int i = 0; i < Vertics.Count; i++)
                        {
                            Vertics[i].y = Vertics[i].z;
                            Vertics[i].z = Vertics[i].y * -1;
                        }
                    }
                }
            }
        }

        private bool AreTheAxisReversed()
        {
            int ly = 0, lz = 0;

            for (int i = 0; i < Faces.Count; i++)
            {
                ly = Math.Max(ly, (int)Math.Abs(Vertics[(int)Faces[i].indexSecond].y));
                lz = Math.Max(lz, (int)Math.Abs(Vertics[(int)Faces[i].indexSecond].z));
            }

            return lz > ly && true || false;
        }

        public void SaveAsXml(string path)
        {
            Mods.ModXmlLoader loader = new Mods.ModXmlLoader(path);
            loader.Save<MBWorldMap>(this);
        }
    }
}
