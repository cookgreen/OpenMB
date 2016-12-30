using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mogre;

namespace AMOFGameEngine.Utilities
{
    class ConfigFileReader
    {
        public static string filename;
        static StreamWriter sw;
        private static bool IsRSWrite;

        public static void saveConfig(ConfigSettings s,NameValuePairList p,string defaultRS="") 
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
    }
}
