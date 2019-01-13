using System;
using System.Collections.Generic;
using System.Text;
using AMOFGameEngine.States;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Utilities;
using Mogre;
using AMOFGameEngine.LogMessage;

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
        private RunState state;
        private Dictionary<string, string> gameOptions;
        public GameApp(Dictionary<string,string> gameOptions = null)
        {
            this.state = RunState.Stopped;
            this.gameOptions = gameOptions;
            AppStateManager.Instance.OnAppStateManagerStarted += new Action(OnAppStateManagerStarted);
        }

        void OnAppStateManagerStarted()
        {
            state = RunState.Running;
        }

        public RunState Run()
        {
            if (!GameManager.Instance.Init("AMGE", gameOptions))
            {
                EngineLogManager.Instance.LogMessage("failed to Initialize the render system!", LogType.Error);
                state = RunState.Error;
            }

            GC.Collect();

            ModChooser.create<ModChooser>("ModChooser");
            MainMenu.create<MainMenu>("MainMenu");
            Pause.create<Pause>("Pause");
            SinglePlayer.create<SinglePlayer>("SinglePlayer");
            Multiplayer.create<Multiplayer>("Multiplayer");
            Credit.create<Credit>("Credit");

            AppStateManager.Instance.start(AppStateManager.Instance.findByName("ModChooser"));

            EngineLogManager.Instance.Dispose();

            return state;
        }
    }
}
