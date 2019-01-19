using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Mods;
using Mogre;
using Mogre_Procedural.MogreBites;

namespace AMOFGameEngine.States
{
    public class LoadingData
    {
        private LoadingType type;
        private string comment;
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

        public LoadingData(LoadingType type, string comment, object data)
        {
            this.type = type;
            this.comment = comment;
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
            Mogre.Vector3 vectorCameraLookat = new Mogre.Vector3(0, 0, 0);
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
                    ModManager.Instance.LoadingModProcessing += new Action<int>(LoadingModProcessing);
                    ModManager.Instance.LoadingModFinished += new Action(LoadingModFinished);
                    ModManager.Instance.LoadMod(GameManager.Instance.loadingData.Data.ToString());
                    break;
            }
        }

        private void LoadingModFinished()
        {
            var modData = ModManager.Instance.ModData;
            changeAppState(findByName("MainMenu"), modData);
        }

        private void LoadingModProcessing(int progress)
        {
            //progressBar.setProgress(progress);
        }

        public override void exit()
        {
            GameManager.Instance.trayMgr.destroyAllWidgets();
            sceneMgr.DestroyCamera(camera);
            if (sceneMgr != null)
                GameManager.Instance.root.DestroySceneManager(sceneMgr);
        }
    }
}
