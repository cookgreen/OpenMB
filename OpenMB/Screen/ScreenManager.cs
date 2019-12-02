using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using OpenMB.Mods;
using OpenMB.Widgets;

namespace OpenMB.Screen
{
    public class ScreenManager : IInitializeMod
	{
        private Stack<IScreen> runningScreenStack;
        private Dictionary<string, IScreen> innerScreens;
        private static ScreenManager instance;
        private Camera camera;
		private ModData modData;

		public event Action<string, string> OnExternalEvent;

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }
        public event Action OnCurrentScreenExit;
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScreenManager();
                }
                return instance;
            }
        }

		public ModData ModData { get { return modData; } }

		public ScreenManager()
        {
            innerScreens = new Dictionary<string, IScreen>();
            IScreen screenCredit = new CreditScreen();
            IScreen screenConsole = new GameConsoleScreen();
            IScreen screenInventory = new InventoryScreen();
            IScreen screenEditor = new GameEditorScreen();
			IScreen screenMainMenu = new GameMainMenuScreen();
			IScreen screenMain = new GameMainScreen();
			IScreen screenMenu = new GameMenuScreen();
			IScreen screenCha = new CharacterScreen();
			IScreen screenNotes = new GameNotesScreen();
			innerScreens.Add(screenMainMenu.Name, screenMainMenu);
			innerScreens.Add(screenCredit.Name, screenCredit);
            innerScreens.Add(screenConsole.Name, screenConsole);
            innerScreens.Add(screenInventory.Name, screenInventory);
            innerScreens.Add(screenEditor.Name, screenEditor);
			innerScreens.Add(screenMain.Name, screenMain);
			innerScreens.Add(screenMenu.Name, screenMenu);
			innerScreens.Add(screenCha.Name, screenCha);
			innerScreens.Add(screenNotes.Name, screenNotes);
			innerScreens.Add("ScriptedScreen", null);
			//TODO: Load all screen script files
			instance = this;
            runningScreenStack = new Stack<IScreen>();
        }

        public void InjectMouseMove(MouseEvent arg)
        {
            if (runningScreenStack.Count > 0)
            {
                runningScreenStack.Peek().InjectMouseMove(arg);
            }
        }
        public void InjectMousePressed(MouseEvent arg, MouseButtonID id)
        {
            if (runningScreenStack.Count > 0)
            {
                runningScreenStack.Peek().InjectMousePressed(arg, id);
            }
        }
        public void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
        {
            if (runningScreenStack.Count > 0)
            {
                runningScreenStack.Peek().InjectMouseReleased(arg, id);
            }
        }
        public void InjectKeyPressed(KeyEvent arg)
        {
            if (runningScreenStack.Count > 0)
            {
                runningScreenStack.Peek().InjectKeyPressed(arg);
            }
        }
        public void InjectKeyReleased(KeyEvent arg)
        {
            if (runningScreenStack.Count > 0)
            {
                runningScreenStack.Peek().InjectKeyReleased(arg);
            }
        }

		public void ChangeScreen(string screenName, bool exitCurrent = false, params object[] param)
        {
            if (runningScreenStack.Count > 0)
            {
                if (runningScreenStack.Peek().Name == screenName)
                {
                    runningScreenStack.Pop().Exit();
					if (runningScreenStack.Count > 0)
					{
						runningScreenStack.Peek().Run();
					}
				}
                else
                {
                    if (innerScreens.ContainsKey(screenName))
					{
						if (!exitCurrent)
						{
							runningScreenStack.Peek().Exit();
						}
						else
						{
							runningScreenStack.Pop().Exit();
						}
						IScreen runScreen = innerScreens[screenName];
						if (runScreen == null && screenName == "ScriptedScreen")
						{
							runScreen = new ScriptedScreen();
						}
                        runScreen.OnScreenExit += CurrentScreen_OnScreenExit;
						runScreen.OnScreenEventChanged += CurrentScreen_OnScreenEventChanged;
						runScreen.Init(param);
                        runScreen.Run();
                        runningScreenStack.Push(runScreen);
                    }
                }
            }
            else
            {
                if (innerScreens.ContainsKey(screenName))
                {
                    IScreen runScreen = innerScreens[screenName];
					if (runScreen == null && screenName == "ScriptedScreen")
					{
						runScreen = new ScriptedScreen();
					}
					runScreen.OnScreenExit += CurrentScreen_OnScreenExit;
					runScreen.OnScreenEventChanged += CurrentScreen_OnScreenEventChanged;
					runScreen.Init(param);
                    runScreen.Run();
                    runningScreenStack.Push(runScreen);
                }
            }
		}

		private void CurrentScreen_OnScreenEventChanged(string widgetName, string value)
		{
			OnExternalEvent?.Invoke(widgetName, value);
		}

		public void ChangeScreenReturn()
		{
			runningScreenStack.Pop().Exit();
			if (runningScreenStack.Count > 0)
			{
				runningScreenStack.Peek().Run();
			}
		}

		public void ReturnLastScreen(params object[] param)
        {
            if (runningScreenStack.Count > 0)
            {
                runningScreenStack.Pop().Exit();
            }
            if (runningScreenStack.Count > 0)
            {
                runningScreenStack.Peek().Init(param);
            }
        }
        
        private void CurrentScreen_OnScreenExit()
        {
            if (OnCurrentScreenExit != null)
            {
                OnCurrentScreenExit();
            }
            //ReturnLastScreen();
        }

        public void Dispose()
        {
            while (runningScreenStack.Count > 0)
            {
                runningScreenStack.Pop().Exit();
            }
        }

        public void UpdateCurrentScreen(float timeSinceLastFrame)
        {
            if (runningScreenStack.Count > 0)
            {
                runningScreenStack.Peek().Update(timeSinceLastFrame);
            }
        }

        public void Update(float timeSinceLastFrame)
        {
            UpdateCurrentScreen(timeSinceLastFrame);
			UIManager.Instance.Update();
        }

        public void ExitCurrentScreen()
        {
            if (runningScreenStack.Count > 0)
            {
                runningScreenStack.Pop().Exit();
            }
        }

        public bool CheckScreenIsVisual(string screenName)
        {
            if (runningScreenStack.Count == 0)
            {
                return false;
            }
            else
            {
                return runningScreenStack.Peek().Name == screenName && runningScreenStack.Peek().IsVisible;
            }
        }

        public bool CheckHasScreen()
        {
            return runningScreenStack.Count > 0;
        }

        public bool CheckEnterScreen(Vector2 mousePos)
        {
            return runningScreenStack.Count > 0 ? runningScreenStack.Peek().CheckEnterScreen(mousePos) : false;
        }

		public void InitMod(ModData modData)
		{
			this.modData = modData;
		}
	}
}
