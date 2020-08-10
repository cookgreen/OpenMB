using OpenMB.Mods.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods.Common.Flags.Music
{
	public class MusicFlagPlayVolume : IModFlag
	{
		public string Name
		{
			get { return "PlayVolume"; }
		}

		public void Enable(string value, params object[] param)
		{
		}
	}
}
