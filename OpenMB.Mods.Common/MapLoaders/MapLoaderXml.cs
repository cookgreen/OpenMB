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
        private SceneManager sceneManager;
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
            fileLoader = new DotSceneLoader.DotSceneLoader();
            fileLoader.LoadSceneFinished += FileLoader_LoadSceneFinished;
            this.sceneManager = sceneManager;
        }

        private void FileLoader_LoadSceneFinished()
        {
            LoadMapFinished?.Invoke();
        }

        public void LoadAsync(SceneManager sceneManager,string mapFile)
        {
            LoadMapStarted?.Invoke();
            loadedMapName = mapFile;
            fileLoader.ParseDotSceneAsync(mapFile, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, sceneManager);
        }

        public void SaveAsync()
        {

        }
    }
}
