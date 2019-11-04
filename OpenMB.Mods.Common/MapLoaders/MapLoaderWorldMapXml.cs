using OpenMB.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods.Common.MapLoaders
{
	public class MapLoaderWorldMapXml : IGameMapLoader
	{
		public string Name
		{
			get
			{
				return "WorldMapXml";
			}
		}

		public string LoadedMapName { get; }

		public event Action LoadMapStarted;
		public event Action LoadMapFinished;

		public void LoadAsync(IGameMap gameMap, string mapFile)
		{
			throw new NotImplementedException();
		}
	}
}
