using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using AMOFGameEngine.Models;
using Mogre;

namespace AMOFGameEngine.Utilities
{
    class UCSFile
    {
        static string path = @"../Locate/";
        static Dictionary<string, string> UCSValueTmp=new Dictionary<string,string>();
        public static void PrepareUCSFile()
        {
            UCSValueTmp.Clear();
        }
        public static bool ProcessUCSFile(string UCSFileName,LOCATE currentlocate)
        {
            string filepath = path + currentlocate.ToString() + @"/" + UCSFileName;
            if (File.Exists(filepath))
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    while (sr.Peek() >= 0 && !sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] outputTmp = Regex.Split(line, "\t");
                        if(!UCSValueTmp.ContainsKey(outputTmp[0]))
                            UCSValueTmp.Add(outputTmp[0], outputTmp[1]);
                    }
                }
                return true;
            }
            else
                return false;
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
    }
}
