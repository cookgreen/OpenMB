using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;
using Mogre_Procedural.MogreBites;
using Mogre_Procedural.MogreBites.Addons;
using MOIS;
using org.critterai.nav;
using OpenMB.Mods.XML;
using OpenMB.Script;
using OpenMB.Screen;
using OpenMB.Mods;
using OpenMB.Utilities;
using OpenMB.Trigger;
using OpenMB.Map;

namespace OpenMB.Game
{
    /// <summary>
    /// Core Component
    /// </summary>
    public class GameWorld
    {
        #region Fields
        //MOD Data
        private ModData modData;
        
        private List<Tuple<string, string, int>> teamRelationship;

        //For Render
        private SceneManager scm;
        private Camera cam;

        //Data
        private Dictionary<string, string> globalVarMap;
        private ScriptLinkTable globalValueTable;

        private ProgressBar pbProgressBar;
        
        private Physics physics;
        private Scene physicsScene;
        #endregion

        #region Properties
        public Camera Camera
        {
            get
            {
                return cam;
            }
        }

        public ScriptLinkTable GlobalValueTable
        {
            get
            {
                return globalValueTable;
            }
        }

        public SceneManager SceneManager
        {
            get
            {
                return scm;
            }
        }

        public GameObject GetObjectById(string objectID, int id)
        {
            return Map.GetObjectById(objectID, id);
        }

        public Character GetAgentById(int id)
        {
            return Map.GetAgentById(id);
        }

