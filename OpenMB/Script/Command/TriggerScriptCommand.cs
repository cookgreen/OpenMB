using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class TriggerScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
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
				return "trigger";
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Block;
			}
		}

		public override List<IScriptCommand> SubCommands
		{
			get;
			set;
		}
		public TriggerScriptCommand()
		{
			commandArgs = new string[] {
				"TriggerName",
				"triggerCondition",
				"ExecuteTime",
				"FreezeTime"
			};
			SubCommands = new List<IScriptCommand>();
		}

		public override void Execute(params object[] executeArgs)
		{
			Context.RegisterTrigger(commandArgs[0], commandArgs[1], float.Parse(commandArgs[2]), float.Parse(commandArgs[3]), SubCommands);
		}
	}
}
