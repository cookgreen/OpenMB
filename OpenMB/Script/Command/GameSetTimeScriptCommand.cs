using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class GameSetTimeScriptCommand : ScriptCommand
    {
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "game_set_time";
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

		public GameSetTimeScriptCommand()
		{
			commandArgs = new string[] {
				"year",
				"month",
				"day",
				"hour",
				"minute",
				"second"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			int year = int.Parse(getVariableValue(commandArgs[0]).ToString());
			int month = int.Parse(getVariableValue(commandArgs[1]).ToString());
			int day = int.Parse(getVariableValue(commandArgs[2]).ToString());
			int hour = int.Parse(getVariableValue(commandArgs[3]).ToString());
			int minute = int.Parse(getVariableValue(commandArgs[4]).ToString());
			int second = int.Parse(getVariableValue(commandArgs[5]).ToString());

			GameTimeManager.Instance.Init(year, month, day, hour, minute, second);
		}
	}
}
