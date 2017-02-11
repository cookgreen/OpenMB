using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AMOFGameEngine.Localization
{
    enum LOCATE
    {
        invalid,//Invalid
        en,//English
        cns,//Simple Chinese
        cnt,//Traditional Chinese
        de,//German
        fr,//French
        ja//Japanese
    }
    class LocateSystem
    {
        private LocateUCSFile ucs;
        private static LOCATE __locate;
        private static string path="./language.txt";
        public static bool IsInit;
        public LocateSystem()
        {
        }
        public static bool InitLocateSystem(LOCATE CurrentLocate)
        {
            __locate = CurrentLocate;
            LocateUCSFile.PrepareUCSFile();
            if (LocateUCSFile.ProcessUCSFile("GameStrings.ucs", __locate) && LocateUCSFile.ProcessUCSFile("GameUI.ucs", __locate))
            {
                return true;
            }
            else
                return false;
        }
        public static string CreateLocateString(string ID)
        {
            string res = LocateUCSFile.SeekValueByKey(ID);
            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }
            else
                return null;
        }
        public static LOCATE getLanguageFromFile()
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
        public static int CovertLocateInfoToIndex(LOCATE locate)
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
        public void SaveLanguageSettingsToFIle(int index)
        {
            try
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
            catch (Exception e)
            {
            }
        }
    }
}
