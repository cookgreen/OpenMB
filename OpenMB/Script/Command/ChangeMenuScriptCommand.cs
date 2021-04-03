using OpenMB.Game;
using OpenMB.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	class ChangeMenuScriptCommand : ScriptCommand
	{
		private string[] commandArgs;

		public override string CommandName
		{
			get { return "switch_menu"; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public ChangeMenuScriptCommand()
		{
			commandArgs = new string[]
			{
				"menuID"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			var world = executeArgs[0] as GameWorld;
			ScreenManager.Instance.ChangeScreen(
				"GameMenu",
				true,
				executeArgs[0] as GameWorld,
				getVariableValue(commandArgs[0]));
		}
	}
}
