using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public enum ScriptCommandType
	{
		None,
		Line,
		Block,
		Conditional,
		ConditionControl,
		ConditionTurining,
		End
	}
	public interface IScriptCommand
	{
		string CommandName { get; }
		ScriptCommandType CommandType { get; }
		string[] CommandArgs { get; }
		ScriptContext Context { get; set; }
		List<IScriptCommand> SubCommands { get; set; }
		void PushArg(string cmdArg, int index);
		void Execute(params object[] executeArgs);
	}
}
