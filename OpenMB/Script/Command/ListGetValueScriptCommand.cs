using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	class ListGetValueScriptCommand : ScriptCommand
	{
		public ListGetValueScriptCommand()
		{
			commandArgs = new string[] {
				"ListVariable",
				"index"
			};
		}
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
				return "list_get_value";
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
			GameWorld world = executeArgs[0] as GameWorld;
			string listVariable = CommandArgs[0].ToString();
			string strIndex = CommandArgs[1].ToString();
			int index = -1;
			if (!int.TryParse(strIndex, out index))
			{
				EngineManager.Instance.log.LogMessage(string.Format("Invalid List index value: `{0}`!", strIndex), LogMessage.LogType.Error);
				return;
			}

			ScriptLinkTableNode list = Context.LocalTable.GetRecord(listVariable);
			if (list != null)
			{
				if (index >= 0)
				{
					if (index < list.NextNodes.Count)
					{
						world.ChangeGobalValue("reg0", list.NextNodes[index].Value);
					}
					else
					{
						EngineManager.Instance.log.LogMessage(string.Format("Invalid List index value: `{0}`!", strIndex), LogMessage.LogType.Error);
					}
				}
				else
				{
					EngineManager.Instance.log.LogMessage(string.Format("Invalid List index value: `{0}`!", strIndex), LogMessage.LogType.Error);
				}
			}
			else
			{
				EngineManager.Instance.log.LogMessage(string.Format("Couldn't find list with name `{0}`!", listVariable), LogMessage.LogType.Error);
			}
		}
	}
}
