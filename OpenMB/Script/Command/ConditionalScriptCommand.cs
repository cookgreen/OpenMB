using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class ConditionalScriptCommand : ScriptCommand
    {
        private string[] commandArgs; 
        public override ScriptCommandType CommandType
        {
            get { return ScriptCommandType.Block; }
        }

        public override string[] CommandArgs
        {
            get { return commandArgs; }
        }

        public string ConditionStr { get { return CommandArgs[0]; } }

        public bool CheckCondition() 
        {
            //TODO: Check the conditional expression
            return true; 
        }

        public ConditionalScriptCommand()
        {
            commandArgs = new string[]
            {
                "condition statement"
            };
        }

        public override void Execute(params object[] executeArgs)
        {
            foreach(var command in SubCommands)
            {
                command.Execute(executeArgs);
            }
        }
    }
    public class ConditionalNoStatementScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public override ScriptCommandType CommandType
        {
            get { return ScriptCommandType.Block; }
        }

        public override string[] CommandArgs
        {
            get { return commandArgs; }
        }

        public ConditionalNoStatementScriptCommand()
        {
            commandArgs = new string[] { };
        }

        public override void Execute(params object[] executeArgs)
        {
            foreach (var command in SubCommands)
            {
                command.Execute(executeArgs);
            }
        }
    }
}
