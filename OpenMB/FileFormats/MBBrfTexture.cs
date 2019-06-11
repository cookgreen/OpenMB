using Mogre;
using OpenMB.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenMB.FileFormats
{
    public class MBBrfTexture
    {
        public string name;
        public uint flags;
        public void Load(BinaryReader reader)
        {
            name = MBUtil.LoadString(reader);
            flags = MBUtil.LoadUInt32(reader);
        }
        public void Load(DataStreamPtr reader)
        {
            name = MBOgreUtil.LoadString(reader);
            flags = MBOgreUtil.LoadUInt32(reader);
        }

        public uint Frames
        {
            get
            {
                return ((flags >> 24) & 0xf) * 4;
            }
        }

        public bool IsAnimable
        {
            get
            {
                return Frames > 0;
            }
        }

        public string GetFrameName(int i)
        {
            return string.Format("{0}_{1}.dds", name, i);
        }

        public void SetDefault()
        {
            name = name + ".dds";
            flags = 0x00000000;
        }
    }
}