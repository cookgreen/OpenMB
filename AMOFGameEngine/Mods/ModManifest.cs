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
        public readonly string InstalledPath;

        public ModManifest(string path)
        {
            InstalledPath = path;
            path = path + "/Module.xml";
            ModXMLLoader loader = new ModXMLLoader(path);
            XML.ModXML xmldata;
            if (loader.Load<XML.ModXML>(out xmldata))
            {
                MetaData = new ModBaseInfo(InstalledPath,
                                           xmldata.ModInfo.Name,
                                           xmldata.ModInfo.Description,
                                           xmldata.ModInfo.Version,
                                           xmldata.ModInfo.Thumb,
                                           xmldata.ModInfo.Movie);
                AssemblyName = xmldata.Assembly;
                Data = new ModDataInfo(xmldata.Data.characterXML,
                                        xmldata.Data.soundXML,
                                        xmldata.Data.musicXML,
                                        xmldata.Data.itemXML,
                                        xmldata.Data.sideXML);
                Media = xmldata.Media.ToArray();
                Scripts = xmldata.Scripts.ToArray();
                Maps = xmldata.Maps.ToArray();
            }
        }
    }
}
