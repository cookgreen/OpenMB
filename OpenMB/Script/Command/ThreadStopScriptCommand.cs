using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class ThreadStopScriptCommand : ScriptCommand
	{
		private string[] commandArgs;

		public override string CommandName
		{
			get { return "thread_stop"; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public ThreadStopScriptCommand()
        {
			//thread_stop %th_new
			commandArgs = new string[]
			{
				"thread variable"
			};
        }

        public override void Execute(params object[] executeArgs)
        {
			string threadVariableName = commandArgs[0];

			if (isValidVariableName(threadVariableName))
			{
				Thread th = null;
				if (isLocalVariable(threadVariableName))
				{
					string threadName = threadVariableName.Substring(1);
					th = Context.GetLocalValue(threadName) as Thread;
				}
				else if (isGlobalVariable(threadVariableName))
				{
					string threadName = threadVariableName.Substring(1);
					th = ScriptGlobalVariableMap.Instance.GetVariable(threadName) as Thread;
				}

				if (th != null)
				{
					th.Abort();
				}
			}
		}
    }
}
