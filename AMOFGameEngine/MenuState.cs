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
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Entering MenuState...");
            m_bQuit = false;

            if (AdvancedMogreFramework.Singleton.ogg == null)
            {
                AdvancedMogreFramework.Singleton.ogg = new OggSound();
                AdvancedMogreFramework.Singleton.ogg.OggFileName = @"./vivaldi_winter_allegro.ogg";
                AdvancedMogreFramework.Singleton.ogg.PlayOgg();
            }

            m_SceneMgr = AdvancedMogreFramework.Singleton.m_Root.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");

            ColourValue cvAmbineLight=new ColourValue(0.7f,0.7f,0.7f);
            m_SceneMgr.AmbientLight=cvAmbineLight;
 
            m_Camera = m_SceneMgr.CreateCamera("MenuCam");
            m_Camera.SetPosition(0,25,-50);
            Mogre.Vector3 vectorCameraLookat=new Mogre.Vector3(0,0,0);
            m_Camera.LookAt(vectorCameraLookat);
            m_Camera.NearClipDistance=1;//setNearClipDistance(1);
 
            m_Camera.AspectRatio=AdvancedMogreFramework.Singleton.m_Viewport.ActualWidth / AdvancedMogreFramework.Singleton.m_Viewport.ActualHeight;
 
            AdvancedMogreFramework.Singleton.m_Viewport.Camera=m_Camera;

            AdvancedMogreFramework.Singleton.m_TrayMgr.destroyAllWidgets();
            AdvancedMogreFramework.Singleton.m_TrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
            AdvancedMogreFramework.Singleton.m_TrayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
            AdvancedMogreFramework.Singleton.m_TrayMgr.showCursor();
            AdvancedMogreFramework.Singleton.m_TrayMgr.createButton(TrayLocation.TL_CENTER, "EnterBtn", "Enter GameState", 250);
            AdvancedMogreFramework.Singleton.m_TrayMgr.createButton(TrayLocation.TL_CENTER, "EnterSinbadBtn", "Enter SinbadState", 250);
            AdvancedMogreFramework.Singleton.m_TrayMgr.createButton(TrayLocation.TL_CENTER, "EnterPhysxBtn", "Enter PhysxState", 250);
            AdvancedMogreFramework.Singleton.m_TrayMgr.createButton(TrayLocation.TL_CENTER, "ExitBtn", "Exit Demo", 250);
            AdvancedMogreFramework.Singleton.m_TrayMgr.createLabel(TrayLocation.TL_TOP, "MenuLbl", "Menu mode", 250);

            AdvancedMogreFramework.Singleton.m_Mouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            AdvancedMogreFramework.Singleton.m_Mouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            AdvancedMogreFramework.Singleton.m_Mouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            AdvancedMogreFramework.Singleton.m_Keyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            AdvancedMogreFramework.Singleton.m_Keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);
            createScene();
        }
        public void createScene()
        { }
        public override void exit()
        {
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Leaving MenuState...");
 
            m_SceneMgr.DestroyCamera(m_Camera);
            if(m_SceneMgr!=null)
                AdvancedMogreFramework.Singleton.m_Root.DestroySceneManager(m_SceneMgr);

            AdvancedMogreFramework.Singleton.m_TrayMgr.clearAllTrays();
            AdvancedMogreFramework.Singleton.m_TrayMgr.destroyAllWidgets();
            AdvancedMogreFramework.Singleton.m_TrayMgr.setListener(null);
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
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
            if (button.getName() == "ExitBtn")
                m_bQuit = true;
            else if (button.getName() == "EnterBtn")
                changeAppState(findByName("GameState"));
            else if (button.getName() == "EnterPhysxBtn")
                changeAppState(findByName("PhysxState"));
            else if (button.getName() == "EnterSinbadBtn")
                changeAppState(findByName("SinbadState"));
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
            AdvancedMogreFramework.Singleton.m_TrayMgr.frameRenderingQueued(m_FrameEvent);
 
            if(m_bQuit == true)
            {
                shutdown();
                return;
            }
        }

        protected bool m_bQuit;
    }
}
