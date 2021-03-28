using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class RemoveScenePropScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "remove_scene_prop";
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

		public RemoveScenePropScriptCommand()
		{
			commandArgs = new string[]
			{
				"propInstanceID"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			world.RemoveSceneProp(
				getVariableValue(commandArgs[0]).ToString(),
				int.Parse(getVariableValue(commandArgs[1]).ToString())
			);
		}
	}
}
