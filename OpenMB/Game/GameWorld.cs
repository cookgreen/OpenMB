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
using OpenMB.Widgets;
using OpenMB.States;
using OpenMB.Render;

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
		private Dictionary<string, object> globalVariableTable;

		private ProgressBarWidget pbProgressBar;
        
        private Physics physics;
        private Scene physicsScene;
        #endregion

        #region Properties
        public GameMap CurrentMap
        {
            get
            {
                return GameMapManager.Instance.CurrentMap;
            }
        }
        public string CurrentMapName
        {
            get
            {
                return GameMapManager.Instance.CurrentMapName;
            }
        }
        public ScriptLinkTable GlobalValueTable
        {
            get
            {
                return globalValueTable;
            }
		}
		public Dictionary<string, object> GlobalVariableTable
		{
			get
			{
				return globalVariableTable;
			}
		}
		public ModData ModData
        {
            get
            {
                return modData;
            }
        }
        public Camera Camera
        {
            get
            {
                return cam;
            }
        }
        public SceneManager SceneManager
        {
            get
            {
                return scm;
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

        #region Core Methods
        /// <summary>
        /// Init the game world
        /// </summary>
        public void Init()
        {
            GameMapManager.Instance.Initization(this);

            GameManager.Instance.mouse.MouseMoved += Mouse_MouseMoved;
            GameManager.Instance.mouse.MousePressed += Mouse_MousePressed;
            GameManager.Instance.mouse.MouseReleased += Mouse_MouseReleased;
            GameManager.Instance.keyboard.KeyPressed += Keyboard_KeyPressed;
            GameManager.Instance.keyboard.KeyReleased += Keyboard_KeyReleased;

            GameManager.Instance.root.FrameRenderingQueued += FrameRenderingQueued;

            /* Will implement them in the script or the map xml file */
            scm = GameManager.Instance.root.CreateSceneManager(SceneType.ST_EXTERIOR_CLOSE, "GameSceneManager");
            scm.AmbientLight = new ColourValue(0.7f, 0.7f, 0.7f);

			OpenGLRenderManager.Initization(scm);

            cam = scm.CreateCamera("gameCam");
            cam.AspectRatio = GameManager.Instance.viewport.ActualWidth / GameManager.Instance.viewport.ActualHeight;
            cam.NearClipDistance = 5;

            GameManager.Instance.viewport.Camera = cam;

            UIManager.Instance.DestroyAllWidgets();
            cam.FarClipDistance = 50000;

			var time = TimerManager.Instance.CurrentTime;
			scm.SetSkyBox(true, GetSkyboxMaterialByTime(time));

			Light light = scm.CreateLight();
            light.Type = Light.LightTypes.LT_POINT;
            light.Position = new Mogre.Vector3(-10, 40, 20);
            light.SpecularColour = ColourValue.White;

            ScreenManager.Instance.Camera = cam;

			TimerManager.Instance.TimeChanged += TimeChanged;

			globalVariableTable = new Dictionary<string, object>();

		}

		private string GetSkyboxMaterialByTime(Time time)
		{
			string skyboxMaterialName = null;
			switch (time)
			{
				case Time.Early_Morning:
					skyboxMaterialName = "Examples/EarlyMorningSkyBox";
					break;
				case Time.Morning:
					skyboxMaterialName = "Examples/MorningSkyBox";
					break;
				case Time.Noon:
					skyboxMaterialName = "Examples/CloudyNoonSkyBox";
					break;
				case Time.Afternoon:
					skyboxMaterialName = "Examples/StormySkyBox";
					break;
				case Time.Night:
					skyboxMaterialName = "Examples/SpaceSkyBox";
					break;
			}
			return skyboxMaterialName;
		}

		private void TimeChanged()
		{
			var time = TimerManager.Instance.CurrentTime;
			scm.SetSkyBox(true, GetSkyboxMaterialByTime(time));
		}

		/// <summary>
		/// Start world
		/// </summary>
		public void Start()
        {
            ScriptPreprocessor.Instance.LoadSpecificFunction("GameStart", this);
        }

        /// <summary>
        /// Change inner scene
        /// </summary>
        /// <param name="mapID"></param>
        public void ChangeScene(string mapID)
        {
			TimerManager.Instance.Pause();

            UIManager.Instance.HideCursor();

            var findMaps = modData.MapInfos.Where(o => o.ID == mapID);
            if (findMaps.Count() > 0)
            {
                var findMap = findMaps.ElementAt(0);
                var findLoaders = modData.MapLoaders.Where(o => o.Name == findMap.Loader);
                if (findLoaders.Count() > 0)
                {
                    var loader = findLoaders.ElementAt(0);
                    GameMapManager.Instance.Load(findMap.File, loader);
                }
            }
        }

        /// <summary>
        /// Change world map
        /// </summary>
        /// <param name="worldMapID"></param>
        public void ChangeWorldMap(string worldMapID)
        {
            UIManager.Instance.ShowCursor();

            var findWorldMaps = modData.WorldMapInfos.Where(o => o.ID == worldMapID);
            if (findWorldMaps.Count() > 0)
            {
                var findWorldMap = findWorldMaps.ElementAt(0);
                var findMaps = modData.MapInfos.Where(o => o.ID == findWorldMap.Map);
                if (findMaps.Count() > 0)
                {
                    var findMap = findMaps.ElementAt(0);

                    var findLoaders = modData.MapLoaders.Where(o => o.Name == findMap.Loader);
                    if (findLoaders.Count() > 0)
                    {
                        var loader = findLoaders.ElementAt(0);
                        GameMapManager.Instance.LoadWorldMap(worldMapID, findMap.File, loader);
                    }
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

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Destroy()
        {
            GameMapManager.Instance.Dispose();

            cam.Dispose();
            scm.Dispose();
            physicsScene.Dispose();
            physics.Dispose();

            GameManager.Instance.mouse.MouseMoved -= Mouse_MouseMoved;
            GameManager.Instance.mouse.MousePressed -= Mouse_MousePressed;
            GameManager.Instance.mouse.MouseReleased -= Mouse_MouseReleased;
            GameManager.Instance.keyboard.KeyPressed -= Keyboard_KeyPressed;
            GameManager.Instance.keyboard.KeyReleased -= Keyboard_KeyReleased;
            GameManager.Instance.root.FrameRenderingQueued -= FrameRenderingQueued;

			TimerManager.Instance.Stop();

			OpenGLRenderManager.Shutdown();
        }

        #endregion

        #region Update Methods
        private bool FrameRenderingQueued(FrameEvent evt)
        {
            GameMapManager.Instance.Update(evt.timeSinceLastFrame);
			TimerManager.Instance.Update();
            return true;
        }
        #endregion

        #region Handle Input
        bool Keyboard_KeyReleased(MOIS.KeyEvent arg)
        {
			ScreenManager.Instance.InjectKeyReleased(arg);
            return true;
        }
        bool Keyboard_KeyPressed(MOIS.KeyEvent arg)
        {
			ScreenManager.Instance.InjectKeyPressed(arg);
            return true;
        }
        bool Mouse_MouseReleased(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
		{
			ScreenManager.Instance.InjectMouseReleased(arg, id);
			return true;
        }
        bool Mouse_MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
		{
			ScreenManager.Instance.InjectMousePressed(arg, id);
			return true;
        }
        bool Mouse_MouseMoved(MOIS.MouseEvent arg)
		{
			ScreenManager.Instance.InjectMouseMove(arg);
			return true;
        }

		#endregion

		#region API
		public Character GetAgentById(int id)
		{
			return CurrentMap.GetAgentById(id);
		}

		public Item GetItemByXml(ModItemDfnXML itemXml)
		{
			return ItemFactory.Instance.Produce(itemXml, this);
		}

		internal List<Character> GetAllCharacters()
		{
			return GameMapManager.Instance.CurrentMap.Agents;
		}
		internal List<Character> GetCharactersByCondition(Func<Character, bool> condition)
		{
			return GameMapManager.Instance.CurrentMap.Agents.Where(condition).ToList();
		}

		internal List<Tuple<string, string, int>> GetTeamRelationshipByCondition(Func<Tuple<string, string, int>, bool> func)
		{
			return teamRelationship.Where(func).ToList();
		}

		private void CreateLoadingScreen(string text)
		{
			UIManager.Instance.DestroyAllWidgets();
			pbProgressBar = UIManager.Instance.CreateProgressBar(UIWidgetLocation.TL_CENTER, "pbProcessBar", "Loading", 500, 300);
			pbProgressBar.setComment(text);
		}

		public void RemoveGameObject(string objectID, GameObject owner)
		{
			CurrentMap.RemoveGameObject(objectID, owner);
		}

		public void RemoveAgent(GameObject owner)
		{
			CurrentMap.RemoveAgent(owner);
		}

		public void CreatePlayer(string trooperID, Mogre.Vector3 position, string teamID)
		{
			GameMapManager.Instance.CurrentMap.CreatePlayer(trooperID, position, teamID);
		}

		public void CreatePlayerSceneProp(string scenePropID, Mogre.Vector3 position)
		{
			GameMapManager.Instance.CurrentMap.CreatePlayerSceneProp(scenePropID, position);
		}
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
			GameMapManager.Instance.CurrentMap.CreateCharacter(characterID, position, teamId, isBot);
		}

		public string CreateSceneProp(string scenePropID, Mogre.Vector3 position)
		{
			return GameMapManager.Instance.CurrentMap.CreateSceneProp(scenePropID, position);
		}

		public int GetScenePropNum(string scenePropID)
		{
			return GameMapManager.Instance.CurrentMap.GetScenePropNum(scenePropID);
		}

		public string GetSceneProp(string scenePropID, string scenePropInstanceNum)
		{
			return GameMapManager.Instance.CurrentMap.GetScenePropInstanceID(scenePropID, int.Parse(scenePropInstanceNum));
		}

		public void RemoveSceneProp(string scenePropID, int propInstanceID)
		{
			GameMapManager.Instance.CurrentMap.RemoveSceneProp(scenePropID, propInstanceID);
		}

		public void MoveSceneProp(string sceneTypeID, int propInstanceID, string axis, string movement)
		{
			GameMapManager.Instance.CurrentMap.MoveSceneProp(sceneTypeID, propInstanceID, int.Parse(axis), int.Parse(movement));
		}

		public void CreatePlane(string materialName, Mogre.Vector3 vector31, float v1, int v2, int v3, int v4, int v5, ushort v6, int v7, int v8, Mogre.Vector3 vector32, Mogre.Vector3 vector33)
		{
			GameMapManager.Instance.CurrentMap.CreatePlane(materialName, vector31, v1, v2, v3, v4, v5, v6, v7, v8, vector32, vector33);
		}

		public void ChangeCameraMode(string mode)
		{
			switch (mode)
			{
				case "0":
					GameMapManager.Instance.CurrentMap.CameraHanlder.ChangeMode(CameraMode.Free);
					break;
				case "1":
					GameMapManager.Instance.CurrentMap.CameraHanlder.ChangeMode(CameraMode.Manual);
					break;
			}
		}

		public void ChangeGameState(string newState)
		{
			AppStateManager.Instance.changeAppState(
				AppStateManager.Instance.findByName(newState),
				modData
			);
		}
		#endregion
	}
}
