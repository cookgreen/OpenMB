using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mogre;
using AMOFGameEngine.Utilities;

namespace AMOFGameEngine
{
    public class ModManager
    {
        OgreConfigFileAdapter ofa;
        List<OgreConfigNode> modData;
        const string MOD_CCONFIG_FILE="Mods.cfg";
        string modPath = Directory.Exists("./mods") ?
            "./mods" : "C:\\Users\\Administrator\\Documents\\AMOFGameEngine\\mods";
        const string modFileName = "mod.xml";

        List<ModBaseInfo> avaliableMods;
        public List<ModBaseInfo> AvaliableMods
        {
            get { return avaliableMods; }
            set { avaliableMods = value; }
        }

        static ModManager singleton;
        public static ModManager Singleton
        {
            get 
            {
                if (singleton == null)
                {
                    singleton = new ModManager();
                }
                return singleton;
            }
        }

        ModManager()
        {
            avaliableMods = new List<ModBaseInfo>();
            ofa = new OgreConfigFileAdapter(MOD_CCONFIG_FILE);
            modData = ofa.ReadConfigData();
            InitMods();
        }

        List<KeyValuePair<string, string>> GetModsConfig()
        {
            return modData.Where(o => o.Section == "").First().Settings.Where(o => o.Key == "Mod").ToList();
        }

        public void InitMods()
        {
            List<OgreConfigNode> modData = ofa.ReadConfigData();
            string modDir=modData.Where(o => o.Section == "").First().Settings["ModDir"];

            List<KeyValuePair<string, string>> mods = GetModsConfig();
            foreach (KeyValuePair<string, string> sMod in mods)
            {
                Root.Singleton.LoadPlugin(modDir+sMod.Value);
                ModBaseInfo mod = new ModBaseInfo();
                mod.ModName = sMod.Value;
                avaliableMods.Add(mod);
            }
        }

        public List<ModBaseInfo> GetAllMods()
        {
            return avaliableMods;
        }

        void ProcessModFiles()
        {

        }
    }
}
