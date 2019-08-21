using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
    public class UIScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public override ScriptCommandType CommandType
        {
            get
            {
                return ScriptCommandType.Block;
            }
        }

        public override string CommandName
        {
            get
            {
                return "ui";
            }
        }

        public override string[] CommandArgs
        {
            get
            {
                return commandArgs;
            }
        }

        public UIScriptCommand()
        {
            commandArgs = new string[]
            {
                "ui name"
            };
        }

        public override void Execute(params object[] executeArgs)
        {
            base.Execute(executeArgs);
        }
    }
}
