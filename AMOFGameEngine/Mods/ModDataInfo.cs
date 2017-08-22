using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Mods
{
    public class ModDataInfo
    {
        public readonly string Characters;
        public readonly string Sound;
        public readonly string Music;
        public readonly string Items;
        public readonly string Sides;

        public ModDataInfo(string characters,string sound,string music,string items,string sides)
        {
            Characters = characters;
            Sound = sound;
            Music = music;
            Items = items;
            Sides = sides;
        }
    }
}
