using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;

namespace AMOFGameEngine
{
    class MenuState : AppState
    {
        public MenuState()
        {
            m_bQuit         = false;
            m_FrameEvent    = new FrameEvent();
        }
        public override void enter()
        {
            AdvancedMogreFramework.Singleton.m_pLog.LogMessage("Entering MenuState...");
            m_bQuit = false;

            AdvancedMogreFramework.Singleton.m_pVorbis = new NAudio.Vorbis.VorbisWaveReader(@".\vivaldi_winter_allegro.ogg");
            AdvancedMogreFramework.Singleton.m_pWaveOut= new NAudio.Wave.WaveOut();
            AdvancedMogreFramework.Singleton.m_pWaveOut.Init(AdvancedMogreFramework.Singleton.m_pVorbis);
            AdvancedMogreFramework.Singleton.m_pWaveOut.Play();
 
            m_pSceneMgr = AdvancedMogreFramework.Singleton.m_pRoot.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");
            ColourValue cvAmbineLight=new ColourValue(0.7f,0.7f,0.7f);
            m_pSceneMgr.AmbientLight=cvAmbineLight;
 
            m_pCamera = m_pSceneMgr.CreateCamera("MenuCam");
            m_pCamera.SetPosition(0,25,-50);
            Mogre.Vector3 vectorCameraLookat=new Mogre.Vector3(0,0,0);
            m_pCamera.LookAt(vectorCameraLookat);
            m_pCamera.NearClipDistance=1;//setNearClipDistance(1);
 
            m_pCamera.AspectRatio=AdvancedMogreFramework.Singleton.m_pViewport.ActualWidth / AdvancedMogreFramework.Singleton.m_pViewport.ActualHeight;
 
            AdvancedMogreFramework.Singleton.m_pViewport.Camera=m_pCamera;

            AdvancedMogreFramework.Singleton.m_pTrayMgr.destroyAllWidgets();
            AdvancedMogreFramework.Singleton.m_pTrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
            AdvancedMogreFramework.Singleton.m_pTrayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
            AdvancedMogreFramework.Singleton.m_pTrayMgr.showCursor();
            AdvancedMogreFramework.Singleton.m_pTrayMgr.createButton(TrayLocation.TL_CENTER, "EnterBtn", "Enter GameState", 250);
            AdvancedMogreFramework.Singleton.m_pTrayMgr.createButton(TrayLocation.TL_CENTER, "EnterSinbadBtn", "Enter SinbadState", 250);
            AdvancedMogreFramework.Singleton.m_pTrayMgr.createButton(TrayLocation.TL_CENTER, "ExitBtn", "Exit AdvancedOgreFramework", 250);
            AdvancedMogreFramework.Singleton.m_pTrayMgr.createLabel(TrayLocation.TL_TOP, "MenuLbl", "Menu mode", 250);

            AdvancedMogreFramework.Singleton.m_pMouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            AdvancedMogreFramework.Singleton.m_pMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            AdvancedMogreFramework.Singleton.m_pMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            AdvancedMogreFramework.Singleton.m_pKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            AdvancedMogreFramework.Singleton.m_pKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);
            createScene();
        }
        public void createScene()
        { }
        public override void exit()
        {
            AdvancedMogreFramework.Singleton.m_pLog.LogMessage("Leaving MenuState...");
 
            m_pSceneMgr.DestroyCamera(m_pCamera);
            if(m_pSceneMgr!=null)
                AdvancedMogreFramework.Singleton.m_pRoot.DestroySceneManager(m_pSceneMgr);

            AdvancedMogreFramework.Singleton.m_pTrayMgr.clearAllTrays();
            AdvancedMogreFramework.Singleton.m_pTrayMgr.destroyAllWidgets();
            AdvancedMogreFramework.Singleton.m_pTrayMgr.setListener(null);
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if(AdvancedMogreFramework.Singleton.m_pKeyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
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
            if (button.getName() == "ExitBtn")
                m_bQuit = true;
            else if (button.getName() == "EnterBtn")
                changeAppState(findByName("GameState"));
            else if (button.getName() == "EnterSinbadBtn")
                changeAppState(findByName("SinbadState"));
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
            AdvancedMogreFramework.Singleton.m_pTrayMgr.frameRenderingQueued(m_FrameEvent);
 
            if(m_bQuit == true)
            {
                shutdown();
                return;
            }
        }

        protected bool m_bQuit;
    }
}
