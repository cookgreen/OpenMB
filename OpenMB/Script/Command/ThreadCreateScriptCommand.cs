using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class ThreadCreateScriptCommand : ScriptCommand
	{
		private string[] commandArgs;

        public override string CommandName
		{
			get { return "thread_create"; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public ThreadCreateScriptCommand()
		{
			//thread_create %th_new somefunc
			commandArgs = new string[]
			{
				"thread value",
				"thread func name"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			string threadVariableName = commandArgs[0];
			string functionName = getVariableValue(commandArgs[1]).ToString();

			if (isValidVariableName(threadVariableName))
			{
				Thread newThread = new Thread(new ParameterizedThreadStart(ThreadExecFunc));

				if (isLocalVariable(threadVariableName))
				{
					string threadName = threadVariableName.Substring(1);
					Context.ChangeLocalValue(threadName, newThread);
				}
				else if (isGlobalVariable(threadVariableName))
				{
					string threadName = threadVariableName.Substring(1);
					ScriptGlobalVariableMap.Instance.ChangeGobalValue(threadName, newThread);
				}
				object[] arr = new object[]
				{
					functionName,
					executeArgs
				};
				newThread.Start(arr);
			}
		}

		private void ThreadExecFunc(object args)
        {
			object[] arr = args as object[];
			string functionName = arr[0].ToString();
			object[] executeArgs = arr[1] as object[];

			var func = Context.GetFunction(functionName);
			func.Execute(executeArgs);
        }
	}
}
