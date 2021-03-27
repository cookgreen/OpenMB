using OpenMB.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script
{
	public class ScriptFunction
	{
		private Dictionary<string, object> returnValueMap { get; set; }

		public string Name { get; set; }
		public List<IScriptCommand> Content { get; set; }

		public ScriptFunction()
        {
			returnValueMap = new Dictionary<string, object>();
        }

		public void Execute(params object[] extraArgs)
		{
			for (int i = 0; i < Content.Count; i++)
			{
				Content[i].Execute(extraArgs);
			}
		}

        public void SetReturnValue(string variableName, object variableValue)
        {
			returnValueMap[variableName] = variableValue;
        }
    }
}