        public Item GetItemByXml(ModItemDfnXML itemXml)
        {
            return ItemFactory.Instance.Produce(itemXml, this);
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
        #endregion

        #region Constructor
        public GameWorld(ModData modData)
        {
            this.modData = modData;

            physics = Physics.Create();
            SceneDesc physicsSceneDesc = new SceneDesc();
            physicsSceneDesc.Gravity = new Mogre.Vector3(0, -9.8f, 0);
            physicsSceneDesc.UpAxis = 1;
            physicsScene = physics.CreateScene(physicsSceneDesc);
            physicsScene.Materials[0].Restitution = 0.5f;
            physicsScene.Materials[0].StaticFriction = 0.5f;
            physicsScene.Materials[0].DynamicFriction = 0.5f;
            physicsScene.Simulate(0);

            teamRelationship = new List<Tuple<string, string, int>>();
            globalVarMap = new Dictionary<string, string>();
            globalVarMap.Add("reg0", "0");
            globalVarMap.Add("reg1", "0");
            globalVarMap.Add("reg2", "0");
            globalVarMap.Add("reg3", "0");
            globalVarMap.Add("reg4", "0");
            globalValueTable = ScriptValueRegister.Instance.GlobalValueTable;

            /*Physx Debugger*/
            if (physics.RemoteDebugger.IsConnected)
            {
                physics.RemoteDebugger.Connect("127.0.0.1", 5425);
            }
            else
            {
                physics.RemoteDebugger.Disconnect();
                physics.RemoteDebugger.Connect("127.0.0.1", 5425);
            }
        }
        #endregion

        public void Start()
        {
            ScriptPreprocessor.Instance.LoadSpecificFunction("GameStart", this);
        }

        #region Core Methods
        public void Init()
        {

            GameMapManager.Instance.Initization(this);

            scm = GameManager.Instance.root.CreateSceneManager(SceneType.ST_EXTERIOR_CLOSE, "GameSceneManager");
            scm.AmbientLight = new ColourValue(0.7f, 0.7f, 0.7f);

            cam = scm.CreateCamera("gameCam");
            cam.AspectRatio = GameManager.Instance.viewport.ActualWidth / GameManager.Instance.viewport.ActualHeight;
            cam.NearClipDistance = 5;

            GameManager.Instance.viewport.Camera = cam;

            GameManager.Instance.trayMgr.destroyAllWidgets();
            cam.FarClipDistance = 50000;

            scm.SetSkyDome(true, "Examples/CloudySky", 5, 8);
            
            Light light = scm.CreateLight();
            light.Type = Light.LightTypes.LT_POINT;
            light.Position = new Mogre.Vector3(-10, 40, 20);
            light.SpecularColour = ColourValue.White;

            GameManager.Instance.trayMgr.hideCursor();

            GameManager.Instance.mouse.MouseMoved += mMouse_MouseMoved;
            GameManager.Instance.mouse.MousePressed += mMouse_MousePressed;
            GameManager.Instance.mouse.MouseReleased += mMouse_MouseReleased;
            GameManager.Instance.keyboard.KeyPressed += mKeyboard_KeyPressed;
            GameManager.Instance.keyboard.KeyReleased += mKeyboard_KeyReleased;

            GameManager.Instance.root.FrameRenderingQueued += FrameRenderingQueued;

            ScreenManager.Instance.Camera = cam;
        }

        public void ChangeScene(string mapID)
        {
            var findMaps = modData.MapInfos.Where(o => o.ID == mapID);
            if (findMaps.Count() > 0)
            {
                var findMap = findMaps.ElementAt(0);
                GameMapManager.Instance.Load(findMap.File);
            }
        }

        public void ChangeWorldMap(string worldMapID)
        {
            var findWorldMaps = modData.WorldMapInfos.Where(o => o.ID == worldMapID);
            if (findWorldMaps.Count() > 0)
            {
                var findWorldMap = findWorldMaps.ElementAt(0);
                var findMaps = modData.MapInfos.Where(o => o.ID == findWorldMap.Map);
                if (findMaps.Count() > 0)
                {
                    var findMap = findMaps.ElementAt(0);


                    GameMapManager.Instance.LoadWorldMap(worldMapID, findMap.File);
                }
                else
                {
                    GameManager.Instance.log.LogMessage(string.Format("Couldn't find map with ID `{0}`", findWorldMap.Map), LogMessage.LogType.Error);
                }
            }
            else
            {
                GameManager.Instance.log.LogMessage(string.Format("Couldn't find world map with ID `{0}`", worldMapID), LogMessage.LogType.Error);
            }
        }

        public void Destroy()
        {
            GameMapManager.Instance.Dispose();

            cam.Dispose();
            scm.Dispose();
            physicsScene.Dispose();
            physics.Dispose();

            GameManager.Instance.mouse.MouseMoved -= mMouse_MouseMoved;
            GameManager.Instance.mouse.MousePressed -= mMouse_MousePressed;
            GameManager.Instance.mouse.MouseReleased -= mMouse_MouseReleased;
            GameManager.Instance.keyboard.KeyPressed -= mKeyboard_KeyPressed;
            GameManager.Instance.keyboard.KeyReleased -= mKeyboard_KeyReleased;
            GameManager.Instance.root.FrameRenderingQueued -= FrameRenderingQueued;
        }

        public void Update(double timeSinceLastFrame)
        {
            
        }

        #endregion

        #region API
        public GameMap Map
        {
            get
            {
                return GameMapManager.Instance.GetCurrentMap();
            }
        }

        public string MapName
        {
            get
            {
                return Map.GetName();
            }
        }
        #endregion

        #region Other Methods

        internal List<Character> GetAllCharacters()
        {
            return GameMapManager.Instance.GetCurrentMap().GetAgents();
        }
        internal List<Character> GetCharactersByCondition(Func<Character, bool> condition)
        {
            return GameMapManager.Instance.GetCurrentMap().GetAgents().Where(condition).ToList();
        }

        internal List<Tuple<string,string, int>> GetTeamRelationshipByCondition(Func<Tuple<string, string, int>, bool> func)
        {
            return teamRelationship.Where(func).ToList();
        }


        private void SceneLoader_LoadSceneFinished()
        {
            pbProgressBar.setComment("Finished");
            GameManager.Instance.trayMgr.destroyAllWidgets();
        }

        private void SceneLoader_LoadSceneStarted()
        {
            CreateLoadingScreen("Loading Scene...");
        }

        private void CreateLoadingScreen(string text)
        {
            GameManager.Instance.trayMgr.destroyAllWidgets();
            pbProgressBar = GameManager.Instance.trayMgr.createProgressBar(TrayLocation.TL_CENTER, "pbProcessBar", "Loading", 500, 300);
            pbProgressBar.setComment(text);
        }

        public void RemoveGameObject(string objectID, GameObject owner)
        {
            Map.RemoveGameObject(objectID, owner);
        }

        public void RemoveAgent(GameObject owner)
        {
            Map.RemoveAgent(owner);
        }

        private bool FrameRenderingQueued(FrameEvent evt)
        {
            GameMapManager.Instance.Update(evt.timeSinceLastFrame);
            return true;
        }
        #endregion

        #region Handle Script
        public void CreateLight(string type, string name, Mogre.Vector3 pos, Mogre.Vector3 dir)
        {
            Light.LightTypes lt;
            switch (type)
            {
                case "point":
                    lt = Light.LightTypes.LT_POINT;
                    break;
                case "direction":
                    lt = Light.LightTypes.LT_DIRECTIONAL;
                    break;
                case "spot_light":
                    lt = Light.LightTypes.LT_SPOTLIGHT;
                    break;
                default:
                    lt = Light.LightTypes.LT_POINT;
                    break;
            }
            Light light = scm.CreateLight(name);
            light.Type = lt;
            light.Position = pos;
            light.Direction = dir;
        }

        public void RemoveLight(string name)
        {
            scm.DestroyLight(name);
        }

        public void ChangeTeamRelationship(string team1Id, string team2Id, int relationship)
        {
            var ret = teamRelationship.Where(o =>
            (o.Item1 == team1Id && o.Item2 == team2Id) ||
            (o.Item1 == team2Id && o.Item2 == team1Id));
            if (ret.Count() == 0)
            {
                teamRelationship.Add(new Tuple<string, string, int>(team1Id, team2Id, relationship));
            }
            else
            {
                Tuple<string, string, int> newTeamRelationship = new Tuple<string, string, int>(team1Id, team2Id, relationship);
                int index = teamRelationship.IndexOf(ret.First());
                teamRelationship.RemoveAt(index);
                teamRelationship.Insert(index, newTeamRelationship);
            }
        }

        public void ChangeGobalValue(string varname, string varvalue)
        {
            if (globalVarMap.ContainsKey(varname))
            {
                globalVarMap[varname] = varvalue;
            }
            else
            {
                globalVarMap.Add(varname, varvalue);
            }
        }

        public string GetGlobalValue(string varname)
        {
            if (globalVarMap.ContainsKey(varname))
            {
                return globalVarMap[varname];
            }
            else
            {
                return null;
            }
        }

        public void CreateCharacter(string characterID, Mogre.Vector3 position, string teamId, bool isBot = true)
        {
            GameMapManager.Instance.GetCurrentMap().CreateCharacter(characterID, position, teamId, isBot);
        }

        public string CreateSceneProp(string scenePropID, Mogre.Vector3 position)
        {
            return GameMapManager.Instance.GetCurrentMap().CreateSceneProp(scenePropID, position);
        }

        public int GetScenePropNum(string scenePropID)
        {
            return GameMapManager.Instance.GetCurrentMap().GetScenePropNum(scenePropID);
        }

        public string GetSceneProp(string scenePropID, string scenePropInstanceNum)
        {
            return GameMapManager.Instance.GetCurrentMap().GetScenePropInstanceID(scenePropID, int.Parse(scenePropInstanceNum));
        }

        public void RemoveSceneProp(string propInstanceID)
        {
            GameMapManager.Instance.GetCurrentMap().RemoveSceneProp(propInstanceID);
        }

        public void MoveSceneProp(string propInstanceID, string axis, string movement)
        {
            GameMapManager.Instance.GetCurrentMap().MoveSceneProp(propInstanceID, int.Parse(axis), int.Parse(movement));
        }

        public void CreatePlane(string materialName, Mogre.Vector3 vector31, float v1, int v2, int v3, int v4, int v5, ushort v6, int v7, int v8, Mogre.Vector3 vector32, Mogre.Vector3 vector33)
        {
            GameMapManager.Instance.GetCurrentMap().CreatePlane(materialName, vector31, v1, v2, v3, v4, v5, v6, v7, v8, vector32, vector33);
        }

        public void ChangeCameraMode(string mode)
        {
            switch (mode)
            {
                case "0":
                    GameMapManager.Instance.GetCurrentMap().CameraHanlder.ChangeMode(CameraMode.Free);
                    break;
                case "1":
                    GameMapManager.Instance.GetCurrentMap().CameraHanlder.ChangeMode(CameraMode.Manual);
                    break;
            }
        }
        #endregion

        #region Handle Input
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

        #endregion
    }
}
