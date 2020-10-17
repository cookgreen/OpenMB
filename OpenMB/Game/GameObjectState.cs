using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
    public class GameObjectState
    {
        private GameObjectState previousState;
        private GameObjectState nextState;
        public GameObjectState PreviousState { get { return previousState; } }
        public GameObjectState NextState { get { return nextState; } }

        public GameObjectState(GameObjectState previousState = null, GameObjectState nextState = null)
        {
            this.previousState = previousState;
            this.nextState = nextState;
        }

        public virtual bool CheckSwitchCondition()
        {
            return true;
        }
    }
}
