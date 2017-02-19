using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine
{
    class ExCamera
    {
        public enum Mode : uint
		{ 
			Chasing = 0, Fixed = 1, FirstPerson = 2
		}

		protected SceneNode mTargetNode;
		protected SceneNode mCameraNode;
		protected Camera mCamera;

		protected SceneManager mSceneMgr;
		protected string mName;

		protected bool mOwnCamera;
		protected float mTightness;

		protected RenderWindow mRenderWindow = null;
		protected Viewport mViewport = null;

        public ExCamera(String name, SceneManager sceneMgr, Camera camera)
		{
			// Basic member references setup
			mName = name;
			mSceneMgr = sceneMgr;

			// Create the camera's node structure
			mCameraNode = mSceneMgr.RootSceneNode.CreateChildSceneNode(mName);
			mTargetNode = mSceneMgr.RootSceneNode.CreateChildSceneNode(mName + "_target");
			mCameraNode.SetAutoTracking(true, mTargetNode); // The camera will always look at the camera target
			mCameraNode.SetFixedYawAxis(true); // Needed because of auto tracking

			// Create our camera if it wasn't passed as a parameter 
			if (camera == null) 
			{
				mCamera = mSceneMgr.CreateCamera(mName);
				mOwnCamera = true;
			}
			else 
			{
				mCamera = camera;
				mOwnCamera = false;
			}
			// ... and attach the Ogre camera to the camera node
			mCameraNode.AttachObject(mCamera);

			// Default tightness
			mTightness = 0.01f;

		}

		public void setTightness (float tightness) 
		{
			mTightness = tightness;
		}

		public float getTightness () 
		{
			return mTightness;
		}

		public Vector3 getCameraPosition () 
		{
			return mCameraNode.Position;
		}

		public void instantUpdate (Vector3 cameraPosition, Vector3 targetPosition) 
		{
			mCameraNode.SetPosition (cameraPosition.x,cameraPosition.y,cameraPosition.z);
			mTargetNode.SetPosition (targetPosition.x,targetPosition.y,targetPosition.z);
		}

		public void update (float elapsedTime, Vector3 cameraPosition, Vector3 targetPosition) 
		{
			// Handle movement
			Vector3 displacement;

			displacement = (cameraPosition - mCameraNode.Position) * mTightness;
			mCameraNode.Translate (displacement);

			displacement = (targetPosition - mTargetNode.Position) * mTightness;
			mTargetNode.Translate (displacement);
		}

		public Camera getCamera()
		{
			return mCamera;
		}
    }
}
