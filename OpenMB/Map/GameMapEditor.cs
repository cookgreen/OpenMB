using OpenMB.Utilities;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Map
{
	public class GameMapEditor : IDisposable
	{
		private GameMap map;
		private AIMesh aimesh;
		private SceneManager scm;
		private Entity objPivot;

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
			aimesh = map.AIMesh;
			scm = map.SceneManager;
			objPivot = map.CreateEntityWithMaterial("MARKER_PIVOT", "marker_pivot.mesh", "marker_pivot");
			SceneNode objPivotSceneNode = scm.RootSceneNode.CreateChildSceneNode();
			objPivotSceneNode.AttachObject(objPivot);
			objPivot.Visible = false;
		}

		public void Initization()
		{
			GenerateVisualAIMesh();
		}

		public void GenerateVisualAIMesh()
		{
			if (aimesh != null)
			{
				int index = 0;
				foreach (var vertexData in aimesh.AIMeshVertexData)
				{
					AIMeshVertex visualVertex = new AIMeshVertex();
					visualVertex.Position = vertexData;
					Entity visualAIMeshVertexEnt = scm.CreateEntity("AIMESH_VERTEX_ENT_" + index, "marker_vertex.mesh");
					SceneNode visualAIMeshVertexSceneNode = scm.RootSceneNode.CreateChildSceneNode("AIMESH_VERTEX_SCENENODE_" + index);
					visualAIMeshVertexSceneNode.AttachObject(visualAIMeshVertexEnt);
					visualAIMeshVertexSceneNode.Position = vertexData;
					visualAIMeshVertexEnt.QueryFlags = 1 << 0;
					visualAIMeshVertexEnt.Visible = true;
					visualVertex.Mesh = visualAIMeshVertexEnt;
					index++;
				}
				index = 0;
				foreach (var indexData in aimesh.AIMeshIndicsData)
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
							Entity visualAIMeshLineEnt = map.CreateEntityWithMaterial(
								"AIMESH_LINE_ENT_" + Guid.NewGuid().ToString(),
								"marker_line.mesh",
								"marker_line"
							);
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
			Entity visualAIMeshVertexEnt = scm.CreateEntity("AIMESH_VERTEX_ENT_" + Guid.NewGuid().ToString(), "marker_vertex.mesh");
			SceneNode visualAIMeshVertexSceneNode = scm.RootSceneNode.CreateChildSceneNode("AIMESH_VERTEX_SCENENODE_" + Guid.NewGuid().ToString());
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

		public Entity ConnectTwoVertex(Vector3 vertexPos1, Vector3 vertexPos2)
		{
			Vector3 vect = vertexPos1 - vertexPos2;
			Vector3 edgePos = new Vector3(
				(vertexPos1.x - vertexPos2.x) / 2,
				(vertexPos1.y - vertexPos2.y) / 2,
				(vertexPos1.z - vertexPos2.z) / 2
			);
			Entity visualAIMeshLineEnt = scm.CreateEntity("AIMESH_LINE_ENT_" + aimesh.AIMeshEdges.Count, "marker_line.mesh");
			SceneNode visualAIMeshLineSceneNode = scm.RootSceneNode.CreateChildSceneNode("AIMESH_LINE_SCENENODE_" + aimesh.AIMeshEdges.Count);
			visualAIMeshLineSceneNode.AttachObject(visualAIMeshLineEnt);
			visualAIMeshLineSceneNode.Position = edgePos;

			Quaternion oritentation = visualAIMeshLineSceneNode.Orientation;
			Vector3 vect2 = oritentation * Vector3.UNIT_Z;
			Vector3 axis = vect.CrossProduct(vect2);
			vect.Normalise();
			vect2.Normalise();
			Radian angle = Mogre.Math.ACos(vect.DotProduct(vect2));
			Quaternion rotate = new Quaternion(angle, axis);
			visualAIMeshLineSceneNode.Rotate(rotate);

			return visualAIMeshLineEnt;
		}

		public AIMeshEdge GetAIMeshEdge(Entity ent)
		{
			if (ent != null)
			{
				if (ent.Name.StartsWith("AIMESH_LINE_ENT_"))
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
			map.CameraHanlder.RestoreLastMode();
			//SceneManager.MovableObjectIterator entities = scm.GetMovableObjectIterator("Entity");
			//while (entities.MoveNext())
			//{
			//    MovableObject mo = entities.Current;
			//    if (mo.Name.StartsWith("AIMESH"))
			//    {
			//        //Destroy this
			//        scm.DestroyEntity(mo.Name);
			//    }
			//}
			//scm.DestroyEntity("MARKER_PIVOT");
		}

		public void ShowPivotAtPosition(Vector3 entCenterPos)
		{
			objPivot.ParentSceneNode.SetVisible(true);
			objPivot.ParentSceneNode.SetPosition(entCenterPos.x, entCenterPos.y, entCenterPos.z);
		}

		public void HidePivot()
		{
			objPivot.ParentSceneNode.SetVisible(false);
		}
	}
}
