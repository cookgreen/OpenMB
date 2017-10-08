using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using AMOFGameEngine.Widgets;

namespace AMOFGameEngine.UI
{
    public class GameUIManager
    {
        private List<KeyValuePair<string,GameUI>> registeredUI;
        private Stack<KeyValuePair<string,GameUI>> runningUI;

        public GameUIManager()
        {
            registeredUI = new List<KeyValuePair<string, GameUI>>();
            runningUI = new Stack<KeyValuePair<string, GameUI>>();
        }

        private static GameUIManager instance;
        public static GameUIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameUIManager();
                }
                return instance;
            }
        }

        public void RegisterUI(string name,GameUI ui)
        {
            KeyValuePair<string, GameUI> info = new KeyValuePair<string, GameUI>
            (name, ui);
            registeredUI.Add(info);
        }

        public void CreateUI(string name)
        {
            GameUI ui= registeredUI.Where(o => o.Key == name).First().Value;
            KeyValuePair<string, GameUI> info = new KeyValuePair<string, GameUI>
            (name, ui);
            runningUI.Push(info);
            runningUI.Peek().Value.Show();
        }

        public GameListUI CreateGameListUI(string name,List<string> columns)
        {
            GameListUI ui=new GameListUI(name,columns);
            ui.Show();
            runningUI.Push(new KeyValuePair<string,GameUI>(name,ui));
            return ui;
        }

        public void ChangeUI(string name)
        {
            GameUI ui = registeredUI.Where(o => o.Key == name).First().Value;
            KeyValuePair<string, GameUI> info = new KeyValuePair<string, GameUI>
            (name, ui);
            runningUI.Push(info);
            runningUI.Peek().Value.Show();
        }

        public void CloseUI(string name)
        {
            GameUI ui = runningUI .Where(o => o.Key == name).First().Value;
            runningUI.Pop().Value.Close();
        }

        public void Update(float timeSinceLastFrame)
        {

        }
    }
}
