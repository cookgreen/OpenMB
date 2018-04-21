using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script.Command
{
    public class ScriptCommand : IScriptCommand
    {
        public virtual string[] CommandArgs
        {
            get
            {
                return new string[] { "placeholderargs" };
            }
        }

        public virtual string CommandName
        {
            get
            {
                return "placeholder";
            }
        }

        public virtual ScriptCommandType CommandType
        {
            get
            {
                return ScriptCommandType.None;
            }
        }

        public virtual List<IScriptCommand> SubCommands
        {
            get;
            set;
        }

        public ScriptCommand ParentCommand
        {
            get;
            set;
        }

        public ScriptContext Context
        {
            get;
            set;
        }

        public virtual void Execute(params object[] executeArgs)
        {

        }

        public virtual void PushArg(string cmdArg, int index)
        {
            CommandArgs[index] = cmdArg;
        }
    }
}
