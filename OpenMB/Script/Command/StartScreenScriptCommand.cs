using OpenMB.Game;
using OpenMB.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class StartScreenScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "start_screen";
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Line;
			}
		}

		public StartScreenScriptCommand()
		{
			commandArgs = new string[]
			{
				"screen name"
			};
		}

		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;

			if (commandArgs.Length == 0)
			{
				GameManager.Instance.log.LogMessage("Missing parameter for `start_screen` script command");
				return;
			}

			ScreenManager.Instance.ChangeScreen(getParamterValue(commandArgs[0], world), true, executeArgs[0] as GameWorld);
		}
	}
}
