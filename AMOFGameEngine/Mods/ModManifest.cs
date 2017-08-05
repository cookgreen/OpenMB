using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Mods
{
    public class ModManifest
    {
        public readonly ModBaseInfo MetaData;
        public readonly string AssemblyName;
        public readonly ModDataInfo Data;
        public readonly string[] Media;
        public readonly string[] Scripts;
        public readonly string[] Maps;

        public ModManifest(string path)
        {
            ModXMLLoader loader = new ModXMLLoader(path);
            if (loader.Load())
            {
                MetaData = new ModBaseInfo(loader.ModXMLData.ModInfo.Name,
                                           loader.ModXMLData.ModInfo.Description,
                                           loader.ModXMLData.ModInfo.Version,
                                           loader.ModXMLData.ModInfo.Thumb);
                AssemblyName = loader.ModXMLData.Assembly;
                Data = new ModDataInfo(loader.ModXMLData.Data.characterXML,
                                        loader.ModXMLData.Data.soundXML,
                                        loader.ModXMLData.Data.musicXML,
                                        loader.ModXMLData.Data.itemXML);
                Media = loader.ModXMLData.Media.ToArray();
                Scripts = loader.ModXMLData.Scripts.ToArray();
                Maps = loader.ModXMLData.Maps.ToArray();
            }
        }
    }
}
