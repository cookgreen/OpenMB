using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mogre;

namespace AMOFGameEngine.Mod
{
    class ModManager
    {
        string modPath = Directory.Exists("./mods")?"./mods":
            "C:\\Users\\Administrator\\Documents\\AMOFGameEngine\\mods";
        const string modFileName = "mod.xml";

        StringVector avaliableModNames;
        public StringVector AvaliableModNames
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
            avaliableModNames = new StringVector();
            InitMod();
        }

        public void InitMod()
        {
            string modXMLLocation = modPath + "/mod.xml";
            string[] modDirs = Directory.GetDirectories(modPath);
            foreach (string modDir in modDirs)
            {
                if (File.Exists(string.Format("{0}/{1}", modDir, modFileName)))
                {
                    avaliableModNames.Add(modDir);
                }
            }
        }

        public StringVector GetAllMods()
        {
            return avaliableModNames;
        }

        public void LoadMod(string modName)
        {

        }

        void ProcessModFiles()
        {

        }
    }
}
