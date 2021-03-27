using OpenMB.Core;
using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class SlotSetScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "set_slot";
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

		public SlotSetScriptCommand()
		{
			commandArgs = new string[]
			{
				"objectID",
				"slotID",
				"slotValue"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;

			string objectID = getParamterValue(commandArgs[0]).ToString();
			string slotID = getParamterValue(commandArgs[1]).ToString();
			string slotValue = getParamterValue(commandArgs[2]).ToString();
			GameSlotManager.Instance.SetSlot(objectID, slotID, slotValue);
		}
	}
}
