using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Sound;
using AMOFGameEngine.Utilities;
using AMOFGameEngine.Mods;

namespace AMOFGameEngine.States
{
    public class Pause : AppState
    {
        public Pause()
        {
            m_bQuit = false;
            m_bQuestionActive = false;
            m_FrameEvent = new FrameEvent();
        }

        public override void enter(ModData e = null)
        {
            m_bQuit = false;

            m_SceneMgr = GameManager.Instance.mRoot.CreateSceneManager(SceneType.ST_GENERIC, "PauseSceneMgr");
            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            m_SceneMgr.AmbientLight = cvAmbineLight;

            m_Camera = m_SceneMgr.CreateCamera("PauseCam");
            m_Camera.Position = new Mogre.Vector3(0, 0, 0);
            m_Camera.NearClipDistance = 1;

            m_Camera.AspectRatio = GameManager.Instance.mViewport.ActualWidth /
            GameManager.Instance.mViewport.ActualHeight;

            GameManager.Instance.mViewport.Camera = m_Camera;

            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            GameManager.Instance.mTrayMgr.showCursor();
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_CENTER, "BackToMenuBtn", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Back To Game"), 250);
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_CENTER, "ExitBtn", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Quit"), 250);
            GameManager.Instance.mTrayMgr.createLabel(TrayLocation.TL_TOP, "PauseLbl", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Game Paused"), 250);

            GameManager.Instance.mMouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            GameManager.Instance.mMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            GameManager.Instance.mMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);

            m_bQuestionActive = true;

            createScene();
        }
        public void createScene()
        { }
        public override void exit()
        {
            if (m_SceneMgr != null)
            {
                m_SceneMgr.DestroyCamera(m_Camera);
                GameManager.Instance.mRoot.DestroySceneManager(m_SceneMgr);
            }

            GameManager.Instance.mTrayMgr.clearAllTrays();
            GameManager.Instance.mTrayMgr.setListener(null);
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_ESCAPE) && !m_bQuestionActive)
            {
                m_bQuit = true;
                return true;
            }

            GameManager.Instance.keyPressed(keyEventRef);

            return true;
        }
        public bool keyReleased(KeyEvent keyEventRef)
        {
            GameManager.Instance.keyReleased(keyEventRef);

            return true;
        }

        public bool mouseMoved(MouseEvent evt)
        {
            if (GameManager.Instance.mTrayMgr.injectMouseMove(evt)) return true;
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Instance.mTrayMgr.injectMouseDown(evt, id)) return true;
            return true;
        }
        public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Instance.mTrayMgr.injectMouseUp(evt, id)) return true;
            return true;
        }

        public override void buttonHit(Button button)
        {
            if (button.getName() == "ExitBtn")
            {
                //AdvancedMogreFramework.m_pTrayMgr.showYesNoDialog("Sure?", "Really leave?");
                //m_bQuestionActive = true;
                shutdown();
            }
            else if (button.getName() == "BackToGameBtn")
            {
                popAllAndPushAppState<Pause>(findByName("GameState"));
                m_bQuit = true;
            }
            else if (button.getName() == "BackToSinbadBtn")
            {
                popAllAndPushAppState<Pause>(findByName("SinbadState"));
                m_bQuit = true;
            }
            else if (button.getName() == "BackToMenuBtn")
                popAllAndPushAppState<Pause>(findByName("MenuState"));
        }
        public override void yesNoDialogClosed(string question, bool yesHit)
        {
            if (yesHit == true)
                shutdown();
            else
                GameManager.Instance.mTrayMgr.closeDialog();

            m_bQuestionActive = false;
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
            GameManager.Instance.mTrayMgr.frameRenderingQueued(m_FrameEvent);

            if (m_bQuit == true)
            {
                popAppState();
                return;
            }
        }

        private bool m_bQuit;
        private bool m_bQuestionActive;
    }
}