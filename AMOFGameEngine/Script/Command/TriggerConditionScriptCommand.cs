using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script.Command
{
    public class TriggerConditionScriptCommand : ScriptCommand
    {
        public override string CommandName
        {
            get
            {
                return "condition";
            }
        }

        public override ScriptCommandType CommandType
        {
            get
            {
                return ScriptCommandType.Block;
            }
        }

        public override string[] CommandArgs
        {
            get
            {
                return null;
            }
        }

        public override void Execute(params object[] executeArgs)
        {

        }
    }
}
