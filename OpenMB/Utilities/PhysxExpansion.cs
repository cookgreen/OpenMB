using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;
using System.IO;

namespace OpenMB.Utilities
{
	public static class PhysxExpansion
	{
		public static ConvexShapeDesc CreateConvexHull(this Physics physics, StaticMeshData meshData)
		{
			// create descriptor for convex hull
			ConvexShapeDesc convexMeshShapeDesc = null;
			ConvexMeshDesc convexMeshDesc = new ConvexMeshDesc();
			convexMeshDesc.PinPoints<float>(meshData.Points, 0, sizeof(float) * 3);
			convexMeshDesc.PinTriangles<uint>(meshData.Indices, 0, sizeof(uint) * 3);
			convexMeshDesc.VertexCount = (uint)meshData.Vertices.Length;
			convexMeshDesc.TriangleCount = (uint)meshData.TriangleCount;
			convexMeshDesc.Flags = ConvexFlags.ComputeConvex;

			MemoryStream stream = new MemoryStream(1024);
			CookingInterface.InitCooking();

			if (CookingInterface.CookConvexMesh(convexMeshDesc, stream))
			{
				stream.Seek(0, SeekOrigin.Begin);
				ConvexMesh convexMesh = physics.CreateConvexMesh(stream);
				convexMeshShapeDesc = new ConvexShapeDesc(convexMesh);
				CookingInterface.CloseCooking();
			}

			convexMeshDesc.UnpinAll();
			return convexMeshShapeDesc;
		}
		public static TriangleMeshShapeDesc CreateTriangleMesh(this Physics physics, StaticMeshData meshData)
		{
			// create descriptor for triangle mesh
			TriangleMeshShapeDesc triangleMeshShapeDesc = null;
			TriangleMeshDesc triangleMeshDesc = new TriangleMeshDesc();
			triangleMeshDesc.PinPoints<float>(meshData.Points, 0, sizeof(float) * 3);
			triangleMeshDesc.PinTriangles<uint>(meshData.Indices, 0, sizeof(uint) * 3);
			triangleMeshDesc.VertexCount = (uint)meshData.Vertices.Length;
			triangleMeshDesc.TriangleCount = (uint)meshData.TriangleCount;

			MemoryStream stream = new MemoryStream(1024);
			CookingInterface.InitCooking();

			if (CookingInterface.CookTriangleMesh(triangleMeshDesc, stream))
			{
				stream.Seek(0, SeekOrigin.Begin);
				TriangleMesh triangleMesh = physics.CreateTriangleMesh(stream);
				triangleMeshShapeDesc = new TriangleMeshShapeDesc(triangleMesh);
				CookingInterface.CloseCooking();
			}

			triangleMeshDesc.UnpinAll();
			return triangleMeshShapeDesc;
		}
	}
}
