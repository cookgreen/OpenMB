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
        public Root m_Root;
        public RenderWindow m_RenderWnd;
        public Viewport m_Viewport;
        public Log m_Log;
        public Timer m_pTimer;

        public MOIS.InputManager m_InputMgr;
        public Keyboard m_Keyboard;
        public Mouse m_Mouse;

        public SdkTrayManager m_TrayMgr;

        public static string LastStateName;

        public OggSound ogg;

        public bool IsConfigCancelClick;

        private NameValuePairList nvl;
        private List<nvlsection> nvll;
        private string defaultRS;
        private struct nvlsection
        {
            public NameValuePairList nvl;
            public string section;
        }
        nvlsection ns;
        Settings s;
        List<Settings> sl = new List<Settings>();
        
        public AdvancedMogreFramework()
        {
            m_Root = null;
            m_RenderWnd = null;
            m_Viewport = null;
            m_Log = null;
            m_pTimer = null;

            m_InputMgr = null;
            m_Keyboard = null;
            m_Mouse = null;
            m_TrayMgr = null;
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
 
            m_Log = LogManager.Singleton.CreateLog("amof.log", true, true, false);
            m_Log.SetDebugOutputEnabled(true);
 
            m_Root = new Root();

            ConfigFile cfo=new ConfigFile();
            ReadSettingsFromConfig(cfo, "ogre.cfg");

            RenderSystem rs = m_Root.GetRenderSystemByName(defaultRS);
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
            m_Root.RenderSystem = rs;
               m_RenderWnd = m_Root.Initialise(true, wndTitle);
 
            m_Viewport = m_RenderWnd.AddViewport(null);
            ColourValue cv=new ColourValue(0.5f,0.5f,0.5f);
            m_Viewport.BackgroundColour=cv;
 
            m_Viewport.Camera=null;
 
            int hWnd = 0;
            //ParamList paramList;
            m_RenderWnd.GetCustomAttribute("WINDOW", out hWnd);
 
            m_InputMgr = InputManager.CreateInputSystem((uint)hWnd);
            m_Keyboard = (MOIS.Keyboard)m_InputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            m_Mouse =  (MOIS.Mouse)m_InputMgr.CreateInputObject(MOIS.Type.OISMouse, true);

            m_Mouse.MouseMoved+=new MouseListener.MouseMovedHandler(mouseMoved);
            m_Mouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            m_Mouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);

            m_Keyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            m_Keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);

            MOIS.MouseState_NativePtr mouseState = m_Mouse.MouseState;
                mouseState.width = m_Viewport.ActualWidth;
                mouseState.height = m_Viewport.ActualHeight;
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

            String secName1, typeName1, archName1;
            ConfigFile cfMusic = new ConfigFile();
            cfMusic.Load("music.cfg", "\t:=", true);
            ConfigFile.SectionIterator seci1 = cfMusic.GetSectionIterator();
            while (seci1.MoveNext())
            {
                secName1 = seci1.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci1.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName1 = pair.Key;
                    archName1 = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName1, typeName1, secName1);
                }
            }

            TextureManager.Singleton.DefaultNumMipmaps=5;
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();

            if (!Models.LocateSystem.IsInit)
                Models.LocateSystem.InitLocateSystem(Models.LocateSystem.getLanguageFromFile());

            m_TrayMgr = new SdkTrayManager("AMOFTrayMgr", m_RenderWnd, m_Mouse, null);

            m_pTimer = new Timer();
            m_pTimer.Reset();
 
            m_RenderWnd.IsActive=true;
            return true;
        }
        public void updateOgre(double timeSinceLastFrame)
        {
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
             if(m_Keyboard.IsKeyDown(MOIS.KeyCode.KC_V))
            {
                m_RenderWnd.WriteContentsToTimestampedFile("AMOF_Screenshot_", ".jpg");
                return true;
            }
 
            if(m_Keyboard.IsKeyDown(MOIS.KeyCode.KC_O))
            {
                if(m_TrayMgr.isLogoVisible())
                {
                    m_TrayMgr.hideFrameStats();
                    m_TrayMgr.hideLogo();
                }
                else
                {
                    m_TrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
                    m_TrayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
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
