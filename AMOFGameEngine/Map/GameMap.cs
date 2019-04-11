using AMOFGameEngine.Game;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Screen;
using AMOFGameEngine.Script;
using AMOFGameEngine.Trigger;
using AMOFGameEngine.Utilities;
using DotSceneLoader;
using Mogre;
using Mogre.PhysX;
using MOIS;
using org.critterai.nav;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Map
{
    public delegate void MapLoadhandler();

    /// <summary>
    /// Define a map in the game
    /// </summary>
    public class GameMap : IMap
    {
        private string mapName;
        private List<ITrigger> mapTriggers;
        private DotSceneLoader.DotSceneLoader mapLoader;
        private List<Character> agents;
        private List<GameObject> gameObjects;
        private List<ActorNode> actorNodeList;
        private ScriptLoader scriptLoader;
        private SceneManager scm;
        //private TerrainGroup terrianGroup;
        private Scene physicsScene;
        //private NavmeshQuery query;
        private ModData modData;
        private Physics physics;
        private ControllerManager controllerMgr;
        private Character playerAgent;
        private Camera cam;
        private GameWorld world;
        private AIMesh aimesh;
        private List<Mogre.Vector3> aimeshVertexData;
        private List<Mogre.Vector3> aimeshIndexData;
        private GameMapEditor editor;
        private CameraHandler cameraHanlder;
        private bool combineKey;
        private KeyCode combineKeyCode;

        public ModData ModData
        {
            get
            {
                return modData;
            }
        }
        public Scene PhysicsScene
        {
            get
            {
                return physicsScene;
            }
        }
        public NavmeshQuery NavmeshQuery
        {
            get
            {
                return null;
            }
        }
        public SceneManager SceneManager
        {
            get
            {
                return world.SceneManager;
            }
        }
        public Camera Camera
        {
            get
            {
                return world.Camera;
            }
        }
        public Character PlayerAgent
        {
            get { return playerAgent; }
            set { playerAgent = value; }
        }

        public event MapLoadhandler LoadMapStarted;
        public event MapLoadhandler LoadMapFinished;

        public GameMap(string name, GameWorld world)
        {
            mapName = name;
            scriptLoader = new ScriptLoader();
            mapTriggers = new List<ITrigger>();
            actorNodeList = new List<ActorNode>();
            this.world = world;
            scm = world.SceneManager;
            modData = world.ModData;
            cam = world.Camera;
            physicsScene = world.PhysicsScene;
            physics = world.PhysicsScene.Physics;
            controllerMgr = physics.ControllerManager;
            aimeshIndexData = new List<Mogre.Vector3>();
            aimeshVertexData = new List<Mogre.Vector3>();
            editor = new GameMapEditor(this);
            cameraHanlder = new CameraHandler(this);
            combineKey = false;

            GameManager.Instance.mouse.MouseMoved += Mouse_MouseMoved;
            GameManager.Instance.mouse.MousePressed += Mouse_MousePressed;
            GameManager.Instance.mouse.MouseReleased += Mouse_MouseReleased;
            GameManager.Instance.keyboard.KeyPressed += Keyboard_KeyPressed;
            GameManager.Instance.keyboard.KeyReleased += Keyboard_KeyReleased;
        }

        public Item Produce(string desc, string meshName, ItemType type, ItemUseAttachOption attachOptionWhenUse, ItemHaveAttachOption attachOptionWhenHave, double damage, int range, GameWorld world, int ammoCapcity, double amourNum)
        {
            return ItemFactory.Instance.Produce(gameObjects.Count, desc, meshName, type, attachOptionWhenUse,
                   attachOptionWhenHave, damage, range, world, ammoCapcity, amourNum);
        }

        public void CreateCharacter(string characterID, Mogre.Vector3 position, string teamId, bool isBot = true)
        {
            var findTrooperList = ModData.CharacterInfos.Where(o => o.ID == characterID);
            if (findTrooperList.Count() == 0)
            {
                GameManager.Instance.log.LogMessage("CREATE TROOP FAILED: Invalid trooper id!", LogMessage.LogType.Warning);
                return;
            }
            var findTrooper = findTrooperList.First();

            var findSkinList = ModData.SkinInfos.Where(o => o.skinID == findTrooper.SkinID);
            if (findSkinList.Count() == 0)
            {
                GameManager.Instance.log.LogMessage("CREATE TROOP FAILED: Invalid skin id!", LogMessage.LogType.Warning);
                return;
            }
            var findSkin = findSkinList.First();

            Character character = new Character(
                world, agents.Count, teamId,
                findTrooper.MeshName,
                position, findSkin, isBot);
            if (!isBot)
            {
                if (playerAgent != null)
                {
                    GameManager.Instance.log.LogMessage("TRY TO ASSIGN TROOPER AS PLAYER FAILED: There is already a trooper assigned!", LogMessage.LogType.Warning);
                    return;
                }
                playerAgent = character;
            }

            gameObjects.Add(character);
        }

        public void CreateSceneProp(string meshName, Mogre.Vector3 position)
        {
            SceneProp sceneProp = new SceneProp(gameObjects.Count, world, meshName, position);
            gameObjects.Add(sceneProp);
        }

        public void CreatePlane(string materialName, Mogre.Vector3 rkNormals, float consts, int width, int height, int xsegements, int ysegements, ushort numTexCoords, int uTile, int vTile, Mogre.Vector3 upVector, Mogre.Vector3 initPosition)
        {
            ScenePlane plane = new ScenePlane(gameObjects.Count, world, rkNormals, consts, materialName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                width, height, xsegements, ysegements, true, numTexCoords, uTile, vTile, upVector, initPosition);
            gameObjects.Add(plane);
        }

        private bool Keyboard_KeyPressed(KeyEvent arg)
        {
            switch (arg.key)
            {
                case KeyCode.KC_LCONTROL:
                    if (combineKey && combineKeyCode == KeyCode.KC_LCONTROL)
                    {
                        break;
                    }
                    combineKey = true;
                    combineKeyCode = arg.key;
                    break;
                case KeyCode.KC_E:
                    if (!combineKey)
                    {
                        //Press `E` normally
                        cameraHanlder.InjectKeyPressed(arg);
                    }
                    else if (!GameManager.Instance.IS_ENABLE_EDIT_MODE)
                    {
                        break;//Press `Ctrl+E` but EditMode is disabled
                    }
                    else
                    {
                        //EditMode
                        ScreenManager.Instance.ChangeScreen("InnerGameEditor", editor);
                    }
                    break;
                case KeyCode.KC_SPACE:
                    if (!combineKey && combineKeyCode != KeyCode.KC_LCONTROL)
                    {
                        break;
                    }
                    else
                    {
                        GameManager.Instance.SetFullScreen();
                    }
                    break;
                case KeyCode.KC_I:
                    //Open Inventory Window
                    if (playerAgent == null)
                    {
                        break;
                    }
                    ScreenManager.Instance.ChangeScreen("Inventory", playerAgent.MeshName, new string[]{
                        playerAgent.GetIdleTopAnim(), playerAgent.GetIdleBaseAnim()
                    });
                    break;
                default:
                    if (playerAgent != null)
                    {
                        playerAgent.InjectKeyPressed(arg);
                    }
                    else
                    {
                        cameraHanlder.InjectKeyPressed(arg);
                    }
                    break;
            }
            return true;
        }

        private bool Keyboard_KeyReleased(KeyEvent arg)
        {
            combineKey = false;
            ScreenManager.Instance.InjectKeyReleased(arg);

            if (playerAgent != null)
            {
                playerAgent.InjectKeyUp(arg);
            }
            else
            {
                cameraHanlder.InjectKeyReleased(arg);
            }
            return true;
        }

        private bool Mouse_MouseMoved(MouseEvent arg)
        {
            if (ScreenManager.Instance.CheckEnterScreen(new Vector2(arg.state.X.abs, arg.state.Y.abs)))
            {
                ScreenManager.Instance.InjectMouseMove(arg);
            }
            else if (playerAgent == null)
            {
                cameraHanlder.InjectMouseMove(arg);
            }
            else
            {
                playerAgent.InjectMouseMove(arg);
            }
            return true;
        }

        private bool Mouse_MouseReleased(MouseEvent arg, MouseButtonID id)
        {
            if (ScreenManager.Instance.CheckHasScreen())
            {
                ScreenManager.Instance.InjectMouseReleased(arg, id);
            }
            else
            {
                cameraHanlder.InjectMouseReleased(arg, id);
            }
            return true;
        }

        private bool Mouse_MousePressed(MouseEvent arg, MouseButtonID id)
        {
            if (ScreenManager.Instance.CheckHasScreen())
            {
                ScreenManager.Instance.InjectMousePressed(arg, id);
            }
            else
            {
                cameraHanlder.InjectMousePressed(arg, id);
            }
            return true;
        }

        public void Destroy()
        {
            GameManager.Instance.mouse.MouseMoved -= Mouse_MouseMoved;
            GameManager.Instance.mouse.MousePressed -= Mouse_MousePressed;
            GameManager.Instance.mouse.MouseReleased -= Mouse_MouseReleased;
            GameManager.Instance.keyboard.KeyPressed -= Keyboard_KeyPressed;
            GameManager.Instance.keyboard.KeyReleased -= Keyboard_KeyReleased;
        }

        public void LoadAsync()
        {
            mapLoader = new DotSceneLoader.DotSceneLoader();
            mapLoader.LoadSceneStarted += mapLoader_LoadMapStarted;
            mapLoader.LoadSceneFinished += mapLoader_LoadMapFinished;
            mapLoader.ParseDotSceneAsync(mapName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, scm);
        }

        private void mapLoader_LoadMapFinished()
        {
            if(LoadMapFinished!=null)
            {
                agents = new List<Character>();
                gameObjects = new List<GameObject>();
                scriptLoader.Parse(System.IO.Path.GetFileNameWithoutExtension(mapName) + ".script", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                scriptLoader.Execute(world);

                aimesh = mapLoader.AIMesh;
                editor.Initization(aimesh);

                LoadMapFinished();
            }
        }

        private void mapLoader_LoadMapStarted()
        {
            if (LoadMapStarted != null)
            {
                LoadMapStarted();
            }
        }

        public void Update(float timeSinceLastFrame)
        {
            updateGameObjects(timeSinceLastFrame);
            updateMapCamera(timeSinceLastFrame);
            updatePhysics(timeSinceLastFrame);
        }
        private void updateGameObjects(double timeSinceLastFrame)
        {
            if (gameObjects == null)
            {
                return;
            }
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update((float)timeSinceLastFrame);
            }
        }

        private void updateMapCamera(float timeSinceLastFrame)
        {
            if (playerAgent == null)
            {
                cameraHanlder.Update(timeSinceLastFrame);
            }
            else
            {
                playerAgent.Update(timeSinceLastFrame);
            }
        }

        private void updatePhysics(float timeSinceLastFrame)
        {
            while (!PhysicsScene.FetchResults(SimulationStatuses.AllFinished, false))
            {
 
            }

            PhysicsScene.Simulate(timeSinceLastFrame);
            PhysicsScene.FlushStream();
        }

        public string GetName()
        {
            return mapName;
        }

        public Character GetAgentById(int agentId)
        {
            return agents.ElementAt(agentId);
        }

        public List<Character> GetAgents()
        {
            return agents;
        }

        public List<GameObject> GetStaticObjects()
        {
            return gameObjects;
        }
    }
}
