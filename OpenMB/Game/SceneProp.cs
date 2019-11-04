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
				entity = sceneManager.CreateEntity(Guid.NewGuid().ToString(), childModel.Mesh);
				entity.SetMaterialName(childModel.Material);
				entNode = sceneManager.RootSceneNode.CreateChildSceneNode();
				entNode.AttachObject(entity);
				entNode.Position = position;
				for (int i = 0; i < entity.NumSubEntities; i++)
				{
					SubEntity subEnt = entity.GetSubEntity((uint)i);
					subEnt.SetMaterialName(childModel.Material);
				}
				entities.Add(entity);
			}
		}

        public bool CheckCollide(SceneProp missileSceneProp)
        {
			return true;
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
