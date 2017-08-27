using System;
using System.Collections.Generic;
using Mogre;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Maps;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Network;
using AMOFGameEngine.Widgets;

namespace AMOFGameEngine.States
{
    public class Multiplayer : AppState
    {
        private delegate bool ServerStartDelegate();
        private InputBox ibServerName;
        private InputBox ibServerPort;
        private CheckBox chkHasPasswd;
        private InputBox ibPasswd;
        private MapManager mapMnger;
        private GameServer thisServer;
        private Dictionary<string, string> option;
        private StringVector serverState;
        private ParamsPanel serverpanel;
        private bool isEscapeMenuOpened;

        public Multiplayer()
        {
            mapMnger = new MapManager();
            option = new Dictionary<string, string>();
            serverState = new StringVector();
        }

        public override void enter(Mods.ModData e = null)
        {
            m_Data = e;
            m_SceneMgr = GameManager.Singleton.mRoot.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");
            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            m_SceneMgr.AmbientLight = cvAmbineLight;
            m_Camera = m_SceneMgr.CreateCamera("multiplayerCam");
            GameManager.Singleton.mViewport.Camera = m_Camera;
            m_Camera.AspectRatio = GameManager.Singleton.mViewport.ActualWidth / GameManager.Singleton.mViewport.ActualHeight;

            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            GameManager.Singleton.mTrayMgr.createLabel(TrayLocation.TL_CENTER, "lbMultiplayer", "Multiplayer", 150);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER,"btnHost","Host Game",200);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "btnJoin", "Join Game",200);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "btnBack", "Back", 200);

            GameManager.Singleton.mKeyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(mKeyboard_KeyPressed);
            GameManager.Singleton.mKeyboard.KeyReleased += new MOIS.KeyListener.KeyReleasedHandler(mKeyboard_KeyReleased);
            
            thisServer = new GameServer();
            thisServer.OnEscapePressed += new Action(Server_OnEscapePressed);
        }

        void HostGameUI()
        {
            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            GameManager.Singleton.mTrayMgr.createLabel(TrayLocation.TL_CENTER, "lbHost", "Host Game", 300);
            ibServerName = GameManager.Singleton.mTrayMgr.createInputBox(TrayLocation.TL_CENTER, "ibServerName", "Server Name:",300, 180, "New Server");
            ibServerPort = GameManager.Singleton.mTrayMgr.createInputBox(TrayLocation.TL_CENTER, "ibServerPort", "Server Port:",300, 180, "7458",true);
            chkHasPasswd = GameManager.Singleton.mTrayMgr.createCheckBox(TrayLocation.TL_CENTER, "chkHasPass", "Has Password", 300);
            GameManager.Singleton.mTrayMgr.createLongSelectMenu(TrayLocation.TL_CENTER, "smServerMaps", "Server Map:", 190, 10);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnOK", "OK");
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnCancel", "Cancel");
        }

        void Server_OnEscapePressed()
        {
            ShowEscapeMenu();
        }

        private void ShowEscapeMenu()
        {
        }

        bool mKeyboard_KeyReleased(MOIS.KeyEvent arg)
        {
            return GameManager.Singleton.mTrayMgr.injectKeyReleased(arg);
        }

        bool mKeyboard_KeyPressed(MOIS.KeyEvent arg)
        {
            if (arg.key == MOIS.KeyCode.KC_ESCAPE)
            {
                if (!this.isEscapeMenuOpened)
                {
                    this.BuildEscapeMenu();
                }
                else
                {
                    GameManager.Singleton.mTrayMgr.destroyAllWidgets();
                    this.serverpanel = GameManager.Singleton.mTrayMgr.createParamsPanel(TrayLocation.TL_CENTER, "serverpanel", 400f, this.serverState);
                    this.isEscapeMenuOpened = false;
                }
            }
            return GameManager.Singleton.mTrayMgr.injectKeyPressed(arg);
        }

        private void BuildEscapeMenu()
        {
            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "choose_side", "Choose Side", 200f);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "choose_chara", "Choose Character", 200f);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "exit_multiplayer", "Exit", 200f);
            this.isEscapeMenuOpened = true;
        }

        //build a dummy scene...
        private unsafe void BuildGameSccene()
        {
            m_SceneMgr.SetSkyBox(true, "Examples/SpaceSkyBox");
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
                        val = AMOFGameEngine.Utilities.Math.Clamp(val, 0f, 1f);
                        *pBlend0++ = val;
                        val = (height - minHeight1) / fadeDist1;
                        val = AMOFGameEngine.Utilities.Math.Clamp(val, 0f, 1f);
                        *pBlend1++ = val;
                    }
                }
                blendMap0.Dirty();
                blendMap1.Dirty();
                blendMap0.Update();
                blendMap1.Update();
            }
        }

        public override void buttonHit(Button button)
        {
            if (button.getName() == "btnJoin")
            {

            }
            else if (button.getName() == "btnHost")
            {
                HostGameUI();
            }
            else if (button.getName() == "btnCancel")
            {
                exit();
                enter();
            }
            else if (button.getName() == "btnOK")
            {
                GameManager.Singleton.mTrayMgr.destroyAllWidgets();
                serverpanel=GameManager.Singleton.mTrayMgr.createParamsPanel(TrayLocation.TL_CENTER, "serverpanel", 400, serverState);
                BuildGameSccene();
                ServerStartDelegate server = new ServerStartDelegate(ServerStart);
                server.Invoke();
            }
            else if (button.getName() == "btnBack")
            {
                changeAppState(findByName("MainMenu"), m_Data);
            }
        }

        public bool ServerStart()
        {
            thisServer.Init();
            return thisServer.Go();
        }

        public override bool pause()
        {
            return base.pause();
        }

        public override void update(double timeSinceLastFrame)
        {
            if (thisServer.Started)
            {
                thisServer.Update();
                thisServer.GetServerState(ref serverState);
                if(!isEscapeMenuOpened)
                    serverpanel.setAllParamValues(serverState);
            }
        }

        public override void exit()
        {
            if (m_SceneMgr != null)
            {
                m_SceneMgr.DestroyCamera(m_Camera);
                GameManager.Singleton.mRoot.DestroySceneManager(m_SceneMgr);
            }
            thisServer.Exit();
        }

        public override void checkBoxToggled(CheckBox box)
        {
           if (box == chkHasPasswd)
           {
               if (box.isChecked())
               {
                   ibPasswd = GameManager.Singleton.mTrayMgr.createInputBox(TrayLocation.TL_CENTER,"ibPasswd","Password:",300,180);
               }
               else
               {
                   GameManager.Singleton.mTrayMgr.destroyWidget("ibPasswd");
               }
           }
        }
    }
}
