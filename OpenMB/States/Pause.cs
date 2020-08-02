using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using OpenMB.Localization;
using OpenMB.Sound;
using OpenMB.Utilities;
using OpenMB.Mods;
using OpenMB.UI;
using OpenMB.UI.Widgets;

namespace OpenMB.States
{
    public class Pause : AppState
    {
        public Pause()
        {
            m_bQuit = false;
            m_bQuestionActive = false;
            frameEvent = new FrameEvent();
        }

        public override void enter(ModData e = null)
        {
            m_bQuit = false;

            sceneMgr = GameManager.Instance.root.CreateSceneManager(SceneType.ST_GENERIC, "PauseSceneMgr");
            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            sceneMgr.AmbientLight = cvAmbineLight;

            camera = sceneMgr.CreateCamera("PauseCam");
            camera.Position = new Mogre.Vector3(0, 0, 0);
            camera.NearClipDistance = 1;

            camera.AspectRatio = GameManager.Instance.viewport.ActualWidth /
            GameManager.Instance.viewport.ActualHeight;

            GameManager.Instance.viewport.Camera = camera;

            UIManager.Instance.DestroyAllWidgets();
            UIManager.Instance.ShowCursor();
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_CENTER, "BackToMenuBtn", LocateSystem.Instance.LOC(LocateFileType.GameQuickString, "Back To Game"), 250);
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_CENTER, "ExitBtn", LocateSystem.Instance.LOC(LocateFileType.GameQuickString, "Quit"), 250);
            UIManager.Instance.CreateLabel(UIWidgetLocation.TL_TOP, "PauseLbl", LocateSystem.Instance.LOC(LocateFileType.GameQuickString, "Game Paused"), 250);

            GameManager.Instance.mouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            GameManager.Instance.mouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            GameManager.Instance.mouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);

            m_bQuestionActive = true;

            createScene();
        }
        public void createScene()
        { }
        public override void exit()
        {
            if (sceneMgr != null)
            {
                sceneMgr.DestroyCamera(camera);
                GameManager.Instance.root.DestroySceneManager(sceneMgr);
            }

            UIManager.Instance.clearAllTrays();
            UIManager.Instance.SetListener(null);
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if (GameManager.Instance.keyboard.IsKeyDown(KeyCode.KC_ESCAPE) && !m_bQuestionActive)
            {
                m_bQuit = true;
                return true;
            }

            GameManager.Instance.keyPressed(keyEventRef);

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

        public override void buttonHit(ButtonWidget button)
        {
            if (button.Name == "ExitBtn")
            {
                //AdvancedMogreFramework.m_pTrayMgr.showYesNoDialog("Sure?", "Really leave?");
                //m_bQuestionActive = true;
                shutdown();
            }
            else if (button.Name == "BackToGameBtn")
            {
                popAllAndPushAppState<Pause>(findByName("GameState"));
                m_bQuit = true;
            }
            else if (button.Name == "BackToSinbadBtn")
            {
                popAllAndPushAppState<Pause>(findByName("SinbadState"));
                m_bQuit = true;
            }
            else if (button.Name == "BackToMenuBtn")
                popAllAndPushAppState<Pause>(findByName("MenuState"));
        }

        public override void update(double timeSinceLastFrame)
        {
            frameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
            UIManager.Instance.FrameRenderingQueued(frameEvent);

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