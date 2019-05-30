using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace OpenMB.FileFormats
{
    public class MBBrfVert
    {
        public int index;
        public uint col;
        public Point3F __norm; // use normal inside frame instead
        public Point3F tang; // tangent dir...
        public byte tangi; // 0: frame TBN right-handed  1: left-handed
        public PointF ta, tb; // texture
        public int globalVersion;
        public MBBrfVert()
        {

        }
        public void Load(BinaryReader reader)
        {
            if (globalVersion == 0)
            {
                index = MBUtil.LoadInt32(reader);
                col = MBUtil.LoadUInt32(reader); // color x vert! as 4 bytes AABBGGRR
                __norm = MBUtil.LoadPoint3F(reader);
                ta = MBUtil.LoadPoint2F(reader);
                ta.Y = 1 - ta.Y;
                tb = MBUtil.LoadPoint2F(reader);
                tb.Y = 1 - tb.Y;
            }
            else if (globalVersion == 1)
            {
                index = MBUtil.LoadInt32(reader);
                col = MBUtil.LoadUInt32(reader); // color x vert! as 4 bytes AABBGGRR
                __norm = MBUtil.LoadPoint3F(reader);
                tang = MBUtil.LoadPoint3F(reader);
                tangi = MBUtil.LoadByte(reader);
                ta = MBUtil.LoadPoint2F(reader);
                ta.Y = 1 - ta.Y;
                tb = ta;
            }
            else if (globalVersion == 2)
            {
                index = MBUtil.LoadInt32(reader);
                col = MBUtil.LoadUInt32(reader); // color x vert! as 4 bytes AABBGGRR
                __norm = MBUtil.LoadPoint3F(reader);
                ta = MBUtil.LoadPoint2F(reader);
                ta.Y = 1 - ta.Y;
                tb = ta;
            }
        }
    };

    public class MBBrfFace
    {
        public int[] index = new int[3];
        internal int globalVersion;

        public MBBrfFace() { }
        public MBBrfFace(int i, int j, int k) { index[0] = i; index[1] = j; index[2] = k; }

        public void Flip()
        {

        }
        public bool Load(BinaryReader reader)
        {
            index[0] = MBUtil.LoadInt32(reader);
            index[1] = MBUtil.LoadInt32(reader);
            index[2] = MBUtil.LoadInt32(reader);
            return true;
        }
    }

    public class MBBrfFrame
    {
        public int time;
        public List<Point3F> pos;
        public List<Point3F> norm;

        public void Load(BinaryReader reader)
        {
            time = MBUtil.LoadInt32(reader);
            MBUtil.LoadVector(reader, ref pos);
            MBUtil.LoadVector(reader, ref norm);
        }
    }

    public class MBBrfSkinning
    {
        int[] boneIndex = new int[4];
        float[] boneWeight = new float[4];

        public MBBrfSkinning()
        {
            boneIndex[0] = boneIndex[1] = boneIndex[2] = boneIndex[3] = -1;
            boneWeight[0] = boneWeight[1] = boneWeight[2] = boneWeight[3] = 0;
        }

        public void Add(int bindex, int vindex)
        {
            bool overflow = false;
            int k = FirstEmpty();
            if (k >= 4)
            {
                k = LeastIndex();
                overflow = true;
            }
            boneIndex[k] = bindex;
            boneWeight[k] = vindex;
            if (overflow) Normalize();
        }

        public int FirstEmpty()
        {
            for (int k = 0; k < 4; k++) if (boneIndex[k] == -1) return k;
            return 4;
        }

        public int LeastIndex()
        {
            int min = 0;

            for (int k = 1; k < 4; k++) if (boneIndex[k] != -1)
                    if (boneWeight[k] < boneWeight[min]) min = k;
            return min;
        }

        public void Normalize()
        {
            float sum = 0;

            for (int k = 0; k < 4; k++) if (boneIndex[k] != -1) sum += boneWeight[k];
            if (sum == 0) return;
            for (int k = 0; k < 4; k++) if (boneIndex[k] != -1) boneWeight[k] /= sum;
        }
    }

    public class TmpSkinning
    {
        public int bindex; // bone index
        public List<TmpRiggingPair> pairs;

        public void Load(BinaryReader reader)
        {
            bindex = MBUtil.LoadInt32(reader);
            uint k = MBUtil.LoadUInt32(reader);
            pairs = new List<TmpRiggingPair>();
            for (uint i = 0; i < k; i++)
            {
                TmpRiggingPair pair = new TmpRiggingPair();
                pair.Load(reader);
                pairs.Add(pair);
            }
        }
    }

    public class TmpRiggingPair
    {
        public int vindex;  // vert index
        public float weight;

        public void Load(BinaryReader reader)
        {
            vindex = MBUtil.LoadInt32(reader);
            weight = MBUtil.LoadFloat(reader);
        }
    }
    public class MBBrfMesh
    {
        private string meshName;
        private string materialName;
        private List<MBBrfFrame> frames;
        private List<MBBrfVert> vertex;
        private List<MBBrfFace> faces;
        private List<MBBrfSkinning> skinning;
        public int globalVersion;

        public string Name
        {
            get
            {
                return meshName;
            }
        }

        public string Material
        {
            get
            {
                return materialName;
            }
        }

        public MBBrfMesh()
        {
            globalVersion = -1;
        }

        public void Load(BinaryReader reader)
        {
            meshName = MBUtil.LoadString(reader);

            uint flags = reader.ReadUInt32();
            
            materialName = MBUtil.LoadString(reader);

            MBBrfFrame oneFrame = new MBBrfFrame();
            frames = new List<MBBrfFrame>(1)
            {
                oneFrame
            };

            if (globalVersion != 0)
            {
                int offset = 1 << 16;
                uint ret = (uint)(flags & offset);
                if (ret == offset)
                {
                    globalVersion = 1;
                }
                else
                {
                    globalVersion = 2;
                }
            }

            MBUtil.LoadVector(reader, ref frames[0].pos);

            uint v = MBUtil.LoadUInt32(reader);
            List<TmpSkinning> tmpRig = new List<TmpSkinning>();
            for (int i = 0; i < v; i++)
            {
                TmpSkinning tmpR = new TmpSkinning();
                tmpR.Load(reader);
                tmpRig.Add(tmpR);
            }

            int k;
            k = MBUtil.LoadInt32(reader);
            for (int i = 0; i < k; i++)
            {
                if (i == 0)
                {
                    frames[0].Load(reader);
                }
                else
                {
                    MBBrfFrame frame = new MBBrfFrame();
                    frame.Load(reader);
                    frames.Add(frame);
                }
            }

            v = MBUtil.LoadUInt32(reader);
            vertex = new List<MBBrfVert>();
            for (uint i = 0; i < v; i++)
            {
                MBBrfVert vert = new MBBrfVert()
                {
                    globalVersion = globalVersion
                };
                vert.Load(reader);
                vertex.Add(vert);
            }

            v = MBUtil.LoadUInt32(reader);
            faces = new List<MBBrfFace>();
            for (uint i = 0; i < v; i++)
            {
                MBBrfFace face = new MBBrfFace()
                {
                    globalVersion = globalVersion
                };
                face.Load(reader);
                faces.Add(face);
            }

            skinning = new List<MBBrfSkinning>();
            for (int i = 0; i < oneFrame.pos.Count; i++)
            {
                skinning.Add(new MBBrfSkinning());
            }
            if (tmpRig.Count > 0)
            {
                MBUtil.TmpRigging2Rigging(ref tmpRig, ref skinning);
            }
            else
            {
                skinning.Clear();
            }
        }
    }
}
