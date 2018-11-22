using AMOFGameEngine.Utilities;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Map
{
    public class GameMapEditor : IDisposable
    {
        private GameMap map;
        private AIMesh aimesh;
        private SceneManager scm;

        public GameMap Map
        {
            get
            {
                return map;
            }
        }

        public GameMapEditor(GameMap map)
        {
            this.map = map;
            scm = map.SceneManager;
        }

        public void Initization(AIMesh aimesh)
        {
            this.aimesh = aimesh;
            GenerateVisualAIMesh();
        }

        public void GenerateVisualAIMesh()
        {
            if (aimesh != null)
            {
                int index = 0;
                foreach(var vertexData in aimesh.AIMeshVertexData)
                {
                    AIMeshVertex visualVertex = new AIMeshVertex();
                    visualVertex.Position = vertexData;
                    Entity visualAIMeshVertexEnt = scm.CreateEntity("AIMESH_VERTEX_ENT_"+index, "marker_vertex.mesh");
                    SceneNode visualAIMeshVertexSceneNode = scm.RootSceneNode.CreateChildSceneNode("AIMESH_VERTEX_SCENENODE_" + index);
                    visualAIMeshVertexSceneNode.AttachObject(visualAIMeshVertexEnt);
                    visualAIMeshVertexSceneNode.Position = vertexData;
                    visualAIMeshVertexEnt.QueryFlags = 1 << 0;
                    visualAIMeshVertexEnt.Visible = true;
                    visualVertex.Mesh = visualAIMeshVertexEnt;
                    index++;
                }
                index = 0;
                foreach(var indexData in aimesh.AIMeshIndicsData)
                {
                    int lastVertexNumber = -1;
                    foreach (int vertexNumber in indexData.VertexNumber)
                    {
                        if (lastVertexNumber != -1)
                        {
                            Vector3 startVertexData = aimesh.AIMeshVertexData.ElementAt(lastVertexNumber - 1);
                            Vector3 endVertexData = aimesh.AIMeshVertexData.ElementAt(vertexNumber - 1);
                            Vector3 startToEndVect = new Vector3(
                                endVertexData.x - startVertexData.x,
                                endVertexData.y - startVertexData.y,
                                endVertexData.z - startVertexData.z
                            );
                            Vector3 centralVertexData = new Vector3(
                                (startVertexData.x - endVertexData.x) / 2,
                                (startVertexData.y - endVertexData.y) / 2,
                                (startVertexData.z - endVertexData.z) / 2
                            );

                            AIMeshEdge visualEdge = new AIMeshEdge();
                            visualEdge.Position = centralVertexData;
                            Entity visualAIMeshLineEnt = scm.CreateEntity("AIMESH_LINE_ENT_" + index, "marker_line.mesh");
                            SceneNode visualAIMeshLineSceneNode = scm.RootSceneNode.CreateChildSceneNode("AIMESH_LINE_SCENENODE_" + index);
                            visualAIMeshLineSceneNode.AttachObject(visualAIMeshLineEnt);
                            visualAIMeshLineSceneNode.Position = centralVertexData;
                            Radian angle = Mogre.Math.ACos(startToEndVect.DotProduct(Vector3.UNIT_Z) / startToEndVect.Normalise());
                            visualAIMeshLineSceneNode.Rotate(Vector3.UNIT_Y, angle);
                            visualEdge.Mesh = visualAIMeshLineEnt;
                        }
                        lastVertexNumber = vertexNumber;
                        index++;
                    }
                }
            }
        }

        public void HideVisualAIMesh()
        {
            SceneManager.MovableObjectIterator entities = scm.GetMovableObjectIterator("Entity");
            while (entities.MoveNext())
            {
                MovableObject mo = entities.Current;
                if (mo.QueryFlags == 1 << 0)
                {
                    //Destroy this
                    mo.Visible = false;
                }
            }
        }

        public void ShowVisualAIMesh()
        {
            SceneManager.MovableObjectIterator entities = scm.GetMovableObjectIterator("Entity");
            while (entities.MoveNext())
            {
                MovableObject mo = entities.Current;
                if (mo.QueryFlags == 1 << 0)
                {
                    //Destroy this
                    mo.Visible = true;
                }
            }
        }

        public Entity AddNewAIMeshVertex(Vector3 newVertexPos)
        {
            AIMeshVertex newVertex = new AIMeshVertex();
            newVertex.Position = newVertexPos;
            aimesh.AIMeshVertics.Add(newVertex);
            Entity visualAIMeshVertexEnt = scm.CreateEntity("AIMESH_VERTEX_ENT_" + aimesh.AIMeshVertexData.Count, "marker_vertex.mesh");
            SceneNode visualAIMeshVertexSceneNode = scm.RootSceneNode.CreateChildSceneNode("AIMESH_VERTEX_SCENENODE_" + aimesh.AIMeshVertexData.Count);
            visualAIMeshVertexSceneNode.AttachObject(visualAIMeshVertexEnt);
            visualAIMeshVertexSceneNode.Position = newVertexPos;
            visualAIMeshVertexEnt.QueryFlags = 1 << 0;
            newVertex.Mesh = visualAIMeshVertexEnt;
            return visualAIMeshVertexEnt;
        }

        public Entity AddNewAIMeshLine(Vector3 newLinePos)
        {
            AIMeshEdge newEdge = new AIMeshEdge();
            newEdge.Position = newLinePos;
            aimesh.AIMeshEdges.Add(newEdge);
            Entity visualAIMeshLineEnt = scm.CreateEntity("AIMESH_LINE_ENT_" + aimesh.AIMeshEdges.Count, "marker_line.mesh");
            SceneNode visualAIMeshLineSceneNode = scm.RootSceneNode.CreateChildSceneNode("AIMESH_LINE_SCENENODE_" + aimesh.AIMeshEdges.Count);
            visualAIMeshLineSceneNode.AttachObject(visualAIMeshLineEnt);
            visualAIMeshLineSceneNode.Position = newLinePos;
            visualAIMeshLineEnt.QueryFlags = 1 << 0;
            newEdge.Mesh = visualAIMeshLineEnt;
            return visualAIMeshLineEnt;
        }

        public AIMeshEdge GetAIMeshEdge(Entity ent)
        {
            if(ent != null)
            {
                if(ent.Name.StartsWith("AIMESH_LINE_ENT_"))
                {
                    string edgeIndex = ent.Name.Split('_').Last();
                    return aimesh.AIMeshEdges[int.Parse(edgeIndex)];
                }
            }
            return null;
        }

        public AIMeshVertex GetAIMeshVertex(Entity ent)
        {
            if (ent != null)
            {
                if (ent.Name.StartsWith("AIMESH_VERTEX_ENT_"))
                {
                    string vertexIndex = ent.Name.Split('_').Last();
                    return aimesh.AIMeshVertics[int.Parse(vertexIndex)];
                }
            }
            return null;
        }

        public void Dispose()
        {
            SceneManager.MovableObjectIterator entities = scm.GetMovableObjectIterator("Entity");
            while (entities.MoveNext())
            {
                MovableObject mo = entities.Current;
                if (mo.QueryFlags == 1 << 0)
                {
                    //Destroy this
                    scm.DestroyEntity(mo.Name);
                }
            }
        }
    }
}
