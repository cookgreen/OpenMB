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
        private Dictionary<string, List<LocateUCSFile>> modUCSFiles;
        private List<string> avaliableLocates;
        private List<LocateLanguage> supprotedLanguages;
        private LocateLanguage currentLanguage;

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

        public LocateLanguage CurrentLanguage
        {
            get
            {
                return currentLanguage;
            }
        }

        static LocateSystem instance;

        public LocateSystem()
        {
            avaliableLocates = new List<string>();
            modUCSFiles = new Dictionary<string, List<LocateUCSFile>>();
            supprotedLanguages = new List<LocateLanguage>();
            supprotedLanguages.Add(new LocateLanguage()
            {
                ID = "en",
                FullName = "English",
                LanguageHandleType = LanguageHandleType.Default
            });
            supprotedLanguages.Add(new LocateLanguage()
            {
                ID = "cns",
                FullName = "Simplified Chinese",
                LanguageHandleType = LanguageHandleType.Unicode
            });
            supprotedLanguages.Add(new LocateLanguage()
            {
                ID = "cnt",
                FullName = "Traditional Chinese",
                LanguageHandleType = LanguageHandleType.Unicode
            });
            supprotedLanguages.Add(new LocateLanguage()
            {
                ID = "ge",
                FullName = "German",
                LanguageHandleType = LanguageHandleType.Default
            });
            supprotedLanguages.Add(new LocateLanguage()
            {
                ID = "fr",
                FullName = "French",
                LanguageHandleType = LanguageHandleType.Unicode
            });
            supprotedLanguages.Add(new LocateLanguage()
            {
                ID = "ja",
                FullName = "Japanese",
                LanguageHandleType = LanguageHandleType.Unicode
            });
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

            currentLanguage = supprotedLanguages.Where(o => o.ID == CurrentLocate.ToString()).FirstOrDefault();
            if (currentLanguage == null)
            {
                GameManager.Instance.log.LogMessage(
                    string.Format(
                        "The specific locate `{0}` isn't supported!", 
                        CurrentLocate.ToString()),
                    LogMessage.LogType.Warning
                );
            }

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
                if (!modUCSFiles.ContainsKey("common"))
                {
                    modUCSFiles.Add("common", new List<LocateUCSFile>());
                }
                modUCSFiles["common"].Add(ucsGameUI);
				modUCSFiles["common"].Add(ucsGameStr);
				modUCSFiles["common"].Add(ucsGameQuickStr);

				return true;
			}
            else
            {
                return false;
            }
        }

        public void AddModLocateFile(string modID, string fullPath)
        {
            LocateUCSFile ucsFile = new LocateUCSFile(fullPath, locate, LocateFileStorageType.Default);
			ucsFile.Process();
            if (!modUCSFiles.ContainsKey(modID))
            {
                modUCSFiles.Add(modID, new List<LocateUCSFile>());
            }
			modUCSFiles[modID].Add(ucsFile);
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

        public string GetLocalizedString(string ID, string originalString = null, string modID = null)
        {
			if (ID.StartsWith("@")) //means it is a quick string
			{
				string content = ID.Substring(1, ID.Length - 1);
				string id = "qstr_" + ID.Replace(" ", "_").Substring(1, ID.Length - 1);
				ID = id;
				originalString = content;
			}
            if (!string.IsNullOrEmpty(modID))
            {
                if (modUCSFiles.ContainsKey(modID))
                {
                    var ucsfiles = modUCSFiles[modID];
                    foreach (var ucs in ucsfiles)
                    {
                        string res = ucs.SeekValueByKey(ID);
                        if (!string.IsNullOrEmpty(res))
                        {
                            return res;
                        }
                    }
                }
            }
            else
            {
                var ucsfiles = modUCSFiles["common"];
                foreach (var ucs in ucsfiles)
                {
                    string res = ucs.SeekValueByKey(ID);
                    if (!string.IsNullOrEmpty(res))
                    {
                        return res;
                    }
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
