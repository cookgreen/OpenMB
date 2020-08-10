using OpenMB.Game;
using OpenMB.Trigger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class HookTriggerEventScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "hook_trigger_event";
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

		public HookTriggerEventScriptCommand()
		{
			commandArgs = new string[] {
				"trggerEventName",
				"hookedFunctionName"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;

			string triggerEvent = getParamterValue(commandArgs[0], world);
			string hookedFunction = getParamterValue(commandArgs[1], world);

			TriggerManager.Instance.HookTriggerFunction(triggerEvent, hookedFunction);
		}
	}
}
