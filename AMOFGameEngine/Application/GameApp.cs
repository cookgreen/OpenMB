using System;
using System.Collections.Generic;
using System.Text;
using AMOFGameEngine.States;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Utilities;
using Mogre;

namespace AMOFGameEngine
{
    enum RunState
    {
        Stopped,
        Running,
        Error
    }

    class GameApp
    {
        RunState state;
        Dictionary<string, string> gameOptions;
        List<OgreConfigNode> renderConfigs;
        Root root;
        public GameApp(Dictionary<string,string> gameOptions,List<OgreConfigNode> renderConfigs,Root r)
        {
            this.state = RunState.Stopped;
            this.root = r;
            this.gameOptions = gameOptions;
            this.renderConfigs = renderConfigs;
            AppStateManager.Singleton.OnAppStateManagerStarted += new Action(OnAppStateManagerStarted);
        }

        void OnAppStateManagerStarted()
        {
            state = RunState.Running;
        }

        public RunState Run()
        {
            if (!GameManager.Singleton.InitRender("AMOFGameEngine Demo", renderConfigs, root))
            {
                LogManager.Singleton.LogMessage("[Engine Error]: failed to Initialize the render system!");
                state = RunState.Error;
            }
            if (!GameManager.Singleton.InitSubSystem(gameOptions))
            {
                LogManager.Singleton.LogMessage("[Engine Error]: failed to Initialize the game system!");
                state = RunState.Error;
            }
            ModChooser.create<ModChooser>("ModChooser");
            MainMenu.create<MainMenu>("MainMenu");
            Pause.create<Pause>("Pause");
            SinglePlayer.create<SinglePlayer>("SinglePlayer");
            Multiplayer.create<Multiplayer>("Multiplayer");

            AppStateManager.Singleton.start(AppStateManager.Singleton.findByName("ModChooser"));
            return state;
        }
    }
}
