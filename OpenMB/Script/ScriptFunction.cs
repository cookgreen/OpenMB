using OpenMB.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script
{
	public class ScriptFunction
	{
		public string Name { get; set; }
		public List<IScriptCommand> Content { get; set; }

		public void Execute(params object[] extraArgs)
		{
			for (int i = 0; i < Content.Count; i++)
			{
				Content[i].Execute(extraArgs);
			}
		}
	}
}
