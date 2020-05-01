using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods
{
	public interface IModStartupBackgroundType
	{
		string Name { get; }

		void StartBackground(string value, params object[] param);
	}
}
