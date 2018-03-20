using AMOFGameEngine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script
{
    class TeamScriptCommand : IScriptCommand
    {
        public TeamScriptCommand()
        {
            CommandArgs = new object[]
            {
                "Team1",
                "Team2",
                "Relation" //-1 - enemy; 0 - netrual; 1 - ally
            };
        }
        public object[] CommandArgs
        {
            get;
        }

        public string CommandName
        {
            get
            {
                return "team";
            }
        }

        public void Execute(params object[] executeArgs)
        {
            string team1Id = CommandArgs[0].ToString();
            string team2Id = CommandArgs[1].ToString();
            string relation = CommandArgs[2].ToString();

            GameWorld word = executeArgs[0] as GameWorld;
            word.ChangeTeamRelationship(team1Id, team2Id, int.Parse(relation));
        }

        public void PushArg(string cmdArg, int index)
        {
            CommandArgs[index] = cmdArg;
        }
    }
}
