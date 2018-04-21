using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Game;
using Mogre;
using AMOFGameEngine.PathFinder;

namespace AMOFGameEngine.PathFinder
{
    public interface IPathFinder
    {
        Path Find(NavGraphPoint startPoint, NavGraphPoint endPoint);
    }
}
