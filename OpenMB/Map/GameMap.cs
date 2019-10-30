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
using OpenMB.Game.ControlObjType;
using System.IO;

namespace OpenMB.Map
{
    public delegate void MapLoadhandler();

    /// <summary>
    /// Define a map in the game
    /// </summary>
    public class GameMap : IGameMap
    {
        private string mapName;
        private IGameMapLoader loader;
        private Dictionary<string, List<GameObject>> gameObjects;
        private List<ActorNode> actorNodeList;
        private ScriptLoader scriptLoader;
        private SceneManager sceneManager;
        //private TerrainGroup terrianGroup;
        private Scene physicsScene;
        //private NavmeshQuery query;
        private ModData modData;
        private Physics physics;
        private ControllerManager controllerMgr;
        private Player player;
        private Character playerAgent;
        private Camera camera;
        private CameraHandler cameraHanlder;
        private GameWorld world;
        private AIMesh aimesh;
        private List<Mogre.Vector3> aimeshVertexData;
        private List<Mogre.Vector3> aimeshIndexData;
        private GameMapEditor editor;
        private bool combineKey;
        private KeyCode combineKeyCode;

        public string Name
        {
            get
            {
                return mapName;
            }
        }
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

        public Player Player
        {
            get
            {
                return player;
            }
        }

        public AIMesh AIMesh
        {
            get
            {
                return aimesh;
            }
        }

        public List<Character> Agents
        {
            get
            {
                List<Character> agents = new List<Character>();
                if(gameObjects.ContainsKey("AGENTS"))
                {
                    foreach (var gameObject in gameObjects["AGENTS"])
                    {
                        agents.Add(gameObject as Character);
                    }
                }
                return agents;
            }
        }

