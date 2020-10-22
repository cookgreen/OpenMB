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
				renderable.Entity = renderable.SceneManager.CreateEntity(Guid.NewGuid().ToString(), childModel.Mesh);
				renderable.Entity.SetMaterialName(childModel.Material);
				renderable.EntityNode = renderable.SceneManager.RootSceneNode.CreateChildSceneNode();
				renderable.EntityNode.AttachObject(renderable.Entity);
				renderable.EntityNode.Position = position;
				for (int i = 0; i < renderable.Entity.NumSubEntities; i++)
				{
					SubEntity subEnt = renderable.Entity.GetSubEntity((uint)i);
					subEnt.SetMaterialName(childModel.Material);
				}
				entities.Add(renderable.Entity);
			}
		}

		public bool CheckCollide(SceneProp missileSceneProp)
		{
			return true;
		}

		public override void Dispose()
		{
			renderable.EntityNode.Dispose();
			renderable.Entity.Dispose();
		}

		public void Move(Vector3 mov)
		{
			renderable.EntityNode.Position += mov;
		}
	}
}
