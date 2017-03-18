using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AMOFGameEngine.Mod
{
    class ModManager
    {
        string modPath = Directory.Exists("./mods")?"./mods":
            "C:\\Users\\Administrator\\Documents\\AMOFGameEngine\\mods";
        const string modFileName = "mod.xml";

        List<string> avaliableModNames;
        public List<string> AvaliableModNames
        {
            get { return avaliableModNames; }
            set { avaliableModNames = value; }
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
            avaliableModNames = new List<string>();
        }

        public void InitMod()
        {
            string modXMLLocation = modPath + "/mod.xml";
            string[] modDirs = Directory.GetDirectories(modPath);
            foreach (string modDir in modDirs)
            {
                if (File.Exists(string.Format("{0}/{1}/{2}", modPath, modDir, modFileName)))
                {
                    avaliableModNames.Add(modDir);
                }
            }
        }

        public void LoadMod(string modName)
        {

        }

        void ProcessModFiles()
        {

        }
    }
}
