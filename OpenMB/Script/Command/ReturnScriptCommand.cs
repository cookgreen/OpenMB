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
        private const string ReturnCommandArgDesc = "ReturnVariable (Optional)";
        private string[] commandArgs;
        public override string CommandName { get { return "return"; } }
        public override string[] CommandArgs { get { return commandArgs; } }

        public ReturnScriptCommand()
        {
            commandArgs = new string[]
            {
                ReturnCommandArgDesc
            };
        }

        public override void Execute(params object[] executeArgs)
        {
            if (commandArgs[0] != ReturnCommandArgDesc)
            {
                var variableName = commandArgs[0].Substring(1);
                var variableValue = getVariableValue(commandArgs[0]);
                if (ParentCommand is FunctionScriptCommand)// inside a function
                {
                    var funcName = (ParentCommand as FunctionScriptCommand).CommandArgs[0];
                    var func = Context.GetFunction(funcName);
                    func.SetReturnValue(variableName, variableValue);
                }
            }
        }
    }
}
