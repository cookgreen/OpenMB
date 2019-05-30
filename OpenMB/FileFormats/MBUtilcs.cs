using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenMB.FileFormats
{
    public class MBUtil
    {
        public static string GetLastPathName(string path)
        {
            int index = path.LastIndexOf("\\");
            return path.Substring(index + 1, path.Length - index - 1);
        }

        public static float LoadFloat(BinaryReader reader)
        {
            return reader.ReadSingle();
        }

        public static int LoadInt32(BinaryReader reader)
        {
            return reader.ReadInt32();
        }

        public static uint LoadUInt32(BinaryReader reader)
        {
            return reader.ReadUInt32();
        }

        public static Point3F LoadPoint3F(BinaryReader reader)
        {
            Point3F vect = new Point3F();
            vect.x = LoadFloat(reader);
            vect.y = LoadFloat(reader);
            vect.z = LoadFloat(reader);
            return vect;
        }

        public static PointF LoadPoint2F(BinaryReader reader)
        {
            PointF vect = new PointF();
            vect.X = LoadFloat(reader);
            vect.Y = LoadFloat(reader);
            return vect;
        }

        public static string LoadString(BinaryReader reader)
        {
            byte b = reader.ReadByte();
            reader.ReadBytes(3);
            byte[] bytes = reader.ReadBytes(b);
            string str = Encoding.UTF8.GetString(bytes);
            return str;
        }

        public static string LoadStringMaybe(BinaryReader reader, string ifnot)
        {
            byte b = reader.ReadByte();
            reader.ReadBytes(3);

            if (b < 99 && b > 0)
            {
                byte[] bytes = reader.ReadBytes(b);
                string str = Encoding.UTF8.GetString(bytes);
                return str;
            }
            else
            {
                reader.BaseStream.Seek(-4, SeekOrigin.Current);
                return ifnot;
            }

        }

        public static byte LoadByte(BinaryReader reader)
        {
            return reader.ReadByte();
        }

        public static bool LoadVector(BinaryReader reader, ref List<int> v)
        {
            uint k;
            k = LoadUInt32(reader);
            for (uint i = 0; i < k; i++)
                v[(int)i] = LoadInt32(reader);
            return true;
        }

        public static bool LoadVector(BinaryReader reader, ref List<Point3F> v)
        {
            uint k;
            k = LoadUInt32(reader);
            v = new List<Point3F>((int)k);
            for (uint i = 0; i < k; i++)
                v.Add(LoadPoint3F(reader));
            return true;
        }

        public static bool LoadVector<T>(BinaryReader reader, ref List<T> v)
        {
            uint k;
            k = LoadUInt32(reader);
            v = new List<T>((int)k);
            for (uint i = 0; i < k; i++)
            {
                //v.Add(0);
            }
            return true;
        }

        public static int TmpRigging2Rigging(ref List<TmpSkinning> v1, ref List<MBBrfSkinning> v2)
        {
            int max = 0;

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v1[i].pairs.Count; j++)
                {
                    v2[v1[i].pairs[j].vindex].Add(v1[i].bindex, v1[i].pairs[j].vindex);
                }
            }

            return max;
        }
    }
}
