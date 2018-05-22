using AMOFGameEngine.Game;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script.Command
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
                "posX",
                "posY",
                "posZ",
                "directionX",
                "directionY",
                "directionZ"
            };
        }

        public override void Execute(params object[] executeArgs)
        {
            string type = commandArgs[0];
            string name = commandArgs[1];
            float posX = float.Parse(commandArgs[2]);
            float posY = float.Parse(commandArgs[3]);
            float posZ = float.Parse(commandArgs[4]);
            float dirX = float.Parse(commandArgs[5]);
            float dirY = float.Parse(commandArgs[6]);
            float dirZ = float.Parse(commandArgs[7]);

            GameWorld world = executeArgs[0] as GameWorld;
            world.CreateLight(type, name, new Vector3(posX, posY, posZ), new Vector3(dirX, dirY, dirZ));
        }
    }
}
