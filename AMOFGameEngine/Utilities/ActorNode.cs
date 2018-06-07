using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Utilities
{
    public class ActorNode
    {
        private SceneNode sceneNode;
        private Actor actor;

        public ActorNode(SceneNode sceneNode, Actor actor)
        {
            this.sceneNode = sceneNode;
            this.actor = actor;
        }

        internal void Update(float deltaTime)
        {
            if (!actor.IsSleeping)
            {
                this.sceneNode.Position = actor.GlobalPosition;
                this.sceneNode.Orientation = actor.GlobalOrientationQuaternion;
            }
        }
    }
}
