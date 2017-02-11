using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Mogre;
using System.Windows.Forms;

namespace AMOFGameEngine.Utilities
{
    class OgreConfigFileAdapter
    {
        private static string filename;
        private List<OgreConfigNode> ogreconfigs;

        public OgreConfigFileAdapter(string fileName)
        {
            filename = fileName;
        }

        public List<OgreConfigNode> ReadConfigData()//Read Config Data to list
        {
            List<OgreConfigNode> settings = new List<OgreConfigNode>();

            string secName;
            ConfigFile cf = new ConfigFile();

            cf.Load(filename, "\t:=", true);
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                OgreConfigNode configNode = new OgreConfigNode();
                configNode.Section = secName;
                ConfigFile.SettingsMultiMap settings2 = seci.Current; ;

                foreach (KeyValuePair<string, string> pv in settings2)
                {
                    configNode.Settings[pv.Key] = pv.Value;
                }
                settings.Add(configNode);
            }
            ogreconfigs = settings;
            return settings;
        }

        public string GetDefaultRenderSystem()//Get Default Render System
        {
            OgreConfigNode node = ogreconfigs.Where(o => o.Section == "" && o.Settings.ContainsKey("Render System")).First();
            if (node != null)
            {
                string defaultRenderSystem = node.Settings["Render System"];
                return defaultRenderSystem;
            }
            else
            {
                return null;
            }
        }
        public void SaveConfig(List<OgreConfigNode> configSettings)//Save Config File
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (OgreConfigNode singleSetting in configSettings)
                {
                    if (!string.IsNullOrEmpty(singleSetting.Section))
                    {
                        sw.WriteLine("\n" + "[" + singleSetting.Section + "]" + "\n");
                    }
                    Dictionary<string,string> settings = singleSetting.Settings;
                    foreach (KeyValuePair<string, string> pd in settings)
                    {
                        sw.WriteLine(pd.Key + "=" + pd.Value + "\n");
                    }
                }
                sw.Flush();
            }
        }

        public OgreConfigNode GetConfigNodeBySection(string section)//Get Settings under section given and save them to OgreConfigNode
        {
            OgreConfigNode setting;

            setting = ogreconfigs.Where(o => o.Section == section).First();
            if (setting != null)
            {
                return setting;
            }
            else
            {
                return null;
            }
        }
    }
}
