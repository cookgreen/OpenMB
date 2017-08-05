using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Mods
{
    public class ModDataInfo
    {
        public readonly string Character;
        public readonly string Sound;
        public readonly string Music;
        public readonly string Item;

        public ModDataInfo(string character,string sound,string music,string item)
        {
            Character = character;
            Sound = sound;
            Music = music;
            Item = item;
        }
    }
}
