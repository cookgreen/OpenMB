using OpenMB.Game;
using OpenMB.Mods.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class DataStoreIndexScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get { return "data_store_index"; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public DataStoreIndexScriptCommand()
		{
			commandArgs = new string[]
			{
				"dest Value",
				"data ID",
				"data Type"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			int dataType = int.Parse(getVariableValue(commandArgs[2]).ToString());
			int dataIndex = -1;
			object item = null;
			switch (dataType)
			{
				case 0://Animations
					item = world.ModData.AnimationInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.AnimationInfos.IndexOf(item as ModAnimationDfnXml);
					break;
				case 1://Characters
					item = world.ModData.CharacterInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.CharacterInfos.IndexOf(item as ModCharacterDfnXML);
					break;
				case 2://Cursors
					item = world.ModData.CursorInfos.Where(o => o.Name == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.CursorInfos.IndexOf(item as ModCursorDfnXml);
					break;
				case 3://Items
					item = world.ModData.ItemInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.ItemInfos.IndexOf(item as ModItemDfnXML);
					break;
				case 4://Item Types
					item = world.ModData.ItemTypeInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.ItemTypeInfos.IndexOf(item as ModItemTypeDfnXml);
					break;
				case 5://Locations
					item = world.ModData.LocationInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.LocationInfos.IndexOf(item as ModLocationDfnXml);
					break;
				case 6://Maps
					item = world.ModData.MapInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.MapInfos.IndexOf(item as ModMapDfnXml);
					break;
				case 7://Menus
					item = world.ModData.MenuInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.MenuInfos.IndexOf(item as ModMenuDfnXml);
					break;
				case 8://Models
					item = world.ModData.ModelInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.ModelInfos.IndexOf(item as ModModelDfnXml);
					break;
				case 9://Music
					item = world.ModData.MusicInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.MusicInfos.IndexOf(item as ModTrackDfnXML);
					break;
				case 10://Scene Props
					item = world.ModData.ScenePropInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.ScenePropInfos.IndexOf(item as ModScenePropDfnXml);
					break;
				case 11://Sides
					item = world.ModData.SideInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.SideInfos.IndexOf(item as ModSideDfnXML);
					break;
				case 12://Skeletons
					item = world.ModData.SkeletonInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.SkeletonInfos.IndexOf(item as ModSkeletonDfnXML);
					break;
				case 13://Skins
					item = world.ModData.SkinInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.SkinInfos.IndexOf(item as ModCharacterSkinDfnXML);
					break;
				case 14://Sounds
					item = world.ModData.SoundInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.SoundInfos.IndexOf(item as ModSoundDfnXML);
					break;
				case 15://Strings
					item = world.ModData.StringInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.StringInfos.IndexOf(item as ModStringDfnXml);
					break;
				case 16://UILayouts
					item = world.ModData.UILayoutInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.UILayoutInfos.IndexOf(item as ModUILayoutDfnXml);
					break;
				case 17://World Maps
					item = world.ModData.WorldMapInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.WorldMapInfos.IndexOf(item as ModWorldMapDfnXml);
					break;
				case 18://World Maps
					item = world.ModData.MapTemplateInfos.Where(o => o.ID == commandArgs[1]).FirstOrDefault();
					dataIndex = world.ModData.MapTemplateInfos.IndexOf(item as ModMapTemplateDfnXml);
					break;
			}
			if (commandArgs[0].StartsWith("%"))
			{
				Context.ChangeLocalValue(commandArgs[0].Substring(1), dataIndex.ToString());
			}
			else if (commandArgs[0].StartsWith("$"))
			{
				world.ChangeGobalValue(commandArgs[0].Substring(1), dataIndex.ToString());
			}
		}
	}
}
