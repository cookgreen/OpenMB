using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using OpenMB.Mods;
using OpenMB.Screen;

namespace OpenMB.States
{
	public class Credit : AppState
	{
		public override void enter(ModData data = null)
		{
			modData = data;
			sceneMgr = EngineManager.Instance.root.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "CreditSceneMgr");
			ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
			sceneMgr.AmbientLight = cvAmbineLight;
			camera = sceneMgr.CreateCamera("GameCamera");
			Mogre.Vector3 vectCameraPostion = new Mogre.Vector3(5, 60, 60);
			camera.Position = vectCameraPostion;
			Mogre.Vector3 vectorCameraLookAt = new Mogre.Vector3(5, 20, 0);
			camera.LookAt(vectorCameraLookAt);
			camera.NearClipDistance = 5;
			camera.AspectRatio = EngineManager.Instance.viewport.ActualWidth / EngineManager.Instance.viewport.ActualHeight;

			EngineManager.Instance.viewport.Camera = camera;

			ScreenManager.Instance.ChangeScreen("Credit");

			EngineManager.Instance.mouse.MousePressed += MousePressed;
		}

		private bool MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
		{
			if (id == MOIS.MouseButtonID.MB_Right)
			{
				ScreenManager.Instance.ExitCurrentScreen();
				changeAppState(findByName("MainMenu"), modData);
			}
			return true;
		}

		public override bool pause()
		{
			return base.pause();
		}

		public override void resume()
		{
			base.resume();
		}

		public override void update(double timeSinceLastFrame)
		{
			ScreenManager.Instance.UpdateCurrentScreen((float)timeSinceLastFrame);
		}

		public override void exit()
		{
			EngineManager.Instance.root.DestroySceneManager(sceneMgr);
			EngineManager.Instance.mouse.MousePressed -= MousePressed;
		}
	}
}
