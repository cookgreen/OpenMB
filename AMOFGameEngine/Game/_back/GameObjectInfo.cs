using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Game
{
    public class GameObjectInfo
    {
        protected Camera cam;
        protected SceneNode node;
        protected Entity mesh;
        protected AnimationState[] animations;
        protected int[] animIds;

        public GameObjectInfo()
        {

        }
        public virtual void Initization(string name, string meshName, Camera cam)
        {

        }
        public virtual void SetTopAnimation(string animId)
        {

        }
        public virtual void SetBaseAnimation(string animId)
        {

        }
        public virtual void Update(float timeSinceLastFrame)
        {

        }
    }
}
