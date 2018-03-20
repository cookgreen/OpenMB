using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Helper;
using MOIS;
using AMOFGameEngine.Mods.XML;
using AMOFGameEngine.Script;
using AMOFGameEngine.Mods;

namespace AMOFGameEngine.Game
{
    /// <summary>
    /// Generate By a map, including characters, terrain etc
    /// </summary>
    public class GameWorld
    {
        //MOD Data
        private ModData modData;

        //Agents Data
        private List<Character> agents;

        //Terrain Data
        private Terrain terrian;

        //Static Objects Data
        private List<GameObject> staticObjects;

        //For Render
        private SceneManager scm;
        private Camera cam;
        private Mogre.Vector3 m_TranslateVector;

        //Map Loader and its script
        private DotSceneLoader sceneLoader;
        private ScriptLoader scriptLoader;

        //Map file name
        private string sceneName;

        public Camera Cam { get; internal set; }

        public GameWorld(ModData modData)
        {
            this.modData = modData;
            sceneLoader = new DotSceneLoader();
            scriptLoader = new ScriptLoader();
        }

        public void Init()
        {
            scm = GameManager.Instance.mRoot.CreateSceneManager(SceneType.ST_GENERIC);
            scm.AmbientLight = new ColourValue(0.7f, 0.7f, 0.7f);

            cam = scm.CreateCamera("gameCam");
            cam.NearClipDistance = 5;

            cam.AspectRatio = GameManager.Instance.mViewport.ActualWidth / GameManager.Instance.mViewport.ActualHeight;

            GameManager.Instance.mViewport.Camera = cam;

            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            cam.FarClipDistance = 50000;

            scm.SetSkyDome(true, "Examples/CloudySky", 5, 8);

            Light light = scm.CreateLight();
            light.Type = Light.LightTypes.LT_POINT;
            light.Position = new Mogre.Vector3(-10, 40, 20);
            light.SpecularColour = ColourValue.White;
            
            GameManager.Instance.mTrayMgr.hideCursor();
            
            GameManager.Instance.mMouse.MouseMoved += new MOIS.MouseListener.MouseMovedHandler(mMouse_MouseMoved);
            GameManager.Instance.mMouse.MousePressed += new MOIS.MouseListener.MousePressedHandler(mMouse_MousePressed);
            GameManager.Instance.mMouse.MouseReleased += new MOIS.MouseListener.MouseReleasedHandler(mMouse_MouseReleased);
            GameManager.Instance.mKeyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(mKeyboard_KeyPressed);
            GameManager.Instance.mKeyboard.KeyReleased += new MOIS.KeyListener.KeyReleasedHandler(mKeyboard_KeyReleased);
        }

        public void SpawnNewCharacter(ModCharacterDfnXML modCharacterDfnXML, Mogre.Vector3 position, bool isBot = true)
        {
            Character c = new Character(this, cam, agents.Count, modCharacterDfnXML.Name, modCharacterDfnXML.MeshName, position, isBot);
            agents.Add(c);
        }

        public void Destroy()
        {
            agents.Clear();
            staticObjects.Clear();
            cam.Dispose();
            scm.Dispose();
        }

        public void ChangeScene(string sceneName)
        {
            this.sceneName = sceneName;
            agents = new List<Character>();
            staticObjects = new List<GameObject>();
            sceneLoader.ParseDotScene(sceneName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, scm);
            scriptLoader.Parse(System.IO.Path.GetFileNameWithoutExtension(sceneName) + ".script", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, this);
        }

        public string GetCurrentScene()
        {
            return sceneName;
        }

