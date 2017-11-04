using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.RPG;

namespace AMOFGameEngine.AI
{
    public class StateMachine<T> where T : RPGObject
    {
        private T owner;
        public T Owner { get { return owner; } }
        public State<T> CurrentState { get; set; }
        public State<T> PreviousState { get; set; }
        public State<T> GlobalState { get; set; }

        public StateMachine(T owner)
        {
            this.owner = owner;
            CurrentState = null;
            PreviousState = null;
            GlobalState = null;
        }

        public void Update()
        {

        }
    }
}
