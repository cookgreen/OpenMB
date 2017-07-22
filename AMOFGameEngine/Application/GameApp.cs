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
        List<OgreConfigNode> renderConfigs;
        Root root;
        public GameApp(Dictionary<string,string> gameOptions,List<OgreConfigNode> renderConfigs,Root r)
        {
            this.root = r;
            this.gameOptions = gameOptions;
            this.renderConfigs = renderConfigs;
        }

        public void Run()
        {
            if (!GameManager.Singleton.InitRender("AMOFGameEngine Demo", renderConfigs,root))
		        return;
            if (!GameManager.Singleton.InitGame(gameOptions))
                return;
            ModChooser.create<ModChooser>("ModChooser");
            MainMenu.create<MainMenu>("MainMenu");
            Pause.create<Pause>("Pause");
            GameState.create<GameState>("SinglePlayer");
            MultiGameState.create<MultiGameState>("Multiplayer");

            AppStateManager.Singleton.start(AppStateManager.Singleton.findByName("MainMenu"));
        }
    }
}
