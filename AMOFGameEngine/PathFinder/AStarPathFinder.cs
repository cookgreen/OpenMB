using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Game;
using AMOFGameEngine.Graph;
using Mogre;

namespace AMOFGameEngine.PathFinder
{
    public class AStarPathFinder : IPathFinder
    {
        private SparseGraph graph;
        public AStarPathFinder(SparseGraph graph)
        {
            this.graph = graph;
        }
        public Path Find(Vector3 startPos, Vector3 endPos)
        {
            return new Path();
        }
    }
}
