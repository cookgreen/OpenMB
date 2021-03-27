using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class GetScenePropScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "get_scene_prop";
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

		public GetScenePropScriptCommand()
		{
			commandArgs = new string[]
			{
				"scenePropID",
				"scenePropInstanceNo"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			world.ChangeGobalValue("reg0",
				world.GetSceneProp(
					getParamterValue(commandArgs[0]).ToString(),
					getParamterValue(commandArgs[1]).ToString()
				)
			);
		}
	}
}
