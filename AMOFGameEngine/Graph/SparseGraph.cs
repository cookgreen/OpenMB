using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Graph
{
    public class SparseGraph
    {
        public List<GraphNode> NodeList { get; set; }
        public List<GraphEdge> EdgeList { get; set; }

        public SparseGraph()
        {
            NodeList = new List<GraphNode>();
            EdgeList = new List<GraphEdge>();
        }

        public void AddNode(GraphNode newNode)
        {
            NodeList.Add(newNode);
        }

        public void RemoveNode(GraphNode node)
        {
            if (node != null)
            {
                NodeList.Remove(node);
                List<GraphEdge> tempEdgeList = new List<GraphEdge>(EdgeList);
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
            EdgeList.Add(new GraphEdge(fromIndex, toIndex));
        }

        public void RemoveEdge(GraphEdge edge)
        {
            EdgeList.Remove(edge);
        }
    }
}
