using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Game;

namespace AMOFGameEngine.AI
{
    public class State<T> where T : GameObject
    {
        private static State<T> instance;
        public static State<T> Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new State<T>();
                }
                return instance;
            }
        }
        public virtual void Enter(T owner, params object[] extraParams) { }
        public virtual void Execute(T owner) { }
        public virtual void Exit(T owner) { }
    }
}
