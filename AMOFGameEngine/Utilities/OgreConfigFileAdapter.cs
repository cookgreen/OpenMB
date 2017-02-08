using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Mogre;

namespace AMOFGameEngine.Utilities
{
    class OgreConfigFileAdapter
    {
        private static string filename;
        private static StreamWriter sw;
        private static bool IsRSWrite;
        public OgreConfigFileAdapter(string fileName)
        {
            filename = fileName;
        }
        public string getDefaultRS()
        {
            StreamReader sr = new StreamReader(filename);
            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();
                string[] temp = line.Split('=');
                if (temp[0] == "Render System")
                {
                    return temp[1];
                }
            }
            return null;
        }

        public static void saveConfig(OgreConfigNode s,NameValuePairList p,string defaultRS="") 
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

        public OgreConfigNode getConfigNode(string section)
        {
            OgreConfigNode cfn = new OgreConfigNode();

            using (StreamReader sr = new StreamReader(filename))
            {
            }

            return cfn;
        }

    }
}
