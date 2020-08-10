using Mogre;
using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class SpawnPlayerCharacterScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "spawn_player_character";
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

		public SpawnPlayerCharacterScriptCommand()
		{
			commandArgs = new string[]
			{
				"characterID",
				"Position",
				"teamId"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			var vectorName = getParamterValue(commandArgs[1], world);
			var vector = world.GlobalValueTable.GetRecord(vectorName);
			if (vector == null)
			{
				return;
			}
			world.CreatePlayer(getParamterValue(commandArgs[0], world), new Vector3()
			{
				x = float.Parse(vector.NextNodes[0].Value),
				y = float.Parse(vector.NextNodes[1].Value),
				z = float.Parse(vector.NextNodes[2].Value),
			}, getParamterValue(commandArgs[2], world));
		}
	}
}
