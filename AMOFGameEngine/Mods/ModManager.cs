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
        //public Action<ModEventArgs> ModStateChangedAction;

        OgreConfigFileAdapter ofa;
        List<OgreConfigNode> modData;
        const string modConfigFile = "Mods.cfg";

        List<ModBaseInfo> avaliableModInfos;
        public List<ModBaseInfo> AvaliableModInfos
        {
            get { return avaliableModInfos; }
            set { avaliableModInfos = value; }
        }
        List<ModData> avaliableMods;

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
            avaliableMods = new List<ModData>();
            avaliableModInfos = new List<ModBaseInfo>();
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
            //List<OgreConfigNode> modData = ofa.ReadConfigData();
            //string modDir = modData.Where(o => o.Section == "").First().Settings["ModDir"];
            //
            //List<KeyValuePair<string, string>> modNames = GetModsConfig();
            //foreach (KeyValuePair<string, string> modkpl in modNames)
            //{
            //
            //    string modPath = string.Format(@"{0}\{1}\{2}.dll", System.Environment.CurrentDirectory, modDir, modkpl.Value);
            //    Assembly modAssembly = Assembly.LoadFile(modPath);
            //    string modClassName = string.Format("{0}.{1}", modkpl.Value, "ModMain");
            //    ModData mod = Activator.CreateInstance(modAssembly.GetType(modClassName)) as IMod;
            //    mod.ModStateChangedEvent += new EventHandler<ModEventArgs>(mod_ModStateChangedEvent);
            //    avaliableMods.Add(mod);
            //    ModBaseInfo modInfo = new ModBaseInfo();
            //    modInfo.ModName = mod.modInfo["Name"];
            //    modInfo.ModDesc = mod.modInfo["Description"];
            //    modInfo.ModThumb = mod.modInfo["Thumb"];
            //    avaliableModInfos.Add(modInfo);
            //}
        }

        public List<ModBaseInfo> GetAllMods()
        {
            return avaliableModInfos;
        }

        public void SetupMod(int modIndex)
        {
            ModData currentMod = avaliableMods.ElementAt(modIndex);
            if (currentMod != null)
            {
                //currentMod.SetupMod(
                //    GameManager.Singleton.mRoot,
                //    GameManager.Singleton.mRenderWnd,
                //    GameManager.Singleton.mTrayMgr,
                //    GameManager.Singleton.mMouse,
                //    GameManager.Singleton.mKeyboard
                //    );
            }
        }
    }
}