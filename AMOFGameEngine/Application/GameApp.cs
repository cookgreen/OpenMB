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

            GameManager.Singleton.mLog.LogMessage("Demo initialized!");

            ModChooser.create<ModChooser>(GameManager.Singleton.m_pAppStateManager, "ModChooser");
            MenuState.create<MenuState>(GameManager.Singleton.m_pAppStateManager, "MenuState");
            GameState.create<GameState>(GameManager.Singleton.m_pAppStateManager, "GameState");
            SinbadState.create<SinbadState>(GameManager.Singleton.m_pAppStateManager, "SinbadState");
            PhysxState.create<PhysxState>(GameManager.Singleton.m_pAppStateManager, "PhysxState");
            PauseState.create<PauseState>(GameManager.Singleton.m_pAppStateManager, "PauseState");

            GameManager.Singleton.m_pAppStateManager.start(GameManager.Singleton.m_pAppStateManager.findByName("ModChooser"));
        }

        //private AppStateManager m_pAppStateManager;
    }
}
