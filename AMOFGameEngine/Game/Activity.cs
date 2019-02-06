using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public enum ActionState { Queued, Done, Cancel}
    //An Action in world
    public abstract class Activity : IUpdate
    {
        public ActionState State;
        public event Action ActivityCompleted;
        public abstract void Update(float deltaTime);
    }
}
