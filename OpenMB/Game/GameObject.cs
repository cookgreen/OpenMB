using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;

namespace OpenMB.Game
{
    public class GameObject : IUpdate
    {
        protected string kindID;
        protected string strID;
        protected int id;
        protected MoveInfo moveInfo;
        protected Vector3 position;
        protected Camera camera;
        protected SceneManager sceneManager;
        protected SceneNode entNode;
        protected Entity entity;
        protected GameWorld world;
        protected Physics physics;
        protected Scene physicsScene;
        protected HealthInfo health;
        private Actor entityActor;
        public int ID
        {
            get
            {
                return id;
            }
			set
			{
				id = value;
			}
        }
        public MoveInfo MoveInfo
        {
            get
            {
                return moveInfo;
            }
        }
        public bool IsStatic
        {
            get
            {
                return moveInfo == null;
            }
        }
        public Vector3 Position
        {
            get
            {
                return position;
            }
        }
        public GameWorld World
        {
            get
            {
                return world;
            }
        }
        public SceneManager SceneManager
        {
            get
            {
                return sceneManager;
            }
        }
        public SceneNode MeshNode
        {
            get
            {
                return entNode;
            }
        }
        public Entity Mesh
        {
            get
            {
                return entity;
            }
        }
        public Actor PhysicsActor
        {
            get
            {
                return entityActor;
            }

            set
            {
                entityActor = value;
            }
        }
        public HealthInfo Health
        {
            get
            {
                return health;
            }
        }

        public GameObject(int id, GameWorld world)
        {
            this.id = id;
            this.world = world;
            camera = world.Camera;
            sceneManager = world.SceneManager;
            physicsScene = world.PhysicsScene;
            physics = world.PhysicsScene.Physics;
            moveInfo = null;
            health = new HealthInfo(this);
        }

        public GameObject(string id)
        {
            moveInfo = null;
            health = new HealthInfo(this);
        }

        public void SetID(string id)
        {
            strID = id;
        }

        protected virtual void create(){ }

        protected virtual void create(GameWorld world)
        {
            this.world = world;
            camera = world.Camera;
            sceneManager = world.SceneManager;
            physicsScene = world.PhysicsScene;
            physics = world.PhysicsScene.Physics;
        }
        public virtual void Update(float timeSinceLastFrame) { }

        public virtual void Dispose() { }

        public bool CheckCollide(GameObject gameObjInstance)
        {
            throw new NotImplementedException();
        }
    }
}
