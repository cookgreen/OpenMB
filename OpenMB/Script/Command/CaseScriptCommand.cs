using System.Collections.Generic;

namespace OpenMB.Script.Command
{
	internal class CaseScriptCommand : ScriptCommand
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

		public CaseScriptCommand()
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