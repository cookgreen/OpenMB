using System;
using System.Collections.Generic;
using Mogre;
using OpenMB.Game;
using OpenMB.Mods;
using System.Linq;
using System.ComponentModel;

namespace OpenMB.Map
{
	public class GameWorldMap : IGameMap
	{
		private string name;
		private GameWorld world;
		private ModData modData;
		private SceneManager sceneManager;
		private List<GameObject> locations;
		private BackgroundWorker worker;

		public event MapLoadhandler LoadMapStarted;
		public event MapLoadhandler LoadMapFinished;

		public string Name
		{
			get
			{
				return name;
			}
		}

		public SceneManager SceneManager
		{
			get
			{
				return sceneManager;
			}
		}

		public GameWorldMap(GameWorld world,  string file, IGameMapLoader loader)
		{
			locations = new List<GameObject>();
			this.world = world;
			sceneManager = world.SceneManager;
			modData = world.ModData;
			name = null;
			worker = new BackgroundWorker();
			worker.DoWork += Worker_DoWork;
			worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
		}

		private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			LoadMapFinished?.Invoke();
		}

		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
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
			LoadMapStarted?.Invoke();
			worker.RunWorkerAsync();
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
					var model = modData.ModelInfos.Where(o => o.ID == locationInfo.Model.Resource).FirstOrDefault();
					model.ModelType = modelTypeInfo;

					GameLocation location = new GameLocation(world, locationInfo);
					location.Spawn();
					location.ID = locations.Count;
					locations.Add(location);
				}
			}
		}
	}
}