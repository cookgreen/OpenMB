using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods.Common.Loaders
{
    public class MapLoaderXml : IGameMapLoader
    {
        private DotSceneLoader.DotSceneLoader fileLoader;
        private SceneManager sceneManager;
        public string Name
        {
            get
            {
                return "Xml";
            }
        }
        public event Action LoadMapFinished;
        public MapLoaderXml(SceneManager sceneManager)
        {
            fileLoader = new DotSceneLoader.DotSceneLoader();
            fileLoader.LoadSceneFinished += FileLoader_LoadSceneFinished;
            this.sceneManager = sceneManager;
        }

        private void FileLoader_LoadSceneFinished()
        {
            LoadMapFinished?.Invoke();
        }

        public void LoadAsync(string mapFile)
        {
            fileLoader.ParseDotSceneAsync(mapFile, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, sceneManager);
        }
    }
}
