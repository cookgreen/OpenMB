using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script.Command
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
            int startVal = int.Parse(CommandArgs[0].ToString());
            int endVal = int.Parse(CommandArgs[1].ToString());
            int step = int.Parse(CommandArgs[2].ToString());
            for (int i = startVal; i < endVal; i = i + step)
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
