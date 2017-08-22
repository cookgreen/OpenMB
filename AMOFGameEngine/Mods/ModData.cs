using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.RPG;
using AMOFGameEngine.Sound;

namespace AMOFGameEngine.Mods
{
    public class ModData
    {
        private ModBaseInfo modBasicInfo;
        private List<XML.ModCharacterDfnXML> characterInfos;
        private List<XML.ModItemDfnXML> itemInfos;
        private List<XML.ModMusicDfnXML> musicInfos;
        private List<XML.ModSideDfnXML> sideInfos;
        private List<XML.ModMapDfnXML> mapInfos;

        public ModBaseInfo BasicInfo
        {
            get { return modBasicInfo; }
            set { modBasicInfo = value; }
        }
        public List<XML.ModMapDfnXML> MapInfos
        {
            get { return mapInfos; }
            set { mapInfos = value; }
        }
        public List<XML.ModSideDfnXML> SideInfos
        {
            get { return sideInfos; }
            set { sideInfos = value; }
        }
        public List<XML.ModMusicDfnXML> MusicInfos
        {
            get { return musicInfos; }
            set { musicInfos = value; }
        }
        public List<XML.ModItemDfnXML> ItemInfos
        {
            get { return itemInfos; }
            set { itemInfos = value; }
        }
        public List<XML.ModCharacterDfnXML> CharacterInfos
        {
            get { return characterInfos; }
            set { characterInfos = value; }
        }

        public ModData()
        {
            characterInfos = new List<XML.ModCharacterDfnXML>();
            itemInfos = new List<XML.ModItemDfnXML>();
            musicInfos = new List<XML.ModMusicDfnXML>();
            sideInfos = new List<XML.ModSideDfnXML>();
            mapInfos = new List<XML.ModMapDfnXML>();
        }
    }
}
