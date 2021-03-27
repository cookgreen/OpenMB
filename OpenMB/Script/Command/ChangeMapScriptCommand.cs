using OpenMB.Game;
using OpenMB.Mods;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class ChangeMapScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public ChangeMapScriptCommand()
		{
			commandArgs = new string[] {
				"map Name",
				"map Template Name",
				"side list"
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
				return "change_map";
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
			string mapID = getParamterValue(commandArgs[0]).ToString();
			string mapTemplateID = getParamterValue(commandArgs[1]).ToString();
			string listVariable = commandArgs[2];
			var list = Context.LocalTable.GetRecord(listVariable.Substring(1));
			List<string> items = new List<string>();
			foreach (var value in list.NextNodes)
			{
				items.Add(value.Value);
			}
			world.ChangeScene(mapID, mapTemplateID, items);
		}
	}
}
