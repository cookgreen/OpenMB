using OpenMB.Game;
using OpenMB.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class StringStoreStrByIndexScriptCommand : ScriptCommand
    {
        private string[] commandArgs;

        public override string CommandName
        {
            get { return "string_store_str_by_index"; }
        }

        public override ScriptCommandType CommandType
        {
            get { return ScriptCommandType.Line; }
        }

        public override string[] CommandArgs
        {
            get { return commandArgs; }
        }

        public StringStoreStrByIndexScriptCommand()
        {
            commandArgs = new string[]
            {
                "dest value",
                "string index"
            };
        }

        public override void Execute(params object[] executeArgs)
        {
            GameWorld world = executeArgs[0] as GameWorld;
            int stringIndex = int.Parse(getParamterValue(commandArgs[1]));
            if (commandArgs[0].StartsWith("%"))
            {
                Context.ChangeLocalValue(commandArgs[0].Substring(1),
                    LocateSystem.Instance.GetLocalizedString(
                        world.ModData.StringInfos[stringIndex].ID,
                        world.ModData.StringInfos[stringIndex].Content));
            }
            else if (commandArgs[0].StartsWith("$"))
            {
                world.ChangeGobalValue(commandArgs[0].Substring(1),
                    LocateSystem.Instance.GetLocalizedString(
                        world.ModData.StringInfos[stringIndex].ID,
                        world.ModData.StringInfos[stringIndex].Content));
            }
        }
    }
}
