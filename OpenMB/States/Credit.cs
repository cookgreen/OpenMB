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
            sceneMgr = GameManager.Instance.root.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "CreditSceneMgr");
            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            sceneMgr.AmbientLight = cvAmbineLight;
            camera = sceneMgr.CreateCamera("GameCamera");
            Mogre.Vector3 vectCameraPostion = new Mogre.Vector3(5, 60, 60);
            camera.Position = vectCameraPostion;
            Mogre.Vector3 vectorCameraLookAt = new Mogre.Vector3(5, 20, 0);
            camera.LookAt(vectorCameraLookAt);
            camera.NearClipDistance = 5;
            camera.AspectRatio = GameManager.Instance.viewport.ActualWidth / GameManager.Instance.viewport.ActualHeight;

            GameManager.Instance.viewport.Camera = camera;

            ScreenManager.Instance.ChangeScreen("Credit");
            ScreenManager.Instance.OnCurrentScreenExit += OnCurrentScreenExit;

            GameManager.Instance.mouse.MousePressed += MousePressed;
        }

        private bool MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            if (id == MOIS.MouseButtonID.MB_Right)
            {
                changeAppState(findByName("MainMenu"), modData);
            }
            return true;
        }

        private void OnCurrentScreenExit()
        {
            changeAppState(findByName("MainMenu"), modData);
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
            GameManager.Instance.root.DestroySceneManager(sceneMgr);
            GameManager.Instance.mouse.MousePressed -= MousePressed;
        }
    }
}
