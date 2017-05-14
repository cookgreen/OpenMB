using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Mogre;

namespace AMOFGameEngine.Localization
{
    class LocateUCSFile
    {
        static string path = @"./Locate/";
        static Dictionary<string, string> UCSValueTmp=new Dictionary<string,string>();

        public static void PrepareUCSFile()
        {
            UCSValueTmp.Clear();
        }

        public static bool ProcessUCSFile(string UCSFileName,LOCATE currentlocate)
        {
            string filepath = string.Format("{0}{1}/{2}",path,currentlocate.ToString(),UCSFileName);
            if (File.Exists(filepath))
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    while (sr.Peek() >= 0 && !sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] outputTmp = Regex.Split(line, "\t");
                        if (!UCSValueTmp.ContainsKey(outputTmp[0]))
                        {
                            UCSValueTmp.Add(outputTmp[0], outputTmp[1]);
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string SeekValueByKey(string ID)
        {
            string result=UCSValueTmp[ID];
            if(!string.IsNullOrEmpty(result))
            {
                return result;
            }
            else
                return null;
        }

        public static void AddNewKeyByStr(string str)
        {
            if (!UCSValueTmp.ContainsValue(str))
            {
                IEnumerable<string> keys = UCSValueTmp.Keys;
                string lastKey = keys.ElementAt(keys.Count()-1);
                int Index = int.Parse(lastKey);
                Index = Index + 1;
                UCSValueTmp.Add(Index.ToString(), str);
            }
        }

        public static void SaveUCSFile(string UCSFileName,LOCATE currentlocate)
        {
            string filepath = string.Format("{0}{1}/{2}",path,currentlocate.ToString(),UCSFileName);
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                foreach (KeyValuePair<string, string> kpl in UCSValueTmp)
                {
                    string line=string.Format("{0}\t{1}",kpl.Key,kpl.Value);
                    sw.WriteLine(line);
                }
                sw.Flush();
            }
        }
    }
}
