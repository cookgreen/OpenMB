using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class GetReturnValueScriptCommand : ScriptCommand
    {
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "get_return_value";
			}
		}

		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Line;
			}
		}

		public GetReturnValueScriptCommand()
		{
			commandArgs = new string[] {
				"FunctionName",
				"VariableName",
				"ReceiveValueVariable"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			string funcName = getVariableValue(commandArgs[0]).ToString();
			string variableName = getVariableValue(commandArgs[1]).ToString();
			string destVariableName = commandArgs[2];

			var func = Context.GetFunction(funcName);
			var variableValue = func.GetReturnValue(variableName);
			if (isValidVariableName(destVariableName))
			{
				setVariableValue(destVariableName, variableValue);
			}
		}
	}
}
