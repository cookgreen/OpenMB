using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script.Command
{
    class FunctionScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public override string CommandName
        {
            get
            {
                return "function";
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
                return ScriptCommandType.Block;
            }
        }

        public FunctionScriptCommand()
        {
            commandArgs = new string[] {
                "FunctionName"
            };
            SubCommands = new List<IScriptCommand>();
        }

        public override void Execute(params object[] executeArgs)
        {
            Context.RegisterFunction(commandArgs[0], SubCommands);
        }
    }
}
