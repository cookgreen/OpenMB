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
        }
        public object[] CommandArgs
        {
            get
            {
                return new object[5]
                    {
                        "CharacterType",
                        "CharacterName",
                        "SpawnX",
                        "SpawnY",
                        "SpawnZ"
                    };
            }
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
            if(CommandArgs.Length == CommandArgs.Length)
            {
                string characterType = CommandArgs[0].ToString();
                string characterName = CommandArgs[1].ToString();
                string spawnX = CommandArgs[2].ToString();
                string spawnY = CommandArgs[3].ToString();
                string spawnZ = CommandArgs[4].ToString();

                GameWorld world = executeArgs[0] as GameWorld;
                var searchRet = world.GetModData().CharacterInfos.Where(o => o.Name == characterName);
                if(searchRet.Count()>0)
                {
                    world.SpawnNewCharacter(searchRet.First(),new Vector3(float.Parse(spawnX),float.Parse(spawnY), float.Parse(spawnZ)));
                }
            }
        }

        public void PushArg(string cmdArg ,int index)
        {
            CommandArgs[index] = cmdArg;
        }
    }
}
