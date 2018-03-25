using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Game;
using Mogre;

namespace AMOFGameEngine.PathFinder
{
    public interface IPathFinder
    {
        Path Find(Vector3 startPos, Vector3 endPos);
    }
}