        public ModData GetModData()
        {
            return modData;
        }
        public void getInput()
        {
            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_A))
                m_TranslateVector.x = -10;

            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_D))
                m_TranslateVector.x = 10;

            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_W))
                m_TranslateVector.z = -10;

            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_S))
                m_TranslateVector.z = 10;

            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_Q))
                m_TranslateVector.y = -10;

            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_E))
                m_TranslateVector.y = 10;

            //camera roll
            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_Z))
                cam.Roll(new Angle(-10));

            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_X))
                cam.Roll(new Angle(10));

            //reset roll
            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_C))
                cam.Roll(-(cam.RealOrientation.Roll));
        }

        public void Update(float timeSinceLastFrame)
        {
            m_TranslateVector = new Mogre.Vector3(0, 0, 0);
            getInput();
            moveCamera();
            updateAgents(timeSinceLastFrame);
        }
        private void updateAgents(float timeSinceLastFrame)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Update(timeSinceLastFrame);
            }
        }
        public void moveCamera()
        {
            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                cam.MoveRelative(m_TranslateVector);
            cam.MoveRelative(m_TranslateVector / 10);
        }
        private unsafe void SetupTerrain()
        {
            Light terrainLight = scm.CreateLight();
            terrainLight.Type = Light.LightTypes.LT_DIRECTIONAL;
            terrainLight.Direction = new Mogre.Vector3(0.55f, -0.3f, 0.75f);
            terrainLight.DiffuseColour = ColourValue.White;
            terrainLight.SpecularColour = new ColourValue(0.4f, 0.4f, 0.4f);
            TerrainGlobalOptions terrainOptions = new TerrainGlobalOptions
            {
                MaxPixelError = 8f,
                CompositeMapDistance = 3000f,
                LightMapDirection = terrainLight.Direction,
                CompositeMapAmbient = scm.AmbientLight,
                CompositeMapDiffuse = terrainLight.DiffuseColour
            };
            TerrainGroup terrainGroup = new TerrainGroup(scm, Terrain.Alignment.ALIGN_X_Z, 0x201, 12000f);
            terrainGroup.SetFilenameConvention("terrain", "dat");
            terrainGroup.Origin = Mogre.Vector3.ZERO;
            Terrain.ImportData importdata = terrainGroup.DefaultImportSettings;
            importdata.terrainSize = 0x201;
            importdata.worldSize = 12000f;
            importdata.inputScale = 600f;
            importdata.minBatchSize = 0x21;
            importdata.maxBatchSize = 0x41;
            importdata.layerList.Resize(3, new Terrain.LayerInstance());
            importdata.layerList[0].worldSize = 100f;
            importdata.layerList[0].textureNames.Add("dirt_grayrocky_diffusespecular.dds");
            importdata.layerList[0].textureNames.Add("dirt_grayrocky_normalheight.dds");
            importdata.layerList[1].worldSize = 30f;
            importdata.layerList[1].textureNames.Add("grass_green-01_diffusespecular.dds");
            importdata.layerList[1].textureNames.Add("grass_green-01_normalheight.dds");
            importdata.layerList[2].worldSize = 200f;
            importdata.layerList[2].textureNames.Add("growth_weirdfungus-03_diffusespecular.dds");
            importdata.layerList[2].textureNames.Add("growth_weirdfungus-03_normalheight.dds");
            for (int x = 0; x <= 0; x++)
            {
                for (int y = 0; y <= 0; y++)
                {
                    string fileName = terrainGroup.GenerateFilename(x, y);
                    if (ResourceGroupManager.Singleton.ResourceExists(terrainGroup.ResourceGroup, fileName))
                    {
                        terrainGroup.DefineTerrain(x, y);
                    }
                    else
                    {
                        Image img = new Image();
                        img.Load("terrain.png", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                        if ((x % 2) != 0)
                        {
                            img.FlipAroundY();
                        }
                        if ((y % 2) != 0)
                        {
                            img.FlipAroundX();
                        }
                        terrainGroup.DefineTerrain(x, y, img);
                    }
                }
            }
            terrainGroup.LoadAllTerrains(true);
            TerrainGroup.TerrainIterator ti = terrainGroup.GetTerrainIterator();
            while (ti.MoveNext())
            {
                Terrain t = ti.Current.instance;
                float minHeight0 = 70f;
                float fadeDist0 = 40f;
                float minHeight1 = 70f;
                float fadeDist1 = 40f;
                TerrainLayerBlendMap blendMap0 = t.GetLayerBlendMap(1);
                TerrainLayerBlendMap blendMap1 = t.GetLayerBlendMap(2);
                float* pBlend0 = blendMap0.BlendPointer;
                float* pBlend1 = blendMap0.BlendPointer;
                for (ushort y = 0; y <= t.LayerBlendMapSize; y = (ushort)(y + 1))
                {
                    for (ushort x = 0; x <= t.LayerBlendMapSize; x = (ushort)(x + 1))
                    {
                        float tx;
                        float ty;
                        blendMap0.ConvertImageToTerrainSpace(x, y, out tx, out ty);
                        float height = t.GetHeightAtTerrainPosition(tx, ty);
                        float val = (height - minHeight0) / fadeDist0;
                        val = AMOFGameEngine.Utilities.Helper.Clamp<float>(val, 0f, 1f);
                        *pBlend0++ = val;
                        val = (height - minHeight1) / fadeDist1;
                        val = AMOFGameEngine.Utilities.Helper.Clamp<float>(val, 0f, 1f);
                        *pBlend1++ = val;
                    }
                }
                blendMap0.Dirty();
                blendMap1.Dirty();
                blendMap0.Update();
                blendMap1.Update();
            }
        }
        
        bool mKeyboard_KeyReleased(MOIS.KeyEvent arg)
        {
            return true;
        }

        bool mKeyboard_KeyPressed(MOIS.KeyEvent arg)
        {
            return true;
        }

        bool mMouse_MouseReleased(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            return true;
        }

        bool mMouse_MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            return true;
        }

        bool mMouse_MouseMoved(MOIS.MouseEvent arg)
        {
            return true;
        }
    }
}
