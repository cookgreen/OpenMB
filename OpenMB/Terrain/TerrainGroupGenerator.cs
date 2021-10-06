using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Terrain
{
	public class TerrainGroupGenerator
	{
		private static TerrainGroupGenerator instance;
		public static TerrainGroupGenerator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TerrainGroupGenerator();
				}
				return instance;
			}
		}

		public TerrainGroupData GenerateTerrain(SceneManager sceneMgr, Light light, bool savedFile = false, string savedFileName = null)
		{
			bool terrainImported = false;
			TerrainGlobalOptions terrainGlobals = new TerrainGlobalOptions();
			TerrainGroup terrainGroup = new TerrainGroup(sceneMgr, Mogre.Terrain.Alignment.ALIGN_X_Z, 513, 12000.0f);
			if (savedFile)
			{
				terrainGroup.SetFilenameConvention(savedFileName, "dat");
			}
			terrainGroup.Origin = Mogre.Vector3.ZERO;

			ConfigureTerrainDefaults(sceneMgr, terrainGlobals, terrainGroup, light);

			for (int x = 0; x <= 0; ++x)
			{
				for (int y = 0; y <= 0; ++y)
				{
					DefineTerrain(terrainGroup, x, y, ref terrainImported);
				}
			}

			terrainGroup.LoadAllTerrains(true);
			if (terrainImported)
			{
				foreach (TerrainGroup.TerrainSlot t in terrainGroup.GetTerrainIterator())
				{
					InitBlendMaps(t.instance);
				}
			}
			terrainGroup.FreeTemporaryResources();
			terrainGroup.SaveAllTerrains(true);

			TerrainGroupData terrainData = new TerrainGroupData(terrainGroup, terrainGlobals);
			return terrainData;
		}

		protected void ConfigureTerrainDefaults(SceneManager sceneMgr, TerrainGlobalOptions terrainGlobals, TerrainGroup terrainGroup, Light light)
		{
			// Configure global
			terrainGlobals.MaxPixelError = 8;
			// testing composite map
			terrainGlobals.CompositeMapDistance = 3000;

			// Important to set these so that the terrain knows what to use for derived (non-realtime) data
			terrainGlobals.LightMapDirection = light.Direction;
			terrainGlobals.CompositeMapAmbient = sceneMgr.AmbientLight;
			terrainGlobals.CompositeMapDiffuse = light.DiffuseColour;

			// Configure default import settings for if we use imported image
			Mogre.Terrain.ImportData defaultimp = terrainGroup.DefaultImportSettings;

			defaultimp.terrainSize = 513;
			defaultimp.worldSize = 12000.0f; // due terrain.png is 8 bpp
			defaultimp.inputScale = 600;
			defaultimp.minBatchSize = 33;
			defaultimp.maxBatchSize = 65;

			// textures
			defaultimp.layerList.Add(new Mogre.Terrain.LayerInstance());
			defaultimp.layerList.Add(new Mogre.Terrain.LayerInstance());
			defaultimp.layerList.Add(new Mogre.Terrain.LayerInstance());

			defaultimp.layerList[0].worldSize = 100;
			defaultimp.layerList[0].textureNames.Add("dirt_grayrocky_diffusespecular.dds");
			defaultimp.layerList[0].textureNames.Add("dirt_grayrocky_normalheight.dds");

			defaultimp.layerList[1].worldSize = 30;
			defaultimp.layerList[1].textureNames.Add("grass_green-01_diffusespecular.dds");
			defaultimp.layerList[1].textureNames.Add("grass_green-01_normalheight.dds");

			defaultimp.layerList[2].worldSize = 200;
			defaultimp.layerList[2].textureNames.Add("growth_weirdfungus-03_diffusespecular.dds");
			defaultimp.layerList[2].textureNames.Add("growth_weirdfungus-03_normalheight.dds");
		}

		protected void DefineTerrain(TerrainGroup terrainGroup, int x, int y, ref bool terrainImported)
		{
			string filename = terrainGroup.GenerateFilename(x, y);

			if (ResourceGroupManager.Singleton.ResourceExists(terrainGroup.ResourceGroup, filename))
				terrainGroup.DefineTerrain(x, y);
			else
			{
				Image img = new Image();
				GetTerrainImage(x % 2 != 0, y % 2 != 0, img);
				terrainGroup.DefineTerrain(x, y, img);
				terrainImported = true;
			}
		}
		protected void GetTerrainImage(bool flipX, bool flipY, Image img)
		{
			img.Load("terrain.png", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);

			if (flipX)
				img.FlipAroundX();

			if (flipY)
				img.FlipAroundY();
		}
		protected unsafe void InitBlendMaps(Mogre.Terrain terrain)
		{
			TerrainLayerBlendMap blendMap0 = terrain.GetLayerBlendMap(1);
			TerrainLayerBlendMap blendMap1 = terrain.GetLayerBlendMap(2);

			float minHeight0 = 70, minHeight1 = 70;
			float fadeDist0 = 40, fadeDist1 = 15;

			float* pBlend1 = blendMap1.BlendPointer;

			for (int y = 0; y < terrain.LayerBlendMapSize; ++y)
				for (int x = 0; x < terrain.LayerBlendMapSize; ++x)
				{
					float tx, ty;

					blendMap0.ConvertImageToTerrainSpace((uint)x, (uint)y, out tx, out ty);

					float height = terrain.GetHeightAtTerrainPosition(tx, ty);
					float val = (height - minHeight0) / fadeDist0;
					val = Clamp(val, 0, 1);

					val = (height - minHeight1) / fadeDist1;
					val = Clamp(val, 0, 1);
					*pBlend1++ = val;
				}

			blendMap0.Dirty();
			blendMap0.Update();
			blendMap1.Dirty();
			blendMap1.Update();
		}
		protected float Clamp(float value, float min, float max)
		{
			if (value <= min)
				return min;
			else if (value >= max)
				return max;

			return value;
		}
	}
}
