using OpenMB.Game;
using OpenMB.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class ChangeUIScriptCommand : ScriptCommand
	{
		private string[] commandArgs;

		public override string CommandName
		{
			get { return "change_ui"; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public ChangeUIScriptCommand()
		{
			commandArgs = new string[]
			{
				"UILayout ID (String)",
				"Is Inner Screen (Bool)"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;

			var uiLayoutID = getVariableValue(commandArgs[0]);
			bool isInnerScreen = bool.Parse(getVariableValue(commandArgs[1]).ToString());
			if (!isInnerScreen)
			{
				ScreenManager.Instance.ChangeScreen("ScriptedScreen", true, executeArgs[0], uiLayoutID);
			}
			else
            {
                ScreenManager.Instance.ChangeScreen(uiLayoutID.ToString(), true);
            }
		}
	}
}
