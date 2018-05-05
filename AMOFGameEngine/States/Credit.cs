using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Screen;

namespace AMOFGameEngine.States
{
    public class Credit : AppState
    {
        public override void enter(ModData data = null)
        {
            m_Data = data;
            m_SceneMgr = GameManager.Instance.mRoot.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "CreditSceneMgr");
            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            m_SceneMgr.AmbientLight = cvAmbineLight;
            m_Camera = m_SceneMgr.CreateCamera("GameCamera");
            Mogre.Vector3 vectCameraPostion = new Mogre.Vector3(5, 60, 60);
            m_Camera.Position = vectCameraPostion;
            Mogre.Vector3 vectorCameraLookAt = new Mogre.Vector3(5, 20, 0);
            m_Camera.LookAt(vectorCameraLookAt);
            m_Camera.NearClipDistance = 5;
            m_Camera.AspectRatio = GameManager.Instance.mViewport.ActualWidth / GameManager.Instance.mViewport.ActualHeight;

            GameManager.Instance.mViewport.Camera = m_Camera;

            ScreenManager.Instance.ChangeScreen("Credit");
            ScreenManager.Instance.OnCurrentScreenExit += OnCurrentScreenExit;

            GameManager.Instance.mMouse.MousePressed += MousePressed;
        }

        private bool MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            if (id == MOIS.MouseButtonID.MB_Right)
            {
                changeAppState(findByName("MainMenu"), m_Data);
            }
            return true;
        }

        private void OnCurrentScreenExit()
        {
            changeAppState(findByName("MainMenu"), m_Data);
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
            m_SceneMgr.DestroyCamera(m_Camera);
            GameManager.Instance.mRoot.DestroySceneManager(m_SceneMgr);
            ScreenManager.Instance.Dispose();
            GameManager.Instance.mMouse.MousePressed -= MousePressed;
        }
    }
}
