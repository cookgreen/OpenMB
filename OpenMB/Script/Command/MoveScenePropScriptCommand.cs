using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class MoveScenePropScriptCommand : ScriptCommand
	{
		private string[] commandArgs;

		public override string CommandName
		{
			get
			{
				return "move_scene_prop";
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

		public MoveScenePropScriptCommand()
		{
			commandArgs = new string[]
			{
				"propInstanceID",
				"axis",
				"movement"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			world.MoveSceneProp(
				getVariableValue(commandArgs[0]).ToString(),
				int.Parse(getVariableValue(commandArgs[1]).ToString()),
				getVariableValue(commandArgs[2]).ToString(),
				getVariableValue(commandArgs[3]).ToString()
			);
		}
	}
}
