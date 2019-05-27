using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
    public class TriggerExecuteSrciptCommand : ScriptCommand
    {
        public override string CommandName
        {
            get
            {
                return "execute";
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
