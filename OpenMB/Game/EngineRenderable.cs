using Mogre;

namespace OpenMB.Game
{
	public class EngineRenderable
	{
		protected Camera camera;
		protected SceneManager sceneManager;
		protected Entity entity;
		protected SceneNode entityNode;
		public SceneManager SceneManager
		{
			get
			{
				return sceneManager;
			}
		}
		public Camera Camera
		{
			get
			{
				return camera;
			}
		}
		public Entity Entity
		{
			get
			{
				return entity;
			}
			set
			{
				entity = value;
			}
		}
		public SceneNode EntityNode
		{
			get
			{
				return entityNode;
			}
			set
			{
				entityNode = value;
			}
		}

		public EngineRenderable(Camera camera)
		{
			this.camera = camera;
			sceneManager = camera.SceneManager;
		}
	}
}