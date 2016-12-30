using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AMOFGameEngine.Utilities;

namespace AMOFGameEngine.Models
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
        private UCSFile ucs;
        private static LOCATE __locate;
        private static string path="./language.txt";
        public static bool IsInit;
        public LocateSystem()
        {
        }
        public static bool InitLocateSystem(LOCATE CurrentLocate)
        {
            __locate = CurrentLocate;
            UCSFile.PrepareUCSFile();
            if (UCSFile.ProcessUCSFile("GameStrings.ucs", __locate) && UCSFile.ProcessUCSFile("GameUI.ucs", __locate))
            {
                return true;
            }
            else
                return false;
        }
        public static string CreateLocateString(string ID)
        {
            string res = UCSFile.SeekValueByKey(ID);
            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }
            else
                return null;
        }
        public static Models.LOCATE getLanguageFromFile()
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
                    return Models.LOCATE.en;
                case "cns":
                    return Models.LOCATE.cns;
                case "cnt":
                    return Models.LOCATE.cnt;
                case "de":
                    return Models.LOCATE.de;
                case "fr":
                    return Models.LOCATE.fr;
                case "ja":
                    return Models.LOCATE.ja;
                default:
                    return Models.LOCATE.invalid;
            }
        }
    }
}
