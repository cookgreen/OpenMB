using Mogre_Procedural.MogreBites;
using OpenMB.Mods;
using OpenMB.Utilities;
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
		}

		public override void Run()
		{
			string str = "@" + modData.BasicInfo.Name;
			GameManager.Instance.trayMgr.showCursor();
			GameManager.Instance.trayMgr.createLabel(TrayLocation.TL_TOP, "MenuLbl", str.ToLocalizedString(), 400);
			if (modData.HasSinglePlayer)
			{
				var btnSingleplayer = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnSingleplayer", "str_single_player".ToStrLocalizedString(), 200);
				btnSingleplayer.OnClick += (sender) => 
				{
					OnScreenEventChanged?.Invoke("btnSingleplayer", null);
				};
				var btnLoadGame = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnLoadGame", "str_load".ToStrLocalizedString(), 200);
				btnLoadGame.OnClick += (sender) =>
				{
					OnScreenEventChanged?.Invoke("btnLoadGame", null);
				};
			}
			var btnMultiplayer = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnMultiplayer", "str_multiplayer".ToStrLocalizedString(), 200);
			btnMultiplayer.OnClick += (sender) =>
			{
				OnScreenEventChanged?.Invoke("btnMultiplayer", null);
			};
			var btnConfigure = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnConfigure", "str_config".ToStrLocalizedString(), 200);
			btnConfigure.OnClick += (sender) =>
			{
				OnScreenEventChanged?.Invoke("btnConfigure", null);
			};
			if (modData.HasCredit)
			{
				var btnCredit = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnCredit", "str_credit".ToStrLocalizedString(), 200);
				btnCredit.OnClick += (sender) =>
				{
					OnScreenEventChanged?.Invoke("btnCredit", null);
				};
			}
			var btnQuit = GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnQuit", "str_quit".ToStrLocalizedString(), 200);
			btnQuit.OnClick += (sender) =>
			{
				OnScreenEventChanged?.Invoke("btnQuit", null);
			};
		}

		public override void Exit()
		{
			GameManager.Instance.trayMgr.destroyAllWidgets();
		}
	}
}
