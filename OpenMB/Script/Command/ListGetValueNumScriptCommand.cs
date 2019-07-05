using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
    class ListGetValueNumScriptCommand : ScriptCommand
    {
        public ListGetValueNumScriptCommand()
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
                return "list_get_num";
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
            if (list != null)
            {
                world.ChangeGobalValue("reg0", list.NextNodes.Count.ToString());
            }
            else
            {
                GameManager.Instance.log.LogMessage(string.Format("Couldn't find list with name `{0}`!", listVariable), LogMessage.LogType.Error);
            }
        }
    }
}
