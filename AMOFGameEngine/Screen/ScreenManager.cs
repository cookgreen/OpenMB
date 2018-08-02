using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Screen
{
    public class ScreenManager
    {
        private IScreen currentScreen;
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
            currentScreen = null;
            screens = new Dictionary<string, IScreen>();
            screens.Add("Credit", new CreditScreen());
        }
        public void ChangeScreen(string screenName)
        {
            if (currentScreen != null)
            {
                currentScreen.Exit();
            }
            if(screens.ContainsKey(screenName))
            {
                currentScreen = screens[screenName];
                currentScreen.OnScreenExit += CurrentScreen_OnScreenExit;
                currentScreen.Init();
                currentScreen.Run();
            }
        }
        
        private void CurrentScreen_OnScreenExit()
        {
            if (OnCurrentScreenExit != null)
            {
                OnCurrentScreenExit();
            }
        }

        public void Dispose()
        {
            if (currentScreen != null)
            {
                currentScreen.Exit();
            }
        }

        public void UpdateCurrentScreen(float timeSinceLastFrame)
        {
            if (currentScreen != null)
            {
                currentScreen.Update(timeSinceLastFrame);
            }
        }

        public void Update(float timeSinceLastFrame)
        {
            UpdateCurrentScreen(timeSinceLastFrame);
        }
    }
}
