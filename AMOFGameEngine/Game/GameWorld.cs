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
using Mogre_Procedural.MogreBites;
using Mogre.PhysX;
using org.critterai.nav;
using Mogre_Procedural.MogreBites.Addons;

namespace AMOFGameEngine.Game
{
    /// <summary>
    /// Generate By a map, including characters, terrain etc
    /// </summary>
    public class GameWorld
    {
        //Current agent under control
        private Character playerAgent;

        //MOD Data
        private ModData modData;

        //Agents Data
        private List<Character> agents;
        private List<Tuple<string, string, int>> teamRelationship;

        //Terrain Data
        private TerrainGroup terrianGroup;

        //Static Objects Data
        private List<GameObject> staticObjects;

        //For Render
        private SceneManager scm;
        private Camera cam;
        public SdkCameraMan camMan;
        private Mogre.Vector3 m_TranslateVector;

        //Map Loader and its script
        private DotSceneLoader sceneLoader;
        private ScriptLoader scriptLoader;
        
        //Map file name
        private string sceneName;

        //Data
        private Dictionary<string, string> globalVarMap;
        private ScriptLinkTable globalValueTable;

        private ProgressBar pbProgressBar;

        private NavmeshQuery query;
        private Physics physics;
        private Scene physicsScene;

        public Camera Cam
        {
            get
            {
                return cam;
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
                return query;
            }
        }

        public ScriptLinkTable GlobalValueTable
        {
            get
            {
                return globalValueTable;
            }
        }

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

            sceneLoader = new DotSceneLoader();
            sceneLoader.LoadSceneStarted += SceneLoader_LoadSceneStarted;
            sceneLoader.LoadSceneFinished += SceneLoader_LoadSceneFinished;
            scriptLoader = new ScriptLoader();
            playerAgent = null;
            teamRelationship = new List<Tuple<string, string, int>>();
            globalVarMap = new Dictionary<string, string>();
            globalVarMap.Add("reg0", "0");
            globalVarMap.Add("reg1", "0");
            globalVarMap.Add("reg2", "0");
            globalVarMap.Add("reg3", "0");
            globalVarMap.Add("reg4", "0");
            globalValueTable = ScriptValueRegister.Instance.GlobalValueTable;
        }

