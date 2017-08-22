using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mogre;
using System.Reflection;
using AMOFGameEngine.Utilities;
using AMOFGameEngine.Mods;

namespace AMOFGameEngine.Mods
{
    using Mods = Dictionary<string, ModManifest>;

    public class ModManager
    {
        private Dictionary<string, ModManifest> InstalledMods;
        private string modInstallRootDir;
        private OgreConfigFileAdapter ofa;
        private List<OgreConfigNode> modConfigData;
        private ModData currentMod;

        public static ModManager Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModManager();
                }
                return instance;
            }
        }
        static ModManager instance;

        public ModManager()
        {
            InstalledMods = new Dictionary<string, ModManifest>();
            currentMod = null;
            modConfigData = new List<OgreConfigNode>();
            ofa = new OgreConfigFileAdapter("Mods.cfg");
        }

        string GetModInstallRootDir()
        {
            modConfigData = ofa.ReadConfigData();
            modInstallRootDir= modConfigData.FirstOrDefault(o => o.Section == "").Settings.FirstOrDefault(o => o.Key == "ModDir").Value;
            return modInstallRootDir;
        }

        public Mods GetInstalledMods()
        {
            GetModInstallRootDir();

            DirectoryInfo d = new DirectoryInfo(modInstallRootDir);

            FileSystemInfo[] modDirs = d.GetFileSystemInfos();

            foreach (var dir in modDirs)
            {
                if (File.Exists(string.Format("{0}/Module.xml", dir.FullName)))
                {
                    ModManifest manifest = new ModManifest(dir.FullName);
                    InstalledMods.Add(manifest.MetaData.Name, manifest);
                }
            }

            return InstalledMods;
        }

        public ModData LoadMod(string name)
        {
            ModData data = null;
            try
            {
                if (InstalledMods == null || InstalledMods.Count <= 0)
                {
                    return data;
                }
                data = new ModData();
                ModManifest manifest = InstalledMods.Where(o => o.Key == name).SingleOrDefault().Value;

                data.BasicInfo = manifest.MetaData;
                ModXMLLoader loader = new ModXMLLoader(manifest.InstalledPath + "/" + manifest.Data.Characters);
                XML.ModCharactersDfnXML characterDfn;
                loader.Load<XML.ModCharactersDfnXML>(out characterDfn);
                data.CharacterInfos = characterDfn.CharacterDfns;
                loader = new ModXMLLoader(manifest.InstalledPath + "/" + manifest.Data.Items);
                XML.ModItemsDfnXML itemDfn;
                loader.Load<XML.ModItemsDfnXML>(out itemDfn);
                data.ItemInfos = itemDfn.Items;
                loader=new ModXMLLoader(manifest.InstalledPath+"/"+manifest.Data.Sides);
                XML.ModSidesDfnXML sideDfn;
                loader.Load<XML.ModSidesDfnXML>(out sideDfn);
                data.SideInfos = sideDfn.Sides;

                return data;
            }
            catch
            {
                return data;
            }
        }
    }
}