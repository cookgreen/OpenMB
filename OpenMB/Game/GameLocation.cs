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
				mesh.Entity = mesh.SceneManager.CreateEntity(Guid.NewGuid().ToString(), model.Mesh);
				mesh.Entity.SetMaterialName(model.Material);
				mesh.EntityNode = mesh.SceneManager.RootSceneNode.CreateChildSceneNode();
				mesh.EntityNode.AttachObject(mesh.Entity);
				mesh.EntityNode.Position = position;
				for (int i = 0; i < mesh.Entity.NumSubEntities; i++)
				{
					SubEntity subEnt = mesh.Entity.GetSubEntity((uint)i);
					subEnt.SetMaterialName(model.Material);
				}
			}
		}
	}
}
