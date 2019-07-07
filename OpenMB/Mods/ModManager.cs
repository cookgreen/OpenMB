using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using OpenMB.Configure;

namespace OpenMB.Mods
{
    using Mogre;
    using Script;
    using Script.Command;
    using XML;
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

                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.ItemTypes);
                XML.ModItemTypesDfnXml itemTypesDfn;
                loader.Load<XML.ModItemTypesDfnXml>(out itemTypesDfn);
                currentMod.ItemTypeInfos = itemTypesDfn != null ? itemTypesDfn.ItemTypes : null;
                worker.ReportProgress(75);

                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.Items);
                XML.ModItemsDfnXML itemDfn;
                loader.Load<XML.ModItemsDfnXML>(out itemDfn);
                currentMod.ItemInfos = itemDfn != null ? itemDfn.Items : null;
                worker.ReportProgress(75);

                for (int j = itemDfn.Items.Count - 1; j >= 0; j--)
                {
                    if (!ValidItemType(currentMod, itemDfn.Items[j].Type))
                    {
                        string itemType = itemDfn.Items[j].Type;
                        string itemID = itemDfn.Items[j].ID;
                        itemDfn.Items.Remove(itemDfn.Items[j]);
                        GameManager.Instance.log.LogMessage(
                            string.Format("Unrecognized Item Type `{0}` in Item `{1}`", itemType, itemID),
                            LogMessage.LogType.Error
                        );
                    }
                }

                
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

                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.Maps);
                XML.ModMapsDfnXml mapsDfn;
                loader.Load<XML.ModMapsDfnXml>(out mapsDfn);
                currentMod.MapInfos = mapsDfn.Maps;

                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.WorldMaps);
                XML.ModWorldMapsDfnXml worldMapsDfn;
                loader.Load<XML.ModWorldMapsDfnXml>(out worldMapsDfn);
                currentMod.WorldMapInfos = worldMapsDfn.WorldMaps;

                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.Locations);
                XML.ModLocationsDfnXml locationsDfn;
                loader.Load<XML.ModLocationsDfnXml>(out locationsDfn);
                currentMod.LocationInfos = locationsDfn.Locations;

                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.Skeletons);
                XML.ModSkeletonsDfnXML skeletonsDfn;
                loader.Load<XML.ModSkeletonsDfnXML>(out skeletonsDfn);
                currentMod.SkeletonInfos = skeletonsDfn.Skeletons;


                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.SceneProps);
                XML.ModScenePropsDfnXml scenePropsDfnXml;
                loader.Load<XML.ModScenePropsDfnXml>(out scenePropsDfnXml);
                currentMod.SceneProps = scenePropsDfnXml.SceneProps;

                loader = new ModXmlLoader(manifest.InstalledPath + "/" + manifest.Data.Models);
                XML.ModModelsDfnXml modelsDfnXml;
                loader.Load<XML.ModModelsDfnXml>(out modelsDfnXml);
                currentMod.Models = modelsDfnXml.Models;

                //--------------------------------Load Types-------------------------
                //Load Internal types
                Type[] internalTypes = this.GetType().Assembly.GetTypes();
                Assembly thisAssembly = GetType().Assembly;
                foreach (var internalType in internalTypes)
                {
                    if (internalType.GetInterface("IModSetting") != null)
                    {
                        var instance = thisAssembly.CreateInstance(internalType.FullName) as IModSetting;
                        var findedSettingInMod = manifest.Settings.Where(o => o.Name == instance.Name);
                        if (findedSettingInMod.Count() > 0)
                        {
                            instance.Value = findedSettingInMod.ElementAt(0).Value;
                            instance.Load(currentMod);
                        }
                        currentMod.ModSettings.Add(instance);
                    }
                    else if (internalType.GetInterface("IModModelType") != null)
                    {
                        var instance = thisAssembly.CreateInstance(internalType.FullName) as IModModelType;
                        currentMod.ModModelTypes.Add(instance);
                    }
                }

                //Load Customized type from the assembly
                if (!string.IsNullOrEmpty(manifest.MetaData.Assembly) &&
                    File.Exists(manifest.InstalledPath + "\\" + manifest.MetaData.Assembly))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFile(manifest.InstalledPath + "\\" + manifest.MetaData.Assembly);
                        Type[] types = assembly.GetTypes();
                        foreach (var type in types)
                        {
                            if (type.GetInterface("IScriptCommand") != null)//avaiable customized script command
                            {
                                var instance = assembly.CreateInstance(type.FullName) as ScriptCommand;
                                ScriptCommandRegister.Instance.RegisterNewCommand(instance.CommandName, type); //register this command
                            }
                            else if (type.GetInterface("IModSetting")!=null)
                            {
                                var instance = assembly.CreateInstance(type.FullName) as IModSetting;
                                var findedSettingInMod = manifest.Settings.Where(o => o.Name == instance.Name);
                                if (findedSettingInMod.Count() > 0)
                                {
                                    instance.Value = findedSettingInMod.ElementAt(0).Value;
                                    instance.Load(currentMod);
                                }
                                currentMod.ModSettings.Add(instance);
                            }
                            else if (type.GetInterface("IModModelType") != null)
                            {
                                var instance = assembly.CreateInstance(type.FullName) as IModModelType;
                                currentMod.ModModelTypes.Add(instance);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        GameManager.Instance.log.LogMessage("Error Loading Assembly, Details: " + ex.ToString(), LogMessage.LogType.Error);
                    }
                }
                //--------------------------------------------

                currentMod.MapDir = manifest.Data.MapDir;
                currentMod.MusicDir = manifest.Data.MusicDir;
                currentMod.ScriptDir = manifest.Data.ScriptDir;

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

        bool ValidItemType(ModData mod, string itemType)
        {
            foreach (var itemTypeDefine in mod.ItemTypeInfos)
            {
                if (itemTypeDefine.ID == itemType)
                {
                    return true;
                }
                else
                {
                    return ValidSubItemType(itemTypeDefine, itemType);
                }
            }
            return false;
        }

        bool ValidSubItemType(ModItemTypeDfnXml itemDefineType, string itemType)
        {
            foreach (var subItemType in itemDefineType.SubTypes)
            {
                if (subItemType.ID == itemType)
                {
                    return true;
                }
                else
                {
                    return ValidSubItemType(subItemType, itemType);
                }
            }
            return false;
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

                        for (int i = 0; i < manifest.Media.MediaSections.Count; i++)
                        {
                            var mediaSection = manifest.Media.MediaSections[i];
                            ResourceGroupManager.Singleton.AddResourceLocation(
                                string.Format("{0}\\{1}", dir.FullName, mediaSection.Directory.Replace("/", "//")), mediaSection.Type, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                        }

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