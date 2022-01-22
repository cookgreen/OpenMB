using System;
using System.Collections.Generic;
using System.Text;
using OpenMB.States;
using OpenMB.Localization;
using OpenMB.Utilities;
using Mogre;
using OpenMB.LogMessage;
using OpenMB.Mods;
using OpenMB.Core;

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
		private GameConfigXml gameOptions;
		private string mod;
		public GameApp(GameConfigXml gameOptions = null, string mod = null)
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
			if (!EngineManager.Instance.Init("OpenMB", gameOptions))
			{
				EngineLogManager.Instance.LogMessage("Failed to initialize the game engine!", LogType.Error);
				return RunState.Error;
			}

			GC.Collect();

			AppState.create<ModChooser>("ModChooser");
			AppState.create<MainMenu>("MainMenu");
			AppState.create<Pause>("Pause");
			AppState.create<SinglePlayer>("SinglePlayer");
			AppState.create<Multiplayer>("Multiplayer");
            AppState.create<Credit>("Credit");
            AppState.create<Loading>("Loading");

			var installedMod = ModManager.Instance.InstalledMods;
			if (!string.IsNullOrEmpty(mod) && installedMod.ContainsKey(mod))
			{
				EngineManager.Instance.loadingData = new LoadingData(LoadingType.LOADING_MOD, "Loading Mod...Please wait", mod, "MainMenu");
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
