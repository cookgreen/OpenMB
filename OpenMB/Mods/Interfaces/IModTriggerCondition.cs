using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods
{
	public interface IModTriggerCondition
	{
		string Name { get; }
		bool CheckCondition(params object[] param);
	}
}
