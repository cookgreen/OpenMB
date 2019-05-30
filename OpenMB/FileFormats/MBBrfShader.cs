using Mogre;
using OpenMB.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenMB.FileFormats
{
    public class MBBrfShaderOpt
    {
        int map;
        uint colorOp, alphaOp, flags;
        public void Load(BinaryReader reader)
        {
            map = MBUtil.LoadInt32(reader);
            colorOp = MBUtil.LoadUInt32(reader);
            alphaOp = MBUtil.LoadUInt32(reader);
            flags = MBUtil.LoadUInt32(reader);
        }
        public void Load(DataStreamPtr reader)
        {
            map = MBOgreUtil.LoadInt32(reader);
            colorOp = MBOgreUtil.LoadUInt32(reader);
            alphaOp = MBOgreUtil.LoadUInt32(reader);
            flags = MBOgreUtil.LoadUInt32(reader);
        }
    }

    public class MBBrfShader
    {
        public string name;
        public string technique;
        public string fallback;
        public uint flags;
        public uint requires;
        public List<MBBrfShaderOpt> opt;
        public void Load(BinaryReader reader)
        {
            name = MBUtil.LoadString(reader);
            flags = MBUtil.LoadUInt32(reader);
            requires = MBUtil.LoadUInt32(reader);
            technique = MBUtil.LoadString(reader);
            uint k = MBUtil.LoadUInt32(reader);
            if (k <= 1)
            {
                if (k == 1)
                {
                    fallback = MBUtil.LoadString(reader);
                }
                else
                {
                    fallback = null;
                }
            }
            k = MBUtil.LoadUInt32(reader);
            opt = new List<MBBrfShaderOpt>();
            for (int i = 0; i < k; i++)
            {
                MBBrfShaderOpt o = new MBBrfShaderOpt();
                o.Load(reader);
                opt.Add(o);
            }
        }
        public void Load(DataStreamPtr reader)
        {
            name = MBOgreUtil.LoadString(reader);
            flags = MBOgreUtil.LoadUInt32(reader);
            requires = MBOgreUtil.LoadUInt32(reader);
            technique = MBOgreUtil.LoadString(reader);
            uint k = MBOgreUtil.LoadUInt32(reader);
            if (k <= 1)
            {
                if (k == 1)
                {
                    fallback = MBOgreUtil.LoadString(reader);
                }
                else
                {
                    fallback = null;
                }
            }
            k = MBOgreUtil.LoadUInt32(reader);
            opt = new List<MBBrfShaderOpt>();
            for (int i = 0; i < k; i++)
            {
                MBBrfShaderOpt o = new MBBrfShaderOpt();
                o.Load(reader);
                opt.Add(o);
            }
        }
    }
}