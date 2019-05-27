using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using OpenMB.Utilities;
using OpenMB.Mods;
using OpenMB.Configure;

namespace OpenMB.Mods
{
    using Mogre;
    using Script;
    using Mods = Dictionary<string, ModManifest>;

    public class ModManager
    {
        private Dictionary<string, ModManifest> installedMods;
        private string modInstallRootDir;
        private IniConfigFileParser parser;
        private IniConfigFile modConfigData;
        private ModData currentMod;
        private string currentModName;
        private BackgroundWorker worker;
        public event Action LoadingModStarted;
        public event Action LoadingModFinished;
        public event Action<int> LoadingModProcessing;
        public ModData ModData
        {
            get { return currentMod; }
        }

        public static ModManager Instance
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

        public Mods InstalledMods
        {
            get
            {
                return installedMods;
            }
        }

        static ModManager instance;

        public ModManager()
        {
            installedMods = new Dictionary<string, ModManifest>();
            currentMod = null;
            modConfigData = new IniConfigFile();
            modInstallRootDir = null;
            parser = new IniConfigFileParser();
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (LoadingModProcessing != null)
            {
                LoadingModProcessing(e.ProgressPercentage);
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.Dispose();
            if (LoadingModFinished != null)
            {
                LoadingModFinished();
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (installedMods == null || installedMods.Count <= 0)
                {
                    return;
                }
                ModManifest manifest = installedMods.Where(o => o.Key == currentModName).SingleOrDefault().Value;
                currentMod = new OpenMB.Mods.ModData();
                currentMod.BasicInfo = manifest.MetaData;
                worker.ReportProgress(25);
                
                ModXmlLoader loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.Characters);
                XML.ModCharactersDfnXML characterDfn;
                loader.Load<XML.ModCharactersDfnXML>(out characterDfn);
                currentMod.CharacterInfos = characterDfn.CharacterDfns;
                worker.ReportProgress(50);
                
                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.Items);
                XML.ModItemsDfnXML itemDfn;
                loader.Load<XML.ModItemsDfnXML>(out itemDfn);
                currentMod.ItemInfos = itemDfn != null ? itemDfn.Items : null;
                worker.ReportProgress(75);
                
                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.Sides);
                XML.ModSidesDfnXML sideDfn;
                loader.Load<XML.ModSidesDfnXML>(out sideDfn);
                currentMod.SideInfos = sideDfn.Sides;
                worker.ReportProgress(80);

                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.Skin);
                XML.ModSkinDfnXML skinDfn;
                loader.Load<XML.ModSkinDfnXML>(out skinDfn);
                currentMod.SkinInfos = skinDfn.CharacterSkinList;

                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.Music);
                XML.ModTracksDfnXML trackDfn;
                loader.Load<XML.ModTracksDfnXML>(out trackDfn);
                currentMod.MusicInfos = trackDfn.Tracks;

                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.Sound);
                XML.ModSoundsDfnXML soundDfn;
                loader.Load<XML.ModSoundsDfnXML>(out soundDfn);
                currentMod.SoundInfos = soundDfn.Sounds;

                worker.ReportProgress(100);

                System.Threading.Thread.Sleep(1000);
            }
            catch
            {
                return;
            }
        }

        string GetModInstallRootDir()
        {
            modConfigData = (IniConfigFile)parser.Load("Game.cfg");
            IniConfigFileSection section = modConfigData["Mods"];
            if (section != null)
            {
                string modDir = section["ModDir"];
                if (!string.IsNullOrEmpty(modDir))
                {
                    modInstallRootDir = modDir;
                }
            }
            return modInstallRootDir;
        }

        public Mods GetInstalledMods()
        {
            GetModInstallRootDir();

            if (!string.IsNullOrEmpty(modInstallRootDir))
            {
                DirectoryInfo d = new DirectoryInfo(modInstallRootDir);

                FileSystemInfo[] modDirs = d.GetFileSystemInfos();

                foreach (var dir in modDirs)
                {
                    if (File.Exists(string.Format("{0}/Module.xml", dir.FullName)))
                    {
                        ModManifest manifest = new ModManifest(dir.FullName);
                        if (installedMods.ContainsKey(manifest.MetaData.Name))
                            continue;
                        installedMods.Add(manifest.MetaData.Name, manifest);
                        ResourceGroupManager.Singleton.AddResourceLocation(
                            string.Format("{0}\\Media\\Textures\\", dir.FullName), "FileSystem", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                        ResourceGroupManager.Singleton.AddResourceLocation(
                            string.Format("{0}\\Media\\Models\\", dir.FullName), "FileSystem", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                        ResourceGroupManager.Singleton.AddResourceLocation(
                            string.Format("{0}\\Media\\Materials\\", dir.FullName), "FileSystem", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                        ResourceGroupManager.Singleton.AddResourceLocation(
                            string.Format("{0}\\Media\\Program\\", dir.FullName), "FileSystem", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);

                        StringVector resources = ResourceGroupManager.Singleton.FindResourceNames(ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, "*.script");
                        ScriptPreprocessor.Instance.Process(resources.ToList());
                    }
                }
            }

            return installedMods;
        }

        public void LoadMod(string name)
        {
            if (LoadingModStarted != null)
            {
                LoadingModStarted();
            }
            currentModName = name;
            worker.RunWorkerAsync();
        }

        public void UnloadAllMods()
        {
            installedMods.Clear();
        }

        public void Update(float timeSinceLastFrame)
        {

        }
    }
}