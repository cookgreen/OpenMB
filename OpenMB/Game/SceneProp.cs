using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
    public class SceneProp : GameObject
    {
        private string name;
        private string meshName;
        private string materialName;
        private Item attachedItem;

        public SceneProp(
            int id, GameWorld world, 
            string name,
            string meshName, 
            string materialName, 
            Vector3 initPosition,
            Item attachedItem
        ) : base(id, world)
        {
            this.name = name;
            this.meshName = meshName;
            this.materialName = materialName;
            this.attachedItem = attachedItem;
            position = initPosition;

            health = new HealthInfo(this, int.MaxValue, false);

            create();
        }

        protected override void create()
        {
            entity = sceneManager.CreateEntity(Guid.NewGuid().ToString(), meshName);
            entity.SetMaterialName(materialName);
            entNode = sceneManager.RootSceneNode.CreateChildSceneNode();
            entNode.AttachObject(entity);
            entNode.Position = position;
            for (int i = 0; i < entity.NumSubEntities; i++)
            {
                SubEntity subEnt = entity.GetSubEntity((uint)i);
                subEnt.SetMaterialName(materialName);
            }
        }

        public bool CheckCollide(SceneProp missileSceneProp)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            entNode.Dispose();
            entity.Dispose();
        }

        public void Move(Vector3 mov)
        {
            entNode.Position += mov;
        }
    }
}
