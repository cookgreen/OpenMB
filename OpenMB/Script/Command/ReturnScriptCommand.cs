using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    /// <summary>
    /// Use with function
    /// </summary>
    public class ReturnScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public override string CommandName { get { return "return"; } }
        public override string[] CommandArgs { get { return commandArgs; } }

        public ReturnScriptCommand()
        {
            commandArgs = new string[]
            {
                "ReturnVariable"
            };
        }

        public override void Execute(params object[] executeArgs)
        {
            var world = executeArgs[0] as GameWorld;
            var value = getParamterValue(commandArgs[0]);
            if (ParentCommand is FunctionScriptCommand)// inside a function
            {
                Context.SetReturnValue((ParentCommand as FunctionScriptCommand).CommandName, value);
            }
        }
    }
}
