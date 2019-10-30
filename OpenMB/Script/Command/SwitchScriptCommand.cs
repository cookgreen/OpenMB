using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class SwitchScriptCommand : ScriptCommand
	{
		private string[] commandArgs;

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Block;
			}
		}

		public override string CommandName
		{
			get
			{
				return "switch";
			}
		}

		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public SwitchScriptCommand()
		{
			commandArgs = new string[]
			{
				"variable"
			};
			SubCommands = new List<IScriptCommand>();
		}

		public override void Execute(params object[] executeArgs)
		{
			var value = getParamterValue(commandArgs[0]);
			foreach (var command in SubCommands)
			{
				if (command.GetType() == typeof(CaseScriptCommand) &&
					(command as CaseScriptCommand).CommandArgs[0] == value)
				{
					command.Execute(executeArgs);
					return;
				}
			}
		}
	}
}