        public event MapLoadhandler LoadMapStarted;
        public event MapLoadhandler LoadMapFinished;
        public GameMap(GameWorld world, IGameMapLoader loader)
        {
            scriptLoader = new ScriptLoader();
            actorNodeList = new List<ActorNode>();
            this.world = world;
            this.loader = loader;
            loader.LoadMapFinished += Loader_LoadMapFinished;

            sceneManager = world.SceneManager;
            modData = world.ModData;
            camera = world.Camera;
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

        private void Loader_LoadMapFinished()
        {
            aimesh = new AIMesh();
            gameObjects = new Dictionary<string, List<GameObject>>();

            var file = scriptLoader.Parse(Path.GetFileNameWithoutExtension(loader.LoadedMapName)+".script", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            scriptLoader.ExecuteFunction(file, "map_loaded", world);
            
            TriggerManager.Instance.Init(world, scriptLoader.currentContext);

            editor.Initization();
            
            LoadMapFinished?.Invoke();
        }

        public void LoadMap(string name)
        {
            mapName = name;
            loader.LoadAsync(sceneManager, mapName);
        }

        public void LoadWorldMap(string name, string file)
        {
            var mesh = Connector.MBOgre.Instance.LoadWorldMap(
                name, sceneManager,
                FileFormats.MBWorldMap.ParseXml(
                    GameMapManager.Instance.FindPath(file)
                )
            );
            if (sceneManager.HasEntity("CURRENT_WORLDMAP"))
            {
                sceneManager.DestroyEntity("CURRENT_WORLDMAP");
            }
            if (sceneManager.HasSceneNode("CURRENT_WORLDMAP_SCENENODE"))
            {
                sceneManager.DestroySceneNode("CURRENT_WORLDMAP_SCENENODE");
            }
            var worldmapEnt = sceneManager.CreateEntity("CURRENT_WORLDMAP", "WORLDMAP-" + name);
            sceneManager.RootSceneNode.CreateChildSceneNode("CURRENT_WORLDMAP_SCENENODE").AttachObject(worldmapEnt);
        }

        public Entity CreateEntityWithMaterial(string name, string entityMeshName, string materialName)
        {
            Entity ent = sceneManager.CreateEntity(name, entityMeshName);
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

        /// <summary>
        /// Create a character that controlled by AI
        /// </summary>
        /// <param name="characterTypeID"></param>
        /// <param name="position"></param>
        /// <param name="teamId"></param>
        /// <param name="isBot"></param>
        public void CreateCharacter(string characterTypeID, Mogre.Vector3 position, string teamId, bool isBot = true)
        {
            var findTrooperList = ModData.CharacterInfos.Where(o => o.ID == characterTypeID);
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

            int characterInstanceID = -1;
            if (gameObjects.ContainsKey("AGENTS"))
            {
                characterInstanceID = gameObjects["AGENTS"].Count;
            }
            else
            {
                characterInstanceID = 0;
                gameObjects.Add("AGENTS", new List<GameObject>());
            }

            Character character = new Character(
                world, characterInstanceID, teamId,
                findTrooper.Name,
                findTrooper.MeshName,
                position, findSkin, true);
            gameObjects["AGENTS"].Add(character);
        }
        
        /// <summary>
        /// Create a character that player can control
        /// </summary>
        /// <param name="characterTypeID"></param>
        /// <param name="position"></param>
        /// <param name="teamId"></param>
        public void CreatePlayer(string characterTypeID, Mogre.Vector3 position, string teamId)
        {
            var findTrooperList = ModData.CharacterInfos.Where(o => o.ID == characterTypeID);
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
            
            int characterInstanceID = -1;
            if (gameObjects.ContainsKey("AGENTS"))
            {
                characterInstanceID = gameObjects["AGENTS"].Count;
            }
            else
            {
                characterInstanceID = 0;
                gameObjects.Add("AGENTS", new List<GameObject>());
            }

            Character character = new Character(
                world, characterInstanceID, teamId,
                findTrooper.Name,
                findTrooper.MeshName,
                position, findSkin, false);
            if (player != null)
            {
                GameManager.Instance.log.LogMessage("TRY TO ASSIGN TROOPER AS PLAYER FAILED: There is already a player assigned!", LogMessage.LogType.Warning);
                return;
            }
            player = new Player(findTrooper.Name, character.ID, new ControlObjectTypeCharacter(character));
            gameObjects[characterTypeID].Add(character);
        }

        /// <summary>
        /// Create a static scene prop
        /// </summary>
        /// <param name="scenePropID"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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
                    Item attachedItem = null;
                    if (MeshMaterialArray.Length == 3)
                    {
                        attachedItem = MeshMaterialArray[2] as Item;
                    }
                    SceneProp sceneProp = new SceneProp(
                        gameObjects.Count,
                        world,
                        findSceneProp.Name,
                        MeshMaterialArray[0].ToString(), 
                        MeshMaterialArray[1].ToString(), 
                        position,
                        null
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

        /// <summary>
        /// Create a scene prop that player can control
        /// </summary>
        /// <param name="scenePropID"></param>
        /// <param name="position"></param>
        public void CreatePlayerSceneProp(string scenePropID, Mogre.Vector3 position)
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
                    Item attachedItem = null;
                    if (MeshMaterialArray.Length == 3)
                    {
                        attachedItem = MeshMaterialArray[2] as Item;
                    }
                    SceneProp sceneProp = new SceneProp(
                        gameObjects.Count,
                        world,
                        findSceneProp.Name,
                        MeshMaterialArray[0].ToString(),
                        MeshMaterialArray[1].ToString(),
                        position,
                        attachedItem
                    );
                    if (!gameObjects.ContainsKey(scenePropID))
                    {
                        gameObjects.Add(scenePropID, new List<GameObject>());
                    }
                    
                    string guidStr = Guid.NewGuid().ToString();
                    sceneProp.SetID(guidStr);
                    player = new Player(findSceneProp.Name, guidStr, new ControlObjectTypeSceneProp(sceneProp));
                }
            }
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
            Agents.Remove((Character)owner);
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
                        ScreenManager.Instance.ChangeScreen("InnerGameEditor", false, editor);
                    }
                    break;
                case KeyCode.KC_I:
                    //Open Inventory Window
                    if (playerAgent == null)
                    {
                        break;
                    }
                    ScreenManager.Instance.ChangeScreen("Inventory", false, playerAgent.MeshName, new string[]{
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
            if (player != null)
            {
                player.Update(timeSinceLastFrame);
            }
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

        public GameObject GetObjectById(string objectTypeID, int objectId)
        {
            if (gameObjects.Count == 0)
            {
                return null;
            }
            if (objectId < 0 || objectId > gameObjects.Count - 1)
            {
                return null;
            }
            return gameObjects[objectTypeID].ElementAt(objectId);
        }

        public Character GetAgentById(int agentId)
        {
            if (Agents.Count == 0)
            {
                return null;
            }
            return Agents.Where(o => o.Id == agentId).FirstOrDefault();
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
