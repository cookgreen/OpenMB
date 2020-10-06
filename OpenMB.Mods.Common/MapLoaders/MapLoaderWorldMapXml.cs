using Mogre;
using OpenMB.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods.Common.MapLoaders
{
	public class MapLoaderWorldMapXml : IGameMapLoader
	{
		private BackgroundWorker worker;
		public event Action LoadMapStarted;
		public event Action LoadMapFinished;
		public string Name
		{
			get
			{
				return "WorldMapXml";
			}
		}

		public string LoadedMapName { get; }

        public List<string> Entities { get { return new List<string>(); } }

        public MapLoaderWorldMapXml()
		{
			worker = new BackgroundWorker();
			worker.DoWork += Worker_DoWork;
			worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
		}

		public void LoadAsync(IGameMap gameMap, string mapFile)
		{
			worker.RunWorkerAsync();
			LoadMapStarted?.Invoke();
		}

		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
		}

		private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			LoadMapFinished?.Invoke();
		}
	}
}
