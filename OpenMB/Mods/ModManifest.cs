using OpenMB.Localization;
using OpenMB.Mods.XML;
using System;
using System.Collections.Generic;
using System.IO;
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
		public readonly string ID;

		public ModManifest(string path)
		{
			InstalledPath = path;
			path = path + "/module.xml";
			XmlObjectLoader loader = new XmlObjectLoader(path);
			ModXML xmldata;
			if (loader.Load<XML.ModXML>(out xmldata))
			{
				MetaData = new ModBaseInfo(InstalledPath,
										   xmldata.ModInfo.Name,
										   xmldata.ModInfo.Description,
										   xmldata.ModInfo.Version,
										   xmldata.ModInfo.Icon,
										   xmldata.ModInfo.Thumb,
										   xmldata.ModInfo.StartupBackground,
										   xmldata.ModInfo.Assemblies,
										   xmldata.ModInfo.DisplayInChooser);
				Data = new ModDataInfo(xmldata.Data.animationXml,
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
										xmldata.Data.cursorsXml,
										xmldata.Data.mapTemplatesXml,
										xmldata.Data.vehicleXml,
										xmldata.Data.DataDir.MapDir,
										xmldata.Data.DataDir.MusicDir,
										xmldata.Data.DataDir.ScriptDir);
				Media = xmldata.Media;

				Settings = xmldata.Settings.Settings;

				ID = (new DirectoryInfo(path)).Name;
			}
		}
	}
}
