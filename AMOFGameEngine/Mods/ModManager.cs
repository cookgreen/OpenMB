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
    public class ModManager
    {
        public Action<ModEventArgs> ModStateChangedAction;

        OgreConfigFileAdapter ofa;
        List<OgreConfigNode> modData;
        const string modConfigFile="Mods.cfg";

        List<ModBaseInfo> avaliableMods;
        public List<ModBaseInfo> AvaliableMods
        {
            get { return avaliableMods; }
            set { avaliableMods = value; }
        }
        List<Assembly> avaliableModAssembly;

        Assembly currentMod;

        public ModManager()
        {
            avaliableModAssembly = new List<Assembly>();
            avaliableMods = new List<ModBaseInfo>();
            ofa = new OgreConfigFileAdapter(modConfigFile);
            modData = ofa.ReadConfigData();
            currentMod = null;
            LoadMods();
        }
        
        List<KeyValuePair<string, string>> GetModsConfig()
        {
            return modData.Where(o => o.Section == "").First().Settings.Where(o => o.Key == "Mod").ToList();
        }

        public void LoadMods()
        {
            List<OgreConfigNode> modData = ofa.ReadConfigData();
            string modDir=modData.Where(o => o.Section == "").First().Settings["ModDir"];

            List<KeyValuePair<string, string>> modNames = GetModsConfig();
            foreach (KeyValuePair<string, string> modName in modNames)
            {

                string modPath = string.Format(@"{0}\{1}\{2}.dll", System.Environment.CurrentDirectory, modDir, modName.Value);
                Assembly mod = Assembly.LoadFile(modPath);
                var sameModResult = from modAssembly in avaliableModAssembly
                                    where modAssembly.FullName == mod.FullName
                                    select modAssembly;
                if (sameModResult.Count() == 0)
                {
                    avaliableModAssembly.Add(mod);
                    avaliableMods.Add(new ModBaseInfo()
                    {
                        ModName = modName.Value
                    });
                }
                else
                {
                    GameManager.Singleton.mLog.LogMessage("Engine Warning: Mod name must be unique!");
                }
            }
        }

        public List<ModBaseInfo> GetAllMods()
        {
            return avaliableMods;
        }

        public void RunMod(int modIndex)
        {
            Assembly modAssembly = avaliableModAssembly.ElementAt(modIndex);
            if (modAssembly != null)
            {
                string modClassName = string.Format("{0}.{1}", avaliableMods.ElementAt(modIndex).ModName,"ModMain");
                Type modType = modAssembly.GetType(modClassName);
                object modObj = Activator.CreateInstance(modType);
                MethodInfo SetupModMethod = modType.GetMethod("SetupMod");
                object ret=SetupModMethod.Invoke(modObj, new object[]{ 
                    GameManager.Singleton.mRoot,
                    GameManager.Singleton.mRenderWnd,
                    GameManager.Singleton.mTrayMgr,
                    GameManager.Singleton.mMouse,
                    GameManager.Singleton.mKeyboard });
                if ((bool)ret)
                {
                    //bind the event
                    EventInfo modStateChangedEvent = modType.GetEvent("ModStateChangedEvent", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
                    modStateChangedEvent.AddEventHandler(modObj, new EventHandler<ModEventArgs>(ModStateChangedHandlerEx));

                    MethodInfo StartModMethod = modType.GetMethod("StartModSP");
                    StartModMethod.Invoke(modObj,null);
                }
                currentMod = modAssembly;
            }
        }

        public void RunModMP(int modIndex)
        {
            Assembly modAssembly = avaliableModAssembly.ElementAt(modIndex);
            if (modAssembly != null)
            {
                string modClassName = string.Format("{0}.{1}", avaliableMods.ElementAt(modIndex).ModName, "ModMain");
                Type modType = modAssembly.GetType(modClassName);
                object modObj = Activator.CreateInstance(modType);
                MethodInfo SetupModMethod = modType.GetMethod("SetupMod");
                object ret = SetupModMethod.Invoke(modObj, new object[]{ 
                    GameManager.Singleton.mRoot,
                    GameManager.Singleton.mRenderWnd,
                    GameManager.Singleton.mTrayMgr,
                    GameManager.Singleton.mMouse,
                    GameManager.Singleton.mKeyboard });
                if ((bool)ret)
                {
                    //bind the event
                    EventInfo modStateChangedEvent = modType.GetEvent("ModStateChangedEvent", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
                    modStateChangedEvent.AddEventHandler(modObj, new EventHandler<ModEventArgs>(ModStateChangedHandlerEx));

                    MethodInfo StartModMethod = modType.GetMethod("StartModMP");
                    StartModMethod.Invoke(modObj, null);
                }
                currentMod = modAssembly;
            }
        }

        public void StopMod()
        {
            if (currentMod != null)
            {
                object modObj = Activator.CreateInstance(currentMod.GetType());
                currentMod.GetType().GetMethod("StopMod").Invoke(modObj,null);
            }
        }

        public void ModStateChangedHandlerEx(object sender, ModEventArgs e)
        {
            if (e.modState == ModState.Stop)
            {
                if (ModStateChangedAction != null)
                {
                    ModStateChangedAction(e);
                }
            }
        }
    }
}
