using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace OpenMB.Utilities.LocateFileEditor
{
    public class UCSFile
    {
        string filePath = null;

        private Dictionary<string, string> ucsData;

        public Dictionary<string, string> UCSData
        {
            get { return ucsData; }
            set { ucsData = value; }
        }

        public UCSFile(string filepath)
        {
            filePath = filepath;
            ucsData = new Dictionary<string, string>();
        }

        public bool Process()
        {
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (sr.Peek() >= 0 && !sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] outputTmp = Regex.Split(line, "\t");
                        if (!ucsData.ContainsKey(outputTmp[0]))
                        {
                            ucsData.Add(outputTmp[0], outputTmp[1]);
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

        public bool Save(Dictionary<string,string> ucsData)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(filePath))
                {
                    foreach (var kpl in ucsData)
                    {
                        string line = kpl.Key + "\t" + kpl.Value;
                        sr.WriteLine(line);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
