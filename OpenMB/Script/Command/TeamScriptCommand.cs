using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	class TeamScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public TeamScriptCommand()
		{
			commandArgs = new string[]
			{
				"Team1",
				"Team2",
				"Relation" //-1 - enemy; 0 - netrual; 1 - ally
            };
		}
		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public override string CommandName
		{
			get
			{
				return "team";
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Line;
			}
		}

		public override void Execute(params object[] executeArgs)
		{
			string team1Id = CommandArgs[0].ToString();
			string team2Id = CommandArgs[1].ToString();
			string relation = CommandArgs[2].ToString();

			GameWorld word = executeArgs[0] as GameWorld;
			word.ChangeTeamRelationship(team1Id, team2Id, int.Parse(relation));
		}
	}
}
