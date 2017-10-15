using System;
using Mogre;
using Mogre_Procedural.MogreBites.Addons;
using MMOC;
using AMOFGameEngine.Mods;
using AMOFGameEngine.RPG;
using AMOFGameEngine.UI;
using AMOFGameEngine.Maps;
using System.ComponentModel;
using AMOFGameEngine.RPG.Managers;
using AMOFGameEngine.RPG.Object;

namespace AMOFGameEngine.States
{
    public class SinglePlayer : AppState
    {
        private CharacterManager characterMgr;
        private MapManager mapMngr;
        private CollisionTools collisionMgr;
        private SdkCameraMan camMan;
        private Player player;

        public SinglePlayer()
        {
            mapMngr = new MapManager();
        }

        public override void enter(Mods.ModData data = null)
        {
            m_Data = data;
            CreateScene();
            SetupTerrain();
            InitGame();

            characterMgr.SpawnPlayer("chara_main_player");
            mapMngr.EnterNewMap(new SceneMap("Cubescene.xml", m_SceneMgr));
            characterMgr.SetSpawnPosition(new Vector3(0, 0,10));
            characterMgr.SpawnCharacter("chara_archer");
            player = characterMgr.GetPlayer();
            
            GameManager.Instance.mRoot.FrameStarted += new FrameListener.FrameStartedHandler(mRoot_FrameStarted);
        }

        bool mRoot_FrameStarted(FrameEvent evt)
        {
            return true;
        }

        public override bool pause()
        {
            return base.pause();
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
        }

        public override void exit()
        {
            GameManager.Instance.AllGameObjects.Clear();

            base.exit();
        }

        private void CreateScene()
        {
            m_SceneMgr = GameManager.Instance.mRoot.CreateSceneManager(SceneType.ST_GENERIC);
            m_SceneMgr.AmbientLight = new ColourValue(0.7f, 0.7f, 0.7f);

            m_Camera = m_SceneMgr.CreateCamera("gameCam");
            m_Camera.NearClipDistance = 5;

            m_Camera.AspectRatio = GameManager.Instance.mViewport.ActualWidth / GameManager.Instance.mViewport.ActualHeight;

            GameManager.Instance.mViewport.Camera = m_Camera;

            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            collisionMgr = new CollisionTools(m_SceneMgr);
            m_Camera.FarClipDistance = 50000;

            m_SceneMgr.SetSkyDome(true, "Examples/CloudySky", 5, 8);

            Light light = m_SceneMgr.CreateLight();
            light.Type = Light.LightTypes.LT_POINT;
            light.Position = new Vector3(-10, 40, 20);
            light.SpecularColour = ColourValue.White;

            camMan = new SdkCameraMan(m_Camera);
            camMan.setStyle(CameraStyle.CS_MANUAL);
            GameManager.Instance.mTrayMgr.hideCursor();
        }

        private void SetInput()
        {
            GameManager.Instance.mMouse.MouseMoved += new MOIS.MouseListener.MouseMovedHandler(mMouse_MouseMoved);
            GameManager.Instance.mMouse.MousePressed += new MOIS.MouseListener.MousePressedHandler(mMouse_MousePressed);
            GameManager.Instance.mMouse.MouseReleased += new MOIS.MouseListener.MouseReleasedHandler(mMouse_MouseReleased);
            GameManager.Instance.mKeyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(mKeyboard_KeyPressed);
            GameManager.Instance.mKeyboard.KeyReleased += new MOIS.KeyListener.KeyReleasedHandler(mKeyboard_KeyReleased);
        }

        private unsafe void SetupTerrain()
        {
                Light terrainLight = base.m_SceneMgr.CreateLight();
                terrainLight.Type = Light.LightTypes.LT_DIRECTIONAL;
                terrainLight.Direction = new Mogre.Vector3(0.55f, -0.3f, 0.75f);
                terrainLight.DiffuseColour = ColourValue.White;
                terrainLight.SpecularColour = new ColourValue(0.4f, 0.4f, 0.4f);
                TerrainGlobalOptions terrainOptions = new TerrainGlobalOptions
                {
                    MaxPixelError = 8f,
                    CompositeMapDistance = 3000f,
                    LightMapDirection = terrainLight.Direction,
                    CompositeMapAmbient = base.m_SceneMgr.AmbientLight,
                    CompositeMapDiffuse = terrainLight.DiffuseColour
                };
                TerrainGroup terrainGroup = new TerrainGroup(base.m_SceneMgr, Terrain.Alignment.ALIGN_X_Z, 0x201, 12000f);
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

        private void InitGame()
        {
            characterMgr = new CharacterManager(m_Camera, GameManager.Instance.mKeyboard, GameManager.Instance.mMouse);
            characterMgr.Init(m_Data.CharacterInfos);
            mapMngr = new MapManager();
        }
    }
}
