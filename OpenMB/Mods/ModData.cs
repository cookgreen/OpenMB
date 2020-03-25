using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.Game;
using OpenMB.Mods.XML;
using OpenMB.Sound;
using OpenMB.Game.ItemTypes;
using OpenMB.Map;
using System.Reflection;

namespace OpenMB.Mods
{
    public class ModData
    {
		private List<Assembly> assemblies;
        private ModBaseInfo modBasicInfo;
		private List<ModAnimationDfnXml> animationInfos;
		private List<ModCharacterDfnXML> characterInfos;
        private List<ModItemDfnXML> itemInfos;
        private List<ModTrackDfnXML> musicInfos;
        private List<ModSoundDfnXML> soundInfos;
        private List<ModSideDfnXML> sideInfos;
        private List<ModCharacterSkinDfnXML> skinInfos;
        private List<ModMapDfnXml> mapInfos;
        private List<ModWorldMapDfnXml> worldMapInfos;
        private List<ModLocationDfnXml> locationInfos;
        private List<ModSkeletonDfnXML> skeletonInfos;
		private List<ModMenuDfnXml> menuInfos;
		private List<ModUILayoutDfnXml> uiLayoutInfos;
		private List<ModStringDfnXml> stringInfos;
		private List<ModCursorDfnXml> cursorInfos;
		private List<ModMapTemplateDfnXml> mapTemplateInfos;
        private List<ModVehicleDfnXml> vehicleInfos;
        private List<ModMediaData> modMediaData;

		public bool HasSinglePlayer { get; set; }
        public bool HasMultiplater { get; set; }
        public bool HasCredit { get; set; }
        public bool HasSavedGame { get; set; }

        public List<IModModelType> ModModelTypes { get; set; }
        public List<IModSetting> ModSettings { get; set; }
        public List<IModTriggerCondition> ModTriggerConditions { get; set; }
        public List<IGameMapLoader> MapLoaders { get; set; }

        public string MusicDir { get; set; }
        public string MapDir { get; set; }
        public string ScriptDir { get; set; }

        public ModBaseInfo BasicInfo
        {
            get { return modBasicInfo; }
            set { modBasicInfo = value; }
		}
		public List<ModAnimationDfnXml> AnimationInfos
		{
			get { return animationInfos; }
			set { animationInfos = value; }
		}
		public List<ModSideDfnXML> SideInfos
        {
            get { return sideInfos; }
            set { sideInfos = value; }
        }
        public List<ModTrackDfnXML> MusicInfos
        {
            get { return musicInfos; }
            set { musicInfos = value; }
        }
        public List<ModItemDfnXML> ItemInfos
        {
            get { return itemInfos; }
            set { itemInfos = value; }
        }
        public List<ModCharacterDfnXML> CharacterInfos
        {
            get { return characterInfos; }
            set { characterInfos = value; }
        }

        public List<ModSoundDfnXML> SoundInfos
        {
            get { return soundInfos; }
            set { soundInfos = value; }
        }

        public List<ModCharacterSkinDfnXML> SkinInfos
        {
            get { return skinInfos; }
            set { skinInfos = value; }
        }

        public List<ModMapDfnXml> MapInfos
        {
            get{ return mapInfos; }
            set{ mapInfos = value; }
        }

        public List<ModWorldMapDfnXml> WorldMapInfos
        {
            get { return worldMapInfos; }
            set { worldMapInfos = value; }
        }

        internal List<ModLocationDfnXml> LocationInfos
        {
            get { return locationInfos; }
            set { locationInfos = value; }
		}
		public List<ModMenuDfnXml> MenuInfos
		{
			get { return menuInfos; }
			set { menuInfos = value; }
		}
		public List<ModSkeletonDfnXML> SkeletonInfos
        {
            get { return skeletonInfos; }
            set { skeletonInfos = value; }
		}
		public List<ModStringDfnXml> StringInfos
		{
			get { return stringInfos; }
			set { stringInfos = value; }
		}
		public List<ModUILayoutDfnXml> UILayoutInfos
		{
			get { return uiLayoutInfos; }
			set { uiLayoutInfos = value; }
		}
		public List<ModCursorDfnXml> CursorInfos
		{
			get { return cursorInfos; }
			set { cursorInfos = value; }
		}
		public List<ModMapTemplateDfnXml> MapTemplateInfos
		{
			get { return mapTemplateInfos; }
			set { mapTemplateInfos = value; }
        }

