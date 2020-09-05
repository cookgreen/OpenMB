using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenMB.Script.Command
{
    public class IFScriptCommand : ConditionalScriptCommand
    {
        public override string CommandName { get { return "if"; } }

        public override void Execute(params object[] executeArgs)
        {
            //Check the condition, if success, the execute `if` block
            //If failed, then search the SubCommands, find the `elseif` block and check one by one
            //If no matched the `elseif` block, the find the `else` block and execute
            if (CheckCondition())
            {
                var commands = SubCommands.Where(o => !(o is ElseIfScriptCommand) && !(o is ElseScriptCommand));
                foreach (var command in commands)
                {
                    command.Execute(executeArgs);
                }
            }
            else
            {
                var elseIfCommands = SubCommands.Where(o => o is ElseIfScriptCommand);
                foreach (var command in elseIfCommands)
                {
                    if ((command as ConditionalScriptCommand).CheckCondition())
                    {
                        command.Execute(executeArgs);
                        return;
                    }
                }

                var elseCommand = SubCommands.Where(o => o is ElseScriptCommand).FirstOrDefault();
                if (elseCommand != null)
                {
                    elseCommand.Execute(executeArgs);
                }
            }
        }
    }
    public class ElseIfScriptCommand : ConditionalScriptCommand
    {
        public override string CommandName { get { return "elseif"; } }
    }
    public class ElseScriptCommand : ConditionalNoStatementScriptCommand
    {
        public override string CommandName { get { return "else"; } }
    }
}
