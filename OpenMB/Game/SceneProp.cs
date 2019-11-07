using Mogre;
using OpenMB.Mods.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
    public class SceneProp : GameObject
    {
		private ModScenePropDfnXml scenePropData;
		private List<ModModelDfnXml> childModelData;
		private List<Entity> entities;

		public SceneProp(
            int id, GameWorld world, 
            string name,
            string meshName, 
            string materialName, 
            Vector3 initPosition,
            Item attachedItem
        ) : base(id, world)
        {
            position = initPosition;

            health = new HealthInfo(this, int.MaxValue, false);

            create();
		}
		public SceneProp(
			GameWorld world,
			ModScenePropDfnXml scenePropData,
			Vector3 initPosition
		) : base(-1, world)
		{
			position = initPosition;
			entities = new List<Entity>();
			childModelData = new List<ModModelDfnXml>();
			health = new HealthInfo(this, int.MaxValue, false);
			this.scenePropData = scenePropData;
		}

		public void AppendChildModelData(ModModelDfnXml modelData)
		{
			childModelData.Add(modelData);
		}

		public void Spawn()
		{
			foreach (var childModel in childModelData)
			{
				mesh.Entity = mesh.SceneManager.CreateEntity(Guid.NewGuid().ToString(), childModel.Mesh);
				mesh.Entity.SetMaterialName(childModel.Material);
				mesh.EntityNode = mesh.SceneManager.RootSceneNode.CreateChildSceneNode();
				mesh.EntityNode.AttachObject(mesh.Entity);
				mesh.EntityNode.Position = position;
				for (int i = 0; i < mesh.Entity.NumSubEntities; i++)
				{
					SubEntity subEnt = mesh.Entity.GetSubEntity((uint)i);
					subEnt.SetMaterialName(childModel.Material);
				}
				entities.Add(mesh.Entity);
			}
		}

        public bool CheckCollide(SceneProp missileSceneProp)
        {
			return true;
        }

        public override void Dispose()
        {
            mesh.EntityNode.Dispose();
            mesh.Entity.Dispose();
        }

        public void Move(Vector3 mov)
        {
            mesh.EntityNode.Position += mov;
        }
	}
}
