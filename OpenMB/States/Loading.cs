using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.Mods;
using Mogre;
using Mogre_Procedural.MogreBites;
using OpenMB.Localization;

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
        private ProgressBar progressBar;
        public override void enter(ModData e = null)
        {
            modData = e;
            sceneMgr = GameManager.Instance.root.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "LoadingSceneMgr");

            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            sceneMgr.AmbientLight = cvAmbineLight;

            camera = sceneMgr.CreateCamera("LoadingScreenCam");
            camera.SetPosition(0, 25, -50);
            Vector3 vectorCameraLookat = new Mogre.Vector3(0, 0, 0);
            camera.LookAt(vectorCameraLookat);
            camera.NearClipDistance = 1;

            camera.AspectRatio = GameManager.Instance.viewport.ActualWidth / GameManager.Instance.viewport.ActualHeight;

            GameManager.Instance.viewport.Camera = camera;

            GameManager.Instance.trayMgr.destroyAllWidgets();
            progressBar = GameManager.Instance.trayMgr.createProgressBar(TrayLocation.TL_CENTER, "pbProcessBar", "Loading", 500, 300);
            progressBar.setComment(GameManager.Instance.loadingData.Comment);

            switch (GameManager.Instance.loadingData.Type)
            {
                case LoadingType.LOADING_MOD:
                    ModManager.Instance.LoadingModProcessing += LoadingModProcessing;
                    ModManager.Instance.LoadingModFinished += LoadingModFinished;
                    ModManager.Instance.LoadMod(GameManager.Instance.loadingData.LoadingObjName);
                    break;
            }
        }

        private void LoadingModFinished()
        {
            var modData = ModManager.Instance.ModData;
            changeAppState(findByName(GameManager.Instance.loadingData.Data.ToString()), modData);
        }

        private void LoadingModProcessing(int progress)
        {
            switch (progress)
            {
                case 25:
                    progressBar.setComment(LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameString, "str_processing_module_file"));
                    break;
                case 50:
                    progressBar.setComment(LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameString, "str_loading_resource"));
                    break;
                case 75:
                    progressBar.setComment(LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameString, "str_loading_module_data"));
                    break;
                case 100:
                    progressBar.setComment(LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameString, "str_finished"));
                    break;
            }
            progressBar.setProgress(progress / 100);
        }

        public override void exit()
        {
            GameManager.Instance.trayMgr.destroyAllWidgets();
            sceneMgr.DestroyCamera(camera);
            if (sceneMgr != null)
                GameManager.Instance.root.DestroySceneManager(sceneMgr);

            ModManager.Instance.LoadingModFinished -= LoadingModFinished;
            ModManager.Instance.LoadingModProcessing -= LoadingModProcessing;
        }
    }
}
