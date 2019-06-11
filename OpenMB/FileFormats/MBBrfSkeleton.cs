using Mogre;
using OpenMB.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenMB.FileFormats
{
    public class MBBrfBone
    {
        private string name;
        private Point3F x, y, z, t;
        private int attach, b;
        private List<int> next;
        public void Load(BinaryReader reader)
        {
            attach = MBUtil.LoadInt32(reader);
            if (!string.IsNullOrEmpty(MBUtil.LoadStringMaybe(reader, "bone")))
            {
                b = MBUtil.LoadInt32(reader);
            }
            x = MBUtil.LoadPoint3F(reader);
            y = MBUtil.LoadPoint3F(reader);
            z = MBUtil.LoadPoint3F(reader);
            t = MBUtil.LoadPoint3F(reader);
        }

        public void Load(DataStreamPtr reader)
        {
            attach = MBOgreUtil.LoadInt32(reader);
            if (!string.IsNullOrEmpty(MBOgreUtil.LoadStringMaybe(reader, "bone")))
            {
                b = MBOgreUtil.LoadInt32(reader);
            }
            x = MBOgreUtil.LoadPoint3F(reader);
            y = MBOgreUtil.LoadPoint3F(reader);
            z = MBOgreUtil.LoadPoint3F(reader);
            t = MBOgreUtil.LoadPoint3F(reader);
        }
    }
    public class MBBrfSkeleton
    {
        private string name;
        private List<MBBrfBone> bones;
        private int root;

        /// <summary>
        /// Build Skeleton
        /// </summary>
        public void BuildTree()
        {

        }

        public void Load(BinaryReader reader)
        {
            name = MBUtil.LoadString(reader);

            uint k;
            k = MBUtil.LoadUInt32(reader);
            bones = new List<MBBrfBone>();
            for (int i = 0; i < k; i++)
            {
                MBBrfBone bone = new MBBrfBone();
                bone.Load(reader);
            }
        }

        public void Load(DataStreamPtr reader)
        {
            name = MBOgreUtil.LoadString(reader);

            uint k;
            k = MBOgreUtil.LoadUInt32(reader);
            bones = new List<MBBrfBone>();
            for (int i = 0; i < k; i++)
            {
                MBBrfBone bone = new MBBrfBone();
                bone.Load(reader);
                bones.Add(bone);
            }
        }
    }
}
