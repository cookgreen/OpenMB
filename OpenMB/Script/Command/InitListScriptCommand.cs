using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
    class InitListScriptCommand : ScriptCommand
    {
        public InitListScriptCommand()
        {
            commandArgs = new string[] {
                "ListVariable"
            };
        }
        private string[] commandArgs;
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
                return "init_list";
            }
        }

        public override ScriptCommandType CommandType
        {
            get
            {
                return ScriptCommandType.Line;
            }
        }

        public override void Execute(params object[] executeArgs)
        {
            GameWorld world = executeArgs[0] as GameWorld;
            string listVariable = CommandArgs[0].ToString();
            
            ScriptLinkTableNode list = Context.LocalTable.GetRecord(listVariable);
            if (list == null)
            {
                list = new ScriptLinkTableNode();
                Context.LocalTable.AddRecord(list);
            }
            else
            {
                GameManager.Instance.log.LogMessage(string.Format("The list variable with name `{0}` already existed!", listVariable), LogMessage.LogType.Warning);
            }
        }
    }
}
