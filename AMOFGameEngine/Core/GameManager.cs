using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural;
using Mogre_Procedural.MogreBites;
using NVorbis;
using AMOFGameEngine.Core;
using AMOFGameEngine.Localization;
using AMOFGameEngine.LogMessage;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Network;
using AMOFGameEngine.Output;
using AMOFGameEngine.Game;
using AMOFGameEngine.Screen;
using AMOFGameEngine.Sound;
using AMOFGameEngine.States;
using AMOFGameEngine.Widgets;
using AMOFGameEngine.Utilities;
using ConfigFile = AMOFGameEngine.Utilities.ConfigFile;

namespace AMOFGameEngine
{
    public class GameManager : IDisposable
    {
        public Root mRoot;
        public RenderWindow mRenderWnd;
        public Viewport mViewport;
        public EngineLog mLog;
        public Log mMogreLog;
        public Timer mTimer;
        public InputManager mInputMgr;
        public Keyboard mKeyboard;
        public Mouse mMouse;
        public SdkTrayManager mTrayMgr;
        public static string LastStateName;

        public event Action<float> Update;

        private string defaultRS;

        private AppStateManager appStateMgr;
        private LocateSystem locateMgr;
        private ModManager modMgr;
        private NetworkManager networkMgr;
        private OutputManager outputMgr;
        private SoundManager soundMgr;
        private ScreenManager uiMgr;

        public Dictionary<int,GameObject> AllGameObjects;
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
            appStateMgr = null;
            soundMgr = null;
            AllGameObjects = new Dictionary<int,GameObject>();
            GameHashMap = new Dictionary<string, uint>();
            videoMode = new NameValuePairList();
         }

        public bool InitRender(String wndTitle, ConfigFile renderconfig)
        {
            mLog = EngineLogManager.Instance.CreateLog("./Log/Engine.log");
            mMogreLog = LogManager.Singleton.CreateLog("./Log/Mogre.log", true, true, false);
            mMogreLog.SetDebugOutputEnabled(true);

            mRoot = Root.Singleton;
            mRoot.FrameStarted += new FrameListener.FrameStartedHandler(mRoot_FrameStarted);

            RenderSystem rs = null;

            defaultRS = renderconfig[""]["Render System"];
            if (!string.IsNullOrEmpty(defaultRS))
            {
                rs = mRoot.GetRenderSystemByName(defaultRS);
                string strVideoMode =  renderconfig[defaultRS]["Video Mode"];
                videoMode["Width"] = strVideoMode.Split('x')[0].Trim();
                videoMode["Height"] = strVideoMode.Split('x')[1].Trim();
            }
            if (rs != null && renderconfig != null)
            {
                ConfigFileSection node = renderconfig[defaultRS];
                if (!string.IsNullOrEmpty(node.Name))
                {
                    foreach (var kpl in node.KeyValuePairs)
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
            AMOFGameEngine.Utilities.ConfigFile conf = new AMOFGameEngine.Utilities.ConfigFile();
            ConfigFileParser parser = new ConfigFileParser();
            conf = parser.Load("resources.cfg");
            for (int i = 0; i < conf.Sections.Count; i++)
            {
                secName = conf.Sections[i].Name;
                for (int j = 0; j < conf.Sections[i].KeyValuePairs.Count; j++)
                {
                    typeName = conf.Sections[i].KeyValuePairs[j].Key;
                    archName = conf.Sections[i].KeyValuePairs[j].Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
                }
            }
            ResourceGroupManager.Singleton.AddResourceLocation(
                string.Format("./Media/Engine/Fonts/{0}/", LocateSystem.Singleton.Locate.ToString()), "FileSystem",
                "General");

            TextureManager.Singleton.DefaultNumMipmaps = 5;
            
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();

            mTrayMgr = new SdkTrayManager("AMOFTrayMgr", mRenderWnd, mMouse, new SdkTrayListener() );

            mTimer = new Timer();
            mTimer.Reset();
 
            mRenderWnd.IsActive=true;

            mLog.LogMessage("Game Started!");

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
            appStateMgr = new AppStateManager();
            locateMgr = LocateSystem.Singleton;
            modMgr = new ModManager();
            networkMgr = new NetworkManager();
            outputMgr = new OutputManager();
            soundMgr = new SoundManager();
            uiMgr = new ScreenManager();

            soundMgr.InitSystem(gameOptions["IsEnableMusic"] == "True" ? true : false, gameOptions["IsEnableSound"] == "True" ? true : false);

            if (!locateMgr.IsInit)
            {
                locateMgr.InitLocateSystem(locateMgr.GetLanguageFromFile());
            }
            
            Update += modMgr.Update;
            Update += outputMgr.Update;
            Update += soundMgr.Update;
            Update += uiMgr.Update;
            
            return true;
        }

        private void InitGame()
        {
            
        }

        public void Exit()
        {
            LocateSystem.Singleton.SaveLocateFile();
            mLog.LogMessage("Game Quit!");
            mLog.Dispose();
        }

        public void UpdateRender(double timeSinceLastFrame)
        {
        }

        public void UpdateGame(double timeSinceLastFrame)
        {
            foreach (var eachGameObj in AllGameObjects)
            {
                eachGameObj.Value.Update((float)timeSinceLastFrame);
            }
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if(mKeyboard.IsKeyDown(MOIS.KeyCode.KC_V))
            {
                mRenderWnd.WriteContentsToTimestampedFile("AMGE_ScreenShot_", ".jpg");
                outputMgr.DisplayMessage(string.Format(locateMgr.GetLocalizedString(LocateFileType.GameString,"str_screenshots_saved_to_{0}"), Environment.CurrentDirectory));
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
