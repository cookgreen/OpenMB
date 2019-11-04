using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
	public class GameLocation : GameObject
	{
		private Entity entity;
		private SceneNode entNode;

		public GameLocation(GameWorld world) : base(-1, world)
		{

		}

		public void Spawn()
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
