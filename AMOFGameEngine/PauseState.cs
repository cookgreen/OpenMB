using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Sound;
using AMOFGameEngine.Utilities;

namespace AMOFGameEngine
{
    class PauseState : AppState
    {
        public PauseState()
        {
            m_bQuit             = false;
            m_bQuestionActive   = false;
            m_FrameEvent        = new FrameEvent();
        }

        public override void enter()
        {
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Entering PauseState...");
            m_bQuit = false;
 
            m_SceneMgr = AdvancedMogreFramework.Singleton.m_Root.CreateSceneManager(SceneType.ST_GENERIC, "PauseSceneMgr");
            ColourValue cvAmbineLight=new ColourValue(0.7f,0.7f,0.7f);
            m_SceneMgr.AmbientLight=cvAmbineLight;
 
            m_Camera = m_SceneMgr.CreateCamera("PauseCam");
            Mogre.Vector3 vectCamPos=new Mogre.Vector3(0,25,-50);
            m_Camera.Position=vectCamPos;
            Mogre.Vector3 vectCamLookAt=new Mogre.Vector3(0,0,0);
            m_Camera.LookAt(vectCamLookAt);
            m_Camera.NearClipDistance=1;
 
            m_Camera.AspectRatio=AdvancedMogreFramework.Singleton.m_Viewport.ActualWidth /
            AdvancedMogreFramework.Singleton.m_Viewport.ActualHeight;
 
            AdvancedMogreFramework.Singleton.m_Viewport.Camera=m_Camera;

            AdvancedMogreFramework.Singleton.m_TrayMgr.destroyAllWidgets();
            AdvancedMogreFramework.Singleton.m_TrayMgr.showCursor();
            switch(AdvancedMogreFramework.LastStateName)
            {
                case "GameState":
                    AdvancedMogreFramework.Singleton.m_TrayMgr.createButton(TrayLocation.TL_CENTER, "BackToGameBtn", LocateSystem.CreateLocateString("11161242"), 250);
                break;
                case "SinbadState":
                AdvancedMogreFramework.Singleton.m_TrayMgr.createButton(TrayLocation.TL_CENTER, "BackToSinbadBtn", LocateSystem.CreateLocateString("11161243"), 250);
                break;
                case "PhysxState":
                AdvancedMogreFramework.Singleton.m_TrayMgr.createButton(TrayLocation.TL_CENTER, "BackToPhysxBtn", LocateSystem.CreateLocateString("11161244"), 250);
                break;
            }
            AdvancedMogreFramework.Singleton.m_TrayMgr.createButton(TrayLocation.TL_CENTER, "BackToMenuBtn", LocateSystem.CreateLocateString("11161245"), 250);
            AdvancedMogreFramework.Singleton.m_TrayMgr.createButton(TrayLocation.TL_CENTER, "ExitBtn", LocateSystem.CreateLocateString("11161246"), 250);
            AdvancedMogreFramework.Singleton.m_TrayMgr.createLabel(TrayLocation.TL_TOP, "PauseLbl", LocateSystem.CreateLocateString("11161241"), 250);

            AdvancedMogreFramework.Singleton.m_Mouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            AdvancedMogreFramework.Singleton.m_Mouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            AdvancedMogreFramework.Singleton.m_Mouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            AdvancedMogreFramework.Singleton.m_Keyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            AdvancedMogreFramework.Singleton.m_Keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);

            m_bQuestionActive = true;
 
            createScene();
        }
        public void createScene()
        { }
        public override void exit()
        {
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Leaving PauseState...");
 
            m_SceneMgr.DestroyCamera(m_Camera);
            if(m_SceneMgr!=null)
                AdvancedMogreFramework.Singleton.m_Root.DestroySceneManager(m_SceneMgr);

            AdvancedMogreFramework.Singleton.m_TrayMgr.clearAllTrays();
            AdvancedMogreFramework.Singleton.m_TrayMgr.destroyAllWidgets();
            AdvancedMogreFramework.Singleton.m_TrayMgr.setListener(null);
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_ESCAPE) && !m_bQuestionActive)
            {
                m_bQuit = true;
                return true;
            }

            AdvancedMogreFramework.Singleton.keyPressed(keyEventRef);
 
            return true;
        }
        public bool keyReleased(KeyEvent keyEventRef)
        {
            AdvancedMogreFramework.Singleton.keyReleased(keyEventRef);
 
            return true;
        }

        public bool mouseMoved(MouseEvent evt)
        {
            if (AdvancedMogreFramework.Singleton.m_TrayMgr.injectMouseMove(evt)) return true;
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            if (AdvancedMogreFramework.Singleton.m_TrayMgr.injectMouseDown(evt, id)) return true;
            return true;
        }
        public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            if (AdvancedMogreFramework.Singleton.m_TrayMgr.injectMouseUp(evt, id)) return true;
            return true;
        }

        public override void buttonHit(Button button)
        {
            if(button.getName() == "ExitBtn")
            {
                //AdvancedMogreFramework.m_pTrayMgr.showYesNoDialog("Sure?", "Really leave?");
                //m_bQuestionActive = true;
                shutdown();
            }
            else if(button.getName() == "BackToGameBtn")
            {
                popAllAndPushAppState<PauseState>(findByName("GameState"));
                m_bQuit = true;
            }
            else if (button.getName() == "BackToSinbadBtn")
            {
                popAllAndPushAppState<PauseState>(findByName("SinbadState"));
                m_bQuit = true;
            }
            else if(button.getName() == "BackToMenuBtn")
                popAllAndPushAppState<PauseState>(findByName("MenuState"));
        }
        public override void yesNoDialogClosed(string question, bool yesHit)
        {
            if(yesHit == true)
                shutdown();
            else
                AdvancedMogreFramework.Singleton.m_TrayMgr.closeDialog();
 
            m_bQuestionActive = false;
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
            AdvancedMogreFramework.Singleton.m_TrayMgr.frameRenderingQueued(m_FrameEvent);
 
            if(m_bQuit == true)
            {
                popAppState();
                return;
            }
        }

        private bool m_bQuit;
        private bool m_bQuestionActive;
    }
}
