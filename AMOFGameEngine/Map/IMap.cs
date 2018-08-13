using AMOFGameEngine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Map
{
    public interface IMap
    {
        event MapLoadhandler LoadMapStarted;
        event MapLoadhandler LoadMapFinished;
        void LoadAsync();
        void Destroy();
        void Update(float timeSinceLastFrame);
        string GetName();
        List<Character> GetAgents();
        List<GameObject> GetStaticObjects();
    }
}
