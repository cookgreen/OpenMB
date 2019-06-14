using System;
using System.Collections.Generic;
using System.Text;
using OpenMB.States;
using OpenMB.Localization;
using OpenMB.Utilities;
using Mogre;
using OpenMB.LogMessage;
using OpenMB.Mods;

namespace OpenMB
{
    public enum RunState
    {
        Stopped,
        Running,
        Error
    }

    public class GameApp
    {
        private RunState state;
        private Dictionary<string, string> gameOptions;
        private string mod;
        public GameApp(Dictionary<string,string> gameOptions = null, string mod = null)
        {
            this.state = RunState.Stopped;
            this.gameOptions = gameOptions;
            this.mod = mod;
            AppStateManager.Instance.OnAppStateManagerStarted += new Action(OnAppStateManagerStarted);
        }

        void OnAppStateManagerStarted()
        {
            state = RunState.Running;
        }

        public RunState Run()
        {
            if (!GameManager.Instance.Init("OpenMB", gameOptions))
            {
                EngineLogManager.Instance.LogMessage("Failed to initialize the game engine!", LogType.Error);
                return RunState.Error;
            }

            GC.Collect();

            ModChooser.create<ModChooser>("ModChooser");
            MainMenu.create<MainMenu>("MainMenu");
            Pause.create<Pause>("Pause");
            SinglePlayer.create<SinglePlayer>("SinglePlayer");
            Multiplayer.create<Multiplayer>("Multiplayer");
            Credit.create<Credit>("Credit");
            Loading.create<Loading>("Loading");

            var installedMod = ModManager.Instance.GetInstalledMods();
            if (!string.IsNullOrEmpty(mod) && installedMod.ContainsKey(mod))
            {
                GameManager.Instance.loadingData = new LoadingData(LoadingType.LOADING_MOD, "Loading Mod...Please wait", mod, "MainMenu");
                AppStateManager.Instance.start(AppStateManager.Instance.findByName("Loading"));
            }
            else
            {
                AppStateManager.Instance.start(AppStateManager.Instance.findByName("ModChooser"));
            }

            EngineLogManager.Instance.Dispose();

            return state;
        }
    }
}
