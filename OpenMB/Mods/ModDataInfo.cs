using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods
{
    public class ModDataInfo
    {
        public readonly string Characters;
        public readonly string Sound;
        public readonly string Music;
        public readonly string Items;
        public readonly string Sides;
        public readonly string Skin;
        public readonly string Maps;
        public readonly string WorldMaps;
        public readonly string Locations;

        public ModDataInfo(
            string characters,
            string sound,
            string music,
            string items,
            string sides, 
            string skin,
            string maps,
            string worldmaps,
            string locations)
        {
            Characters = characters;
            Sound = sound;
            Music = music;
            Items = items;
            Sides = sides;
            Skin = skin;
            Maps = maps;
            WorldMaps = worldmaps;
            Locations = locations;
        }
    }
}
