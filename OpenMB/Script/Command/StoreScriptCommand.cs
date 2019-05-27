using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
    public class StoreScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        private ScriptContext context;
        public StoreScriptCommand(ScriptContext context)
        {
            this.context = context;
            commandArgs = null;
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
                return "store";
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
            if(CommandArgs.Length == 2)
            {
                GameWorld world = executeArgs[0] as GameWorld;
                string destVar = (string)CommandArgs[0];
                string srcVar = (string)CommandArgs[1];
                if (destVar.StartsWith("%"))//local var
                {
                    if (srcVar.StartsWith("%"))
                    {
                        context.ChangeLocalValue(destVar.Substring(1, destVar.IndexOf(destVar.Last())), context.GetLocalValue(srcVar.Substring(1, srcVar.IndexOf(srcVar.Last()))));
                    }
                    else if (srcVar.StartsWith("$"))
                    {
                        context.ChangeLocalValue(destVar.Substring(1, destVar.IndexOf(destVar.Last())), world.GetGlobalValue(srcVar.Substring(1, srcVar.IndexOf(srcVar.Last()))));
                    }
                }
                else if (destVar.StartsWith("$"))//global var
                {
                    if (srcVar.StartsWith("%"))
                    {
                        world.ChangeGobalValue(destVar.Substring(1, destVar.IndexOf(destVar.Last())), context.GetLocalValue(srcVar.Substring(1, srcVar.IndexOf(srcVar.Last()))));
                    }
                    else if (srcVar.StartsWith("$"))
                    {
                        world.ChangeGobalValue(destVar.Substring(1, destVar.IndexOf(destVar.Last())), world.GetGlobalValue(srcVar.Substring(1, srcVar.IndexOf(srcVar.Last()))));
                    }
                }
            }
        }
    }
}
