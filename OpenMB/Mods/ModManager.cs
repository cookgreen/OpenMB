using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Mogre;
using OpenMB.Game.ItemTypes;
using OpenMB.Configure;
using OpenMB.Localization;
using OpenMB.Map;
using OpenMB.Mods.XML;
using OpenMB.Script;
using OpenMB.Script.Command;
using OpenMB.Screen;
using OpenMB.UI;
using OpenMB.Sound;
using OpenMB.Core;

namespace OpenMB.Mods
{
	using Mods = Dictionary<string, ModManifest>;
	public class ModManager : ISubSystemManager
	{
		private Dictionary<string, ModManifest> installedMods;
		private string modInstallRootDir;
		private IniConfigFileParser parser;
		private IniConfigFile modConfigData;
		private ModData currentMod;
		private string currentModName;
		private BackgroundWorker loadModWorker;
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
			loadModWorker = new BackgroundWorker();
			loadModWorker.WorkerReportsProgress = true;
			loadModWorker.DoWork += loadModWorker_DoWork;
			loadModWorker.RunWorkerCompleted += loadModWorker_RunWorkerCompleted;
			loadModWorker.ProgressChanged += loadModWorker_ProgressChanged;
		}

		void loadModWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			LoadingModProcessing?.Invoke(e.ProgressPercentage);
		}

		void loadModWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			loadModWorker.Dispose();
			LoadingModFinished?.Invoke();
		}

		void loadModWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				if (installedMods == null || installedMods.Count <= 0)
				{
					return;
				}
				ModManifest manifest = installedMods.Where(o => o.Key == currentModName).SingleOrDefault().Value;
				currentMod = new ModData(manifest);
				currentMod.BasicInfo = manifest.MetaData;
				loadModWorker.ReportProgress(25);

				ChangeModIcon(manifest);
				LoadXmlData(manifest);
				LoadInternalTypes(manifest);
				LoadExternalTypes(manifest);
				VerifyItemTypes();
				LoadModMedia(manifest);
				LoadModLocalization(manifest);

				ModLocalizedFieldManager.Instance.InitMod(currentMod);
				GameMapManager.Instance.InitMod(currentMod);
				ScreenManager.Instance.InitMod(currentMod);
				MusicSoundManager.Instance.InitMod(currentMod);
				UIManager.Instance.InitMod(currentMod);

				StringVector resources = ResourceGroupManager.Singleton.FindResourceNames(ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, "*.script");
				ScriptPreprocessor.Instance.Process(resources.ToList());

				currentMod.MapDir = manifest.Data.MapDir;
				currentMod.MusicDir = manifest.Data.MusicDir;
				currentMod.ScriptDir = manifest.Data.ScriptDir;

				loadModWorker.ReportProgress(100);

				System.Threading.Thread.Sleep(1000);
			}
			catch
			{
				return;
			}
		}

		private void ChangeModIcon(ModManifest manifest)
		{
			if (string.IsNullOrEmpty(manifest.MetaData.Icon))
			{
				return;
			}

			if (ResourceGroupManager.Singleton.ResourceExists(
				ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
				manifest.MetaData.Icon))
			{
				var dataStreamPtr = ResourceGroupManager.Singleton.OpenResource(manifest.MetaData.Icon);
				var stream = Utilities.Helper.DataPtrToStream(dataStreamPtr);

				IntPtr hwnd;
				EngineManager.Instance.renderWindow.GetCustomAttribute("WINDOW", out hwnd);
				Utilities.Helper.SetRenderWindowIcon(new System.Drawing.Icon(stream), hwnd);
			}
		}

		private void LoadXmlData(ModManifest manifest)
		{
			XmlObjectLoader loader = null;

			if (!string.IsNullOrEmpty(manifest.Data.Animations))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Animations);
				ModAnimationsDfnXml animationDfn;
				loader.Load(out animationDfn);
				currentMod.AnimationInfos = animationDfn.Animations;
				loadModWorker.ReportProgress(20);
			}

			if (!string.IsNullOrEmpty(manifest.Data.Characters))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Characters);
				ModCharactersDfnXML characterDfn;
				loader.Load(out characterDfn);
				currentMod.CharacterInfos = characterDfn.CharacterDfns;
				loadModWorker.ReportProgress(50);
			}

			if (!string.IsNullOrEmpty(manifest.Data.ItemTypes))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.ItemTypes);
				ModItemTypesDfnXml itemTypesDfn;
				loader.Load(out itemTypesDfn);
				currentMod.ItemTypeInfos = itemTypesDfn != null ? itemTypesDfn.ItemTypes : null;
				loadModWorker.ReportProgress(75);
			}

			if (!string.IsNullOrEmpty(manifest.Data.Items))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Items);
				ModItemsDfnXML itemDfn;
				loader.Load(out itemDfn);
				currentMod.ItemInfos = itemDfn != null ? itemDfn.Items : null;
				loadModWorker.ReportProgress(75);
			}

			if (!string.IsNullOrEmpty(manifest.Data.Sides))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Sides);
				ModSidesDfnXML sideDfn;
				loader.Load(out sideDfn);
				currentMod.SideInfos = sideDfn.Sides;
				loadModWorker.ReportProgress(80);
			}

			if (!string.IsNullOrEmpty(manifest.Data.Skin))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Skin);
				ModSkinDfnXML skinDfn;
				loader.Load(out skinDfn);
				currentMod.SkinInfos = skinDfn.CharacterSkinList;
			}

			if (!string.IsNullOrEmpty(manifest.Data.Music))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Music);
				ModTracksDfnXML trackDfn;
				loader.Load(out trackDfn);
				currentMod.MusicInfos = trackDfn.Tracks;
			}

			if (!string.IsNullOrEmpty(manifest.Data.Sound))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Sound);
				ModSoundsDfnXML soundDfn;
				loader.Load(out soundDfn);
				currentMod.SoundInfos = soundDfn.Sounds;
			}

			if (!string.IsNullOrEmpty(manifest.Data.Maps))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Maps);
				ModMapsDfnXml mapsDfn;
				loader.Load(out mapsDfn);
				currentMod.MapInfos = mapsDfn.Maps;
			}

			if (!string.IsNullOrEmpty(manifest.Data.WorldMaps))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.WorldMaps);
				ModWorldMapsDfnXml worldMapsDfn;
				loader.Load(out worldMapsDfn);
				currentMod.WorldMapInfos = worldMapsDfn.WorldMaps;
			}

			if (!string.IsNullOrEmpty(manifest.Data.Locations))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Locations);
				ModLocationsDfnXml locationsDfn;
				loader.Load(out locationsDfn);
				currentMod.LocationInfos = locationsDfn.Locations;
			}

			if (!string.IsNullOrEmpty(manifest.Data.Skeletons))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Skeletons);
				ModSkeletonsDfnXML skeletonsDfn;
				loader.Load(out skeletonsDfn);
				currentMod.SkeletonInfos = skeletonsDfn.Skeletons;
			}

			if (!string.IsNullOrEmpty(manifest.Data.SceneProps))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.SceneProps);
				ModScenePropsDfnXml scenePropsDfnXml;
				loader.Load(out scenePropsDfnXml);
				currentMod.ScenePropInfos = scenePropsDfnXml.SceneProps;
			}

			if (!string.IsNullOrEmpty(manifest.Data.Models))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Models);
				ModModelsDfnXml modelsDfnXml;
				loader.Load(out modelsDfnXml);
				currentMod.ModelInfos = modelsDfnXml.Models;
			}

			if (!string.IsNullOrEmpty(manifest.Data.Menus))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Menus);
				ModMenusDfnXml menusDfnXml;
				loader.Load(out menusDfnXml);
				currentMod.MenuInfos = menusDfnXml.Menus;
			}

			if (!string.IsNullOrEmpty(manifest.Data.UILayouts))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.UILayouts);
				ModUILayoutsDfnXml uiLayoutsDfnXml;
				loader.Load(out uiLayoutsDfnXml);
				currentMod.UILayoutInfos = uiLayoutsDfnXml.UILayouts;
			}

			if (!string.IsNullOrEmpty(manifest.Data.Strings))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Strings);
				ModStringsDfnXml stringsDfnXml;
				loader.Load(out stringsDfnXml);
				currentMod.StringInfos = stringsDfnXml.Strings;
			}

			if (!string.IsNullOrEmpty(manifest.Data.Cursors))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Cursors);
				ModCursorsDfnXml cursorsDfnXml;
				loader.Load(out cursorsDfnXml);
				currentMod.CursorInfos = cursorsDfnXml.Cursors;
			}

			if (!string.IsNullOrEmpty(manifest.Data.MapTemplates))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.MapTemplates);
				ModMapTemplatesDfnXml mapTemplatesDfnXml;
				loader.Load(out mapTemplatesDfnXml);
				currentMod.MapTemplateInfos = mapTemplatesDfnXml.MapTemplates;
			}

			if (!string.IsNullOrEmpty(manifest.Data.Vehicles))
			{
				loader = new XmlObjectLoader(manifest.InstalledPath + "/" + manifest.Data.Vehicles);
				ModVehiclesDfnXml vehiclesDfnXml;
				loader.Load(out vehiclesDfnXml);
				currentMod.VehicleInfos = vehiclesDfnXml.VehicleDfns;
			}
		}

		private void LoadInternalTypes(ModManifest manifest)
		{
			//--------------------------------Load Types-------------------------
			//Load Internal types
			Assembly thisAssembly = GetType().Assembly;
			Type[] internalTypes = thisAssembly.GetTypes();
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
				else if (internalType.GetInterface("IModTriggerCondition") != null)
				{
					var instance = thisAssembly.CreateInstance(internalType.FullName) as IModTriggerCondition;
					currentMod.ModTriggerConditions.Add(instance);
				}
				else if (internalType.GetInterface("IGameMapLoader") != null)
				{
					var instance = thisAssembly.CreateInstance(internalType.FullName) as IGameMapLoader;
					currentMod.MapLoaders.Add(instance);
				}
				else if (internalType.GetInterface("IModStartupBackgroundType") != null)
				{
					var instance = thisAssembly.CreateInstance(internalType.FullName) as IModStartupBackgroundType;
					currentMod.StartupBackgroundTypes.Add(instance);
				}
				else if (internalType.GetInterface("IScriptCommand") != null)//avaiable customized script command
				{
					var instance = thisAssembly.CreateInstance(internalType.FullName) as ScriptCommand;
					ScriptCommandRegister.Instance.RegisterNewCommand(instance.CommandName, internalType); //register this command
				}
			}
			currentMod.Assemblies.Add(thisAssembly);
		}

		private void LoadExternalTypes(ModManifest manifest)
		{
			//Load Customized type from the assembly
			for (int i = 0; i < manifest.MetaData.Assemblies.Count; i++)
			{
				string assemblyXml = manifest.MetaData.Assemblies[i];
				bool isCurrentMod;
				string assemblyPath = getAssemblyRealPath(assemblyXml, out isCurrentMod);
				if (string.IsNullOrEmpty(assemblyPath))
				{
					continue;
				}
				if (isCurrentMod)
				{
					assemblyPath = manifest.InstalledPath + "\\" + assemblyPath;
				}
				if (File.Exists(assemblyPath))
				{
					try
					{
						Assembly assemblyDll = Assembly.LoadFile(assemblyPath);
						Type[] types = assemblyDll.GetTypes();
						foreach (var type in types)
						{
							if (type.GetInterface("IScriptCommand") != null)//avaiable customized script command
							{
								var instance = assemblyDll.CreateInstance(type.FullName) as ScriptCommand;
								ScriptCommandRegister.Instance.RegisterNewCommand(instance.CommandName, type); //register this command
							}
							else if (type.GetInterface("IModSetting") != null)
							{
								var instance = assemblyDll.CreateInstance(type.FullName) as IModSetting;
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
								var instance = assemblyDll.CreateInstance(type.FullName) as IModModelType;
								currentMod.ModModelTypes.Add(instance);
							}
							else if (type.GetInterface("IModTriggerCondition") != null)
							{
								var instance = assemblyDll.CreateInstance(type.FullName) as IModTriggerCondition;
								currentMod.ModTriggerConditions.Add(instance);
							}
							else if (type.GetInterface("IItemType") != null)
							{
								var instance = assemblyDll.CreateInstance(type.FullName) as IItemType;
								currentMod.ItemTypes.Add(instance);
							}
							else if (type.GetInterface("IGameMapLoader") != null)
							{
								var instance = assemblyDll.CreateInstance(type.FullName) as IGameMapLoader;
								currentMod.MapLoaders.Add(instance);
							}
							else if (type.GetInterface("IModStartupBackgroundType") != null)
							{
								var instance = assemblyDll.CreateInstance(type.FullName) as IModStartupBackgroundType;
								currentMod.StartupBackgroundTypes.Add(instance);
							}
						}
						currentMod.Assemblies.Add(assemblyDll);
					}
					catch (Exception ex)
					{
						EngineManager.Instance.log.LogMessage("Error Loading Assembly, Details: " + ex.ToString(), LogMessage.LogType.Error);
					}
				}
				else
				{
					EngineManager.Instance.log.LogMessage("Requested Assembly Path don't exist!", LogMessage.LogType.Error);
				}
			}
			//--------------------------------------------
		}

		private void LoadModLocalization(ModManifest manifest)
		{
			//load mod localization files
			string localizationFolder = "Locate";
			string localizationFullPath = manifest.InstalledPath + "//" + localizationFolder;
			string currentLocateFullPath = localizationFullPath + "//" + LocateSystem.Instance.Locate.ToString();
			DirectoryInfo directory = new DirectoryInfo(currentLocateFullPath);
			if (!Directory.Exists(currentLocateFullPath))
			{
				Directory.CreateDirectory(currentLocateFullPath);
			}
			else
			{
				var fileSystemInfos = directory.EnumerateFileSystemInfos();
				foreach (var fileSystemInfo in fileSystemInfos)
				{
					if (fileSystemInfo.Attributes != FileAttributes.Directory && Path.GetExtension(fileSystemInfo.Name) == ".ucs")
					{
						LocateSystem.Instance.AddModLocateFile(manifest.ID, fileSystemInfo.FullName);
					}
				}
			}
		}

		private void LoadModMedia(ModManifest manifest)
		{
			//load mod media
			for (int i = 0; i < manifest.Media.MediaSections.Count; i++)
			{
				var mediaSection = manifest.Media.MediaSections[i];
				string fullMediaDir = string.Format("{0}\\{1}", manifest.InstalledPath, mediaSection.Directory.Replace("/", "//"));
				if (Directory.Exists(fullMediaDir))
				{
					ResourceGroupManager.Singleton.AddResourceLocation(fullMediaDir, mediaSection.ResourceLoadType, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
					DirectoryInfo di = new DirectoryInfo(fullMediaDir);
					var fileSystemInfos = di.EnumerateFileSystemInfos();
					foreach (var fileSystemInfo in fileSystemInfos)
					{
						if (fileSystemInfo.Attributes != FileAttributes.Directory)
						{
							if (mediaSection.ResourceType != ResourceType.Other)
							{
								ResourceGroupManager.Singleton.DeclareResource(fileSystemInfo.Name, mediaSection.ResourceType.ToString(), ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
							}
							currentMod.ModMediaData.Add(new ModMediaData(fileSystemInfo.Name, fileSystemInfo.FullName, mediaSection.ResourceType));
						}
					}
				}
			}

			ResourceGroupManager.Singleton.InitialiseResourceGroup(ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
		}

		private void VerifyItemTypes()
		{
			//Valid Item Type
			for (int j = currentMod.ItemInfos.Count - 1; j >= 0; j--)
			{
				if (!verifyItemType(currentMod, currentMod.ItemInfos[j].Type))
				{
					string itemType = currentMod.ItemInfos[j].Type;
					string itemID = currentMod.ItemInfos[j].ID;
					currentMod.ItemInfos.Remove(currentMod.ItemInfos[j]);
					EngineManager.Instance.log.LogMessage(
						string.Format("Unrecognized Item Type `{0}` in Item `{1}`", itemType, itemID),
						LogMessage.LogType.Error
					);
				}
			}
		}

		private string getAssemblyRealPath(string assemblyPathExpression, out bool isCurrentMod)
		{
			isCurrentMod = false;
			if (string.IsNullOrEmpty(assemblyPathExpression))
			{
				return string.Empty;
			}

			var expression = ModPathExpressionResolver.Resolve(assemblyPathExpression);
			if (expression.Prefix != "this")
			{
				if (installedMods.ContainsKey(expression.Prefix))
				{
					ModManifest modManifest = installedMods[expression.Prefix];
					return modManifest.InstalledPath + "\\" + expression.Value + ".dll";
				}
				return null;
			}
			else
			{
				isCurrentMod = true;
				return expression.Value + ".dll";
			}
		}

		public Mods GetInstalledMods()
		{
			getModInstallRootDir();

			if (!string.IsNullOrEmpty(modInstallRootDir))
			{
				DirectoryInfo d = new DirectoryInfo(modInstallRootDir);

				FileSystemInfo[] modDirs = d.GetFileSystemInfos();

				foreach (var dir in modDirs)
				{
					if (File.Exists(string.Format("{0}/Module.xml", dir.FullName)))
					{
						ModManifest manifest = new ModManifest(dir.FullName);
						if (!installedMods.ContainsKey(dir.Name))
						{
							installedMods.Add(dir.Name, manifest);
							LoadModLocalization(manifest);
							ResourceGroupManager.Singleton.AddResourceLocation(manifest.InstalledPath, "FileSystem", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
						}
					}
				}
			}

			return installedMods;
		}

		public void LoadMod(string name)
		{
			LoadingModStarted?.Invoke();
			currentModName = name;
			loadModWorker.RunWorkerAsync();
		}

		public void UnloadAllMods()
		{
			installedMods.Clear();
		}

		public void Update(float timeSinceLastFrame)
		{

		}

		private string getModInstallRootDir()
		{
			modInstallRootDir = EngineManager.Instance.gameOptions.ModConfig.ModDir;
			return modInstallRootDir;
		}

		private bool verifyItemType(ModData mod, string itemType)
		{
			bool ret = false;
			foreach (var itemTypeDefine in mod.ItemTypeInfos)
			{
				if (itemTypeDefine.ID == itemType)
				{
					return true;
				}
				else
				{
					ret = verifySubItemType(itemTypeDefine, itemType);
					if (ret)
					{
						return true;
					}
				}
			}
			return ret;
		}

		private bool verifySubItemType(ModItemTypeDfnXml itemDefineType, string itemType)
		{
			bool ret = false;
			foreach (var subItemType in itemDefineType.SubTypes)
			{
				if (subItemType.ID == itemType)
				{
					return true;
				}
				else
				{
					ret = verifySubItemType(subItemType, itemType);
					if (ret)
					{
						return true;
					}
				}
			}
			return ret;
		}
	}
}