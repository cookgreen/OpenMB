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
using AMOFGameEngine.Mods.XML;
using AMOFGameEngine.Script;
using AMOFGameEngine.Screen;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Utilities;
using AMOFGameEngine.Trigger;
using AMOFGameEngine.Map;

namespace AMOFGameEngine.Game
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

        public Character GetAgentById(int id)
        {
            return GetCurrentMap().GetAgents().ElementAt(id);
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

            TriggerManager.Instance.Triggers.Add(new GameTrigger(this));

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

        public void ChangeScene(string sceneName)
        {
            GameMapManager.Instance.Load(sceneName);
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
            TriggerManager.Instance.Update((float)timeSinceLastFrame);
        }

        #endregion

        #region API
        public GameMap GetCurrentMap()
        {
            return GameMapManager.Instance.GetCurrentMap();
        }

        public string GetCurrentScene()
        {
            return GameMapManager.Instance.GetCurrentMapName();
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

        public void CreateSceneProp(string meshName, Mogre.Vector3 position)
        {
            GameMapManager.Instance.GetCurrentMap().CreateSceneProp(meshName, position);
        }

        public void CreatePlane(string materialName, Mogre.Vector3 vector31, float v1, int v2, int v3, int v4, int v5, ushort v6, int v7, int v8, Mogre.Vector3 vector32, Mogre.Vector3 vector33)
        {
            GameMapManager.Instance.GetCurrentMap().CreatePlane(materialName, vector31, v1, v2, v3, v4, v5, v6, v7, v8, vector32, vector33);
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
