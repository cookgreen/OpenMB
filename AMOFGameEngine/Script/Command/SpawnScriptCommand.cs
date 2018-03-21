using AMOFGameEngine.Game;
using AMOFGameEngine.Mods;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script.Command
{
    public class SpawnScriptCommand : IScriptCommand
    {
        public SpawnScriptCommand()
        {
            CommandArgs = new object[] {
                "CharacterType",
                "CharacterID",
                "CharacterTeam",
                "SpawnX",
                "SpawnY",
                "SpawnZ"
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
                return "spawn";
            }
        }

        public void Execute(params object[] executeArgs)
        {
            string characterType = CommandArgs[0].ToString();
            string characterID = CommandArgs[1].ToString();
            string characterTeam = CommandArgs[2].ToString();
            string spawnX = CommandArgs[3].ToString();
            string spawnY = CommandArgs[4].ToString();
            string spawnZ = CommandArgs[5].ToString();

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

        public void PushArg(string cmdArg ,int index)
        {
            CommandArgs[index] = cmdArg;
        }
    }
}
