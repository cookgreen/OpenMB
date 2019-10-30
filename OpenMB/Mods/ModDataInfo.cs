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
        public readonly string ItemTypes;
        public readonly string Sides;
        public readonly string Skin;
        public readonly string Maps;
        public readonly string WorldMaps;
        public readonly string Locations;
        public readonly string Skeletons;
        public readonly string SceneProps;
        public readonly string Models;
        public readonly string MapDir;
        public readonly string MusicDir;
        public readonly string ScriptDir;
		public readonly string Menus;

        public ModDataInfo(
            string characters,
            string sound,
            string music,
            string items,
            string itemTypes,
            string sides, 
            string skin,
            string maps,
            string worldmaps,
            string locations,
            string skeletons,
            string models,
            string sceneProps,
			string menus,
            string mapDir,
            string musicDir,
            string scriptDir)
        {
            Characters = characters;
            Sound = sound;
            Music = music;
            Items = items;
            ItemTypes = itemTypes;
            Sides = sides;
            Skin = skin;
            Maps = maps;
            WorldMaps = worldmaps;
            Locations = locations;
            Skeletons = skeletons;
            SceneProps = sceneProps;
            Models = models;
            MapDir = mapDir;
            MusicDir = musicDir;
            ScriptDir = scriptDir;
			Menus = menus;
        }
    }
}
