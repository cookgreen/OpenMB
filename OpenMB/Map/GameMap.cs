using OpenMB.Game;
using OpenMB.Mods;
using OpenMB.Screen;
using OpenMB.Script;
using OpenMB.Trigger;
using OpenMB.Utilities;
using DotSceneLoader;
using Mogre;
using Mogre.PhysX;
using MOIS;
using org.critterai.nav;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Map
{
    public delegate void MapLoadhandler();

    /// <summary>
    /// Define a map in the game
    /// </summary>
    public class GameMap : IMap
    {
        private string mapName;
        private DotSceneLoader.DotSceneLoader mapLoader;
        private List<Character> agents;

        private Dictionary<string, List<GameObject>> gameObjects;
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
        private CameraHandler cameraHanlder;
        private GameWorld world;
        private AIMesh aimesh;
        private List<Mogre.Vector3> aimeshVertexData;
        private List<Mogre.Vector3> aimeshIndexData;
        private GameMapEditor editor;
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

        public CameraHandler CameraHanlder
        {
            get
            {
                return cameraHanlder;
            }
        }

        public event MapLoadhandler LoadMapStarted;
        public event MapLoadhandler LoadMapFinished;

        public GameMap(string name, GameWorld world)
        {
            mapName = name;
            scriptLoader = new ScriptLoader();
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
            gameObjects = new Dictionary<string, List<GameObject>>();
            combineKey = false;

            GameManager.Instance.mouse.MouseMoved += Mouse_MouseMoved;
            GameManager.Instance.mouse.MousePressed += Mouse_MousePressed;
            GameManager.Instance.mouse.MouseReleased += Mouse_MouseReleased;
            GameManager.Instance.keyboard.KeyPressed += Keyboard_KeyPressed;
            GameManager.Instance.keyboard.KeyReleased += Keyboard_KeyReleased;
        }
        public GameMap(string name, string file, GameWorld world)
        {
            mapName = name;
            scriptLoader = new ScriptLoader();
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
            gameObjects = new Dictionary<string, List<GameObject>>();
            combineKey = false;

            var mesh = Connector.MBOgre.Instance.LoadWorldMap(
                name, scm,
                FileFormats.MBWorldMap.ParseXml(
                    GameMapManager.Instance.FindPath(file)
                )
            );
            if (scm.HasEntity("CURRENT_WORLDMAP"))
            {
                scm.DestroyEntity("CURRENT_WORLDMAP");
            }
            if (scm.HasSceneNode("CURRENT_WORLDMAP_SCENENODE"))
            {
                scm.DestroySceneNode("CURRENT_WORLDMAP_SCENENODE");
            }
            var worldmapEnt = scm.CreateEntity("CURRENT_WORLDMAP", "WORLDMAP-" + name);
            scm.RootSceneNode.CreateChildSceneNode("CURRENT_WORLDMAP_SCENENODE").AttachObject(worldmapEnt);

            GameManager.Instance.mouse.MouseMoved += Mouse_MouseMoved;
            GameManager.Instance.mouse.MousePressed += Mouse_MousePressed;
            GameManager.Instance.mouse.MouseReleased += Mouse_MouseReleased;
            GameManager.Instance.keyboard.KeyPressed += Keyboard_KeyPressed;
            GameManager.Instance.keyboard.KeyReleased += Keyboard_KeyReleased;
        }

        public Entity CreateEntityWithMaterial(string name, string entityMeshName, string materialName)
        {
            Entity ent = scm.CreateEntity(name, entityMeshName);
            ent.SetMaterialName(materialName);
            uint subEntNum = ent.NumSubEntities;
            for (uint i = 0; i < subEntNum; i++)
            {
                SubEntity subEnt = ent.GetSubEntity(i);
                subEnt.SetMaterialName(materialName);
            }
            return ent;
        }


        public Item CreateItem(
            string desc, 
            string meshName, 
            ItemType type, 
            ItemUseAttachOption attachOptionWhenUse, 
            ItemHaveAttachOption attachOptionWhenHave, 
            double damage, 
            int range, 
            GameWorld world, 
            int ammoCapcity, 
            double amourNum)
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
                findTrooper.Name,
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

            agents.Add(character);
        }

        public string CreateSceneProp(string scenePropID, Mogre.Vector3 position)
        {
            var findSceneProps = modData.SceneProps.Where(o => o.ID == scenePropID);
            if (findSceneProps.Count() > 0)
            {
                var findSceneProp = findSceneProps.ElementAt(0);
                var findModelTypes = modData.ModModelTypes.Where(o => o.Name == findSceneProp.ModelType);
                if (findModelTypes.Count() > 0)
                {
                    var findModelType = findModelTypes.ElementAt(0);
                    object[] MeshMaterialArray = findModelType.Process(modData, findSceneProp.Model) as object[];

                    SceneProp sceneProp = new SceneProp(
                        gameObjects.Count, 
                        world, 
                        MeshMaterialArray[0].ToString(), 
                        MeshMaterialArray[1].ToString(), 
                        position
                    );
                    if (!gameObjects.ContainsKey(scenePropID))
                    {
                        gameObjects.Add(scenePropID, new List<GameObject>());
                    }
                    gameObjects[scenePropID].Add(sceneProp);
                    string guidStr = Guid.NewGuid().ToString();
                    sceneProp.SetID(guidStr);
                    return guidStr;
                }
            }
            return null;
        }

        public SceneProp GetSceneProp(string propInstanceID)
        {
            foreach (var gameObj in gameObjects)
            {
                foreach (var gameObjInstance in gameObj.Value)
                {
                    if (gameObjInstance.ID == propInstanceID)
                    {
                        return gameObjInstance as SceneProp;
                    }
                }
            }
            return null;
        }

        public int GetScenePropNum(string scenePropID)
        {
            return gameObjects.ContainsKey(scenePropID) ? gameObjects[scenePropID].Count : -1;
        }

        public void RemoveSceneProp(string propInstanceID)
        {
            SceneProp prop = GetSceneProp(propInstanceID);
            if (prop != null)
            {
                prop.Dispose();
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    for (int j = gameObjects.ElementAt(i).Value.Count - 1; j >= 0; j--)
                    {
                        if (gameObjects.ElementAt(i).Value[j].ID == propInstanceID)
                        {
                            gameObjects.ElementAt(i).Value.RemoveAt(j);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Move the specific scene prop with movement
        /// </summary>
        /// <param name="propInstanceID">Scene Prop ID</param>
        /// <param name="axis">Axis 0 - X, 1 - Y, 2 - Z</param>
        /// <param name="movement">movement</param>
        public void MoveSceneProp(string propInstanceID, int axis, int movement)
        {
            Mogre.Vector3 mov = new Mogre.Vector3();
            switch (axis)
            {
                case 0://X Axis
                    mov.x = movement;
                    break;
                case 1://Y Axis
                    mov.y = movement;
                    break;
                case 2://Z Axis
                    mov.z = movement;
                    break;
            }
            SceneProp prop = GetSceneProp(propInstanceID);
            if (prop != null)
            {
                prop.Move(mov);
            }
        }

        public void CreatePlane(
            string materialName, 
            Mogre.Vector3 rkNormals, 
            float consts, 
            int width, 
            int height, 
            int xsegements, 
            int ysegements, 
            ushort numTexCoords, 
            int uTile, 
            int vTile, 
            Mogre.Vector3 upVector, 
            Mogre.Vector3 initPosition)
        {
            ScenePlane plane = new ScenePlane(gameObjects.Count, world, rkNormals, consts, materialName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                width, height, xsegements, ysegements, true, numTexCoords, uTile, vTile, upVector, initPosition);
            if (!gameObjects.ContainsKey("PLANE"))
            {
                gameObjects.Add("PLANE", new List<GameObject>());
            }
            gameObjects["PLANE"].Add(plane);
        }

        public string GetScenePropInstanceID(string scenePropID, int scenePropInstanceNum)
        {
            if (scenePropInstanceNum < 0)
            {
                return null;
            }
            if (gameObjects.ContainsKey(scenePropID))
            {
                if (gameObjects[scenePropID].Count > scenePropInstanceNum)
                {
                    return (gameObjects[scenePropID].ElementAt(scenePropInstanceNum) as SceneProp).ID;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void RemoveAgent(GameObject owner)
        {
            agents.Remove((Character)owner);
            owner.Dispose();
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
                        cameraHanlder.ChangeMode(CameraMode.Manual);//Manually control
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

        public void RemoveGameObject(string objectID, GameObject owner)
        {
            if (gameObjects.ContainsKey(objectID))
            {
                gameObjects[objectID].Remove(owner);
            }
            owner.Dispose();
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
                gameObjects = new Dictionary<string, List<GameObject>>();

                var file = scriptLoader.Parse(mapLoader.ScriptName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                scriptLoader.ExecuteFunction(file, "map_loaded", world);

                TriggerManager.Instance.Init(world, scriptLoader.currentContext);

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
            CheckCollide(timeSinceLastFrame);
            TriggerManager.Instance.Update(timeSinceLastFrame);
        }

        private void CheckCollide(float timeSinceLastFrame)
        {
            foreach(var gameObj in gameObjects)
            {
                foreach (var gameObjInstance in gameObj.Value)
                {
                    foreach (var gameObj2 in gameObjects)
                    {
                        foreach (var gameObjInstance2 in gameObj.Value)
                        {
                            if (gameObjInstance2.CheckCollide(gameObjInstance) && gameObjInstance.ID != gameObjInstance2.ID)
                            {
                                TriggerManager.Instance.ScenePropHit(gameObjInstance, gameObjInstance2);
                            }
                        }
                    }
                }
            }
        }

        private void updateGameObjects(double timeSinceLastFrame)
        {
            if (gameObjects == null)
            {
                return;
            }
            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                for (int j = gameObjects.ElementAt(i).Value.Count - 1; j >= 0; j--)
                {
                    gameObjects.ElementAt(i).Value[j].Update((float)timeSinceLastFrame);
                }
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

        public GameObject GetObjectById(string objectID, int objectId)
        {
            if (gameObjects.Count == 0)
            {
                return null;
            }
            if (objectId < 0 || objectId > gameObjects.Count - 1)
            {
                return null;
            }
            return gameObjects[objectID].ElementAt(objectId);
        }

        public Character GetAgentById(int agentId)
        {
            if (agents.Count == 0)
            {
                return null;
            }
            return (Character)agents.ElementAt(agentId);
        }

        public List<Character> GetAgents()
        {
            return agents;
        }

        public List<GameObject> GetGameObjects(string objectID)
        {
            if (gameObjects.ContainsKey(objectID))
            {
                return gameObjects[objectID];
            }
            else
            {
                return null;
            }
        }
    }
}
