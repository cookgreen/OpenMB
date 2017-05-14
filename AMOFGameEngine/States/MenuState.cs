using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Sound;
using AMOFGameEngine.Utilities;
using AMOFGameEngine.Models;

namespace AMOFGameEngine.States
{
    class MenuState : AppState
    {
        public MenuState()
        {
            m_bQuit         = false;
            m_FrameEvent    = new FrameEvent();
        }
        public override void enter(AppStateArgs e=null)
        {
            GameManager.Singleton.mLog.LogMessage("Entering MenuState...");
            m_bQuit = false;

            if (GameManager.Singleton.ogg == null)
            {
                GameManager.Singleton.ogg = new OggSound();
                GameManager.Singleton.ogg.OggFileName = @"./Music/vivaldi_winter_allegro.ogg";
                GameManager.Singleton.ogg.PlayOgg();
            }

            m_SceneMgr = GameManager.Singleton.mRoot.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");

            ColourValue cvAmbineLight=new ColourValue(0.7f,0.7f,0.7f);
            m_SceneMgr.AmbientLight=cvAmbineLight;
 
            m_Camera = m_SceneMgr.CreateCamera("MenuCam");
            m_Camera.SetPosition(0,25,-50);
            Mogre.Vector3 vectorCameraLookat=new Mogre.Vector3(0,0,0);
            m_Camera.LookAt(vectorCameraLookat);
            m_Camera.NearClipDistance=1;//setNearClipDistance(1);
 
            m_Camera.AspectRatio=GameManager.Singleton.mViewport.ActualWidth / GameManager.Singleton.mViewport.ActualHeight;
 
            GameManager.Singleton.mViewport.Camera=m_Camera;

            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            GameManager.Singleton.mTrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
            GameManager.Singleton.mTrayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
            GameManager.Singleton.mTrayMgr.showCursor();
            GameManager.Singleton.mTrayMgr.createLabel(TrayLocation.TL_TOP, "MenuLbl",e!=null?LocateSystem.LOC(e.modName):LocateSystem.LOC("MenuState") , 400);

            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "SinglePlayer", LocateSystem.LOC("Single Player"),250);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "LoadGame", LocateSystem.LOC("Load Game"), 250);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "MultiPlayer", LocateSystem.LOC("Multiplayer"), 250);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "Option", LocateSystem.LOC("Option"), 250);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "Quit", LocateSystem.LOC("Quit"), 250);

            GameManager.Singleton.mMouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            GameManager.Singleton.mMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            GameManager.Singleton.mMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            GameManager.Singleton.mKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            GameManager.Singleton.mKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);
            createScene();
        }
        public void createScene()
        { }
        public override void exit()
        {
            GameManager.Singleton.mLog.LogMessage("Leaving MenuState...");
 
            m_SceneMgr.DestroyCamera(m_Camera);
            if(m_SceneMgr!=null)
                GameManager.Singleton.mRoot.DestroySceneManager(m_SceneMgr);

            GameManager.Singleton.mTrayMgr.setListener(null);
            GameManager.Singleton.mTrayMgr.clearAllTrays();
            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if(GameManager.Singleton.mKeyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
            {
                m_bQuit = true;
                return true;
            }

            GameManager.Singleton.keyPressed(keyEventRef);
            return true;
        }
        public bool keyReleased(KeyEvent keyEventRef)
        {
            GameManager.Singleton.keyReleased(keyEventRef);
            return true;
        }

        public bool mouseMoved(MouseEvent evt)
        {
            if (GameManager.Singleton.mTrayMgr.injectMouseMove(evt)) return true;
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Singleton.mTrayMgr.injectMouseDown(evt, id)) return true;
            return true;
        }
        public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Singleton.mTrayMgr.injectMouseUp(evt, id)) return true;
            return true;
        }

        public override void buttonHit(Button button)
        {
            if (button.getName() == "Quit")
                m_bQuit = true;
            else if (button.getName() == "LoadGame")
                changeAppState(findByName("GameState"));
            else if (button.getName() == "MultiPlayer")
                changeAppState(findByName("PhysxState"));
            else if (button.getName() == "SinglePlayer")
                changeAppState(findByName("SinbadState"));
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
            GameManager.Singleton.mTrayMgr.frameRenderingQueued(m_FrameEvent);
 
            if(m_bQuit == true)
            {
                shutdown();
                return;
            }
        }

        protected bool m_bQuit;
    }
}
