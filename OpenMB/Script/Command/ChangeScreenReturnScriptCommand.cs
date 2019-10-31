using OpenMB.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class ChangeScreenReturnScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "change_screen_return";
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

		public ChangeScreenReturnScriptCommand()
		{
			commandArgs = new string[0] { };
		}

		public override void Execute(params object[] executeArgs)
		{
			ScreenManager.Instance.ChangeScreenReturn();
		}
	}
}
