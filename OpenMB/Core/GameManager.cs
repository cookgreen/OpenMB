﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mogre;
using MOIS;
using Mogre_Procedural;
using Mogre_Procedural.MogreBites;
using NVorbis;
using OpenMB.Configure;
using OpenMB.Localization;
using OpenMB.LogMessage;
using OpenMB.Mods;
using OpenMB.Network;
using OpenMB.Output;
using OpenMB.Game;
using OpenMB.Screen;
using OpenMB.Sound;
using OpenMB.States;
using OpenMB.Widgets;
using OpenMB.Core;

namespace OpenMB
{
    public enum EngineState
    {
        NORMAL,
        EDIT_MODE
    }
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
        private EngineState currentState;
        public GameConfigXml gameOptions;
        public Root root;
        public RenderWindow renderWindow;
        public Viewport viewport;
        public EngineLog log;
        public Log rendererLog;
        public Timer timer;
        public InputManager inputMgr;
        public Keyboard keyboard;
        public Mouse mouse;
        public SdkTrayManager trayMgr;
        public static string LastStateName;
        public event Action<float> Update;
        public Dictionary<int, GameObject> AllGameObjects;
        public Dictionary<string, uint> GameHashMap;
        public LoadingData loadingData;
        public static string DEFAULT_PLUGIN_DIR = "Plugins";
        public static string DEFAULT_RESOURCE_DIR = "Media";
        public bool IS_ENABLE_EDIT_MODE
        {
            get
            {
                return isEditMode;
            }
        }
        public bool IS_ENABLE_CHEAT_MODE
        {
            get
            {
                return isCheatMode;
            }
        }
        public EngineState CurrentState
        {
            get {
                return currentState;
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
            root = null;
            renderWindow = null;
            viewport = null;
            log = null;
            timer = null;

            inputMgr = null;
            keyboard = null;
            mouse = null;
            trayMgr = null;
            appStateMgr = null;
            soundMgr = null;
            AllGameObjects = new Dictionary<int,GameObject>();
            GameHashMap = new Dictionary<string, uint>();
            videoMode = new NameValuePairList();
            isEditMode = false;
            isCheatMode = false;
            loadingData = new LoadingData(LoadingType.NONE, null, null, null);
         }

        public bool Init(string windowTitle, GameConfigXml gameOptions)
        {
            if (!InitRender(windowTitle, ref gameOptions))
                return false;

            if (!InitSubSystem(gameOptions))
                return false;

            if (!InitGame(gameOptions))
                return false;

            return true;
        }

        private bool InitRender(string wndTitle, ref GameConfigXml gameOptions)
        {
            root = Root.Singleton == null ? new Root() : Root.Singleton;
            root.FrameStarted += new FrameListener.FrameStartedHandler(frameStarted);

            log = EngineLogManager.Instance.CreateLog("./Log/Engine.log");
            rendererLog = LogManager.Singleton.CreateLog("./Log/Mogre.log", true, true, false);
            rendererLog.SetDebugOutputEnabled(true);

            RenderSystem rs = null;
            IniConfigFileParser parser = new IniConfigFileParser();

            if(gameOptions==null)
            {
                gameOptions = GameConfigXml.Load("game.xml");
            }

            defaultRenderSystemName = gameOptions.GraphicConfig.CurrentRenderSystem;
            var renderParams = gameOptions.GraphicConfig[gameOptions.GraphicConfig.CurrentRenderSystem];
            if (!string.IsNullOrEmpty(defaultRenderSystemName))
            {
                var videModeRenderParam = renderParams.Where(o => o.Name == "Video Mode").First();
                rs = root.GetRenderSystemByName(defaultRenderSystemName);
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
                    string renderParamKey = kpl.Name;
                    string renderParamValue = kpl.Value;
                    //Validate the render parameter
                    if (!ogreConfigMap[renderParamKey].possibleValues.Contains(renderParamValue))
                    {
                        renderParamValue = ogreConfigMap[renderParamKey].possibleValues[0];
                    }
                    rs.SetConfigOption(renderParamKey, renderParamValue);
                }
                root.RenderSystem = rs;
            }
            renderWindow = root.Initialise(true, wndTitle);
 
            viewport = renderWindow.AddViewport(null);
            ColourValue cv = new ColourValue(0.5f, 0.5f, 0.5f);
            viewport.BackgroundColour = cv;

            viewport.Camera = null;
 
            int hWnd = 0;
            
            renderWindow.GetCustomAttribute("WINDOW", out hWnd);
 
            inputMgr = InputManager.CreateInputSystem((uint)hWnd);
            keyboard = (Keyboard)inputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            mouse =  (Mouse)inputMgr.CreateInputObject(MOIS.Type.OISMouse, true);

            mouse.MouseMoved+=new MouseListener.MouseMovedHandler(mouseMoved);
            mouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            mouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);

            keyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);

            MouseState_NativePtr mouseState = mouse.MouseState;
                mouseState.width = viewport.ActualWidth;
                mouseState.height = viewport.ActualHeight;

            foreach (var resource in gameOptions.ResourcesConfig.Resources)
            {
                foreach (var resLoc in resource.ResourceLocs)
                {
                    ResourceGroupManager.Singleton.AddResourceLocation(resLoc, resource.Type, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                }
            }

            if (!LocateSystem.Instance.IsInit)
            {
                LocateSystem.Instance.InitLocateSystem(LocateSystem.Instance.ConvertReadableStringToLocate(gameOptions.LocateConfig.CurrentLocate));
            }

            ResourceGroupManager.Singleton.AddResourceLocation(
                string.Format("./Media/Engine/Fonts/{0}/", LocateSystem.Instance.Locate.ToString()), "FileSystem",
                "General");

            TextureManager.Singleton.DefaultNumMipmaps = 5;
            
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();

            trayMgr = new SdkTrayManager("AMOFTrayMgr", renderWindow, mouse, new SdkTrayListener() );

            timer = new Timer();
            timer.Reset();
 
            renderWindow.IsActive=true;

            this.gameOptions = gameOptions;

            log.LogMessage("Game Started!");

            return true;
        }

        public void SetFullScreen()
        {
            renderWindow.SetFullscreen(
                !renderWindow.IsFullScreen,
                Convert.ToUInt32(videoMode["Width"]),
                Convert.ToUInt32(videoMode["Height"])
            );
        }

        private bool InitSubSystem(GameConfigXml gameOptions)
        {
            appStateMgr = new AppStateManager();
            locateMgr = LocateSystem.Instance;
            modMgr = new ModManager();
            networkMgr = new NetworkManager();
            outputMgr = new OutputManager();
            soundMgr = new SoundManager();
            uiMgr = new ScreenManager();

            SoundManager.Instance.InitSystem(
                gameOptions.AudioConfig.EnableMusic, 
                gameOptions.AudioConfig.EnableSound
            );
            
            Update += modMgr.Update;
            Update += outputMgr.Update;
            Update += soundMgr.Update;
            Update += uiMgr.Update;
            
            return true;
        }

        private bool InitGame(GameConfigXml gameOptions)
        {
            try
            {
                isEditMode = gameOptions.CoreConfig.IsEnableEditMode;

				TimerManager.Instance.Init(1257, 3, 29, 9, 0, 0);

                return true;
            }
            catch
            {
                return false;
            }
        }

        bool frameStarted(FrameEvent evt)
        {
            if (Update != null)
            {
                Update(evt.timeSinceLastFrame);
            }
            return true;
        }

        public void Exit()
        {
            LocateSystem.Instance.SaveLocateFile();
            log.LogMessage("Game Quit!");
            log.Dispose();
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
            if(keyboard.IsKeyDown(KeyCode.KC_V))
            {
                renderWindow.WriteContentsToTimestampedFile("AMGE_ScreenShot_", ".jpg");
                outputMgr.DisplayMessage(string.Format(locateMgr.GetLocalizedString(LocateFileType.GameString,"str_screenshots_saved_to_{0}"), Environment.CurrentDirectory));
                return true;
            }
            else if(keyboard.IsKeyDown(KeyCode.KC_O))
            {
                if(trayMgr.isLogoVisible())
                {
                    trayMgr.hideFrameStats();
                    trayMgr.hideLogo();
                }
                else
                {
                    trayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
                    trayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
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

        public void ChangeState(EngineState newState)
        {
            if (currentState == newState)
            {
                return;
            }
            currentState = newState;
        }

        public void Dispose()
        {
            root.Dispose();
            trayMgr.Dispose();
            timer.Dispose();
        }
    }
}
