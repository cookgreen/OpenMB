using AMOFGameEngine.Game;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Script;
using AMOFGameEngine.Trigger;
using AMOFGameEngine.Utilities;
using Helper;
using Mogre;
using Mogre.PhysX;
using org.critterai.nav;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Map
{
    public delegate void MapLoadhandler();

    /// <summary>
    /// Game Logic
    /// </summary>
    public class GameMap : IMap
    {
        private string mapName;
        private List<ITrigger> mapTriggers;
        private DotSceneLoader mapLoader;
        private List<Character> agents;
        private List<GameObject> staticObjects;
        private List<ActorNode> actorNodeList;
        private ScriptLoader scriptLoader;
        private SceneManager scm;
        private TerrainGroup terrianGroup;
        private Scene physicsScene;
        private NavmeshQuery query;
        private ModData modData;
        private Physics physics;
        private Character playerAgent;
        private Camera cam;
        private GameWorld world;

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

        public event MapLoadhandler LoadMapStarted;
        public event MapLoadhandler LoadMapFinished;

        public GameMap(string name, SceneManager scm)
        {
            mapName = name;
        }

        public void Destroy()
        {

        }

        public void LoadAsync()
        {
            mapLoader = new DotSceneLoader();
            mapLoader.LoadSceneStarted += mapLoader_LoadMapStarted;
            mapLoader.LoadSceneFinished += mapLoader_LoadMapFinished;

            agents = new List<Character>();
            staticObjects = new List<GameObject>();
            mapLoader.ParseDotSceneAsync(mapName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, scm);
            scriptLoader.Parse(System.IO.Path.GetFileNameWithoutExtension(mapName) + ".script", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            scriptLoader.Execute(this);

            MeshManager.Singleton.CreatePlane("floor", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                new Plane(Mogre.Vector3.UNIT_Y, 0), 100, 100, 10, 10, true, 1, 10, 10, Mogre.Vector3.UNIT_Z);
            Entity floor = scm.CreateEntity("Floor", "floor");
            floor.SetMaterialName("Examples/Rockwall");
            floor.CastShadows = (false);
            scm.RootSceneNode.AttachObject(floor);
            ActorDesc actorDesc = new ActorDesc();
            actorDesc.Density = 4;
            actorDesc.Body = null;
            actorDesc.Shapes.Add(physics.CreateTriangleMesh(new
                StaticMeshData(floor.GetMesh())));
            Actor floorActor = physicsScene.CreateActor(actorDesc);
            actorNodeList.Add(new ActorNode(scm.RootSceneNode, floorActor));

            Navmesh floorNavMesh = MeshToNavmesh.LoadNavmesh(floor);
            org.critterai.Vector3 pointStart = new org.critterai.Vector3(0, 0, 0);
            org.critterai.Vector3 pointEnd = new org.critterai.Vector3(0, 0, 0);
            org.critterai.Vector3 extents = new org.critterai.Vector3(2, 2, 2);

            NavStatus status = NavmeshQuery.Create(floorNavMesh, 100, out query);
        }

        private void mapLoader_LoadMapFinished()
        {
            if(LoadMapFinished!=null)
            {
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
            updateAgents(timeSinceLastFrame);
        }
        private void updateAgents(double timeSinceLastFrame)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Update((float)timeSinceLastFrame);
            }
        }

        public string GetName()
        {
            return mapName;
        }

        public List<Character> GetAgents()
        {
            return agents;
        }

        public List<GameObject> GetStaticObjects()
        {
            return staticObjects;
        }

        public void SpawnNewCharacter(string characterID, Mogre.Vector3 position, string teamId, bool isBot = true)
        {
            var searchRet = ModData.CharacterInfos.Where(o => o.ID == characterID);
            if (searchRet.Count() > 0)
            {
                Character character = new Character(
                    world, cam, agents.Count, teamId, searchRet.First().Name + agents.Count, searchRet.First().MeshName, position, !isBot);
                if (!isBot)
                {
                    playerAgent = character;
                }
                character.OnCharacterUseWeaponAttack += Character_OnCharacterUseWeaponAttack;
                character.OnCharacterDie += Character_OnCharacterDie;
                agents.Add(character);
            }
        }

        private void Character_OnCharacterUseWeaponAttack(int arg1, int arg2, double arg3)
        {
            throw new NotImplementedException();
        }

        private void Character_OnCharacterDie(int obj)
        {
            throw new NotImplementedException();
        }
    }
}
