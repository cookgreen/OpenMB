using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	class RemoveLightScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "remove_light";
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

		public RemoveLightScriptCommand()
		{
			commandArgs = new string[] {
				"Name"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			string name = commandArgs[0];

			GameWorld world = executeArgs[0] as GameWorld;
			world.RemoveLight(name);
		}
	}
}
