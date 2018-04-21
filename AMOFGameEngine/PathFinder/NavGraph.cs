using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.PathFinder
{
    public class NavGraph
    {
        public List<NavGraphPoint> NodeList { get; set; }
        public List<NavGraphEdge> EdgeList { get; set; }

        public NavGraph()
        {
            NodeList = new List<NavGraphPoint>();
            EdgeList = new List<NavGraphEdge>();
        }

        public void AddNode(NavGraphPoint newNode)
        {
            NodeList.Add(newNode);
        }

        public void RemoveNode(NavGraphPoint node)
        {
            if (node != null)
            {
                NodeList.Remove(node);
                List<NavGraphEdge> tempEdgeList = new List<NavGraphEdge>(EdgeList);
                for (int i = 0; i < EdgeList.Count; i++)
                {
                    if (EdgeList[i].From == node.Index || EdgeList[i].To == node.Index)
                    {
                        tempEdgeList.Remove(EdgeList[i]);
                    }
                }
            }
        }
        public void AddEdge(int fromIndex, int toIndex)
        {
            EdgeList.Add(new NavGraphEdge(fromIndex, toIndex));
        }

        public void RemoveEdge(NavGraphEdge edge)
        {
            EdgeList.Remove(edge);
        }
    }
}
