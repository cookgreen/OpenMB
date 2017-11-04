using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.RPG;

namespace AMOFGameEngine.AI
{
    public class State<T> where T : RPGObject
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
        public virtual void Enter(T owner) { }
        public virtual void Execute(T owner) { }
        public virtual void Exit(T owner) { }
    }
}
