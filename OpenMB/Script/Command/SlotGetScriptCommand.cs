using OpenMB.Core;
using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class SlotGetScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "get_slot";
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Line;
			}
		}

		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public SlotGetScriptCommand()
		{
			commandArgs = new string[]
			{
				"slotValue",
				"objectID",
				"slotID"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;

			string variable = commandArgs[0];
			string objectID = getParamterValue(commandArgs[1], world);
			string slotID = getParamterValue(commandArgs[2], world);
			string slotValue = GameSlotManager.Instance.GetSlot(objectID, slotID);
			if (variable.StartsWith("%"))
			{
				Context.ChangeLocalValue(variable.Substring(1), slotValue);
			}
			else if (variable.StartsWith("$"))
			{
				world.ChangeGobalValue(variable.Substring(1), slotValue);
			}
		}
	}
}
