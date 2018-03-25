using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Graph
{
    public class GraphEdge
    {
        private int from;
        private int to;
        private double cost;

        public int From
        {
            get
            {
                return from;
            }

            set
            {
                from = value;
            }
        }
        public int To
        {
            get
            {
                return to;
            }

            set
            {
                to = value;
            }
        }
        public double Cost
        {
            get
            {
                return cost;
            }

            set
            {
                cost = value;
            }
        }

        public GraphEdge(int from, int to)
        {
            this.from = from;
            this.to = to;
            this.cost = 1.0;
        }
        public GraphEdge()
        {
            from = -1;
            to = -1;
            cost = 1.0;
        }
    }
}
