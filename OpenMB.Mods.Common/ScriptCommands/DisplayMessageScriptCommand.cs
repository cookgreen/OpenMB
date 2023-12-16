using OpenMB.Core;
using OpenMB.Game;
using OpenMB.Screen;
using OpenMB.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods.Common.ScriptCommands
{
	public class DisplayMessageScriptCommand : ScriptCommand
	{
		private string[] commandArgs;

		public override string CommandName
		{
			get { return "display_message"; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public DisplayMessageScriptCommand()
		{
			commandArgs = new string[]
			{
				"outputStr",
				"colorCode(Optional)"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			var world = executeArgs[0] as GameWorld;
			string message = executeArgs[1] as string;
			if(executeArgs.Length == 3)
			{
				string colorCode = executeArgs[2] as string;
				OutputMessageManager.Instance.DisplayMessage(message, colorCode);
			}
			else
			{
				OutputMessageManager.Instance.DisplayMessage(message);
			}
		}
	}
}
