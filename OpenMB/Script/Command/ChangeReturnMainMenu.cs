using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class ChangeReturnMainMenu : ScriptCommand
	{
		private string[] commandArgs;

		public override string CommandName
		{
			get { return "change_return_main_menu"; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public ChangeReturnMainMenu()
		{
			commandArgs = new string[0] { };
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			world.ChangeGameState("MainMenu");
		}
	}
}
