using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Graph
{
    public class GraphNode
    {
        protected int index;
        private Vector3 position;
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        protected Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public GraphNode()
        {
            index = -1;
            position = new Vector3();
        }

        public GraphNode(int index)
        {
            this.index = index;
            position = new Vector3();
        }

        public GraphNode(int index, Vector3 position)
        {
            this.index = index;
            this.position = position;
        }
    }
}
