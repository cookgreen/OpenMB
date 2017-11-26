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
        private List<OgreConfigNode> renderConfigs;
        private Root root;
        public GameApp(Dictionary<string,string> gameOptions,List<OgreConfigNode> renderConfigs,Root r)
        {
            this.state = RunState.Stopped;
            this.root = r;
            this.gameOptions = gameOptions;
            this.renderConfigs = renderConfigs;
            AppStateManager.Instance.OnAppStateManagerStarted += new Action(OnAppStateManagerStarted);
        }

        void OnAppStateManagerStarted()
        {
            state = RunState.Running;
        }

        public RunState Run()
        {
            if (!GameManager.Instance.InitRender("AMOFGameEngine Demo", renderConfigs, root))
            {
                EngineLogManager.Instance.LogMessage("failed to Initialize the render system!", LogType.Error);
                state = RunState.Error;
            }
            if (!GameManager.Instance.InitSubSystem(gameOptions))
            {
                EngineLogManager.Instance.LogMessage("failed to Initialize the game system!", LogType.Error);
                state = RunState.Error;
            }

            GameManager.Instance.Update += UI.GameUIManager.Instance.Update;
            GameManager.Instance.Update += Mods.ModManager.Instance.Update;
            GameManager.Instance.Update += Maps.MapManager.Instance.Update;
            GameManager.Instance.Update += Sound.SoundManager.Instance.Update;

            GC.Collect();

            ModChooser.create<ModChooser>("ModChooser");
            MainMenu.create<MainMenu>("MainMenu");
            Pause.create<Pause>("Pause");
            SinglePlayer.create<SinglePlayer>("SinglePlayer");
            Multiplayer.create<Multiplayer>("Multiplayer");

            AppStateManager.Instance.start(AppStateManager.Instance.findByName("ModChooser"));

            EngineLogManager.Instance.Dispose();

            return state;
        }
    }
}
