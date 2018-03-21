using AMOFGameEngine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script.Command
{
    class AssignScriptCommand : IScriptCommand
    {
        private ScriptContext context;
        public AssignScriptCommand(ScriptContext context)
        {
            this.context = context;
        }
        public object[] CommandArgs
        {
            get;
        }

        public string CommandName
        {
            get
            {
                return "assign";
            }
        }

        public void Execute(params object[] executeArgs)
        {
            if (CommandArgs.Length == 2)
            {
                GameWorld world = executeArgs[0] as GameWorld;

                string varname = (string)CommandArgs[0];
                string varvalue = (string)CommandArgs[1];

                if(varname.StartsWith("%"))//local var
                {
                    context.ChangeValue(varname.Substring(1, varname.IndexOf(varname.Last())), varvalue);
                }
                else if(varname.StartsWith("$"))//global var
                {
                    world.ChangeValue(varname.Substring(1, varname.IndexOf(varname.Last())), varvalue);
                }
            }
            else
            {
                GameManager.Instance.mLog.LogMessage("[Script Error]: Assign: Invalid argument number");
            }
        }

        public void PushArg(string cmdArg, int index)
        {
            CommandArgs[index] = cmdArg;
        }
    }
}
