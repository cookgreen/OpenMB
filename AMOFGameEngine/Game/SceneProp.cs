using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class SceneProp : GameObject
    {
        private string meshName;

        public SceneProp(int id, GameWorld world, string meshName, Vector3 initPosition) : base(id, world)
        {
            this.meshName = meshName;
            position = initPosition;
            create();
        }

        protected override void create()
        {
            entity = sceneManager.CreateEntity(Guid.NewGuid().ToString(), meshName);
            entNode = sceneManager.RootSceneNode.CreateChildSceneNode();
            entNode.AttachObject(entity);
            entNode.Position = position;
        }
    }
}
