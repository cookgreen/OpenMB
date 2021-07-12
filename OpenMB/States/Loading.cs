using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.Mods;
using Mogre;
using Mogre_Procedural.MogreBites;
using OpenMB.Localization;
using OpenMB.UI;
using OpenMB.UI.Widgets;
using MOIS;

namespace OpenMB.States
{
	public class LoadingData
	{
		private LoadingType type;
		private string comment;
		private string loadingObjName;
		private object data;

		public LoadingType Type
		{
			get
			{
				return type;
			}
		}

		public string Comment
		{
			get
			{
				return comment;
			}
		}

		public object Data
		{
			get
			{
				return data;
			}
		}

		public string LoadingObjName
		{
			get
			{
				return loadingObjName;
			}
		}

		public LoadingData(
			LoadingType type,
			string comment,
			string loadingObjName,
			object data)
		{
			this.type = type;
			this.comment = comment;
			this.loadingObjName = loadingObjName;
			this.data = data;
		}
	}
	public enum LoadingType
	{
		NONE,
		LOADING_MOD,
		LOADING_SCREEN
	}
	public class Loading : AppState
	{
		private ProgressBarWidget progressBar;
		public override void enter(ModData e = null)
		{
			modData = e;
			sceneMgr = EngineManager.Instance.root.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "LoadingSceneMgr");

			ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
			sceneMgr.AmbientLight = cvAmbineLight;

			camera = sceneMgr.CreateCamera("LoadingScreenCam");
			camera.SetPosition(0, 25, -50);
			Mogre.Vector3 vectorCameraLookat = new Mogre.Vector3(0, 0, 0);
			camera.LookAt(vectorCameraLookat);
			camera.NearClipDistance = 1;

			camera.AspectRatio = EngineManager.Instance.viewport.ActualWidth / EngineManager.Instance.viewport.ActualHeight;

			EngineManager.Instance.viewport.Camera = camera;

			UIManager.Instance.DestroyAllWidgets();
			progressBar = UIManager.Instance.CreateProgressBar(UIWidgetLocation.TL_CENTER, "pbProcessBar", "Loading", 500, 300);
			progressBar.setComment(EngineManager.Instance.loadingData.Comment);

			switch (EngineManager.Instance.loadingData.Type)
			{
				case LoadingType.LOADING_MOD:
					ModManager.Instance.LoadingModProcessing += LoadingModProcessing;
					ModManager.Instance.LoadingModFinished += LoadingModFinished;
					ModManager.Instance.LoadMod(EngineManager.Instance.loadingData.LoadingObjName);
					break;
			}

			EngineManager.Instance.mouse.MouseMoved += mouseMoved;
			EngineManager.Instance.mouse.MousePressed += mousePressed;
			EngineManager.Instance.mouse.MouseReleased += mouseReleased;
			EngineManager.Instance.keyboard.KeyPressed += keyPressed;
			EngineManager.Instance.keyboard.KeyReleased += keyReleased;
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

		private void LoadingModFinished()
		{
			var modData = ModManager.Instance.ModData;
			changeAppState(findByName(EngineManager.Instance.loadingData.Data.ToString()), modData);
		}

		private void LoadingModProcessing(int progress)
		{
			switch (progress)
			{
				case 25:
					progressBar.setComment(LocateSystem.Instance.GetLocalizedString(LocateFileType.GameString, "str_processing_module_file"));
					break;
				case 50:
					progressBar.setComment(LocateSystem.Instance.GetLocalizedString(LocateFileType.GameString, "str_loading_resource"));
					break;
				case 75:
					progressBar.setComment(LocateSystem.Instance.GetLocalizedString(LocateFileType.GameString, "str_loading_module_data"));
					break;
				case 100:
					progressBar.setComment(LocateSystem.Instance.GetLocalizedString(LocateFileType.GameString, "str_finished"));
					break;
			}
			progressBar.setProgress(progress / 100);
		}

		public override void exit()
		{
			UIManager.Instance.DestroyAllWidgets();
			sceneMgr.DestroyCamera(camera);
			if (sceneMgr != null)
				EngineManager.Instance.root.DestroySceneManager(sceneMgr);

			ModManager.Instance.LoadingModFinished -= LoadingModFinished;
			ModManager.Instance.LoadingModProcessing -= LoadingModProcessing;
		}
	}
}
