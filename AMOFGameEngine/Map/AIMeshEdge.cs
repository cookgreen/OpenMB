using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Map
{
    public class AIMeshEdge
    {
        private AIMeshVertex vertex1;
        private AIMeshVertex vertex2;
        private Vector3 position;
        private Entity ent;

        public AIMeshVertex Vertex1
        {
            get
            {
                return vertex1;
            }

            set
            {
                vertex1 = value;
            }
        }
        public AIMeshVertex Vertex2
        {
            get
            {
                return vertex2;
            }

            set
            {
                vertex2 = value;
            }
        }
        public Vector3 Position
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
        public Entity Mesh
        {
            get
            {
                return ent;
            }

            set
            {
                ent = value;
            }
        }

        public AIMeshEdge()
        {
            vertex1 = null;
            vertex2 = null;
        }

        public AIMeshEdge(AIMeshVertex vertex1, AIMeshVertex vertex2)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }

        public void Connect(AIMeshVertex vertex1, AIMeshVertex vertex2)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }
    }
}
