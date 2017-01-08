using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine
{
    class Character
    {
        protected SceneNode mMainNode;
        protected SceneNode mSightNode;
        protected SceneNode mCameraNode;
        protected Entity mCharaEnt;
        protected SceneManager scm;

        public virtual void update(float elapsedTime) { }
        public SceneNode getSightNode()
        {
            return mSightNode;
        }

        public SceneNode getCameranode()
        {
            return mCameraNode;
        }

        public Vector3 getWorldPosition()
        {
            return mMainNode._getDerivedPosition();
        }
    }

    class OgreCharacter : Character
    {
        protected bool mForward = false;
		    protected bool mBackward = false;
		    protected bool mLeft = false;
		    protected bool mRight = false;
        protected string mName;
        public OgreCharacter(string name,SceneManager sceneMgr)
        {
            mName = name;
            scm = sceneMgr;

            mMainNode = scm.RootSceneNode.CreateChildSceneNode(mName);
            mSightNode = scm.RootSceneNode.CreateChildSceneNode(mName+"_sight",new Vector3(0,0,100));
            mCameraNode = scm.RootSceneNode.CreateChildSceneNode(mName+"_camera",new Vector3(0,50,-100));

            mCharaEnt = scm.CreateEntity(mName,"OgreHead.mesh");
            mMainNode.AttachObject(mCharaEnt);
        }

        public override void update(float elapsedTime)
        {
            // Handle movement
            if (mForward)
            {
                mMainNode.Translate(mMainNode.Orientation * (new Vector3(0, 0, 100 * elapsedTime)));
            }
            if (mBackward)
            {
                mMainNode.Translate(mMainNode.Orientation * (new Vector3(0, 0, -50 * elapsedTime)));
            }
            if (mLeft)
            {
                mMainNode.Yaw(new Radian(2 * elapsedTime));
            }
            if (mRight)
            {
                mMainNode.Yaw(new Radian(-2 * elapsedTime));
            }

            mForward = false;
            mBackward = false;
            mLeft = false;
            mRight = false;
        }
        public void setVisible(bool visible)
        {
            mMainNode.SetVisible(visible);
        }

        public void Forward(bool forward)
        {
            mForward = forward;
        }

        public void Backward(bool backward)
        {
            mBackward = backward;
        }

        public void Left(bool left)
        {
            mLeft = left;
        }

        public void Right(bool right)
        {
            mRight = right;
        }
    }
}
