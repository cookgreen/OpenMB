using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Utilities;

namespace AMOFGameEngine.Models
{
    enum LOCATE
    {
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
        private string path;
        public LocateSystem()
        {
        }
        public static void InitLocateSystem(LOCATE CurrentLocate)
        {
            __locate = CurrentLocate;
            UCSFile.PrepareUCSFile();
            if (UCSFile.ProcessUCSFile("GameStrings.ucs", __locate) 
                && 
                UCSFile.ProcessUCSFile("GameUI.ucs", __locate))
            {
                
            }
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
    }
}
