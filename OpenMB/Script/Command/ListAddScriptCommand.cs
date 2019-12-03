using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class ListAddScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get { return "list_add"; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public ListAddScriptCommand()
		{
			commandArgs = new string[]
			{
				"ListVariable",
				"Item"
			};
		}
		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			string listVariable = CommandArgs[0].ToString();
			string strValue = CommandArgs[1].ToString();
			ScriptLinkTableNode list = Context.LocalTable.GetRecord(listVariable);
			if (list != null)
			{
				list.NextNodes.Add(new ScriptLinkTableNode()
				{
					Value = strValue
				});
			}
		}
	}
}
