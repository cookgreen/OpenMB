using AMOFGameEngine.Game;
using AMOFGameEngine.Mods;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script
{
    public class SpawnScriptCommand : IScriptCommand
    {
        public SpawnScriptCommand()
        {
            CommandArgs = new object[] {
                "CharacterType",
                "CharacterName",
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
            string characterName = CommandArgs[1].ToString();
            string characterTeam = CommandArgs[2].ToString();
            string spawnX = CommandArgs[3].ToString();
            string spawnY = CommandArgs[4].ToString();
            string spawnZ = CommandArgs[5].ToString();

            GameWorld world = executeArgs[0] as GameWorld;
            var searchRet = world.ModData.CharacterInfos.Where(o => o.ID == characterName);
            if(searchRet.Count()>0)
            {
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
                    searchRet.First(),
                    new Vector3(float.Parse(spawnX),float.Parse(spawnY), float.Parse(spawnZ)), 
                    characterTeam,
                    isBot);
            }
        }

        public void PushArg(string cmdArg ,int index)
        {
            CommandArgs[index] = cmdArg;
        }
    }
}
