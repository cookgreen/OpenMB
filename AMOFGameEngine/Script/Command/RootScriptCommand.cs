using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script.Command
{
    public class RootScriptCommand : ScriptCommand
    {
        public RootScriptCommand()
        {
            SubCommands = new List<IScriptCommand>();
        }

        public override ScriptCommandType CommandType
        {
            get
            {
                return ScriptCommandType.Block;
            }
        }

        public override void Execute(params object[] executeArgs)
        {
            int cmdNum = SubCommands.Count;
            for (int i = 0; i < cmdNum; i++)
            {
                SubCommands[i].Execute(executeArgs);
            }
        }
    }
}
