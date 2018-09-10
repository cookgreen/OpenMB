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
        }
        #endregion

        #region Core Methods
        public void Init()
        {

            GameMapManager.Instance.Initization(this);

            scm = GameManager.Instance.mRoot.CreateSceneManager(SceneType.ST_EXTERIOR_CLOSE, "GameSceneManager");
            scm.AmbientLight = new ColourValue(0.7f, 0.7f, 0.7f);

            cam = scm.CreateCamera("gameCam");
            cam.AspectRatio = GameManager.Instance.mViewport.ActualWidth / GameManager.Instance.mViewport.ActualHeight;
            cam.NearClipDistance = 5;

            GameManager.Instance.mViewport.Camera = cam;

            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            cam.FarClipDistance = 50000;

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

            GameManager.Instance.mMouse.MouseMoved -= mMouse_MouseMoved;
            GameManager.Instance.mMouse.MousePressed -= mMouse_MousePressed;
            GameManager.Instance.mMouse.MouseReleased -= mMouse_MouseReleased;
            GameManager.Instance.mKeyboard.KeyPressed -= mKeyboard_KeyPressed;
            GameManager.Instance.mKeyboard.KeyReleased -= mKeyboard_KeyReleased;
            GameManager.Instance.mRoot.FrameRenderingQueued -= FrameRenderingQueued;
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

        //private void Character_OnCharacterUseWeaponAttack(int attacker, int victim, int damage)
        //{
        //    Character charaAttacker = agents.Find(o => o.Id == attacker);
        //    Character charaVictim = agents.Find(o => o.Id == victim);
        //    if (charaAttacker != null && charaVictim!=null)
        //    {
        //        charaVictim.Hitpoint -= damage;
        //        if (charaVictim.Hitpoint < 0)
        //        {
        //            Output.OutputManager.Instance.DisplayMessage(string.Format(
        //                Localization.LocateSystem.Singleton.GetLocalizedString(
        //                    Localization.LocateFileType.GameQuickString, 
        //                    "qstr_{0}_was_killed_by_{1}"), charaVictim.Name, charaAttacker.Name));
        //        }
        //    }
        //}

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
            GameManager.Instance.mTrayMgr.destroyAllWidgets();
        }

        private void SceneLoader_LoadSceneStarted()
        {
            CreateLoadingScreen("Loading Scene...");
        }

        private void CreateLoadingScreen(string text)
        {
            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            pbProgressBar = GameManager.Instance.mTrayMgr.createProgressBar(TrayLocation.TL_CENTER, "pbProcessBar", "Loading", 500, 300);
            pbProgressBar.setComment(text);
        }

        private bool FrameRenderingQueued(FrameEvent evt)
        {
            //GameMapManager.Instance.Update(evt.timeSinceLastFrame);
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

        public void SpawnNewCharacter(string characterID, Mogre.Vector3 position, string teamId, bool isBot = true)
        {
            GameMapManager.Instance.GetCurrentMap().SpawnNewCharacter(characterID, position, teamId, isBot);
        }
        #endregion

        #region Handle Input
        bool mKeyboard_KeyReleased(MOIS.KeyEvent arg)
        {
            return true;
        }
        bool mKeyboard_KeyPressed(MOIS.KeyEvent arg)
        {
            switch(arg.key)
            {
                case KeyCode.KC_I:
                    //Open Inventory Window
                    ScreenManager.Instance.ChangeScreen("Inventory");
                    break;
            }
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
