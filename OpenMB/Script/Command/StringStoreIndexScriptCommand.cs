using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class StringStoreIndexScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public override string CommandName
        {
            get { return "string_store_index"; }
        }

        public override ScriptCommandType CommandType
        {
            get { return ScriptCommandType.Line; }
        }

        public override string[] CommandArgs
        {
            get { return commandArgs; }
        }

        public StringStoreIndexScriptCommand()
        {
            commandArgs = new string[]
            {
                "dest Value",
                "string ID"
            };
        }

        public override void Execute(params object[] executeArgs)
        {
            GameWorld world = executeArgs[0] as GameWorld;
            var item = world.ModData.StringInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
            if (commandArgs[0].StartsWith("%"))
            {
                Context.ChangeLocalValue(commandArgs[0].Substring(1), world.ModData.StringInfos.IndexOf(item).ToString());
            }
            else if (commandArgs[0].StartsWith("$"))
            {
                world.ChangeGobalValue(commandArgs[0].Substring(1), world.ModData.StringInfos.IndexOf(item).ToString());
            }
        }
    }
}