        public List<ModVehicleDfnXml> VehicleInfos
        {
            get { return vehicleInfos; }
            set { vehicleInfos = value; }
        }

        public List<ModItemTypeDfnXml> ItemTypeInfos { get; set; }

        public List<ModScenePropDfnXml> ScenePropInfos { get; set; }
        public List<ModModelDfnXml> ModelInfos { get; set; }
        public List<IItemType> ItemTypes { get; set; }

        public List<ModMediaData> ModMediaData { get { return modMediaData; } }

		public List<IModStartupBackgroundType> StartupBackgroundTypes { get; set; }
		public List<Assembly> Assemblies
		{
			get
			{
				return assemblies;
			}
		}

		public ModData()
        {
            characterInfos = new List<ModCharacterDfnXML>();
            itemInfos = new List<ModItemDfnXML>();
            musicInfos = new List<ModTrackDfnXML>();
            sideInfos = new List<ModSideDfnXML>();
            soundInfos = new List<ModSoundDfnXML>();
            skinInfos = new List<ModCharacterSkinDfnXML>();
            mapInfos = new List<ModMapDfnXml>();
            worldMapInfos = new List<ModWorldMapDfnXml>();
            locationInfos = new List<ModLocationDfnXml>();
            SkeletonInfos = new List<ModSkeletonDfnXML>();
            ItemTypeInfos = new List<ModItemTypeDfnXml>();
			menuInfos = new List<ModMenuDfnXml>();
			modMediaData = new List<ModMediaData>();
			cursorInfos = new List<ModCursorDfnXml>();
            vehicleInfos = new List<ModVehicleDfnXml>();

			HasSinglePlayer = true;
            HasMultiplater = true;
            HasCredit = true;
            HasSavedGame = true;

            ModModelTypes = new List<IModModelType>();
            ModSettings = new List<IModSetting>();
            ModTriggerConditions = new List<IModTriggerCondition>();
            ItemTypes = new List<IItemType>();
            MapLoaders = new List<IGameMapLoader>();
			StartupBackgroundTypes = new List<IModStartupBackgroundType>();

			assemblies = new List<Assembly>();
		}

        public ModItemTypeDfnXml FindItemType(string itemType)
        {
            foreach (var itemTypeDefine in ItemTypeInfos)
            {
                if (itemTypeDefine.ID == itemType)
                {
                    return itemTypeDefine;
                }
                else
                {
                    return FindSubItemType(itemTypeDefine, itemType);
                }
            }
            return null;
        }


        ModItemTypeDfnXml FindSubItemType(ModItemTypeDfnXml itemDefineType, string itemType)
        {
            foreach (var subItemType in itemDefineType.SubTypes)
            {
                if (subItemType.ID == itemType)
                {
                    return subItemType;
                }
                else
                {
                    return FindSubItemType(subItemType, itemType);
                }
            }
            return null;
        }
    }

    public class ModMediaData
    {
        private string mediaName;
        private ResourceType mediaType;
        private string fullMediaPath;

        public ResourceType MediaType
        {
            get
            {
                return mediaType;
            }
        }

        public string FullMediaPath
        {
            get
            {
                return fullMediaPath;
            }
        }

        public string MediaName
        {
            get
            {
                return mediaName;
            }
        }

        public ModMediaData(string mediaName, string fullMediaPath, ResourceType mediaType)
        {
            this.mediaName = mediaName;
            this.fullMediaPath = fullMediaPath;
            this.mediaType = mediaType;
        }
    }
}
