using System;
using System.Collections.Generic;
using System.Text;
using AMOFGameEngine.States;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Utilities;
using Mogre;

namespace AMOFGameEngine
{
    class GameApp
    {
        Dictionary<string, string> gameOptions;
        LocateSystem locateSystem;
        List<OgreConfigNode> renderConfigs;
        Root root;
        public GameApp(Dictionary<string,string> gameOptions,LocateSystem ls,List<OgreConfigNode> renderConfigs,Root r)
        {
            this.root = r;
            this.gameOptions = gameOptions;
            this.locateSystem = ls;
            this.renderConfigs = renderConfigs;
        }

        public void Run()
        {
            if (!GameManager.Singleton.InitRender("AMOFGameEngine Demo", renderConfigs,root))
		        return;
            if (!GameManager.Singleton.InitGame(gameOptions,locateSystem))
                return;

            ModChooser.create<ModChooser>(GameManager.Singleton.mAppStateMgr, "ModChooser");
            MainMenu.create<MainMenu>(GameManager.Singleton.mAppStateMgr, "MainMenu");
            Pause.create<Pause>(GameManager.Singleton.mAppStateMgr, "Pause");
            GameState.create<GameState>(GameManager.Singleton.mAppStateMgr, "SinglePlayer");
            MultiGameState.create<MultiGameState>(GameManager.Singleton.mAppStateMgr, "Multiplayer");

            GameManager.Singleton.mAppStateMgr.start(GameManager.Singleton.mAppStateMgr.findByName("MainMenu"));
        }
    }
}
