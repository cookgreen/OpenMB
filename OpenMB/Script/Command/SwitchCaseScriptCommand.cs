using System.Collections.Generic;

namespace OpenMB.Script.Command
{
	internal class SwitchCaseScriptCommand : ScriptCommand
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
				return "case";
			}
		}

		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public SwitchCaseScriptCommand()
		{
			commandArgs = new string[]
			{
				"variable value"
			};
			SubCommands = new List<IScriptCommand>();
		}

		public override void Execute(params object[] executeArgs)
		{
			foreach (var command in SubCommands)
			{
				command.Execute(executeArgs);
			}
		}
	}
}