using Mogre;
using OpenMB.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenMB.FileFormats
{
    public class MBBrfBodyPart
    {
        public enum Type
        {
            MANIFOLD,
            FACE,
            CAPSULE,
            SPHERE,
            N_TYPE
        }
        public enum Attribute
        {
            RADIUS,
            LEN_TOP,
            LEN_BOT,
            POSX,
            POSY,
            POSZ,
            ROTX,
            ROTY
        }
        private int ori;
        private List<Point3F> pos;
        private List<List<int>> faces;
        private Point3F center;
        private float radius;
        private Point3F dir;
        private uint flags;
        private Box3F bbox;
        private Type type;

        public void Load(BinaryReader reader, string str = null)
        {
            string firstWord = null;
            if (string.IsNullOrEmpty(str))
            {
                firstWord = MBUtil.LoadString(reader);
            }
            else
            {
                faces = new List<List<int>>();
                firstWord = str;
                if (firstWord == "manifold")
                {
                    type = Type.MANIFOLD;

                    MBUtil.LoadVector(reader, ref pos);

                    int k = MBUtil.LoadInt32(reader);
                    for (int i = 0; i < k; i++)
                    {
                        ori = MBUtil.LoadInt32(reader);
                        int h = MBUtil.LoadInt32(reader);
                        List<int> v = new List<int>();
                        for (int j = 0; j < h; j++)
                        {
                            int pp = MBUtil.LoadInt32(reader);
                            v.Add(pp);
                        }
                        faces.Add(v);
                    }
                }
                else if (firstWord == "capsule")
                {
                    type = Type.CAPSULE;
                    radius = MBUtil.LoadFloat(reader);
                    center = MBUtil.LoadPoint3F(reader);
                    dir = MBUtil.LoadPoint3F(reader);
                    flags = MBUtil.LoadUInt32(reader);
                }
                else if (firstWord == "sphere")
                {
                    type = Type.SPHERE;
                    radius = MBUtil.LoadFloat(reader);
                    center = MBUtil.LoadPoint3F(reader);
                    flags = MBUtil.LoadUInt32(reader);
                }
                else if (firstWord == "face")
                {
                    type = Type.FACE;
                    MBUtil.LoadVector(reader, ref pos);

                    int k = pos.Count;
                    List<int> aface = new List<int>();
                    for (int i = 0; i < k; i++)
                    {
                        aface.Add(i);
                    }
                    faces.Add(aface);

                    flags = MBUtil.LoadUInt32(reader);
                }
                else
                {
                    Console.WriteLine(string.Format("Unknown body (collision mesh) type `{0}`\n", firstWord));
                }
            }
        }
        public void Load(DataStreamPtr reader, string str = null)
        {
            string firstWord = null;
            if (string.IsNullOrEmpty(str))
            {
                firstWord = MBOgreUtil.LoadString(reader);
            }
            else
            {
                faces = new List<List<int>>();
                firstWord = str;
                if (firstWord == "manifold")
                {
                    type = Type.MANIFOLD;

                    MBOgreUtil.LoadVector(reader, ref pos);

                    int k = MBOgreUtil.LoadInt32(reader);
                    for (int i = 0; i < k; i++)
                    {
                        ori = MBOgreUtil.LoadInt32(reader);
                        int h = MBOgreUtil.LoadInt32(reader);
                        List<int> v = new List<int>();
                        for (int j = 0; j < h; j++)
                        {
                            int pp = MBOgreUtil.LoadInt32(reader);
                            v.Add(pp);
                        }
                        faces.Add(v);
                    }
                }
                else if (firstWord == "capsule")
                {
                    type = Type.CAPSULE;
                    radius = MBOgreUtil.LoadFloat(reader);
                    center = MBOgreUtil.LoadPoint3F(reader);
                    dir = MBOgreUtil.LoadPoint3F(reader);
                    flags = MBOgreUtil.LoadUInt32(reader);
                }
                else if (firstWord == "sphere")
                {
                    type = Type.SPHERE;
                    radius = MBOgreUtil.LoadFloat(reader);
                    center = MBOgreUtil.LoadPoint3F(reader);
                    flags = MBOgreUtil.LoadUInt32(reader);
                }
                else if (firstWord == "face")
                {
                    type = Type.FACE;
                    MBOgreUtil.LoadVector(reader, ref pos);

                    int k = pos.Count;
                    List<int> aface = new List<int>();
                    for (int i = 0; i < k; i++)
                    {
                        aface.Add(i);
                    }
                    faces.Add(aface);

                    flags = MBOgreUtil.LoadUInt32(reader);
                }
                else
                {
                    Console.WriteLine(string.Format("Unknown body (collision mesh) type `{0}`\n", firstWord));
                }
            }
        }
    }
    public class MBBrfBody
    {
        private string name;
        private List<MBBrfBodyPart> parts;

        public void Load(BinaryReader reader)
        {
            name = MBUtil.LoadString(reader);
            string str = MBUtil.LoadString(reader);
            if (str == "composite")
            {
                uint k = MBUtil.LoadUInt32(reader);
                parts = new List<MBBrfBodyPart>();
                for (int i = 0; i < k; i++)
                {
                    MBBrfBodyPart part = new MBBrfBodyPart();
                    part.Load(reader);
                    parts.Add(part);
                }
            }
            else
            {
                MBBrfBodyPart part = new MBBrfBodyPart();
                part.Load(reader, str);
                parts.Add(part);
            }
        }
        public void Load(DataStreamPtr reader)
        {
            name = MBOgreUtil.LoadString(reader);
            string str = MBOgreUtil.LoadString(reader);
            if (str == "composite")
            {
                uint k = MBOgreUtil.LoadUInt32(reader);
                parts = new List<MBBrfBodyPart>();
                for (int i = 0; i < k; i++)
                {
                    MBBrfBodyPart part = new MBBrfBodyPart();
                    part.Load(reader);
                    parts.Add(part);
                }
            }
            else
            {
                MBBrfBodyPart part = new MBBrfBodyPart();
                part.Load(reader, str);
                parts.Add(part);
            }
        }
    }
}
