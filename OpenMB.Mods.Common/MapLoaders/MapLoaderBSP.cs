using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using OpenMB.Game;
using OpenMB.Map;
using System.ComponentModel;

namespace OpenMB.Mods.Common.Loaders
{
    public class MapLoaderBSP : IGameMapLoader
    {
        private string mapFile;
        private SceneManager sceneManager;
        private BackgroundWorker worker;

        public MapLoaderBSP()
        {
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
            sceneManager.SetWorldGeometry(mapFile);
        }

        public string LoadedMapName
        {
            get
            {
                return mapFile;
            }
        }

        public string Name
        {
            get
            {
                return "BSP";
            }
        }

        public event Action LoadMapFinished;
        public event Action LoadMapStarted;

        public void LoadAsync(SceneManager sceneManager, string mapFile)
        {
            this.mapFile = mapFile;
            this.sceneManager = sceneManager;
            LoadMapStarted?.Invoke();
        }
    }
}
