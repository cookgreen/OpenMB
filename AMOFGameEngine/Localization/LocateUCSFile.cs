using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Mogre;

namespace AMOFGameEngine.Localization
{
    public class LocateUCSFile : IDisposable
    {
        private const string PATH = @"./Locate/";
        private Dictionary<string, string> UCSValueTmp;
        private Dictionary<string, string> UCSOriginal;
        private string fileName;
        private LOCATE currentLocate;
        private bool disposed;

        public LocateUCSFile(string fileName, LOCATE currentLocate)
        {
            this.fileName = fileName;
            this.currentLocate = currentLocate;
            UCSValueTmp = new Dictionary<string, string>();
            //UCSOriginal = new Dictionary<string, string>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
            }
            disposed = true;
        }

        public void Prepare()
        {
            UCSValueTmp.Clear();
            //ProcessOriginal();
        }

        public bool ProcessOriginal()
        {
            string filepath = string.Format("{0}en/{1}", PATH, fileName);
            if (File.Exists(filepath))
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    while (sr.Peek() >= 0 && !sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] outputTmp = Regex.Split(line, "\t");
                        if (!UCSOriginal.ContainsKey(outputTmp[0]))
                        {
                            UCSOriginal.Add(outputTmp[0], outputTmp[1]);
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

        public bool Process()
        {
            string filepath = string.Format("{0}{1}/{2}",PATH,currentLocate.ToString(),fileName);
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

        public string SeekValueByKey(string ID)
        {
            string result = UCSValueTmp.ContainsKey(ID) ? UCSValueTmp[ID] : string.Empty;

            return result;
        }

        public string SeekKeyByValue(string value)
        {
            string key = string.Empty;
            if (UCSValueTmp.ContainsValue(value))
            {
                foreach (KeyValuePair<string,string> kpl in UCSValueTmp)
                {
                    if (kpl.Value == value)
                    {
                        key= kpl.Key;
                        break;
                    }
                }
            }
            return key;
        }

        public string SeekKeyByValueOriginal(string value)
        {
            string key = string.Empty;
            if (UCSOriginal .ContainsValue(value))
            {
                foreach (KeyValuePair<string, string> kpl in UCSOriginal)
                {
                    if (kpl.Value == value)
                    {
                        key = kpl.Key;
                        break;
                    }
                }
            }
            return key;
        }

        private bool SeekLocalizedString(string str)
        {
            string localizedKey = SeekKeyByValueOriginal(str);
            if (UCSValueTmp.ContainsKey(localizedKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string AddNewKeyByStrIfNotExist(string str)
        {
            if (!SeekLocalizedString(str))
            {
                IEnumerable<string> keys = UCSValueTmp.Keys;
                string lastKey = keys.ElementAt(keys.Count()-1);
                int Index = int.Parse(lastKey);
                Index = Index + 1;
                UCSValueTmp.Add(Index.ToString(), str);
                return Index.ToString();
            }
            else
            {
                return SeekKeyByValueOriginal(str);
            }
        }

        public void Save()
        {
            string filepath = string.Format("{0}{1}/{2}", PATH, currentLocate.ToString(), fileName);
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

        public string GenerateKeyIfNotExist(string str)
        {
            if (!UCSValueTmp.ContainsValue(str))
            {
                string prefix = null;
                prefix = str.Replace(' ', '_').ToLower();
                string key = "qstr_" + prefix;
                UCSValueTmp.Add(key, str);

                return key;
            }
            else
            {
                string key = null;
                foreach (var kpl in UCSValueTmp)
                {
                    if (kpl.Value == str)
                    {
                        key = kpl.Key;
                        break;
                    }
                }
                return key;
            }
        }
    }
}
