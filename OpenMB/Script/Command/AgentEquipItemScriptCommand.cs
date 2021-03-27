using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class AgentEquipItemScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public AgentEquipItemScriptCommand()
		{
			commandArgs = new string[]
			{
				"AgentId",
				"ItemId"
			};
		}
		public override string CommandName
		{
			get
			{
				return "agent_equip_item";
			}
		}

		public override string[] CommandArgs
		{
			get
			{
				return base.CommandArgs;
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
			string agentId = CommandArgs[0].StartsWith("%") ? Context.GetLocalValue(CommandArgs[0].Substring(1)).ToString() : CommandArgs[0];
			string itemId = CommandArgs[1].StartsWith("%") ? Context.GetLocalValue(CommandArgs[1].Substring(1)).ToString() : CommandArgs[1];
			var world = executeArgs[0] as GameWorld;
			var itemXml = world.ModData.ItemInfos.Where(o => o.ID == itemId).First();
			if (itemXml != null)
			{
				var agent = world.GetAgentById(int.Parse(agentId));
				if (agent != null)
				{
					Item item = world.GetItemByXml(itemXml);
					agent.EquipWeapon(item);
					agent.EquipmentSystem.EquipNewItem(item);
				}
			}
		}
	}
}
