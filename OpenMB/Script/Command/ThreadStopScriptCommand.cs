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
			string threadName = commandArgs[0].Substring(1);
			var th = Context.GetLocalValue(threadName) as Thread;
			th.Abort();
		}
    }
}
