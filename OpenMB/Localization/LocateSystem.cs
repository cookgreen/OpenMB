using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OpenMB.Localization
{
    public enum LOCATE
    {
        invalid,//Invalid
        en,//English
        cns,//Simple Chinese
        cnt,//Traditional Chinese
        de,//German
        fr,//French
        ja//Japanese
    }

    public enum LocateFileType
    {
        GameUI,
        GameString,
        GameQuickString
    }

    public class LocateSystem : IDisposable
    {
        private LOCATE locate;
        private string path="./language.txt";
        private bool isInit;
        private bool disposed;
        private LocateUCSFile ucsGameStr;
        private LocateUCSFile ucsGameUI;
        private LocateUCSFile ucsGameQuickStr;
        private List<LocateUCSFile> modUCSFiles;
        private List<string> avaliableLocates;
        public bool IsInit
        {
            get
            {
                return isInit;
            }
        }

        public LOCATE Locate
        {
            get
            {
                return locate;
            }
        }

        public static LocateSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LocateSystem();
                }
                return instance;
            }
        }

        public List<string> AvaliableLocates
        {
            get
            {
                return avaliableLocates;
            }
        }

        static LocateSystem instance;

        public LocateSystem()
        {
            avaliableLocates = new List<string>();
            modUCSFiles = new List<LocateUCSFile>();
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
                if (ucsGameStr != null)
                {
                    ucsGameStr.Dispose();
                    ucsGameStr = null;
                }
                if (ucsGameUI != null)
                {
                    ucsGameUI.Dispose();
                    ucsGameUI = null;
                }
                
            }
            disposed = true;
        }

        public bool InitLocateSystem(LOCATE CurrentLocate)
        {
            locate = CurrentLocate;
            ucsGameStr = new LocateUCSFile("GameStrings.ucs", locate, LocateFileStorageType.Engine);
            ucsGameUI = new LocateUCSFile("GameUI.ucs", locate, LocateFileStorageType.Engine);
            ucsGameQuickStr = new LocateUCSFile("GameQuickString.ucs", locate, LocateFileStorageType.Engine);
            isInit = true;

            ucsGameStr.Prepare();
            ucsGameUI.Prepare();
            
            DirectoryInfo di = new DirectoryInfo("./locate/");
            FileSystemInfo[] fsi = di.GetFileSystemInfos();
            foreach (var dir in fsi)
            {
                if (File.Exists(string.Format(@"{0}\GameQuickString.ucs", dir.FullName)) &&
                    File.Exists(string.Format(@"{0}\GameStrings.ucs", dir.FullName)) &&
                    File.Exists(string.Format(@"{0}\GameUI.ucs", dir.FullName)))
                {
                    //valid locate directory
                    RegisterLocate(dir.Name);
				}
            }

            if (ucsGameUI.Process() && 
				ucsGameStr.Process() &&
				ucsGameQuickStr.Process())
			{
				modUCSFiles.Add(ucsGameUI);
				modUCSFiles.Add(ucsGameStr);
				modUCSFiles.Add(ucsGameQuickStr);

				return true;
			}
            else
            {
                return false;
            }
        }

        public void AddModLocateFile(string fullPath)
        {
            LocateUCSFile ucsFile = new LocateUCSFile(fullPath, locate, LocateFileStorageType.Default);
			ucsFile.Process();
			modUCSFiles.Add(ucsFile);
        }

        public string LOC(LocateFileType file, string str)
        {
            file = LocateFileType.GameQuickString;
            LocateUCSFile ucs = GetUCSInstanceByType(file);
            string localizedStr = null;
            localizedStr = GetLocalizedString(LocateFileType.GameQuickString, ucs.GenerateQuickStrKeyIfNotExist(str));
            return localizedStr;
        }

        public string GetLocalizedString(LocateFileType fileType,string ID)
        {
            LocateUCSFile ucs = GetUCSInstanceByType(fileType);
            if (ucs != null)
            {
                string res = ucs.SeekValueByKey(ID);
                if (!string.IsNullOrEmpty(res))
                {
                    return res;
                }
            }
            return string.Format("$No Such Key '{0}'!", ID);
        }

        public string GetLocalizedStringMod(string ID, string originalString = null)
        {
			if (ID.StartsWith("@")) //means it is a quick string
			{
				string content = ID.Substring(1, ID.Length - 1);
				string id = "qstr_" + ID.Replace(" ", "_").Substring(1, ID.Length - 1);
				ID = id;
				originalString = content;
			}
            foreach(var ucs in modUCSFiles)
            {
                string res = ucs.SeekValueByKey(ID);
                if (!string.IsNullOrEmpty(res))
                {
                    return res;
                }
            }
            return originalString;
        }

        [Obsolete("No need to read locate setting from language.txt")]
        public LOCATE GetLanguageFromFile()
        {
            string locate;
            if (!File.Exists(path))
            {
                File.CreateText(path);
            }
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader sr = new StreamReader(fs))
            {
                locate = sr.ReadLine();
                sr.Close();
            }
            return ConvertLocateShortStringToLocateInfo(locate);
        }

        private LocateUCSFile GetUCSInstanceByType(LocateFileType fileType)
        {
            LocateUCSFile ucs = null;
            switch (fileType)
            {
                case LocateFileType.GameString:
                    ucs = ucsGameStr;
                    break;
                case LocateFileType.GameUI:
                    ucs = ucsGameUI;
                    break;
                case LocateFileType.GameQuickString:
                    ucs = ucsGameQuickStr;
                    break;
            }
            return ucs;
        }

        public void SaveLanguageSettingsToFIle(int index)
        {
            if (!File.Exists(path))
            {
                File.CreateText(path);
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                string tmpw;
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (StreamReader sr = new StreamReader(fs))
                {
                    string tmpr = sr.ReadLine();
                    tmpw = tmpr;
                    sr.Close();
                }
                if (ConvertLocateShortStringToReadableString(tmpw) != index.ToString())
                {
                    sw.BaseStream.Seek(0, SeekOrigin.Begin);
                    sw.Write(CovertIndexToLocateInfo(index));
                }
                sw.Flush();
                sw.Close();
            }
        }

        public void SaveLocateFile()
        {
            ucsGameStr.Save();
            ucsGameUI.Save();
            ucsGameQuickStr.Save();
        }

        #region Convertion Function
        public int CovertLocateInfoToIndex(LOCATE locate)
        {
            switch (locate)
            {
                case LOCATE.en:
                    return 0;
                case LOCATE.cns:
                    return 1;
                case LOCATE.cnt:
                    return 2;
                case LOCATE.de:
                    return 3;
                case LOCATE.fr:
                    return 4;
                case LOCATE.ja:
                    return 5;
                default:
                    return 0;
            }
        }

        public LOCATE CovertIndexToLocateInfo(int index)
        {
            switch (index)
            {
                case 0:
                    return LOCATE.en;
                case 1:
                    return LOCATE.cns;
                case 2:
                    return LOCATE.cnt;
                case 3:
                    return LOCATE.de;
                case 4:
                    return LOCATE.fr;
                case 5:
                    return LOCATE.ja;
                default:
                    return LOCATE.en;
            }
        }

        /// <summary>
        /// Convert a readable string to short string
        /// </summary>
        /// <param name="locate">Readable String, i.e. English</param>
        /// <returns>Short string, i.e. en</returns>
        public string ConvertReadableStringToLocateShortString(string locate)
        {
            switch (locate)
            {
                case "English":
                    return "en";
                case "Simplified Chinese":
                    return "cns";
                case "Traditional Chinese":
                    return "cnt";
                case "German":
                    return "de";
                case "French":
                    return "fr";
                case "Japanese":
                    return "ja";
                default:
                    return "en";
            }
        }

        /// <summary>
        /// Convert a readable string to short string
        /// </summary>
        /// <param name="locate">Readable String, i.e. English</param>
        /// <returns>Short string, i.e. en</returns>
        public LOCATE ConvertReadableStringToLocate(string locate)
        {
            switch (locate)
            {
                case "English":
                    return LOCATE.en;
                case "Simplified Chinese":
                    return LOCATE.cns;
                case "Traditional Chinese":
                    return LOCATE.cnt;
                case "German":
                    return LOCATE.de;
                case "French":
                    return LOCATE.fr;
                case "Japanese":
                    return LOCATE.ja;
                default:
                    return LOCATE.en;
            }
        }

        public string ConvertLocateShortStringToReadableString(string locate)
        {
            switch (locate)
            {
                case "en":
                    return "English";
                case "cns":
                    return "Simplified Chinese";
                case "cnt":
                    return "Traditional Chinese";
                case "de":
                    return "German";
                case "fr":
                    return "French";
                case "ja":
                    return "Japanese";
                default:
                    return "English";
            }
        }

        public LOCATE ConvertLocateShortStringToLocateInfo(string locate)
        {
            switch (locate)
            {
                case "en":
                    return LOCATE.en;
                case "cns":
                    return LOCATE.cns;
                case "cnt":
                    return LOCATE.cnt;
                case "de":
                    return LOCATE.de;
                case "fr":
                    return LOCATE.fr;
                case "ja":
                    return LOCATE.ja;
                default:
                    return LOCATE.invalid;
            }
        }
        #endregion

        public void RegisterLocate(string locate)
        {
            AvaliableLocates.Add(locate);
        }
    }
}
