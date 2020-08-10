using Mogre;
using Mogre_Procedural.MogreBites;
using OpenMB.Core;
using OpenMB.Mods;
using OpenMB.Utilities;
using OpenMB.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Screen
{
	public class GameMainMenuScreen : Screen
	{
		public override event Action<string, string> OnScreenEventChanged;
		private ModData modData;
		private SceneManager sceneManager;

		public override string Name
		{
			get { return "MainMenu"; }
		}
		public GameMainMenuScreen()
		{

		}

		public override void Init(params object[] param)
		{
			modData = param[0] as ModData;
			sceneManager = param[1] as SceneManager;
		}

		public override void Run()
		{
			var startupBkType = modData.StartupBackgroundTypes.Where(o => o.Name == modData.BasicInfo.StartupBackground.Type).FirstOrDefault();
			if (startupBkType != null)
			{
				startupBkType.StartBackground(modData.BasicInfo.StartupBackground.Value, sceneManager);
			}

			UIManager.Instance.ShowCursor();

			if (modData.HasSinglePlayer)
			{
				var btnSingleplayer = UIManager.Instance.CreateButton(UIWidgetLocation.TL_CENTER, "btnSingleplayer", GameString.FromString("str_single_player").ToString(), 200);
				btnSingleplayer.OnClick += (sender) =>
				{
					OnScreenEventChanged?.Invoke("btnSingleplayer", null);
				};
				var btnLoadGame = UIManager.Instance.CreateButton(UIWidgetLocation.TL_CENTER, "btnLoadGame", GameString.FromString("str_load").ToString(), 200);
				btnLoadGame.OnClick += (sender) =>
				{
					OnScreenEventChanged?.Invoke("btnLoadGame", null);
				};
			}
			var btnMultiplayer = UIManager.Instance.CreateButton(UIWidgetLocation.TL_CENTER, "btnMultiplayer", GameString.FromString("str_multiplayer").ToString(), 200);
			btnMultiplayer.OnClick += (sender) =>
			{
				OnScreenEventChanged?.Invoke("btnMultiplayer", null);
			};
			var btnConfigure = UIManager.Instance.CreateButton(UIWidgetLocation.TL_CENTER, "btnConfigure", GameString.FromString("str_config").ToString(), 200);
			btnConfigure.OnClick += (sender) =>
			{
				OnScreenEventChanged?.Invoke("btnConfigure", null);
			};
			if (modData.HasCredit)
			{
				var btnCredit = UIManager.Instance.CreateButton(UIWidgetLocation.TL_CENTER, "btnCredit", GameString.FromString("str_credit").ToString(), 200);
				btnCredit.OnClick += (sender) =>
				{
					OnScreenEventChanged?.Invoke("btnCredit", null);
				};
			}
			var btnQuit = UIManager.Instance.CreateButton(UIWidgetLocation.TL_CENTER, "btnQuit", GameString.FromString("str_quit").ToString(), 200);
			btnQuit.OnClick += (sender) =>
			{
				OnScreenEventChanged?.Invoke("btnQuit", null);
			};
		}

		public override void Exit()
		{
			UIManager.Instance.DestroyAllWidgets();
		}
	}
}
