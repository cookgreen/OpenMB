using System;
using System.Collections.Generic;
using Mogre;
using OpenMB.Game;
using OpenMB.Mods;
using System.Linq;

namespace OpenMB.Map
{
	public class GameWorldMap : IGameMap
	{
		private ModData modData;
		private SceneManager sceneManager;
		private List<GameObject> locations;

		public string Name => throw new System.NotImplementedException();

		public SceneManager SceneManager
		{
			get
			{
				return sceneManager;
			}
		}

		public GameWorldMap(GameWorld world)
		{
			locations = new List<GameObject>();
			sceneManager = world.SceneManager;
			modData = world.ModData;
		}

		public void Destroy()
		{
			locations.Clear();
		}

		public List<GameObject> GetGameObjects(string objectID)
		{
			return locations;
		}

		public void LoadAsync()
		{
		}

		public void Update(float timeSinceLastFrame)
		{
		}

		public void CreateLocation(string locationID, Vector3 position)
		{
			var locationInfo = modData.LocationInfos.Where(o => o.ID == locationID).FirstOrDefault();
			if (locationInfo != null)
			{
				var modelTypeInfo = modData.ModModelTypes.Where(o => o.Name == locationInfo.Model.Type).FirstOrDefault();
				if (modelTypeInfo != null)
				{
					var model = modData.Models.Where(o => o.ID == locationInfo.Model.Resource).FirstOrDefault();
					model.ModelType = modelTypeInfo;


				}
			}
		}
	}
}