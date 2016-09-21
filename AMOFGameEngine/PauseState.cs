using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;

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
            AdvancedMogreFramework.Singleton.m_pLog.LogMessage("Entering PauseState...");
            m_bQuit = false;
 
            m_pSceneMgr = AdvancedMogreFramework.Singleton.m_pRoot.CreateSceneManager(SceneType.ST_GENERIC, "PauseSceneMgr");
            ColourValue cvAmbineLight=new ColourValue(0.7f,0.7f,0.7f);
            m_pSceneMgr.AmbientLight=cvAmbineLight;
 
            m_pCamera = m_pSceneMgr.CreateCamera("PauseCam");
            Mogre.Vector3 vectCamPos=new Mogre.Vector3(0,25,-50);
            m_pCamera.Position=vectCamPos;
            Mogre.Vector3 vectCamLookAt=new Mogre.Vector3(0,0,0);
            m_pCamera.LookAt(vectCamLookAt);
            m_pCamera.NearClipDistance=1;
 
            m_pCamera.AspectRatio=AdvancedMogreFramework.Singleton.m_pViewport.ActualWidth /
            AdvancedMogreFramework.Singleton.m_pViewport.ActualHeight;
 
            AdvancedMogreFramework.Singleton.m_pViewport.Camera=m_pCamera;

            AdvancedMogreFramework.Singleton.m_pTrayMgr.destroyAllWidgets();
            AdvancedMogreFramework.Singleton.m_pTrayMgr.showCursor();
            AdvancedMogreFramework.Singleton.m_pTrayMgr.createButton(TrayLocation.TL_CENTER, "BackToGameBtn", "Return to GameState", 250);
            AdvancedMogreFramework.Singleton.m_pTrayMgr.createButton(TrayLocation.TL_CENTER, "BackToSinbadBtn", "Return to SinbadState", 250);
            AdvancedMogreFramework.Singleton.m_pTrayMgr.createButton(TrayLocation.TL_CENTER, "BackToMenuBtn", "Return to Menu", 250);
            AdvancedMogreFramework.Singleton.m_pTrayMgr.createButton(TrayLocation.TL_CENTER, "ExitBtn", "Exit AdvancedOgreFramework", 250);
            AdvancedMogreFramework.Singleton.m_pTrayMgr.createLabel(TrayLocation.TL_TOP, "PauseLbl", "Pause mode", 250);

            AdvancedMogreFramework.Singleton.m_pMouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            AdvancedMogreFramework.Singleton.m_pMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            AdvancedMogreFramework.Singleton.m_pMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            AdvancedMogreFramework.Singleton.m_pKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            AdvancedMogreFramework.Singleton.m_pKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);

            m_bQuestionActive = true;
 
            createScene();
        }
        public void createScene()
        { }
        public override void exit()
        {
            AdvancedMogreFramework.Singleton.m_pLog.LogMessage("Leaving PauseState...");
 
            m_pSceneMgr.DestroyCamera(m_pCamera);
            if(m_pSceneMgr!=null)
                AdvancedMogreFramework.Singleton.m_pRoot.DestroySceneManager(m_pSceneMgr);

            AdvancedMogreFramework.Singleton.m_pTrayMgr.clearAllTrays();
            AdvancedMogreFramework.Singleton.m_pTrayMgr.destroyAllWidgets();
            AdvancedMogreFramework.Singleton.m_pTrayMgr.setListener(null);
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if(AdvancedMogreFramework.Singleton.m_pKeyboard.IsKeyDown(KeyCode.KC_ESCAPE) && !m_bQuestionActive)
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
            if (AdvancedMogreFramework.Singleton.m_pTrayMgr.injectMouseMove(evt)) return true;
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            if (AdvancedMogreFramework.Singleton.m_pTrayMgr.injectMouseDown(evt, id)) return true;
            return true;
        }
        public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            if (AdvancedMogreFramework.Singleton.m_pTrayMgr.injectMouseUp(evt, id)) return true;
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
                AdvancedMogreFramework.Singleton.m_pTrayMgr.closeDialog();
 
            m_bQuestionActive = false;
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
            AdvancedMogreFramework.Singleton.m_pTrayMgr.frameRenderingQueued(m_FrameEvent);
 
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
