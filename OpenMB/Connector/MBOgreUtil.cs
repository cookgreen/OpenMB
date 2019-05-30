using Mogre;
using OpenMB.FileFormats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OpenMB.Connector
{
    public class MBOgreUtil
    {
        public static unsafe float LoadFloat(DataStreamPtr reader)
        {
            byte[] bytes = new byte[4];
            fixed (byte* buff = bytes)
            {
                reader.Read(buff, 4);
            }

            return BitConverter.ToSingle(bytes, 0);
        }

        public static unsafe int LoadInt32(DataStreamPtr reader)
        {
            byte[] bytes = new byte[4];
            fixed (byte* buff = bytes)
            {
                reader.Read(buff, 4);
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        public static unsafe uint LoadUInt32(DataStreamPtr reader)
        {
            byte[] bytes = new byte[4];
            fixed (byte* buff = bytes)
            {
                reader.Read(buff, 4);
            }
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static Point3F LoadPoint3F(DataStreamPtr reader)
        {
            Point3F vect = new Point3F();
            vect.x = LoadFloat(reader);
            vect.y = LoadFloat(reader);
            vect.z = LoadFloat(reader);
            return vect;
        }

        public static PointF LoadPoint2F(DataStreamPtr reader)
        {
            PointF vect = new PointF();
            vect.X = LoadFloat(reader);
            vect.Y = LoadFloat(reader);
            return vect;
        }

        public static unsafe string LoadString(DataStreamPtr reader)
        {
            byte[] bytes = new byte[1];
            fixed (byte* buff = bytes)
            {
                reader.Read(buff, 1);
            }

            byte b = bytes[0];
            bytes = new byte[3];
            fixed (byte* buff = bytes)
            {
                reader.Read(buff, 3);
            }
            bytes = new byte[b];
            fixed (byte* buff = bytes)
            {
                reader.Read(buff, b);
            }
            string str = Encoding.UTF8.GetString(bytes);
            return str;
        }

        public static unsafe string LoadStringMaybe(DataStreamPtr reader, string ifnot)
        {
            byte[] bytes = new byte[1];
            fixed (byte* buff = bytes)
            {
                reader.Read(buff, 1);
            }

            byte b = bytes[0];
            bytes = new byte[3];
            fixed (byte* buff = bytes)
            {
                reader.Read(buff, 3);
            }
            if (b < 99 && b > 0)
            {
                bytes = new byte[b];
                fixed (byte* buff = bytes)
                {
                    reader.Read(buff, b);
                }
                string str = Encoding.UTF8.GetString(bytes);
                return str;
            }
            else
            {
                uint pos = reader.Tell();
                pos = pos - 4;
                reader.Seek(pos);
                return ifnot;
            }

        }

        public static unsafe byte LoadByte(DataStreamPtr reader)
        {
            byte[] bytes = new byte[1];
            fixed (byte* buff = bytes)
            {
                reader.Read(buff, 1);
            }
            return bytes[0];
        }

        public static bool LoadVector(DataStreamPtr reader, ref List<int> v)
        {
            uint k;
            k = LoadUInt32(reader);
            for (uint i = 0; i < k; i++)
                v[(int)i] = LoadInt32(reader);
            return true;
        }

        public static bool LoadVector(DataStreamPtr reader, ref List<Point3F> v)
        {
            uint k;
            k = LoadUInt32(reader);
            v = new List<Point3F>((int)k);
            for (uint i = 0; i < k; i++)
                v.Add(LoadPoint3F(reader));
            return true;
        }
    }
}
