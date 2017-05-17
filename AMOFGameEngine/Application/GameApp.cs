using System;
using System.Collections.Generic;
using System.Text;
using AMOFGameEngine.States;

namespace AMOFGameEngine
{
    class GameApp
    {
        public GameApp()
        {
        }

        public void Run()
        {
            if (!GameManager.Singleton.InitOgre("AMOFGameEngine Demo"))
		        return;
            if (!GameManager.Singleton.InitGame())
                return;

            ModChooser.create<ModChooser>(GameManager.Singleton.mAppStateMgr, "ModChooser");
            MainMenu.create<MainMenu>(GameManager.Singleton.mAppStateMgr, "MainMenu");
            Pause.create<Pause>(GameManager.Singleton.mAppStateMgr, "Pause");

            GameManager.Singleton.mAppStateMgr.start(GameManager.Singleton.mAppStateMgr.findByName("ModChooser"));
        }
    }
}
