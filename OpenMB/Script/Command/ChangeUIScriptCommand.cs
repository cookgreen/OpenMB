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
				"UILayout ID"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			var uiLayoutID = getParamterValue(commandArgs[0]);
			ScreenManager.Instance.ChangeScreen("ScriptedScreen", true, executeArgs[0], uiLayoutID);
		}
	}
}
