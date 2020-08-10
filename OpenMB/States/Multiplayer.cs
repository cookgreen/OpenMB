using System;
using System.Collections.Generic;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using OpenMB.Mods;
using OpenMB.Network;
using OpenMB.Screen;
using OpenMB.UI;

namespace OpenMB.States
{
	public class Multiplayer : AppState
	{
		private delegate bool ServerStartDelegate();
		private GameServer thisServer;
		private Dictionary<string, string> option;
		private StringVector serverState;

		public Multiplayer()
		{
			option = new Dictionary<string, string>();
			serverState = new StringVector();
			thisServer = new GameServer();
		}

		public override void enter(Mods.ModData e = null)
		{
			modData = e;
			sceneMgr = GameManager.Instance.root.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");
			ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
			sceneMgr.AmbientLight = cvAmbineLight;
			camera = sceneMgr.CreateCamera("multiplayerCam");
			GameManager.Instance.viewport.Camera = camera;
			camera.AspectRatio = GameManager.Instance.viewport.ActualWidth / GameManager.Instance.viewport.ActualHeight;
			GameManager.Instance.viewport.OverlaysEnabled = true;

			ScreenManager.Instance.OnExternalEvent += OnExternalEvent;
			ScreenManager.Instance.ChangeScreen("MultiplayerServerBrowser", true, modData);

			GameManager.Instance.mouse.MouseMoved += mouseMoved;
			GameManager.Instance.mouse.MousePressed += mousePressed;
			GameManager.Instance.mouse.MouseReleased += mouseReleased;
			GameManager.Instance.keyboard.KeyPressed += keyPressed;
			GameManager.Instance.keyboard.KeyReleased += keyReleased;
		}

		public bool keyPressed(KeyEvent keyEventRef)
		{
			return true;
		}
		public bool keyReleased(KeyEvent keyEventRef)
		{
			return true;
		}

		public bool mouseMoved(MouseEvent evt)
		{
			UIManager.Instance.InjectMouseMove(evt);
			return true;
		}
		public bool mousePressed(MouseEvent evt, MouseButtonID id)
		{
			UIManager.Instance.InjectMouseDown(evt, id);
			return true;
		}
		public bool mouseReleased(MouseEvent evt, MouseButtonID id)
		{
			UIManager.Instance.InjectMouseUp(evt, id);
			return true;
		}

		private void OnExternalEvent(string arg1, string arg2)
		{
		}


		void Server_OnEscapePressed()
		{
			ShowEscapeMenu();
		}

		private void ShowEscapeMenu()
		{
		}

		bool mKeyboard_KeyReleased(MOIS.KeyEvent arg)
		{
			return true;
		}

		public bool ServerStart()
		{
			thisServer.Init();
			return thisServer.Go();
		}

		public override bool pause()
		{
			return base.pause();
		}

		public override void update(double timeSinceLastFrame)
		{
			if (thisServer != null && thisServer.Started)
			{
				thisServer.Update();
				thisServer.GetServerState(ref serverState);
			}
		}

		public override void exit()
		{
			if (sceneMgr != null)
			{
				sceneMgr.DestroyCamera(camera);
				GameManager.Instance.root.DestroySceneManager(sceneMgr);
			}
			if (thisServer != null)
			{
				thisServer.Exit();
			}
		}
	}
}
