using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using OpenMB.Mods;

namespace OpenMB.Screen
{
    public class ScreenManager
    {
        private Stack<IScreen> runningScreenStack;
        private Dictionary<string, IScreen> innerScreens;
        private static ScreenManager instance;
        private Camera camera;

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

		public ModData ModData { get; set; }

		public ScreenManager()
        {
            innerScreens = new Dictionary<string, IScreen>();
            IScreen screenCredit = new CreditScreen();
            IScreen screenConsole = new GameConsoleScreen();
            IScreen screenInventory = new InventoryScreen();
            IScreen screenEditor = new GameEditorScreen();
			IScreen screenMain = new GameMainScreen();
			IScreen screenMenu = new GameMenuScreen();
			innerScreens.Add(screenCredit.Name, screenCredit);
            innerScreens.Add(screenConsole.Name, screenConsole);
            innerScreens.Add(screenInventory.Name, screenInventory);
            innerScreens.Add(screenEditor.Name, screenEditor);
			innerScreens.Add(screenMain.Name, screenMain);
			innerScreens.Add(screenMenu.Name, screenMenu);
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
                        runScreen.OnScreenExit += CurrentScreen_OnScreenExit;
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
                    runScreen.OnScreenExit += CurrentScreen_OnScreenExit;
                    runScreen.Init(param);
                    runScreen.Run();
                    runningScreenStack.Push(runScreen);
                }
            }
		}

		public void ExitCurrentScreenReturn()
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
    }
}
