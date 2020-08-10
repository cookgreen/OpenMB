using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class CallScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "call";
			}
		}

		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Line;
			}
		}

		public CallScriptCommand()
		{
			commandArgs = new string[]
			{
				"CallFunctionName"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			if (commandArgs.Length > 0)
			{
				if (!commandArgs[0].StartsWith("%") && !commandArgs[0].StartsWith("$"))
				{
					ScriptFunction func = Context.GetFunction(commandArgs[0]);
					if (func != null)
					{
						func.Execute(executeArgs);
					}
					else
					{
						GameManager.Instance.log.LogMessage(string.Format("Function `{0}` didn't exist!", commandArgs[0]), LogMessage.LogType.Error);
					}
				}
			}
		}
	}
}
