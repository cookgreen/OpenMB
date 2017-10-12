using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Mogre;
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
        static ModManager instance;

        public ModManager()
        {
            InstalledMods = new Dictionary<string, ModManifest>();
            currentMod = null;
            modConfigData = new List<OgreConfigNode>();
            ofa = new OgreConfigFileAdapter("Mods.cfg");
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
                if (InstalledMods == null || InstalledMods.Count <= 0)
                {
                    return;
                }
                ModManifest manifest = InstalledMods.Where(o => o.Key == currentModName).SingleOrDefault().Value;
                currentMod = new AMOFGameEngine.Mods.ModData();
                currentMod.BasicInfo = manifest.MetaData;
                worker.ReportProgress(25);
                
                ModXMLLoader loader = new ModXMLLoader(manifest.InstalledPath + "/" + manifest.Data.Characters);
                XML.ModCharactersDfnXML characterDfn;
                loader.Load<XML.ModCharactersDfnXML>(out characterDfn);
                currentMod.CharacterInfos = characterDfn.CharacterDfns;
                worker.ReportProgress(50);
                
                loader = new ModXMLLoader(manifest.InstalledPath + "/" + manifest.Data.Items);
                XML.ModItemsDfnXML itemDfn;
                loader.Load<XML.ModItemsDfnXML>(out itemDfn);
                currentMod.ItemInfos = itemDfn != null ? itemDfn.Items : null;
                worker.ReportProgress(75);
                
                loader = new ModXMLLoader(manifest.InstalledPath + "/" + manifest.Data.Sides);
                XML.ModSidesDfnXML sideDfn;
                loader.Load<XML.ModSidesDfnXML>(out sideDfn);
                currentMod.SideInfos = sideDfn.Sides;
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
            InstalledMods.Clear();
        }

        public void Update(float timeSinceLastFrame)
        {

        }
    }
}