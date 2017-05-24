using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Mods;

namespace AMOFGameEngine.Mods.Common
{
    public class IMod
    {
        public event EventHandler<ModEventArgs> ModStateChangedEvent;//0-Stop;1-Run
        public NameValuePairList modInfo;

        public IMod()
        {
            modInfo = new NameValuePairList();
        }

        public virtual bool SetupMod(Root root, RenderWindow win, SdkTrayManager trayMgr, Mouse mouse, Keyboard keyboard) { return true; }

        public virtual void StartModSP() { }

        public virtual void StartModMP() { }

        public virtual void StopMod() { }

        public virtual void UpdateMod(float timeSinceLastFrame) { }
    }
}
