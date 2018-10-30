using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Screen
{
    public class ScreenManager
    {
        private Stack<IScreen> screenStack;
        private Dictionary<string, IScreen> screens;
        private static ScreenManager instance;
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

        public ScreenManager()
        {
            screens = new Dictionary<string, IScreen>();
            IScreen screenCredit = new CreditScreen();
            IScreen screenConsole = new GameConsoleScreen();
            IScreen screenInventory = new InventoryScreen();
            IScreen screenEditor = new GameEditorScreen();
            screens.Add(screenCredit.Name, screenCredit);
            screens.Add(screenConsole.Name, screenConsole);
            screens.Add(screenInventory.Name, screenInventory);
            screens.Add(screenEditor.Name, screenEditor);
            instance = this;
            screenStack = new Stack<IScreen>();
        }

        public void InjectMouseMove(MouseEvent arg)
        {
            if (screenStack.Count > 0)
            {
                screenStack.Peek().InjectMouseMove(arg);
            }
        }
        public void InjectMousePressed(MouseEvent arg, MouseButtonID id)
        {
            if (screenStack.Count > 0)
            {
                screenStack.Peek().InjectMousePressed(arg, id);
            }
        }
        public void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
        {
            if (screenStack.Count > 0)
            {
                screenStack.Peek().InjectMouseReleased(arg, id);
            }
        }
        public void InjectKeyPressed(KeyEvent arg)
        {
            if (screenStack.Count > 0)
            {
                screenStack.Peek().InjectKeyPressed(arg);
            }
        }
        public void InjectKeyReleased(KeyEvent arg)
        {
            if (screenStack.Count > 0)
            {
                screenStack.Peek().InjectKeyReleased(arg);
            }
        }

        public void ChangeScreen(string screenName,params object[] param)
        {
            if (screenStack.Count > 0)
            {
                screenStack.Peek().Exit();
            }
            if(screens.ContainsKey(screenName))
            {
                IScreen runScreen = screens[screenName];
                runScreen.OnScreenExit += CurrentScreen_OnScreenExit;
                runScreen.Init(param);
                runScreen.Run();
                screenStack.Push(runScreen);
            }
        }

        public void ReturnLastScreen(params object[] param)
        {
            if (screenStack.Count > 0)
            {
                screenStack.Pop().Exit();
            }
            if (screenStack.Count > 0)
            {
                screenStack.Peek().Init(param);
            }
        }
        
        private void CurrentScreen_OnScreenExit()
        {
            if (OnCurrentScreenExit != null)
            {
                OnCurrentScreenExit();
            }
            ReturnLastScreen();
        }

        public void Dispose()
        {
            while (screenStack.Count > 0)
            {
                screenStack.Pop().Exit();
            }
        }

        public void UpdateCurrentScreen(float timeSinceLastFrame)
        {
            if (screenStack.Count > 0)
            {
                screenStack.Peek().Update(timeSinceLastFrame);
            }
        }

        public void Update(float timeSinceLastFrame)
        {
            UpdateCurrentScreen(timeSinceLastFrame);
        }

        public void ExitCurrentScreen()
        {
            if (screenStack.Count > 0)
            {
                screenStack.Pop().Exit();
            }
        }

        public bool CheckScreenIsVisual(string screenName)
        {
            if (screenStack.Count == 0)
            {
                return false;
            }
            else
            {
                return screenStack.Peek().Name == screenName && screenStack.Peek().IsVisible;
            }
        }
    }
}
