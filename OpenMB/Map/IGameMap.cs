using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Map
{
    public interface IGameMap
    {
        void LoadAsync();
        void Destroy();
        void Update(float timeSinceLastFrame);
        string Name { get; }
        List<GameObject> GetGameObjects(string objectID);

		event MapLoadhandler LoadMapStarted;
		event MapLoadhandler LoadMapFinished;
	}
}
