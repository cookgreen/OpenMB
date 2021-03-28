using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	class DataStoreIdByIndexScriptCommand : ScriptCommand
	{
		private string[] commandArgs;

		public override string CommandName
		{
			get { return "data_store_id_by_index"; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public DataStoreIdByIndexScriptCommand()
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
					value = world.ModData.CharacterInfos[dataIndex].ID;
					break;
				case 2://Cursors
					value = world.ModData.CursorInfos[dataIndex].Name;
					break;
				case 3://Items
					value = world.ModData.ItemInfos[dataIndex].ID;
					break;
				case 4://Item Types
					value = world.ModData.ItemTypeInfos[dataIndex].ID;
					break;
				case 5://Locations
					value = world.ModData.LocationInfos[dataIndex].ID;
					break;
				case 6://Maps
					value = world.ModData.MapInfos[dataIndex].ID;
					break;
				case 7://Menus
					value = world.ModData.MenuInfos[dataIndex].ID;
					break;
				case 8://Models
					value = world.ModData.ModelInfos[dataIndex].ID;
					break;
				case 9://Music
					value = world.ModData.MusicInfos[dataIndex].ID;
					break;
				case 10://Scene Props
					value = world.ModData.ScenePropInfos[dataIndex].ID;
					break;
				case 11://Sides
					value = world.ModData.SideInfos[dataIndex].ID;
					break;
				case 12://Skeletons
					value = world.ModData.SkeletonInfos[dataIndex].ID;
					break;
				case 13://Skins
					value = world.ModData.SkinInfos[dataIndex].ID;
					break;
				case 14://Sounds
					value = world.ModData.SoundInfos[dataIndex].ID;
					break;
				case 15://Strings
					value = world.ModData.StringInfos[dataIndex].ID;
					break;
				case 16://UILayouts
					value = world.ModData.UILayoutInfos[dataIndex].ID;
					break;
				case 17://World Maps
					value = world.ModData.WorldMapInfos[dataIndex].ID;
					break;
				case 18://Map Templates
					value = world.ModData.MapTemplateInfos[dataIndex].ID;
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
