using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.Game;
using OpenMB.Mods.XML;
using OpenMB.Sound;

namespace OpenMB.Mods
{
    public class ModData
    {
        private ModBaseInfo modBasicInfo;
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

        public string MusicDir { get; set; }
        public string MapDir { get; set; }
        public string ScriptDir { get; set; }

        public ModBaseInfo BasicInfo
        {
            get { return modBasicInfo; }
            set { modBasicInfo = value; }
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

        public List<ModSkeletonDfnXML> SkeletonInfos
        {
            get { return skeletonInfos; }
            set { skeletonInfos = value; }
        }

        public List<ModItemTypeDfnXml> ItemTypeInfos { get; set; }

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
        }
    }
}
