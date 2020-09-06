using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenMB.Script.Expression;

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

        public bool CheckCondition(params object[] exeArgs) 
        {
            ConditionalExpression conditionExpr = new ConditionalExpression(ConditionStr);
            return conditionExpr.Execute(exeArgs); 
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
