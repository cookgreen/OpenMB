using Mogre;
using OpenMB.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.Game;

namespace OpenMB.Mods.Common.Loaders
{
    public class MapLoaderXml : IGameMapLoader
    {
        private string loadedMapName;
        private DotSceneLoader.DotSceneLoader fileLoader;
        public string Name
        {
            get
            {
                return "Xml";
            }
        }

        public string LoadedMapName
        {
            get
            {
                return loadedMapName;
            }
        }

        public AIMesh AIMesh
        {
            get
            {
                return null;
            }
        }

        public event Action LoadMapFinished;
        public event Action LoadMapStarted;

        public MapLoaderXml()
        {
        }

        private void FileLoader_LoadSceneFinished()
        {
            LoadMapFinished?.Invoke();
        }

        public void LoadAsync(IGameMap map,string mapFile)
		{
			fileLoader = new DotSceneLoader.DotSceneLoader((GameMap)map);
			fileLoader.LoadSceneFinished += FileLoader_LoadSceneFinished;
			LoadMapStarted?.Invoke();
            loadedMapName = mapFile;
            fileLoader.ParseDotSceneAsync(mapFile, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, ((GameMap)map).SceneManager);
        }

        public void SaveAsync()
        {

        }
    }
}
