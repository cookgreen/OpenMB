using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
    public class GameObjectFiniteStateMachine : IUpdate
    {
        private GameObjectState currentState;
        private List<GameObjectState> registeredStates;

        public GameObjectFiniteStateMachine()
        {
            registeredStates = new List<GameObjectState>();
        }

        public void RegisterNewState(GameObjectState newState)
        {
            registeredStates.Add(newState);
        }

        private void SwitchState(GameObjectState newState)
        {
            int index = registeredStates.IndexOf(newState);
            if (index > -1)
            {
                currentState = newState;
            }
        }

        public void Update(float timeSinceLastFrame)
        {

        }
    }
}
