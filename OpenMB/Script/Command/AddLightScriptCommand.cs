using OpenMB.Game;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	class AddLightScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "add_light";
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

		public AddLightScriptCommand()
		{
			commandArgs = new string[] {
				"AddType",
				"Name",
				"PositionVector",
				"DirectionVector"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			string type = commandArgs[0];
			string name = commandArgs[1];
			string posVector = commandArgs[2];
			string dirVector = commandArgs[3];

			GameWorld world = executeArgs[0] as GameWorld;
			var vectorPos = world.GlobalValueTable.GetRecord(posVector);
			var vectorDir = world.GlobalValueTable.GetRecord(dirVector);

			world.CreateLight(type, name,
				new Vector3(
						float.Parse(vectorPos.NextNodes[0].Value),
						float.Parse(vectorPos.NextNodes[1].Value),
						float.Parse(vectorPos.NextNodes[2].Value)
					),
				new Vector3(
						float.Parse(vectorDir.NextNodes[0].Value),
						float.Parse(vectorDir.NextNodes[1].Value),
						float.Parse(vectorDir.NextNodes[2].Value)
					));
		}
	}
}
