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
            m_pAppStateManager = null;
        }
        ~GameApp()
        {
            m_pAppStateManager = null;
        }

        public void Run()
        {
            if (!GameManager.Singleton.InitOgre("AMOFGameEngine Demo"))
		        return;
            if (!GameManager.Singleton.InitGame())
                return;

            GameManager.Singleton.mLog.LogMessage("Demo initialized!");
 
	        m_pAppStateManager = new AppStateManager();

            ModChooser.create<ModChooser>(m_pAppStateManager,"ModChooser");
            MenuState.create<MenuState>(m_pAppStateManager, "MenuState");
            GameState.create<GameState>(m_pAppStateManager, "GameState");
            SinbadState.create<SinbadState>(m_pAppStateManager, "SinbadState");
            PhysxState.create<PhysxState>(m_pAppStateManager,"PhysxState");
            PauseState.create<PauseState>(m_pAppStateManager, "PauseState");

            m_pAppStateManager.start(m_pAppStateManager.findByName("ModChooser"));
        }

        private AppStateManager m_pAppStateManager;
    }
}
