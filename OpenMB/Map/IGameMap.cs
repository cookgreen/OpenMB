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
        string GetName();
        List<GameObject> GetGameObjects(string objectID);
    }
}
