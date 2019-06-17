#region
/*
 Original written by Swyter in Cartographer with Lua Language
 https://bitbucket.org/Swyter/cartographer
 */
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.FileFormats
{
    public enum MBWorldMapTerrainType
    {
        rt_water = 0,
        rt_mountain = 1,
        rt_steppe = 2,
        rt_plain = 3,
        rt_snow = 4,
        rt_desert = 5,
        rt_bridge = 7,
        rt_river = 8,
        rt_mountain_forest = 9,
        rt_steppe_forest = 10,
        rt_forest = 11,
        rt_snow_forest = 12,
        rt_desert_forest = 13,
        rt_deep_water = 15,
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
        private int vertexNum;
        private int faceNum;

        public MBWorldMap()
        {
            Vertics = new List<Point3F>();
            Faces = new List<MBWorldMapFace>();
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
                if (int.TryParse(str, out vertexNum))
                {
                    for (int i = 0; i < vertexNum; i++)
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
                    faceNum = int.Parse(str);
                    for (int i = 0; i < faceNum; i++)
                    {
                        str = reader.ReadLine();
                        var tokens = str.Split(' ');
                        MBWorldMapFace face = new MBWorldMapFace();
                        Console.WriteLine(tokens[1]);
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

            return ly > lz && true || false;
        }

        public void SaveAsXml(string path)
        {
            MBWorldMap worldMap = new MBWorldMap();
            Mods.ModXmlLoader loader = new Mods.ModXmlLoader(path);
            loader.Save<MBWorldMap>(this);
        }
    }
}
