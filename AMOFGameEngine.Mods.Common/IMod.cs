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
    public interface IMod
    {
        event EventHandler<ModEventArgs> ModStateChangedEvent;//0-Stop;1-Run

        bool SetupMod(Root root,RenderWindow win,SdkTrayManager trayMgr,Mouse mouse,Keyboard keyboard);

        void StartMod();

        void StopMod();
    }
}
