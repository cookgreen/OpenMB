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
		protected GameMesh mesh;
        protected string kindID;
        protected int id;
        protected MoveInfo moveInfo;
        protected GameWorld world;
        protected Physics physics;
        protected Scene physicsScene;
        protected HealthInfo health;
		protected Vector3 position;
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
        public SceneNode MeshNode
        {
            get
            {
                return mesh.EntityNode;
            }
        }
        public GameMesh Mesh
        {
            get
            {
                return mesh;
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
            physicsScene = world.PhysicsScene;
            physics = world.PhysicsScene.Physics;
			mesh = new GameMesh(world.Camera);
        }

        protected virtual void create(){ }

        protected virtual void create(GameWorld world)
        {
        }
        public virtual void Update(float timeSinceLastFrame) { }

        public virtual void Dispose() { }

        public bool CheckCollide(GameObject gameObjInstance)
        {
			return false;
        }

		/// <summary>
		/// Use RTT to render a preview image
		/// </summary>
		/// <returns>preview image material name</returns>
		public virtual MaterialPtr RenderPreview()
		{
			return null;
		}

		public virtual void Destroy()
		{
		}
	}
}
