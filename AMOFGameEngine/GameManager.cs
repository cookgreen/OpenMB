using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural;
using Mogre_Procedural.MogreBites;
using NVorbis;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Sound;
using AMOFGameEngine.Utilities;

namespace AMOFGameEngine
{
    class GameManager 
    {
        public Root mRoot;
        public RenderWindow mRenderWnd;
        public Viewport mViewport;
        public Log mLog;
        public Timer m_pTimer;

        public MOIS.InputManager mInputMgr;
        public Keyboard mKeyboard;
        public Mouse mMouse;

        public SdkTrayManager mTrayMgr;

        public static string LastStateName;

        public OggSound ogg;

        private NameValuePairList nvl;
        private List<nvlsection> nvll;
        private string defaultRS;
        private struct nvlsection
        {
            public NameValuePairList nvl;
            public string section;
        }
        nvlsection ns;
        OgreConfigNode s;
        List<OgreConfigNode> sl = new List<OgreConfigNode>();
        
        public GameManager()
        {
            mRoot = null;
            mRenderWnd = null;
            mViewport = null;
            mLog = null;
            m_pTimer = null;

            mInputMgr = null;
            mKeyboard = null;
            mMouse = null;
            mTrayMgr = null;
            nvl = new NameValuePairList();
            nvll = new List<nvlsection>();
            ns = new nvlsection();
         }
        private void ReadSettingsFromConfig(ConfigFile cf,string filename)
        {
            String secName;
            cf.Load(filename, "\t:=", true);

            ConfigFile.SectionIterator seci = cf.GetSectionIterator();
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    s.section = secName;
                    s.settings = settings;
                    nvl[pair.Key] = pair.Value;
                    if(pair.Key=="Render System" && !string.IsNullOrEmpty(pair.Value))
                    {
                        defaultRS = pair.Value;
                    }
                }
                sl.Add(s);
                ns.nvl = nvl;
                ns.section = secName;
                nvll.Add(ns);
            }
        }
        public bool initOgre(String wndTitle)
        {
            LogManager logMgr = new LogManager();
 
            mLog = LogManager.Singleton.CreateLog("amof.log", true, true, false);
            mLog.SetDebugOutputEnabled(true);
 
            mRoot = new Root();

            ConfigFile cfo=new ConfigFile();
            ReadSettingsFromConfig(cfo, "ogre.cfg");

            RenderSystem rs = mRoot.GetRenderSystemByName(defaultRS);
            for (int i = 0; i < sl.Count;i++ )
            {
                if (!string.IsNullOrEmpty(sl[i].section) && sl[i].section == defaultRS)
                {
                    foreach (KeyValuePair<string, string> p in sl[i].settings)
                    {
                        rs.SetConfigOption(p.Key, p.Value);
                    }
                }
            }
            mRoot.RenderSystem = rs;
               mRenderWnd = mRoot.Initialise(true, wndTitle);
 
            mViewport = mRenderWnd.AddViewport(null);
            ColourValue cv=new ColourValue(0.5f,0.5f,0.5f);
            mViewport.BackgroundColour=cv;
 
            mViewport.Camera=null;
 
            int hWnd = 0;
            //ParamList paramList;
            mRenderWnd.GetCustomAttribute("WINDOW", out hWnd);
 
            mInputMgr = InputManager.CreateInputSystem((uint)hWnd);
            mKeyboard = (MOIS.Keyboard)mInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            mMouse =  (MOIS.Mouse)mInputMgr.CreateInputObject(MOIS.Type.OISMouse, true);

            mMouse.MouseMoved+=new MouseListener.MouseMovedHandler(mouseMoved);
            mMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            mMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);

            mKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            mKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);

            MOIS.MouseState_NativePtr mouseState = mMouse.MouseState;
                mouseState.width = mViewport.ActualWidth;
                mouseState.height = mViewport.ActualHeight;
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

            mTrayMgr = new SdkTrayManager("AMOFTrayMgr", mRenderWnd, mMouse, null);

            m_pTimer = new Timer();
            m_pTimer.Reset();
 
            mRenderWnd.IsActive=true;
            return true;
        }
        public bool initGame()
        {
            if (!LocateSystem.IsInit)
                LocateSystem.InitLocateSystem(LocateSystem.getLanguageFromFile());
            return true;
        }
        public void updateOgre(double timeSinceLastFrame)
        {
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
             if(mKeyboard.IsKeyDown(MOIS.KeyCode.KC_V))
            {
                mRenderWnd.WriteContentsToTimestampedFile("AMOF_Screenshot_", ".jpg");
                return true;
            }
 
            if(mKeyboard.IsKeyDown(MOIS.KeyCode.KC_O))
            {
                if(mTrayMgr.isLogoVisible())
                {
                    mTrayMgr.hideFrameStats();
                    mTrayMgr.hideLogo();
                }
                else
                {
                    mTrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
                    mTrayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
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
        public static GameManager instance;
        public static GameManager Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }
    }
}
