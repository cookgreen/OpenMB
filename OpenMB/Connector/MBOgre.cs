using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.FileFormats;
using Mogre;
using OpenMB.Map;

namespace OpenMB.Connector
{
	public class MBOgre : IConnector
	{
		private Dictionary<string, MaterialPtr> materials;
		private Dictionary<string, TexturePtr> textures;
		private Dictionary<string, MeshPtr> meshes;
		private Dictionary<string, MBOgreManifest> brfes;
		private static int cp, cv;
		private static MBOgre instance;
		public static MBOgre Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MBOgre();
				}
				return instance;
			}
		}

		public MBOgre()
		{
			materials = new Dictionary<string, MaterialPtr>();
			textures = new Dictionary<string, TexturePtr>();
			meshes = new Dictionary<string, MeshPtr>();
			brfes = new Dictionary<string, MBOgreManifest>();
		}

		public MBOgreManifest LoadBrfFile(MBOgreBrf mbBrfFile)
		{
			MBOgreManifest manifest = new MBOgreManifest();
			if (mbBrfFile == null)
			{
				return null;
			}
			if (brfes.ContainsKey(mbBrfFile.FileName))
			{
				return brfes[mbBrfFile.FileName];
			}
			manifest.MaterialNames = (from mbBrfMaterial in mbBrfFile.Brf.Materials
									  select mbBrfMaterial.name).ToList();
			manifest.MeshNames = (from mbBrfMesh in mbBrfFile.Brf.Meshes
								  select mbBrfMesh.Name).ToList();
			manifest.TextureNames = (from mbBrfTexture in mbBrfFile.Brf.Textures
									 select mbBrfTexture.name).ToList();
			brfes.Add(mbBrfFile.FileName, manifest);
			return manifest;
		}

		public TexturePtr LoadBrfTextureIntoOgre(SceneManager sceneManager, MBBrfTexture brfTexture)
		{
			if (brfTexture == null)
			{
				return null;
			}
			if (textures.ContainsKey(brfTexture.name))
			{
				return textures[brfTexture.name];
			}
			return null;

		}

		public MaterialPtr LoadBrfMaterialIntoOgre(SceneManager sceneManager, MBBrfMaterial brfMaterial)
		{
			if (brfMaterial == null)
			{
				return null;
			}
			if (materials.ContainsKey(brfMaterial.name))
			{
				return materials[brfMaterial.name];
			}
			return null;
		}

		public MeshPtr LoadStaticBrfMeshIntoOgre(SceneManager sceneManager, MBBrfMesh brfMesh, int frame = 0)
		{
			//Convert Vertex and Faces to Ogre Mesh Format
			if (brfMesh == null)
			{
				return null;
			}
			if (meshes.ContainsKey(brfMesh.Name))
			{
				return meshes[brfMesh.Name];
			}
			ManualObject mbMesh = sceneManager.CreateManualObject(brfMesh.Name + "-" + Guid.NewGuid().ToString());
			mbMesh.Begin(brfMesh.Material);

			int np = 0, nv = 0;
			for (int i = 0; i < brfMesh.Frames[frame].pos.Count; i++)
			{
				mbMesh.Position(
					brfMesh.Frames[frame].pos[i].x,
					brfMesh.Frames[frame].pos[i].y,
					brfMesh.Frames[frame].pos[i].z
				);
				np++;
			}

			for (int i = 0; i < brfMesh.Frames[frame].norm.Count; i++)
			{
				mbMesh.Normal(
					-brfMesh.Frames[frame].norm[i].x,
					brfMesh.Frames[frame].norm[i].y,
					brfMesh.Frames[frame].norm[i].z
				);
				mbMesh.TextureCoord(
					brfMesh.Vertex[i].ta.X,
					brfMesh.Vertex[i].ta.Y
				);
				nv++;
			}

			for (int i = 0; i < brfMesh.Faces.Count; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					mbMesh.Triangle(
						(uint)(brfMesh.Vertex[brfMesh.Faces[i].index[j]].index + 1 + cp),
						(uint)(brfMesh.Faces[i].index[j] + 1 + cv),
						(uint)(brfMesh.Faces[i].index[j] + 1 + cv)
					);
				}
			}

			mbMesh.End();

			cp += np;
			cv += nv;

			return mbMesh.ConvertToMesh(brfMesh.Name + "-" + Guid.NewGuid().ToString());
		}

		public MeshPtr LoadWorldMap(string worldMapID, SceneManager sceneManager, MBWorldMap worldMap, bool faceted = false)
		{
			//Convert Vertex and Faces to Ogre Mesh Format
			if (worldMap == null)
			{
				return null;
			}
			if (meshes.ContainsKey(worldMapID))
			{
				return meshes[worldMapID];
			}
			ManualObject worldMapTerrain = sceneManager.CreateManualObject("WORLDMAP-MANUAL-OBJECT-" + worldMapID);
			worldMapTerrain.Begin("", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);

			calculateNormal(worldMapTerrain, worldMap);

			for (int i = 0; i < worldMap.Faces.Count; i++)
			{
				int vindex1 = worldMap.Faces[i].indexFirst;
				int vindex2 = worldMap.Faces[i].indexSecond;
				int vindex3 = worldMap.Faces[i].indexThird;

				float[] vnindex1 = worldMap.fcn[i];
				float[] vnindex2 = worldMap.fcn[i];
				float[] vnindex3 = worldMap.fcn[i];

				worldMapTerrain.Position(
					worldMap.Vertics[vindex1].x,
					worldMap.Vertics[vindex1].z,
					worldMap.Vertics[vindex1].y
				);
				worldMapTerrain.Normal(vnindex1[0], vnindex1[1], vnindex1[2]);
				worldMapTerrain.Colour(worldMap.Color[worldMap.Faces[i].TerrainType]);

				worldMapTerrain.Position(
					worldMap.Vertics[vindex2].x,
					worldMap.Vertics[vindex2].z,
					worldMap.Vertics[vindex2].y
				);
				worldMapTerrain.Normal(vnindex2[0], vnindex2[1], vnindex2[2]);
				worldMapTerrain.Colour(worldMap.Color[worldMap.Faces[i].TerrainType]);

				worldMapTerrain.Position(
					worldMap.Vertics[vindex3].x,
					worldMap.Vertics[vindex3].z,
					worldMap.Vertics[vindex3].y
				);
				worldMapTerrain.Normal(vnindex3[0], vnindex3[1], vnindex3[2]);
				worldMapTerrain.Colour(worldMap.Color[worldMap.Faces[i].TerrainType]);

				worldMapTerrain.Triangle(
					(uint)(vindex1),
					(uint)(vindex2),
					(uint)(vindex3)
				);
			}

			worldMapTerrain.End();

			var mesh = worldMapTerrain.ConvertToMesh("WORLDMAP-" + worldMapID);
			meshes.Add(worldMapID, mesh);
			return mesh;
		}

		/// <summary>
		/// Calculate the normal data
		/// </summary>
		/// <param name="worldMapTerrain">Manual Object</param>
		/// <param name="worldMap">World Map</param>
		private void calculateNormal(ManualObject worldMapTerrain, MBWorldMap worldMap)
		{
			worldMap.fcn = new List<float[]>();
			worldMap.vtn = new List<float[]>();
			worldMap.cfa = new List<float>();

			List<List<int>> vtxi = new List<List<int>>();
			for (int i = 0; i < worldMap.Vertics.Count; i++)
			{
				vtxi.Add(new List<int>());
			}
			for (int i = 0; i < worldMap.Faces.Count; i++)
			{
				worldMap.vtn.Add(new float[] { 0, 0, 0 });
			}

			for (int i = 0; i < worldMap.Faces.Count; i++)
			{
				int vta = worldMap.Faces[i].indexFirst;
				int vtb = worldMap.Faces[i].indexSecond;
				int vtc = worldMap.Faces[i].indexThird;

				vtxi[vta].Add(i);
				vtxi[vtb].Add(i);
				vtxi[vtc].Add(i);

				Vector3 normalData = computeNorm(new int[]
				{
					worldMap.Faces[i].indexFirst,
					worldMap.Faces[i].indexSecond,
					worldMap.Faces[i].indexThird
				}, worldMap.Vertics);
				worldMap.fcn.Add(new float[] { normalData.x, normalData.y, normalData.z });

				worldMap.cfa.Add(computeArea(new int[]
				{
					worldMap.Faces[i].indexFirst,
					worldMap.Faces[i].indexSecond,
					worldMap.Faces[i].indexThird
				}, worldMap.Vertics));
			}

			for (int i = 0; i < vtxi.Count; i++)
			{
				if (vtxi[i] != null && vtxi[i].Count > 0)
				{
					Vector3 triangle = new Vector3(0, 0, 0);
					for (int u = 0; u < vtxi[i].Count; u++)
					{
						var currentface = vtxi[i][u];
						triangle = triangle + new Vector3(worldMap.Faces[currentface].indexFirst, worldMap.Faces[currentface].indexSecond, worldMap.Faces[currentface].indexThird) * worldMap.cfa[currentface];
					}
					var normalizedVector = (triangle / vtxi[i].Count).NormalisedCopy;
					worldMap.vtn[i] = (new float[] { normalizedVector.x, normalizedVector.y, normalizedVector.z });
				}
			}
		}

		private float computeArea(int[] triangle, List<Point3F> Vertics)
		{
			Point3F a = Vertics[triangle[0]];
			Point3F b = Vertics[triangle[1]];
			Point3F c = Vertics[triangle[2]];

			float D1 = (a.ToVector3() - b.ToVector3()).Length;
			float D2 = (b.ToVector3() - c.ToVector3()).Length;
			float D3 = (c.ToVector3() - a.ToVector3()).Length;

			float H = (D1 + D2 + D3) * 0.5f;

			return Mogre.Math.Sqrt((H * (H - D1) * (H - D2) * (H - D3)));
		}

		private Vector3 computeNorm(int[] triangle, List<Point3F> Vertics)
		{
			Point3F a = Vertics[triangle[0]];
			Point3F b = Vertics[triangle[1]];
			Point3F c = Vertics[triangle[2]];

			Vector3 U = new Vector3(
				b.x - a.x,
				b.y - a.y,
				b.z - a.z
			 );

			Vector3 V = new Vector3(
				c.x - a.x,
				c.y - a.y,
				c.z - a.z
			);

			return U.CrossProduct(V).NormalisedCopy;
		}
	}
}
