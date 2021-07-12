using OpenMB.Game;
using OpenMB.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace OpenMB.Mods.Common.ScriptCommands
{
	/// <summary>
	/// Spawn a scene prop and make player can control it
	/// </summary>
	public class SpawnPlayerScenePropScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "spawn_player_scene_prop";
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

		public SpawnPlayerScenePropScriptCommand()
		{
			commandArgs = new string[]
			{
				"scenePropId",
				"position"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			string vectorName = getVariableValue(CommandArgs[1]).ToString();

			var vector = world.GlobalValueTable.GetRecord(vectorName);
			if (vector == null)
			{
				EngineManager.Instance.log.LogMessage("Invalid Vector Name!", LogMessage.LogType.Error);
				return;
			}
			world.CreatePlayerSceneProp(getVariableValue(CommandArgs[0]).ToString(),
				new Vector3()
				{
					x = float.Parse(vector.NextNodes[0].Value),
					y = float.Parse(vector.NextNodes[1].Value),
					z = float.Parse(vector.NextNodes[2].Value),
				});
		}
	}
}
