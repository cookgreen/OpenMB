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
			var model = world.ModData.Models.Where(o => o.ID == lotData.Model.Resource).FirstOrDefault();
			if (model != null)
			{
				entity = sceneManager.CreateEntity(Guid.NewGuid().ToString(), model.Mesh);
				entity.SetMaterialName(model.Material);
				entNode = sceneManager.RootSceneNode.CreateChildSceneNode();
				entNode.AttachObject(entity);
				entNode.Position = position;
				for (int i = 0; i < entity.NumSubEntities; i++)
				{
					SubEntity subEnt = entity.GetSubEntity((uint)i);
					subEnt.SetMaterialName(model.Material);
				}
			}
		}
	}
}
