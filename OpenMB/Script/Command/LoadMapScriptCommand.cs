using OpenMB.Game;
using OpenMB.Mods;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
    public class LoadMapScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public LoadMapScriptCommand()
        {
            commandArgs = new string[] {
                "mapName"
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
            string mapName = getParamterValue(CommandArgs[0]);

            GameWorld world = executeArgs[0] as GameWorld;
            world.ChangeScene(mapName+".xml");
        }
    }
}
