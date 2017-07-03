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
        private string modName;
        private string modDescription;
        private List<Character> characterInfos;
        private List<Item> itemInfos;
        private List<GameSound> soundInfos;

        public List<GameSound> SoundInfos
        {
            get { return soundInfos; }
            set { soundInfos = value; }
        }
        public List<Item> ItemInfos
        {
            get { return itemInfos; }
            set { itemInfos = value; }
        }
        public List<Character> CharacterInfos
        {
            get { return characterInfos; }
            set { characterInfos = value; }
        }
        public string ModDescription
        {
            get { return modDescription; }
            set { modDescription = value; }
        }
        public string ModName
        {
            get { return modName; }
            set { modName = value; }
        }

        public ModData()
        {
            modName = "";
            modDescription = "";
            characterInfos = new List<Character>();
            itemInfos = new List<Item>();
            soundInfos = new List<GameSound>();
        }
    }
}
