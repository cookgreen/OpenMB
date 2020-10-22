using Mogre;
using OpenMB.Mods.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
	public class GameLocation : GameObject
	{
		private ModLocationDfnXml lotData;

		public GameLocation(GameWorld world, ModLocationDfnXml lotData) : base(-1, world)
		{
			this.lotData = lotData;
		}

		public void Spawn()
		{
			var model = world.ModData.ModelInfos.Where(o => o.ID == lotData.Model.Resource).FirstOrDefault();
			if (model != null)
			{
				renderable.Entity = renderable.SceneManager.CreateEntity(Guid.NewGuid().ToString(), model.Mesh);
				renderable.Entity.SetMaterialName(model.Material);
				renderable.EntityNode = renderable.SceneManager.RootSceneNode.CreateChildSceneNode();
				renderable.EntityNode.AttachObject(renderable.Entity);
				renderable.EntityNode.Position = position;
				for (int i = 0; i < renderable.Entity.NumSubEntities; i++)
				{
					SubEntity subEnt = renderable.Entity.GetSubEntity((uint)i);
					subEnt.SetMaterialName(model.Material);
				}
			}
		}
	}
}
