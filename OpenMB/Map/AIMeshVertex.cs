using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace OpenMB.Map
{
    public class AIMeshVertex
    {
        private Entity ent;
        private Vector3 position;

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

        public AIMeshVertex()
        {

        }
    }
}
