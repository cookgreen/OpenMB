using AMOFGameEngine.Game;
using AMOFGameEngine.Mods;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script.Command
{
    public class SpawnScriptCommand : ScriptCommand
    {
        public SpawnScriptCommand()
        {
            CommandArgs = new string[] {
                "CharacterType",
                "CharacterID",
                "CharacterTeam",
                "SpawnX",
                "SpawnY",
                "SpawnZ"
            };
        }
        public override string[] CommandArgs
        {
            get;
        }

        public override string CommandName
        {
            get
            {
                return "spawn";
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
            string characterType = CommandArgs[0].StartsWith("%") ? Context.GetLocalValue(CommandArgs[0].Substring(1)): CommandArgs[0];
            string characterID = CommandArgs[1].StartsWith("%") ? Context.GetLocalValue(CommandArgs[1].Substring(1)) : CommandArgs[1];
            string characterTeam = CommandArgs[2].StartsWith("%") ? Context.GetLocalValue(CommandArgs[2].Substring(1)) : CommandArgs[2];
            string spawnX = CommandArgs[3].StartsWith("%") ? Context.GetLocalValue(CommandArgs[3].Substring(1)) : CommandArgs[3];
            string spawnY = CommandArgs[4].StartsWith("%") ? Context.GetLocalValue(CommandArgs[4].Substring(1)) : CommandArgs[4];
            string spawnZ = CommandArgs[5].StartsWith("%") ? Context.GetLocalValue(CommandArgs[5].Substring(1)) : CommandArgs[5];

            GameWorld world = executeArgs[0] as GameWorld;
            bool isBot = false;
            if(characterType == "player")
            {
                isBot = false;
            }
            else if(characterType == "bot")
            {
                isBot = true;
            }

            world.SpawnNewCharacter(
                characterID,
                new Vector3(float.Parse(spawnX),float.Parse(spawnY), float.Parse(spawnZ)), 
                characterTeam,
                isBot);
        }
    }
}
