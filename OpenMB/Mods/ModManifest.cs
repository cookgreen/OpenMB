using OpenMB.Mods.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods
{
    public class ModManifest
    {
        public readonly ModBaseInfo MetaData;
        public readonly ModDataInfo Data;
        public readonly ModMediaXml Media;
        public readonly List<ModSettingDfnXml> Settings;
        public readonly string InstalledPath;

        public ModManifest(string path)
        {
            InstalledPath = path;
            path = path + "/module.xml";
            ModXmlLoader loader = new ModXmlLoader(path);
            ModXML xmldata;
            if (loader.Load<XML.ModXML>(out xmldata))
            {
                MetaData = new ModBaseInfo(InstalledPath,
                                           xmldata.ModInfo.Name,
                                           xmldata.ModInfo.Description,
                                           xmldata.ModInfo.Version,
                                           xmldata.ModInfo.Thumb,
                                           xmldata.ModInfo.StartupBackground,
                                           xmldata.ModInfo.Assemblies,
                                           xmldata.ModInfo.DisplayInChooser);
                Data = new ModDataInfo( xmldata.Data.animationXml,
										xmldata.Data.characterXML,
                                        xmldata.Data.soundXML,
                                        xmldata.Data.musicXML,
                                        xmldata.Data.itemXML,
                                        xmldata.Data.itemTypeXML,
                                        xmldata.Data.sideXML,
                                        xmldata.Data.skinXML,
                                        xmldata.Data.mapsXml,
                                        xmldata.Data.worldMapsXML,
                                        xmldata.Data.locationsXML,
                                        xmldata.Data.skeletonsXML,
                                        xmldata.Data.modelsXml,
                                        xmldata.Data.scenePropsXML,
										xmldata.Data.menusXml,
										xmldata.Data.stringsXml,
										xmldata.Data.uiLayoutsXml,
                                        xmldata.Data.DataDir.MapDir,
                                        xmldata.Data.DataDir.MusicDir,
                                        xmldata.Data.DataDir.ScriptDir);
                Media = xmldata.Media;

                Settings = xmldata.Settings.Settings;
            }
        }
    }
}
