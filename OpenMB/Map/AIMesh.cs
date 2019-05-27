using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using org.critterai.nav;

namespace OpenMB.Map
{
    public class AIMesh
    {
        private Navmesh navmesh;
        public List<Vector3> AIMeshVertexData { get; set; }
        public List<AIMeshIndexData> AIMeshIndicsData { get; set; }
        public List<AIMeshVertex> AIMeshVertics { get; set; }
        public List<AIMeshEdge> AIMeshEdges { get; set; }
        public AIMesh()
        {
            navmesh = null;
            AIMeshVertexData = new List<Vector3>();
            AIMeshIndicsData = new List<AIMeshIndexData>();
            AIMeshVertics = new List<AIMeshVertex>();
            AIMeshEdges = new List<AIMeshEdge>();
        }

        public void GenerateNavMesh()
        {

        }

        public List<Vector3> QueryNavMesh(Vector3 startPoint, Vector3 endPoint)
        {
            return new List<Vector3>();
        }
        public void Dispose()
        {
            AIMeshIndicsData.Clear();
            AIMeshVertexData.Clear();
            navmesh.RequestDisposal();
        }
    }
}
