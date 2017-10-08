using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural;
using Mogre_Procedural.MogreBites;
using NVorbis;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Maps;
using AMOFGameEngine.Mods;
using AMOFGameEngine.RPG;
using AMOFGameEngine.Sound;
using AMOFGameEngine.States;
using AMOFGameEngine.Widgets;
using AMOFGameEngine.Utilities;
using Editor;

namespace AMOFGameEngine
{
    public class GameManager : IDisposable
    {
        public Root mRoot;
        public RenderWindow mRenderWnd;
        public Viewport mViewport;
        public Log mLog;
        public Timer mTimer;
        public MOIS.InputManager mInputMgr;
        public Keyboard mKeyboard;
        public Mouse mMouse;
        public GameTrayManager mTrayMgr;
        public static string LastStateName;

        public event Action<float> Update;

        public OggSound ogg;

        private string defaultRS;

        private AppStateManager mAppStateMgr;
        private SoundManager mSoundMgr;
        private ModManager mModMgr;
        private LocateSystem mLocateMgr;

        public MogreConsole console;

        public List<RPGObject> AllGameObjects;
        public Dictionary<string, uint> GameHashMap;

        static GameManager instance;
        public static GameManager Instance
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

        private NameValuePairList videoMode;
        

        public GameManager()
        {
            mRoot = null;
            mRenderWnd = null;
            mViewport = null;
            mLog = null;
            mTimer = null;

            mInputMgr = null;
            mKeyboard = null;
            mMouse = null;
            mTrayMgr = null;
            mAppStateMgr = null;
            mSoundMgr = null;
            console = new MogreConsole();
            AllGameObjects = new List<RPGObject>();
            GameHashMap = new Dictionary<string, uint>();
            videoMode = new NameValuePairList();
         }

        public bool InitRender(String wndTitle, List<OgreConfigNode> renderconfigs,Root r)
        {
            LogManager logMgr = new LogManager();
 
            mLog = LogManager.Singleton.CreateLog("./Log/amof.log", true, true, false);
            mLog.SetDebugOutputEnabled(true);

            mRoot = r;
            mRoot.FrameStarted += new FrameListener.FrameStartedHandler(mRoot_FrameStarted);

            RenderSystem rs = null;

            defaultRS = renderconfigs.Where(o => o.Section == "").FirstOrDefault().Settings["Render System"];
            if (!string.IsNullOrEmpty(defaultRS))
            {
                rs = mRoot.GetRenderSystemByName(defaultRS);
                string strVideoMode =  renderconfigs.Where(o => o.Section == defaultRS).FirstOrDefault().Settings["Video Mode"];
                videoMode["Width"] = strVideoMode.Split('x')[0].Trim();
                videoMode["Height"] = strVideoMode.Split('x')[1].Trim();
            }
            if (rs != null && renderconfigs != null)
            {
                OgreConfigNode node = renderconfigs.Where(o => o.Section == defaultRS).First();
                if (!string.IsNullOrEmpty(node.Section))
                {

                    foreach (KeyValuePair<string, string> kpl in node.Settings)
                    {
                        rs.SetConfigOption(kpl.Key, kpl.Value);
                    }
                }
                mRoot.RenderSystem = rs;
            }
            mRenderWnd = mRoot.Initialise(true, wndTitle);
 
            mViewport = mRenderWnd.AddViewport(null);
            ColourValue cv=new ColourValue(0.5f,0.5f,0.5f);
            mViewport.BackgroundColour=cv;
 
            mViewport.Camera=null;
 
            int hWnd = 0;
            
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

            mTrayMgr = new GameTrayManager("AMOFTrayMgr", mRenderWnd, mMouse, new SdkTrayListener() );

            mTimer = new Timer();
            mTimer.Reset();
 
            mRenderWnd.IsActive=true;
            return true;
        }

        bool mRoot_FrameStarted(FrameEvent evt)
        {
            if (Update != null)
            {
                Update(evt.timeSinceLastFrame);
            }
            UpdateGame(evt.timeSinceLastFrame);
            UpdateRender(evt.timeSinceLastFrame);
            return true;
        }

        public bool InitSubSystem(Dictionary<string, string> gameOptions)
        {
            //console.InitConsole(ref mRoot);
            console.AddCommand("help", new MogreConsole.CommandDelegate(console_showHelp));

            mLocateMgr = LocateSystem.Singleton;
            if (!LocateSystem.Singleton.IsInit)
            {
                LocateSystem.Singleton.InitLocateSystem(mLocateMgr.GetLanguageFromFile());
            }
            mSoundMgr = new SoundManager();
            if (gameOptions["IsEnableMusic"] == "True")
            {
                mSoundMgr.SystemInit();
            }

            mModMgr = new ModManager();
            mAppStateMgr = new AppStateManager();
            return true;
        }

        private void InitGame()
        {
            
        }

        public void UpdateRender(double timeSinceLastFrame)
        {
        }

        public void UpdateGame(double timeSinceLastFrame)
        {
            foreach (var eachGameObj in AllGameObjects)
            {
                eachGameObj.Update((float)timeSinceLastFrame);
            }
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

            if (mKeyboard.IsKeyDown(KeyCode.KC_HOME))
            {
                console.Visible = true;
            }

            if (mKeyboard.IsKeyDown(KeyCode.KC_LSHIFT) && mKeyboard.IsKeyDown(KeyCode.KC_SPACE))
            {
                mRenderWnd.SetFullscreen(!mRenderWnd.IsFullScreen, Convert.ToUInt32(videoMode["Width"]), Convert.ToUInt32(videoMode["Height"]));
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

        void console_showHelp(List<string> args)
        {

        }

        public void Dispose()
        {
            mRoot.Dispose();
            mTrayMgr.Dispose();
            mTimer.Dispose();
        }
    }
}
