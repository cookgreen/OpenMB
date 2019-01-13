using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private string defaultRenderSystemName;
        private bool isEditMode;
        private bool isCheatMode;
        private AppStateManager appStateMgr;
        private LocateSystem locateMgr;
        private ModManager modMgr;
        private NetworkManager networkMgr;
        private OutputManager outputMgr;
        private SoundManager soundMgr;
        private ScreenManager uiMgr;
        private Dictionary<string, string> gameOptions;
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
        public Dictionary<int, GameObject> AllGameObjects;
        public Dictionary<string, uint> GameHashMap;
        public bool EDIT_MODE
        {
            get
            {
                return isEditMode;
            }
        }
        public bool CHEAT_MODE
        {
            get
            {
                return isCheatMode;
            }
        }

        private static GameManager instance;
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

        public NameValuePairList videoMode;
        

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
            isEditMode = false;
            isCheatMode = false;
         }

        public bool Init(string windowTitle, Dictionary<string, string> gameOptions)
        {
            if (!InitRender(windowTitle, ref gameOptions))
                return false;

            if (!InitSubSystem(gameOptions))
                return false;

            if (!InitGame(gameOptions))
                return false;

            return true;
        }

        private bool InitRender(string wndTitle, ref Dictionary<string, string> gameOptions)
        {
            mRoot = Root.Singleton == null ? new Root() : Root.Singleton;
            mRoot.FrameStarted += new FrameListener.FrameStartedHandler(mRoot_FrameStarted);

            mLog = EngineLogManager.Instance.CreateLog("./Log/Engine.log");
            mMogreLog = LogManager.Singleton.CreateLog("./Log/Mogre.log", true, true, false);
            mMogreLog.SetDebugOutputEnabled(true);

            RenderSystem rs = null;
            ConfigFileParser parser = new ConfigFileParser();
            if (gameOptions == null)
            {
                gameOptions = new Dictionary<string, string>();

                ConfigFile cf = parser.Load("Game.cfg");
                var sections = cf.Sections;
                foreach (var section in sections)
                {
                    foreach(var kpl in section.KeyValuePairs)
                    {
                        gameOptions.Add(kpl.Key, kpl.Value);
                    }
                }

                cf = parser.Load("ogre.cfg");
                sections = cf.Sections;
                string renderSystem = null;
                foreach (var section in sections)
                {
                    if (section.Name == "")
                    {
                        foreach (var kpl in section.KeyValuePairs)
                        {
                            renderSystem = kpl.Value;
                            gameOptions.Add(kpl.Key, kpl.Value);
                        }
                    }
                    else if(section.Name == renderSystem)
                    {
                        foreach (var kpl in section.KeyValuePairs)
                        {
                            gameOptions.Add("Render Params_" + kpl.Key, kpl.Value);
                        }
                    }
                }
            }

            defaultRenderSystemName = gameOptions.Where(o => o.Key == "Render System").First().Value;
            var renderParams = gameOptions.Where(o => o.Key.StartsWith("Render Params"));
            if (!string.IsNullOrEmpty(defaultRenderSystemName))
            {
                var videModeRenderParam = renderParams.Where(o => o.Key == "Render Params_Video Mode").First();
                rs = mRoot.GetRenderSystemByName(defaultRenderSystemName);
                string strVideoMode =  Regex.Match(
                    videModeRenderParam.Value, 
                    "[0-9]{3,4} x [0-9]{3,4}").Value;
                videoMode["Width"] = strVideoMode.Split('x')[0].Trim();
                videoMode["Height"] = strVideoMode.Split('x')[1].Trim();
            }

            var ogreConfigMap = rs.GetConfigOptions();

            if (rs != null && renderParams != null)
            {
                foreach (var kpl in renderParams)
                {
                    string renderParamKey = kpl.Key.Split('_')[1];
                    string renderParamValue = kpl.Value;
                    //Validate the render parameter
                    if (!ogreConfigMap[renderParamKey].possibleValues.Contains(renderParamValue))
                    {
                        renderParamValue = ogreConfigMap[renderParamKey].possibleValues[0];
                    }
                    rs.SetConfigOption(renderParamKey, renderParamValue);
                }
                mRoot.RenderSystem = rs;
            }
            mRenderWnd = mRoot.Initialise(true, wndTitle);
 
            mViewport = mRenderWnd.AddViewport(null);
            ColourValue cv = new ColourValue(0.5f, 0.5f, 0.5f);
            mViewport.BackgroundColour = cv;

            mViewport.Camera = null;
 
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

            if (!LocateSystem.Singleton.IsInit)
            {
                LocateSystem.Singleton.InitLocateSystem(LocateSystem.Singleton.ConvertLocateShortStringToLocateInfo(gameOptions.Where(o => o.Key == "CurrentLocate").First().Value));
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

            this.gameOptions = gameOptions;

            mLog.LogMessage("Game Started!");

            return true;
        }

        private bool InitSubSystem(Dictionary<string, string> gameOptions)
        {
            appStateMgr = new AppStateManager();
            locateMgr = LocateSystem.Singleton;
            modMgr = new ModManager();
            networkMgr = new NetworkManager();
            outputMgr = new OutputManager();
            soundMgr = new SoundManager();
            uiMgr = new ScreenManager();

            SoundManager.Instance.InitSystem(gameOptions["EnableMusic"] == "True" ? true : false, gameOptions["EnableSound"] == "True" ? true : false);

            if (!locateMgr.IsInit)
            {
                locateMgr.InitLocateSystem(locateMgr.ConvertLocateShortStringToLocateInfo(gameOptions["CurrentLocae"]));
            }
            
            Update += modMgr.Update;
            Update += outputMgr.Update;
            Update += soundMgr.Update;
            Update += uiMgr.Update;
            
            return true;
        }

        private bool InitGame(Dictionary<string, string> gameOptions)
        {
            try
            {
                isEditMode = gameOptions["IsEnableEditMode"] == "True" ? true : false;
                return true;
            }
            catch
            {
                return false;
            }
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
            if(mKeyboard.IsKeyDown(KeyCode.KC_V))
            {
                mRenderWnd.WriteContentsToTimestampedFile("AMGE_ScreenShot_", ".jpg");
                outputMgr.DisplayMessage(string.Format(locateMgr.GetLocalizedString(LocateFileType.GameString,"str_screenshots_saved_to_{0}"), Environment.CurrentDirectory));
                return true;
            }
            else if(mKeyboard.IsKeyDown(KeyCode.KC_O))
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
            else if (mKeyboard.IsKeyDown(KeyCode.KC_LSHIFT) && 
                     mKeyboard.IsKeyDown(KeyCode.KC_SPACE))//Left Shift + Space
            {
                mRenderWnd.SetFullscreen(
                    !mRenderWnd.IsFullScreen, 
                    Convert.ToUInt32(videoMode["Width"]), 
                    Convert.ToUInt32(videoMode["Height"])
                );
            }
            else if(mKeyboard.IsKeyDown(KeyCode.KC_LSHIFT) &&
                    mKeyboard.IsKeyDown(KeyCode.KC_I))//Left Shift + I
            {
                if(!uiMgr.CheckScreenIsVisual("Console"))
                {
                    uiMgr.ChangeScreen("Console");
                }
                else
                {
                    uiMgr.ExitCurrentScreen();
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

        public void Dispose()
        {
            mRoot.Dispose();
            mTrayMgr.Dispose();
            mTimer.Dispose();
        }
    }
}
