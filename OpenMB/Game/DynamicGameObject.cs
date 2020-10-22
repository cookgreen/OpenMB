using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
    public class DynamicGameObject : GameObject
    {
        protected GameObjectFiniteStateMachine stateMachine;
        public DynamicGameObject(int id, GameWorld world) : base(id, world)
        {
            
        }

        public override void Update(float timeSinceLastFrame)
        {
            stateMachine.Update(timeSinceLastFrame);

            base.Update(timeSinceLastFrame);
        }
    }
}
