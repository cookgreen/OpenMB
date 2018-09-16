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
            screens.Add("Credit", new CreditScreen());
            screens.Add("Console", new GameConsoleScreen());
            screens.Add("Inventory", new InventoryScreen());
            instance = this;
            screenStack = new Stack<IScreen>();
        }
        public void ChangeScreen(string screenName,params object[] param)
        {
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
                ReturnLastScreen();
            }
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

        public void HideCurrentScreen()
        {
            if (screenStack.Count > 0)
            {
                screenStack.Pop().Exit();
            }
        }

        public bool CheckScreen(string screenName)
        {
            return screenStack.Peek().Name == screenName;
        }
    }
}
