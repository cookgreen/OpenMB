using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AMOFGameEngine.Localization
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
        public bool IsInit;
        private bool disposed;
        LocateUCSFile ucsGameStr;
        LocateUCSFile ucsGameUI;
        LocateUCSFile ucsGameQuickStr;

        public LOCATE Locate
        {
            get
            {
                return locate;
            }
        }

        public static LocateSystem Singleton
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
        static LocateSystem instance;

        public LocateSystem()
        {
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
            ucsGameStr = new LocateUCSFile("GameStrings.ucs", locate);
            ucsGameUI = new LocateUCSFile("GameUI.ucs", locate);
            ucsGameQuickStr = new LocateUCSFile("GameQuickString.ucs", locate);

            ucsGameStr.Prepare();
            ucsGameUI.Prepare();
            if (ucsGameStr.Process() && ucsGameUI.Process() && ucsGameQuickStr.Process())
            {
                return true;
            }
            else
            {
                return false;
            }
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
                if (CovertLocateInfoStringToReadableString(tmpw) != index.ToString())
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

        public string CovertReadableStringToLocateShortString(string locate)
        {
            switch (locate)
            {
                case "English":
                    return "en";
                case "Simple Chinese":
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

        public string CovertLocateInfoStringToReadableString(string locate)
        {
            switch (locate)
            {
                case "en":
                    return "English";
                case "cns":
                    return "Simple Chinese";
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
    }
}