        public void CreateLight(string type, string name, Mogre.Vector3 pos, Mogre.Vector3 dir)
        {
            Light.LightTypes lt;
            switch(type)
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

        private void SceneLoader_LoadSceneFinished()
        {
            terrianGroup = sceneLoader.TerrainGroup;
            pbProgressBar.setComment("Finished");
            GameManager.Instance.mTrayMgr.destroyAllWidgets();
        }

        private void SceneLoader_LoadSceneStarted()
        {
            CreateLoadingScreen("Loading Scene...");
        }

        public void Init()
        {
            scm = GameManager.Instance.mRoot.CreateSceneManager(SceneType.ST_EXTERIOR_CLOSE);
            scm.AmbientLight = new ColourValue(0.7f, 0.7f, 0.7f);

            cam = scm.CreateCamera("gameCam");
            cam.AspectRatio = GameManager.Instance.mViewport.ActualWidth / GameManager.Instance.mViewport.ActualHeight;
            cam.NearClipDistance = 5;
            
            GameManager.Instance.mViewport.Camera = cam;

            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            //cam.FarClipDistance = 50000;

            scm.SetSkyDome(true, "Examples/CloudySky", 5, 8);

            Light light = scm.CreateLight();
            light.Type = Light.LightTypes.LT_POINT;
            light.Position = new Mogre.Vector3(-10, 40, 20);
            light.SpecularColour = ColourValue.White;
            
            GameManager.Instance.mTrayMgr.hideCursor();
            
            GameManager.Instance.mMouse.MouseMoved += mMouse_MouseMoved;
            GameManager.Instance.mMouse.MousePressed += mMouse_MousePressed;
            GameManager.Instance.mMouse.MouseReleased += mMouse_MouseReleased;
            GameManager.Instance.mKeyboard.KeyPressed += mKeyboard_KeyPressed;
            GameManager.Instance.mKeyboard.KeyReleased += mKeyboard_KeyReleased;

            GameManager.Instance.mRoot.FrameRenderingQueued += FrameRenderingQueued;
        }

        private bool FrameRenderingQueued(FrameEvent evt)
        {
            updateAgents(evt.timeSinceLastFrame);
            return true;
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

        public void SpawnNewCharacter(string characterID, Mogre.Vector3 position,string teamId, bool isBot = true)
        {
            var searchRet = ModData.CharacterInfos.Where(o => o.ID == characterID);
            if (searchRet.Count() > 0)
            {
                Character character = new Character(this, cam, agents.Count, teamId, searchRet.First().Name + agents.Count, searchRet.First().MeshName, position, !isBot);
                if (!isBot)
                {
                    playerAgent = character;
                }
                agents.Add(character);
            }
        }

        public void Destroy()
        {
            agents.Clear();
            staticObjects.Clear();
            cam.Dispose();
            scm.Dispose();
            physicsScene.Dispose();
            physics.Dispose();

            GameManager.Instance.mMouse.MouseMoved -= mMouse_MouseMoved;
            GameManager.Instance.mMouse.MousePressed -= mMouse_MousePressed;
            GameManager.Instance.mMouse.MouseReleased -= mMouse_MouseReleased;
            GameManager.Instance.mKeyboard.KeyPressed -= mKeyboard_KeyPressed;
            GameManager.Instance.mKeyboard.KeyReleased -= mKeyboard_KeyReleased;
        }

        public void ChangeScene(string sceneName)
        {
            this.sceneName = sceneName;
            agents = new List<Character>();
            staticObjects = new List<GameObject>();
            sceneLoader.ParseDotSceneAsync(sceneName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, scm);
            scriptLoader.Parse(System.IO.Path.GetFileNameWithoutExtension(sceneName) + ".script", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, this);
        }

        public string GetCurrentScene()
        {
            return sceneName;
        }

        public int GetCurrentPlayerAgentId()
        {
            if (playerAgent != null)
            {
                //there is an agent under player's control
                return playerAgent.Id;
            }
            else
            {
                //No agent under player's control
                return -1;
            }
        }

        public List<Character> GetCharactersByCondition(Func<Character, bool> condition)
        {
            return agents.Where(condition).ToList();
        }

        public List<Tuple<string,string, int>> GetTeamRelationshipByCondition(Func<Tuple<string, string, int>, bool> func)
        {
            return teamRelationship.Where(func).ToList();
        }

        public void Update(double timeSinceLastFrame)
        {
            m_TranslateVector = new Mogre.Vector3(0, 0, 0);
            if (GetCurrentPlayerAgentId() == -1)
            {
                getInput();
                moveCamera();
            }
            else
            {
            }
            physicsScene.FlushStream();
            physicsScene.FetchResults(SimulationStatuses.AllFinished, true);
            physicsScene.Simulate(timeSinceLastFrame);
        }
        private void updateAgents(double timeSinceLastFrame)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Update((float)timeSinceLastFrame);
            }
        }
        private void getInput()
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
        }
        private void moveCamera()
        {
            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                cam.MoveRelative(m_TranslateVector);
            cam.MoveRelative(m_TranslateVector / 10);
        }
        
        bool mKeyboard_KeyReleased(MOIS.KeyEvent arg)
        {
            if (GetCurrentPlayerAgentId() != -1)
                playerAgent.Controller.injectKeyUp(arg);
            return true;
        }

        bool mKeyboard_KeyPressed(MOIS.KeyEvent arg)
        {
            if (GetCurrentPlayerAgentId() != -1)
                playerAgent.Controller.injectKeyDown(arg);
            return true;
        }

        bool mMouse_MouseReleased(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            return true;
        }

        bool mMouse_MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            if (GetCurrentPlayerAgentId() != -1)
                playerAgent.Controller.injectMouseDown(arg, id);
            return true;
        }

        bool mMouse_MouseMoved(MOIS.MouseEvent arg)
        {
            if (GetCurrentPlayerAgentId() != -1)
                playerAgent.Controller.injectMouseMove(arg);
            return true;
        }

        private void CreateLoadingScreen(string text)
        {
            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            pbProgressBar = GameManager.Instance.mTrayMgr.createProgressBar(TrayLocation.TL_CENTER, "pbProcessBar", "Loading", 500, 300);
            pbProgressBar.setComment(text);
        }
    }
}
