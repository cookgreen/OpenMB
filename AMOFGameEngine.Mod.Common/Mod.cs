using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using System.Collections;

namespace AMOFGameEngine.Mod.Common
{
    public class Mod : IComparer
    {
        NameValuePairList modInfo;
        Root root;
        RenderWindow window;
        Keyboard keyboard;
        Mouse mouse;
        SceneManager scm;
        bool bDone;
        bool bResourcesLoaded;
        bool bContentSetup;

        public NameValuePairList ModInfo
        {
            get { return modInfo; }
            set { modInfo = value; }
        }

        public int Compare(object x, object y)
        {
            string xTitle = (x as Mod).ModInfo["Title"];
            string yTitle = (y as Mod).ModInfo["Title"];
            return xTitle.CompareTo(yTitle);
        }
        Mod()
        {

        }
    }
}
