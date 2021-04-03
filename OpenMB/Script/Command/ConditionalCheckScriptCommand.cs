using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class ConditionalCheckScriptCommand : ScriptCommand
    {
        public virtual bool DoCheck(params object[] executeArgs)
        {
            return true;
        }
    }
}
