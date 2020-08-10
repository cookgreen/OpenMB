using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using OpenMB.FileFormats;

namespace OpenMB.Connector
{
	public class MBOgreBrf
	{
		private string brfFile;
		private MBBrf brf;

		public MBBrf Brf
		{
			get
			{
				return brf;
			}
		}

		public string FileName { get { return brfFile; } }

		public MBOgreBrf(string brfFile)
		{
			this.brfFile = brfFile;
		}

		public void ReadFromSpecificGroup(string groupName)
		{
			DataStreamPtr stream = ResourceGroupManager.Singleton.OpenResource(brfFile, groupName);
			brf = new MBBrf(brfFile, stream);
		}

		public void ReadFromFileSystem(string fullPath)
		{
			brf = new MBBrf(brfFile, fullPath);
		}
	}
}
