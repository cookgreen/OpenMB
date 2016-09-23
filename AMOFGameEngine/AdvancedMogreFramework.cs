using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural;
using Mogre_Procedural.MogreBites;
using NVorbis;

namespace AMOFGameEngine
{
    class AdvancedMogreFramework 
    {
        public Root m_pRoot;
        public RenderWindow m_pRenderWnd;
        public Viewport m_pViewport;
        public Log m_pLog;
        public Timer m_pTimer;

        public MOIS.InputManager m_pInputMgr;
        public Keyboard m_pKeyboard;
        public Mouse m_pMouse;

        public SdkTrayManager m_pTrayMgr;

        //public NVorbis.NAudioSupport.VorbisWaveReader m_pVorbis;
        public NAudio.Vorbis.VorbisWaveReader m_pVorbis;
        public NAudio.Wave.WaveOut m_pWaveOut;
        
        public AdvancedMogreFramework()
        {
            m_pRoot = null;
            m_pRenderWnd = null;
            m_pViewport = null;
            m_pLog = null;
            m_pTimer = null;

            m_pInputMgr = null;
            m_pKeyboard = null;
            m_pMouse = null;
            m_pTrayMgr = null;
         }
        ~AdvancedMogreFramework()
        {
            //LogManager.Singleton.LogMessage("Shutdown OGRE...");
            //if (AdvancedMogreFramework.m_pTrayMgr != null) m_pTrayMgr = null;
            //if (AdvancedMogreFramework.m_pInputMgr != null) InputManager.DestroyInputSystem(m_pInputMgr);
            //if (AdvancedMogreFramework.m_pRoot != null) m_pRoot = null;
        }

        public bool initOgre(String wndTitle)
        {
            LogManager logMgr = new LogManager();
 
            m_pLog = LogManager.Singleton.CreateLog("OgreLogfile.log", true, true, false);
            m_pLog.SetDebugOutputEnabled(true);
 
            m_pRoot = new Root();
 
            if(!m_pRoot.ShowConfigDialog())
                return false;
               m_pRenderWnd = m_pRoot.Initialise(true, wndTitle);
 
            m_pViewport = m_pRenderWnd.AddViewport(null);
            ColourValue cv=new ColourValue(0.5f,0.5f,0.5f);
            m_pViewport.BackgroundColour=cv;
 
            m_pViewport.Camera=null;
 
            int hWnd = 0;
            //ParamList paramList;
            m_pRenderWnd.GetCustomAttribute("WINDOW", out hWnd);
 
            m_pInputMgr = InputManager.CreateInputSystem((uint)hWnd);
            m_pKeyboard = (MOIS.Keyboard)m_pInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            m_pMouse =  (MOIS.Mouse)m_pInputMgr.CreateInputObject(MOIS.Type.OISMouse, true);

            m_pMouse.MouseMoved+=new MouseListener.MouseMovedHandler(mouseMoved);
            m_pMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            m_pMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);

            m_pKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            m_pKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);

            MOIS.MouseState_NativePtr mouseState = m_pMouse.MouseState;
                mouseState.width = m_pViewport.ActualWidth;
                mouseState.height = m_pViewport.ActualHeight;
            //m_pMouse.MouseState = tempMouseState;

 
            String secName, typeName, archName;
            ConfigFile cf=new ConfigFile();
            cf.Load("resources.cfg","\t:=",true);
 
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName = pair.Key;
                    archName = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
                }
            }
            TextureManager.Singleton.DefaultNumMipmaps=5;
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups(); 
 
            m_pTrayMgr = new SdkTrayManager("AOFTrayMgr", m_pRenderWnd, m_pMouse, null);
 
            m_pTimer = new Timer();
            m_pTimer.Reset();
 
            m_pRenderWnd.IsActive=true;
 
            return true;
        }
        public void updateOgre(double timeSinceLastFrame)
        {
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
             if(m_pKeyboard.IsKeyDown(MOIS.KeyCode.KC_V))
            {
                m_pRenderWnd.WriteContentsToTimestampedFile("AMOF_Screenshot_", ".jpg");
                return true;
            }
 
            if(m_pKeyboard.IsKeyDown(MOIS.KeyCode.KC_O))
            {
                if(m_pTrayMgr.isLogoVisible())
                {
                    m_pTrayMgr.hideFrameStats();
                    m_pTrayMgr.hideLogo();
                }
                else
                {
                    m_pTrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
                    m_pTrayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
                }
            }
 
            return true;
        }
        public bool keyReleased(KeyEvent keyEventRef)
        {
            return true;
        }

        public bool mouseMoved(MouseEvent evt)
        {
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            return true;
        }
        public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            return true;
        }
        public float Clamp(float val, float minval, float maxval)
        {
            return System.Math.Max(System.Math.Min(val, maxval), minval);
        }
        public static AdvancedMogreFramework instance;
        public static AdvancedMogreFramework Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdvancedMogreFramework();
                }
                return instance;
            }
        }
    }
}
