using OpenMB.Game;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class SpawnScenePropScriptCommand : ScriptCommand
	{
		private string[] commandArgs;

		public override string CommandName
		{
			get
			{
				return "spawn_scene_prop";
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

		public SpawnScenePropScriptCommand()
		{
			commandArgs = new string[]
			{
				"scenePropID",
				"Position Vector",
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			string scenePropID = getParamterValue(commandArgs[0]);
			string vectorName = getParamterValue(commandArgs[1]);
			var vector = world.GlobalValueTable.GetRecord(vectorName);

			var propInstanceID = world.CreateSceneProp(
				scenePropID,
				new Vector3(
					float.Parse(vector.NextNodes[0].Value),
					float.Parse(vector.NextNodes[1].Value),
					float.Parse(vector.NextNodes[2].Value)));
			if (!string.IsNullOrEmpty(propInstanceID))
			{
				world.ChangeGobalValue("reg0", propInstanceID); //Store into the reg0
			}
		}
	}
}
