using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using OpenMB.Configure;
using OpenMB.Core;
using OpenMB.Game;
using OpenMB.Localization;
using OpenMB.LogMessage;
using OpenMB.Mods;
using OpenMB.Network;
using OpenMB.Screen;
using OpenMB.Sound;
using OpenMB.States;
using OpenMB.Utilities;
using OpenMB.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
        private MusicSoundManager soundMgr;
        private ScreenManager uiMgr;
        private EngineState currentState;
		private InputKeyMouseManager keyMouseManager;
		public GameConfigXml gameOptions;
        public Root root;
        public RenderWindow renderWindow;
        public Viewport viewport;
        public EngineLog log;
        public Log rendererLog;
        public Timer timer;
        public MOIS.InputManager inputMgr;
        public Keyboard keyboard;
        public Mouse mouse;
        public static string LastStateName;
        public LoadingData loadingData;
		public Dictionary<string, object> GlobalValueTable;
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

        public NameValuePairList VideoMode { get; set; }


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
            appStateMgr = null;
            soundMgr = null;
            VideoMode = new NameValuePairList();
            isEditMode = false;
            isCheatMode = false;
            loadingData = new LoadingData(LoadingType.NONE, null, null, null);
			GlobalValueTable = new Dictionary<string, object>();
		 }

        public void Update(float timeSinceLastFrame)
        {
            ModManager.Instance.Update(timeSinceLastFrame);
            UIManager.Instance.Update();
            OutputManager.Instance.Update(timeSinceLastFrame);
            MusicSoundManager.Instance.Update(timeSinceLastFrame);
            BackendTaskManager.Instance.Update();
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

			if (gameOptions == null)
			{
				gameOptions = GameConfigXml.Load("game.xml", root);
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
                VideoMode["Width"] = strVideoMode.Split('x')[0].Trim();
                VideoMode["Height"] = strVideoMode.Split('x')[1].Trim();
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

            IntPtr hwnd;
            renderWindow.GetCustomAttribute("WINDOW", out hwnd);
            Helper.SetRenderWindowIcon(new System.Drawing.Icon(Path.Combine(Environment.CurrentDirectory, "app.ico")), hwnd);

            viewport = renderWindow.AddViewport(null);
            ColourValue cv = new ColourValue(0.5f, 0.5f, 0.5f);
            viewport.BackgroundColour = cv;

            viewport.Camera = null;
 
            int hWnd = 0;
            
            renderWindow.GetCustomAttribute("WINDOW", out hWnd);

			inputMgr = MOIS.InputManager.CreateInputSystem((uint)hWnd);
            keyboard = (Keyboard)inputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            mouse =  (Mouse)inputMgr.CreateInputObject(MOIS.Type.OISMouse, true);
			keyMouseManager = new InputKeyMouseManager();
			keyMouseManager.SomeKeyPressd += KeyMouseManager_SomeKeyPressd;

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

			foreach (var keyMapper in gameOptions.InputConfig.Mappers)
			{
				KeyMapperManager.Instance.AddKeyMapper(keyMapper.GameKeyCode, keyMapper.GetKeyCollections());
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

			UIManager.Instance.Init("AMOFTrayMgr", renderWindow, mouse, new UIListener());


			timer = new Timer();
            timer.Reset();
 
            renderWindow.IsActive=true;

            this.gameOptions = gameOptions;

			log.LogMessage("Game Started!");

            return true;
        }

		public void SwitchFullScreen()
        {
            renderWindow.SetFullscreen(
                !renderWindow.IsFullScreen,
                Convert.ToUInt32(VideoMode["Width"]),
                Convert.ToUInt32(VideoMode["Height"])
            );
        }

        private bool InitSubSystem(GameConfigXml gameOptions)
        {
            appStateMgr = new AppStateManager();
            locateMgr = LocateSystem.Instance;
            modMgr = new ModManager();
            networkMgr = new NetworkManager();
            outputMgr = new OutputManager();
            soundMgr = new MusicSoundManager();
            uiMgr = new ScreenManager();

            MusicSoundManager.Instance.InitSystem(
                gameOptions.AudioConfig.EnableMusic, 
                gameOptions.AudioConfig.EnableSound
            );
            
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
            return true;
        }

        public void Exit()
        {
            LocateSystem.Instance.SaveLocateFile();
            log.LogMessage("Game Quit!");
            log.Dispose();
        }

        public bool keyPressed(KeyEvent key)
        {
			if (keyboard.IsKeyDown(KeyCode.KC_V))
			{
				return true;
			}
			else if (keyboard.IsKeyDown(KeyCode.KC_O))
			{
			}
			else if (keyboard.IsKeyDown(KeyCode.KC_SPACE))
			{

			}
 
            return true;
        }

		private void KeyMouseManager_SomeKeyPressd(KeyCollection keyCollection)
		{
			if (keyCollection == KeyMapperManager.Instance.GetKeyCollection(GameKeyCode.FullScreen))
			{
				SwitchFullScreen();
			}
			else if(keyCollection == KeyMapperManager.Instance.GetKeyCollection(GameKeyCode.Screenshot))
			{
				renderWindow.WriteContentsToTimestampedFile("ScreenShot_", ".jpg");
				outputMgr.DisplayMessage(string.Format(locateMgr.GetLocalizedString(LocateFileType.GameString, "str_screenshots_saved_to_{0}"), Environment.CurrentDirectory));
			}
			//else if (keyCollection == KeyMapperManager.Instance.GetKeyCollection(GameKeyCode.ShowOgreLogo))
			//{
			//	if (UIManager.Instance.isLogoVisible())
			//	{
			//		UIManager.Instance.HideFrameStats();
			//		UIManager.Instance.hideLogo();
			//	}
			//	else
			//	{
			//		UIManager.Instance.showFrameStats(UIWidgetLocation.TL_BOTTOMLEFT);
			//		UIManager.Instance.ShowLogo(UIWidgetLocation.TL_BOTTOMRIGHT);
			//	}
			//}
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
			UIManager.Instance.Dispose();
            timer.Dispose();
        }
    }
}
