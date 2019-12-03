using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class ValueAddScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get { return "value_add"; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public ValueAddScriptCommand()
		{
			commandArgs = new string[]
			{
				"dest value",
				"add value"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			string variable = commandArgs[0];
			int ret = int.Parse(getParamterValue(variable, world)) + int.Parse(getParamterValue(commandArgs[1], world));
			if (variable.StartsWith("%"))
			{
				Context.ChangeLocalValue(variable.Substring(1), ret.ToString());
			}
			else if (variable.StartsWith("$"))
			{
				world.ChangeGobalValue(variable.Substring(1), ret.ToString());
			}
		}
	}
}
