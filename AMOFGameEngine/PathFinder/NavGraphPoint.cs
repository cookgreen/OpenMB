using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.PathFinder
{
    public class NavGraphPoint
    {
        private int index;
        private Vector3 position;
        private NavGraphPoint parent;
        private NavGraphPoint sub;
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

        public NavGraphPoint Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        public NavGraphPoint SubNode
        {
            get
            {
                return sub;
            }

            set
            {
                sub = value;
            }
        }

        public NavGraphPoint()
        {
            index = -1;
            position = new Vector3();
        }

        public NavGraphPoint(int index)
        {
            this.index = index;
            position = new Vector3();
        }

        public NavGraphPoint(Vector3 position)
        {
            this.position = position;
        }

        public static bool operator == (NavGraphPoint node1, NavGraphPoint node2)
        {
            return node1.Position == node2.Position;
        }

        public static bool operator !=(NavGraphPoint node1, NavGraphPoint node2)
        {
            return node1.Position != node2.Position;
        }
    }
}
