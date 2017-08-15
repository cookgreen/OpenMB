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
            ofa = new OgreConfigFileAdapter("Mod.cfg");
        }

        string GetModInstallRootDir()
        {
            modConfigData = ofa.ReadConfigData();
            modInstallRootDir= modConfigData.FirstOrDefault(o => o.Section == "").Settings.FirstOrDefault(o => o.Key == "ModDir").Value;
            return modInstallRootDir;
        }

        public Mods GetInstalledMods()
        {
            DirectoryInfo d = new DirectoryInfo(modInstallRootDir);

            FileSystemInfo[] modDirs = d.GetFileSystemInfos();

            foreach (var dir in modDirs)
            {
                if (File.Exists(string.Format("{0}/module.xml", dir.FullName)))
                {
                    ModManifest manifest = new ModManifest(dir.FullName);
                    InstalledMods.Add(dir.Name, manifest);
                }
            }

            return InstalledMods;
        }
    }
}