using Mogre;
using OpenMB.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenMB.FileFormats
{
	public class MBBrfMaterial
	{
		public string name;
		public uint flags;
		public string shader;
		public string diffuseA;
		public string diffuseB;
		public string bump;
		public string enviro;
		public string spec;
		public float specular;
		public float r, g, b;
		public void Load(BinaryReader reader)
		{
			name = MBUtil.LoadString(reader);
			flags = MBUtil.LoadUInt32(reader);
			shader = MBUtil.LoadString(reader);
			diffuseA = MBUtil.LoadString(reader);
			diffuseB = MBUtil.LoadString(reader);
			bump = MBUtil.LoadString(reader);
			enviro = MBUtil.LoadString(reader);
			spec = MBUtil.LoadStringMaybe(reader, "none");
			specular = MBUtil.LoadFloat(reader);
			r = MBUtil.LoadFloat(reader);
			g = MBUtil.LoadFloat(reader);
			b = MBUtil.LoadFloat(reader);
		}
		public void Load(DataStreamPtr reader)
		{
			name = MBOgreUtil.LoadString(reader);
			flags = MBOgreUtil.LoadUInt32(reader);
			shader = MBOgreUtil.LoadString(reader);
			diffuseA = MBOgreUtil.LoadString(reader);
			diffuseB = MBOgreUtil.LoadString(reader);
			bump = MBOgreUtil.LoadString(reader);
			enviro = MBOgreUtil.LoadString(reader);
			spec = MBOgreUtil.LoadStringMaybe(reader, "none");
			specular = MBOgreUtil.LoadFloat(reader);
			r = MBOgreUtil.LoadFloat(reader);
			g = MBOgreUtil.LoadFloat(reader);
			b = MBOgreUtil.LoadFloat(reader);
		}
	}
}