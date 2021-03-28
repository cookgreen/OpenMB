using OpenMB.Game;
using OpenMB.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class DataStoreValueByIndexScriptCommand : ScriptCommand
	{
		private string[] commandArgs;

		public override string CommandName
		{
			get { return "data_store_value_by_index"; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public DataStoreValueByIndexScriptCommand()
		{
			commandArgs = new string[]
			{
				"dest value",
				"data index",
				"data type"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			int dataIndex = int.Parse(getVariableValue(commandArgs[1]).ToString());
			int dataType = int.Parse(getVariableValue(commandArgs[2]).ToString());
			string value = null;
			switch (dataType)
			{
				case 0://Animations
					break;
				case 1://Characters
					value = LocateSystem.Instance.GetLocalizedString(
							world.ModData.CharacterInfos[dataIndex].ID,
							world.ModData.CharacterInfos[dataIndex].Name);
					break;
				case 2://Cursors
					break;
				case 3://Items
					value = LocateSystem.Instance.GetLocalizedString(
							world.ModData.ItemInfos[dataIndex].ID,
							world.ModData.ItemInfos[dataIndex].Name);
					break;
				case 4://Item Types
					value = LocateSystem.Instance.GetLocalizedString(
							world.ModData.ItemTypeInfos[dataIndex].ID,
							world.ModData.ItemTypeInfos[dataIndex].Name);
					break;
				case 5://Locations
					value = LocateSystem.Instance.GetLocalizedString(
							world.ModData.LocationInfos[dataIndex].ID,
							world.ModData.LocationInfos[dataIndex].Name);
					break;
				case 6://Maps
					break;
				case 7://Menus
					break;
				case 8://Models
					break;
				case 9://Music
					break;
				case 10://Scene Props
					break;
				case 11://Sides
					value = LocateSystem.Instance.GetLocalizedString(
							world.ModData.SideInfos[dataIndex].ID,
							world.ModData.SideInfos[dataIndex].Name);
					break;
				case 12://Skeletons
					break;
				case 13://Skins
					break;
				case 14://Sounds
					break;
				case 15://Strings
					value = LocateSystem.Instance.GetLocalizedString(
							world.ModData.StringInfos[dataIndex].ID,
							world.ModData.StringInfos[dataIndex].Content);
					break;
				case 16://UILayouts
					break;
				case 17://World Maps
					break;
				case 18://Map Templates
					break;
			}
			if (commandArgs[0].StartsWith("%"))
			{
				Context.ChangeLocalValue(commandArgs[0].Substring(1), value);
			}
			else if (commandArgs[0].StartsWith("$"))
			{
				world.ChangeGobalValue(commandArgs[0].Substring(1), value);
			}
		}
	}
}
