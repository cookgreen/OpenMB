using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class ConditionalCheckEqualScriptCommand : ConditionalCheckScriptCommand
    {
        public override string CommandName
        {
            get { return "eq"; }
        }

        private string[] commandArgs;
        public override ScriptCommandType CommandType
        {
            get { return ScriptCommandType.Line; }
        }

        public override string[] CommandArgs
        {
            get { return commandArgs; }
        }

        public ConditionalCheckEqualScriptCommand()
        {
            commandArgs = new string[]
            {
                "opt1",
                "opt2"
            };
        }

        public override void Execute(params object[] executeArgs)
        {

        }

        public override bool DoCheck(params object[] executeArgs)
        {
            var var1 = getVariableValue(commandArgs[0]);
            var var2 = getVariableValue(commandArgs[1]);

            return var1 == var2;
        }
    }
}
