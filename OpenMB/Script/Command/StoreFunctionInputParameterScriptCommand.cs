using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class StoreFunctionInputParameterScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public StoreFunctionInputParameterScriptCommand()
		{
			commandArgs = new string[2] {
				"variable",
				"index"
			};
		}
		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public override string CommandName
		{
			get
			{
				return "store_input_param";
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Line;
			}
		}

		public override void Execute(params object[] executeArgs)
		{
			if (CommandArgs.Length == 2)
			{
				GameWorld world = executeArgs[0] as GameWorld;
				string destVar = (string)CommandArgs[0];
				int parameterIndex = int.Parse(CommandArgs[1]);
				var paramter = executeArgs[parameterIndex + 1].ToString();
				if (destVar.StartsWith("%"))//local var
				{
					Context.ChangeLocalValue(destVar.Substring(1, destVar.IndexOf(destVar.Last())), paramter);
				}
				else if (destVar.StartsWith("$"))//global var
				{
					world.ChangeGobalValue(destVar.Substring(1, destVar.IndexOf(destVar.Last())), paramter);
				}
			}
		}
	}
}
