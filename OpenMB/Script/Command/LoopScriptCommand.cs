using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class LoopScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public LoopScriptCommand()
		{
			SubCommands = new List<IScriptCommand>();
			commandArgs = new string[]
			{
				"StartVal",
				"EndVal",
				"Step"
			};
		}
		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public override string CommandName
		{
			get
			{
				return "loop";
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Block;
			}
		}

		public override List<IScriptCommand> SubCommands
		{
			get;
			set;
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			int startVal = int.Parse(getParamterValue(commandArgs[0]).ToString());
			int endVal = int.Parse(getParamterValue(commandArgs[1]).ToString());
			int step = int.Parse(getParamterValue(commandArgs[2]).ToString());
			for (int i = startVal; i < endVal; i += step)
			{
				Context.ChangeLocalValue("current", i.ToString());
				int cmdNum = SubCommands.Count;
				for (int j = 0; j < cmdNum; j++)
				{
					SubCommands[j].Execute(executeArgs);
				}
			}
		}
	}
}
