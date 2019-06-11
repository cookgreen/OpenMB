using Mogre;
using OpenMB.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenMB.FileFormats
{
    public class TmpCas3F {
        public Point3F rot;
        public int findex;

        public void Load(BinaryReader reader)
        {
            findex = MBUtil.LoadInt32(reader);
            rot = MBUtil.LoadPoint3F(reader);
        }
        public void Load(DataStreamPtr reader)
        {
            findex = MBOgreUtil.LoadInt32(reader);
            rot = MBOgreUtil.LoadPoint3F(reader);
        }
    }
    public class TmpCas4F
    {
        public Point4F rot;
        public int findex;
        
        public void Load(BinaryReader reader)
        {
            findex = MBUtil.LoadInt32(reader);
            rot = MBUtil.LoadPoint4F(reader);
        }
        public void Load(DataStreamPtr reader)
        {
            findex = MBOgreUtil.LoadInt32(reader);
            rot = MBOgreUtil.LoadPoint4F(reader);
        }
    }

    public class TmpBone4
    {
        public List<TmpCas4F> casList;

        public void Load(BinaryReader reader)
        {
            uint k = MBUtil.LoadUInt32(reader);
            casList = new List<TmpCas4F>();
            for (int i = 0; i < k; i++)
            {
                TmpCas4F cas = new TmpCas4F();
                cas.Load(reader);
                casList.Add(cas);
            }
        }
        public void Load(DataStreamPtr reader)
        {
            uint k = MBOgreUtil.LoadUInt32(reader);
            casList = new List<TmpCas4F>();
            for (int i = 0; i < k; i++)
            {
                TmpCas4F cas = new TmpCas4F();
                cas.Load(reader);
                casList.Add(cas);
            }
        }
    }
    public class MBBrfAnimationFrame
    {
        public int index;
        public List<Point4F> rot;
        public Point3F tra;
        public List<bool> wasImplicit;
    }
    public class MBBrfAnimation
    {
        private string name;
        private int nbones;
        private List<MBBrfAnimationFrame> frames;

        public void Load(BinaryReader reader)
        {
            name = MBUtil.LoadString(reader);
            List<TmpBone4> tmpBone4v = new List<TmpBone4>();
            uint k = MBUtil.LoadUInt32(reader);
            for (int i = 0; i < k; i++)
            {
                TmpBone4 tmpBone = new TmpBone4();
                tmpBone.Load(reader);
                tmpBone4v.Add(tmpBone);
            }
            nbones = (int)k;

            List<TmpCas3F> tmpCas3f = new List<TmpCas3F>();
            k = MBUtil.LoadUInt32(reader);
            for (int i = 0; i < k; i++)
            {
                TmpCas3F tmpcas3 = new TmpCas3F();
                tmpcas3.Load(reader);
                tmpCas3f.Add(tmpcas3);
            }

            MBUtil.TmpBone2BrfFrame(tmpBone4v, tmpCas3f, out frames);
        }
        public void Load(DataStreamPtr reader)
        {
            name = MBOgreUtil.LoadString(reader);
            List<TmpBone4> tmpBone4v = new List<TmpBone4>();
            uint k = MBOgreUtil.LoadUInt32(reader);
            for (int i = 0; i < k; i++)
            {
                TmpBone4 tmpBone = new TmpBone4();
                tmpBone.Load(reader);
                tmpBone4v.Add(tmpBone);
            }
            nbones = (int)k;

            List<TmpCas3F> tmpCas3f = new List<TmpCas3F>();
            k = MBOgreUtil.LoadUInt32(reader);
            for (int i = 0; i < k; i++)
            {
                TmpCas3F tmpcas3 = new TmpCas3F();
                tmpcas3.Load(reader);
                tmpCas3f.Add(tmpcas3);
            }

            MBUtil.TmpBone2BrfFrame(tmpBone4v, tmpCas3f, out frames);
        }
    }
}
