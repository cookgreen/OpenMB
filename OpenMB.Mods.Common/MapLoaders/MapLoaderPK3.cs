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
    public class MapLoaderPK3 : IGameMapLoader
    {
        private BackgroundWorker worker;

        public MapLoaderPK3()
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
        }

        public AIMesh AIMesh
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string LoadedMapName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                return "PK3";
            }
        }

        public event Action LoadMapFinished;
        public event Action LoadMapStarted;

        public void LoadAsync(string mapFile)
        {
            LoadMapStarted?.Invoke();
        }
    }
}
