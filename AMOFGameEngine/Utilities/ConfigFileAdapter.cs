using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Mogre;

namespace AMOFGameEngine.Utilities
{
    class ConfigFileAdapter
    {
        private static string filename;
        private static StreamWriter sw;
        private static bool IsRSWrite;
        public ConfigFileAdapter(string fileName)
        {
            filename = fileName;
        }

        public static void saveConfig(ConfigNode s,NameValuePairList p,string defaultRS="") 
        {
            if(sw==null)
            {
                sw = new StreamWriter(filename,true);
            }
               if (defaultRS != "")
               {
                   if (!IsRSWrite)
                   {
                       sw.WriteLine("\n" + "Render System=" + defaultRS + "\n\n");
                       IsRSWrite = true;
                   }
               }
               ConfigFile.SettingsMultiMap smm = s.settings;
               if (s.section.Length > 0)
               {
                   sw.WriteLine("\n" + "[" + s.section + "]" + "\n");
                   foreach (KeyValuePair<string, string> pd in smm)
                   {
                       sw.WriteLine(pd.Key + "=" + p[pd.Key] + "\n");
                   }
               }
               sw.Flush();
          }

        public bool IsSection(string data)
        {
            if (Regex.IsMatch(data, @"\\[\\]"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ConfigNode getConfigNode(string section)
        {
            ConfigNode cfn = new ConfigNode();

            using (StreamReader sr = new StreamReader(filename))
            {
            }

            return cfn;
        }

    }
}
