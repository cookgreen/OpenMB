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
    public class OgreConfigFileAdapter : IDisposable
    {
        private string filename;
        private List<OgreConfigNode> ogreconfigs;
        private bool disposed;

        public OgreConfigFileAdapter(string fileName)
        {
            filename = fileName;
            disposed = false;
        }

        public List<OgreConfigNode> ReadConfigData()//Read Config Data to list
        {
            List<OgreConfigNode> settings = new List<OgreConfigNode>();

            string secName;
            Mogre.ConfigFile cf = new Mogre.ConfigFile();

            cf.Load(filename, "\t:=", true);
            Mogre.ConfigFile.SectionIterator seci = cf.GetSectionIterator();
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                OgreConfigNode configNode = new OgreConfigNode();
                configNode.Section = secName;
                Mogre.ConfigFile.SettingsMultiMap settings2 = seci.Current; ;

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

        public void SetDefaultRenderSystem(List<OgreConfigNode>  configSettings, string defaultRenderSystem)//Get Default Render System
        {
            OgreConfigNode ogreDefaultRS = configSettings.Where(o => o.Section == "").First();
            ogreDefaultRS.Settings.Remove("Render System");
            Dictionary<string, string> ogreDefaultRSSetting = new Dictionary<string, string>();
            ogreDefaultRSSetting.Add("Render System", defaultRenderSystem);
            ogreDefaultRS.Settings = ogreDefaultRSSetting;
        }

        public void SaveConfig(List<OgreConfigNode> configSettings, string defaultRenderSystem)//Save Config File
        {
            SetDefaultRenderSystem(configSettings, defaultRenderSystem);

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
                    Dictionary<string, string> settings = singleSetting.Settings;
                    foreach (KeyValuePair<string, string> pd in settings)
                    {
                        sw.WriteLine(pd.Key + "=" + pd.Value + "\n");
                    }
                    sw.WriteLine();
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

        public void Dispose()
        {
            if (!disposed)
            {
                ogreconfigs.Clear();
                ogreconfigs = null;
            }
        }
    }
}
