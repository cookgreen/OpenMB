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
        //public Action<ModEventArgs> ModStateChangedAction;
        Dictionary<string, ModManifest> InstalledMods;
        string modInstallRootDir;
        OgreConfigFileAdapter ofa;
        List<OgreConfigNode> modConfigData;

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

        ModData currentMod;

        public ModManager()
        {
            InstalledMods = new Dictionary<string, ModManifest>();
        }

        string GetModInstallRootDir()
        {
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