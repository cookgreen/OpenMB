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
    }
}