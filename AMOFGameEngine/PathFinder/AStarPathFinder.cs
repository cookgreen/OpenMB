using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Game;
using AMOFGameEngine.PathFinder;
using Mogre;

namespace AMOFGameEngine.PathFinder
{
    public class AStarPathFinder : IPathFinder
    {
        private NavGraph graph;
        public AStarPathFinder(NavGraph graph)
        {
            this.graph = graph;
        }
        public Path Find(NavGraphPoint startPoint, NavGraphPoint endPoint)
        { 
            List<NavGraphPoint> closeLst = new List<NavGraphPoint>();
            List<NavGraphPoint> openLst = new List<NavGraphPoint>();

            openLst.Add(startPoint);
            while (openLst.Count > 0)
            {
            }

            return new Path();
        }
    }
}
