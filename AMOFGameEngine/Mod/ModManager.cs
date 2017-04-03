using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Mogre;
using AMOFGameEngine.Mod.Common;

namespace AMOFGameEngine.Mod
{
    public class ModManager
    {
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
            
            InitMods();
        }

        public void InitMods()
        {
            string[] modDirs = Directory.GetDirectories(modPath);
            foreach (string modDir in modDirs)
            {
                if (File.Exists(string.Format("{0}/{1}", modDir, modFileName)))
                {
                    ModBaseInfo modInfo = new ModBaseInfo();
                    modInfo.ModName = modDir;
                    avaliableMods.Add(modInfo);
                }
            }
        }

        public List<ModBaseInfo> GetAllMods()
        {
            return avaliableMods;
        }

        public void LoadMod(string modName)
        {
            ModContext.Singleton.SetupMod(
                GameManager.Singleton.mRenderWnd,
                GameManager.Singleton.mKeyboard,
                GameManager.Singleton.mMouse,
                null,
                modName
                );
            (new ModApp()).Run();
        }

        void ProcessModFiles()
        {

        }
    }
}
