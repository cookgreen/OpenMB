using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using org.critterai.nav;
using Mogre.PhysX;
using AMOFGameEngine.Sound;

namespace AMOFGameEngine.Game
{
    /// <summary>
    /// Controller used by Character
    /// </summary>
    public class CharacterController
    {
        private int NUM_ANIMS = 13;          // number of animations the character has
        private const int CHAR_HEIGHT = 5;      // height of character's center of mass above ground
        private const int CAM_HEIGHT = 2;          // height of camera above character's center of mass
        private const int RUN_SPEED = 17;           // character running speed in units per second
        private const float TURN_SPEED = 500.0f;      // character turning in degrees per second
        private const float ANIM_FADE_SPEED = 7.5f;   // animation crossfade speed in % of full weight per second
        private const float JUMP_ACCEL = 30.0f;       // character jump acceleration in upward units per squared second
        private const float GRAVITY = 90.0f;          // gravity in downward units per squared second
        private SceneManager mSceneMgr;
        private Camera mCamera;
        private SceneNode mBodyNode;
        private SceneNode mCameraPivot;
        private SceneNode mCameraGoal;
        private SceneNode mCameraNode;
        private float mPivotPitch;
        private Entity mBodyEnt;
        private Entity mSword1;
        private Entity mSword2;
        private List<AnimationState> mAnims = new List<AnimationState>();    // master animation list
        private AnimID mBaseAnimID;                   // current base (full- or lower-body) animation
        private AnimID mTopAnimID;                    // current top (upper-body) animation
        private List<bool> mFadingIn = new List<bool>();            // which animations are fading in
        private List<bool> mFadingOut = new List<bool>();           // which animations are fading out
        private bool mSwordsDrawn;
        private Mogre.Vector3 mKeyDirection;      // player's local intended direction based on WASD keys
        private Mogre.Vector3 mGoalDirection;     // actual intended direction in world-space
        private float mVerticalVelocity;     // for jumping
        private float mTimer;                // general timer to see how long animations have been playing
        private string charaName;
        private string charaMeshName;
        private bool mControlled;
        private NavmeshQuery mQuery;
        private Actor mActor;
        private Physics mPhysics;
        private Scene mPhysicsScene;
        private Mogre.Vector3 mTargetDestinaton;
        private float mDistance;
        private Mogre.Vector3 mDirection;
        public Mogre.Vector3 Position
        {
            get
            {
                return mBodyNode.Position;
            }
            set
            {
                mBodyNode.Position = value;
            }
        }

        public Mogre.Vector3 Direction
        {
            get
            {
                return mDirection;
            }
        }

        enum AnimID
        {
            ANIM_IDLE_BASE,
            ANIM_IDLE_TOP,
            ANIM_RUN_BASE,
            ANIM_RUN_TOP,
            ANIM_HANDS_CLOSED,
            ANIM_HANDS_RELAXED,
            ANIM_DRAW_SWORDS,
            ANIM_SLICE_VERTICAL,
            ANIM_SLICE_HORIZONTAL,
            ANIM_DANCE,
            ANIM_JUMP_START,
            ANIM_JUMP_LOOP,
            ANIM_JUMP_END,
            ANIM_NONE
        };

        List<Entity> itemAttached;//Item that attached to character
        private Queue<Mogre.Vector3> mWalkList;

        public CharacterController(
            Camera cam,
            NavmeshQuery query,
            Scene physicsScene,
            string name, 
            string meshName, 
            bool controlled)
        {
            mCamera = cam;
            mControlled = controlled;
            itemAttached = new List<Entity>();
            this.mSceneMgr = cam.SceneManager;
            charaName = name;
            charaMeshName = meshName;
            setupBody();
            mTargetDestinaton = Mogre.Vector3.ZERO;
            if (controlled)
            {
                setupCamera(cam);
            }
            setupAnimations();
        }

        /// <summary>
        /// Attach a item to character body
        /// </summary>
        /// <param name="boneName">The bone name which want to attach</param>
        /// <param name="itemEnt">Entity that you want to attach</param>
        public void AttachEntityToChara(string boneName,Entity itemEnt)
        {
            if (itemEnt.IsAttached)
            {
                mBodyEnt.DetachObjectFromBone(itemEnt);
            }
            mBodyEnt.AttachObjectToBone(boneName, itemEnt);
            itemAttached.Add(itemEnt);
        }

        /// <summary>
        /// Detach a item from character entity
        /// </summary>
        /// <param name="itemEnt">entity that you want to detach</param>
        public void DetachEntity(Entity itemEnt)
        {
            if (!itemEnt.IsAttached)
            {
                mBodyEnt.DetachObjectFromBone(itemEnt);
                itemAttached.Remove(itemEnt);
            }
        }

        /// <summary>
        /// Create character body
        /// </summary>
        /// <returns>success return True, fail return False</returns>
        private bool setupBody()
        {
            try
            {
                if (!string.IsNullOrEmpty(charaMeshName) && !string.IsNullOrEmpty(charaName))
                {
                    mBodyNode = mSceneMgr.RootSceneNode.CreateChildSceneNode(Mogre.Vector3.UNIT_Y * CHAR_HEIGHT);
                    mBodyEnt = mSceneMgr.CreateEntity(charaName, charaMeshName);
                    mBodyNode.AttachObject(mBodyEnt);
                    mKeyDirection = Mogre.Vector3.ZERO;
                    mVerticalVelocity = 0;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                GameManager.Instance.mLog.LogMessage("Engine Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Update Character
        /// </summary>
        /// <param name="deltaTime"></param>
        public void addTime(float deltaTime)
        {
            updateBody(deltaTime);
            updateAnimations(deltaTime);
            if (mControlled)
            {
                updateCamera(deltaTime);
            }
            WalkState(deltaTime);
        }

        public void injectKeyDown(KeyEvent evt)
        {
            if (evt.key == KeyCode.KC_Q && (mTopAnimID == AnimID.ANIM_IDLE_TOP || mTopAnimID == AnimID.ANIM_RUN_TOP))
            {
                // take swords out (or put them back, since it's the same animation but reversed)
                setTopAnimation(AnimID.ANIM_DRAW_SWORDS, true);
                mTimer = 0;
            }
            else if (evt.key == KeyCode.KC_E && !mSwordsDrawn)
            {
                if (mTopAnimID == AnimID.ANIM_IDLE_TOP || mTopAnimID == AnimID.ANIM_RUN_TOP)
                {
                    // start dancing
                    setBaseAnimation(AnimID.ANIM_DANCE, true);
                    setTopAnimation(AnimID.ANIM_NONE);
                    // disable hand animation because the dance controls hands
                    mAnims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled = false;
                }
                else if (mBaseAnimID == AnimID.ANIM_DANCE)
                {
                    // stop dancing
                    setBaseAnimation(AnimID.ANIM_IDLE_BASE);
                    setTopAnimation(AnimID.ANIM_IDLE_TOP);
                    // re-enable hand animation
                    mAnims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled = true;
                }
            }

            // keep track of the player's intended direction
            else if (evt.key == KeyCode.KC_W) mKeyDirection.z = -1;
            else if (evt.key == KeyCode.KC_A) mKeyDirection.x = -1;
            else if (evt.key == KeyCode.KC_S) mKeyDirection.z = 1;
            else if (evt.key == KeyCode.KC_D) mKeyDirection.x = 1;

            else if (evt.key == KeyCode.KC_SPACE && (mTopAnimID == AnimID.ANIM_IDLE_TOP || mTopAnimID == AnimID.ANIM_RUN_TOP))
            {
                // jump if on ground
                setBaseAnimation(AnimID.ANIM_JUMP_START, true);
                setTopAnimation(AnimID.ANIM_NONE);
                mTimer = 0;
            }

            else if(evt.key == KeyCode.KC_C)
            {
                //Battle Cry
                SoundManager.Instance.PlaySoundAtNode(mBodyNode,"battle_cry_1");
            }

            if (!mKeyDirection.IsZeroLength && mBaseAnimID == AnimID.ANIM_IDLE_BASE)
            {
                // start running if not already moving and the player wants to move
                setBaseAnimation(AnimID.ANIM_RUN_BASE, true);
                if (mTopAnimID == AnimID.ANIM_IDLE_TOP) setTopAnimation(AnimID.ANIM_RUN_TOP, true);
            }
        }

        public void injectKeyUp(KeyEvent evt)
        {
            if (evt.key == KeyCode.KC_W && mKeyDirection.z == -1) mKeyDirection.z = 0;
            else if (evt.key == KeyCode.KC_A && mKeyDirection.x == -1) mKeyDirection.x = 0;
            else if (evt.key == KeyCode.KC_S && mKeyDirection.z == 1) mKeyDirection.z = 0;
            else if (evt.key == KeyCode.KC_D && mKeyDirection.x == 1) mKeyDirection.x = 0;

            if (mKeyDirection.IsZeroLength && mBaseAnimID == AnimID.ANIM_RUN_BASE)
            {
                // stop running if already moving and the player doesn't want to move
                setBaseAnimation(AnimID.ANIM_IDLE_BASE);
                if (mTopAnimID == AnimID.ANIM_RUN_TOP) setTopAnimation(AnimID.ANIM_IDLE_TOP);
            }
        }

        public void injectMouseMove(MouseEvent evt)
        {
            if (mControlled)
            {
                updateCameraGoal(-0.05f * evt.state.X.rel, -0.05f * evt.state.Y.rel, -0.0005f * evt.state.Z.rel);
            }
        }

        public void injectMouseDown(MouseEvent evt, MouseButtonID id)
        {
            if (mSwordsDrawn && (mTopAnimID == AnimID.ANIM_IDLE_TOP || mTopAnimID == AnimID.ANIM_RUN_TOP))
            {
                // if swords are out, and character's not doing something weird, then SLICE!
                if (id == MouseButtonID.MB_Left) setTopAnimation(AnimID.ANIM_SLICE_VERTICAL, true);
                else if (id == MouseButtonID.MB_Right) setTopAnimation(AnimID.ANIM_SLICE_HORIZONTAL, true);
                mTimer = 0;
            }
        }

        /// <summary>
        /// Set Animations for Character
        /// </summary>
        private void setupAnimations()
        {
            mBodyEnt.Skeleton.BlendMode = SkeletonAnimationBlendMode.ANIMBLEND_CUMULATIVE;

            var animNames = new String[]
	    {"IdleBase", "IdleTop", "RunBase", "RunTop", "HandsClosed", "HandsRelaxed", "DrawSwords",
	    "SliceVertical", "SliceHorizontal", "Dance", "JumpStart", "JumpLoop", "JumpEnd"};

            // populate our animation list
            for (int i = 0; i < NUM_ANIMS; i++)
            {
                AnimationState animState;
                animState = mBodyEnt.GetAnimationState(animNames[i]);
                mAnims.Add(animState);
                animState.Loop = (true);
                bool fadingIn = false;
                mFadingIn.Add(fadingIn);
                bool fadingOut = false;
                mFadingOut.Add(fadingOut);
            }

            // start off in the idle state (top and bottom together)
            setBaseAnimation(AnimID.ANIM_IDLE_BASE);
            setTopAnimation(AnimID.ANIM_IDLE_TOP);

            // relax the hands since we're not holding anything
            mAnims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled = (true);

            mSwordsDrawn = false;
        }

        /// <summary>
        /// Set the camera
        /// </summary>
        /// <param name="cam">Camera Instance</param>
        private void setupCamera(Camera cam)
        {
            mCameraPivot = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
            // this is where the camera should be soon, and it spins around the pivot
            mCameraGoal = mCameraPivot.CreateChildSceneNode(new Mogre.Vector3(0, 0, 15));
            // this is where the camera actually is
            mCameraNode = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
            mCameraNode.Position = mCameraPivot.Position + mCameraGoal.Position;

            mCameraPivot.SetFixedYawAxis(true);
            mCameraGoal.SetFixedYawAxis(true);
            mCameraNode.SetFixedYawAxis(true);

            // our model is quite small, so reduce the clipping planes
            cam.NearClipDistance = 0.1f;
            cam.FarClipDistance = 100f;
            mCameraNode.AttachObject(cam);

            mPivotPitch = 0;
        }

        private void updateBody(float deltaTime)
        {
            mGoalDirection = Mogre.Vector3.ZERO;   // we will calculate this

            if (mKeyDirection != Mogre.Vector3.ZERO && mBaseAnimID != AnimID.ANIM_DANCE)
            {
                // calculate actually goal direction in world based on player's key directions
                mGoalDirection += mKeyDirection.z * mCameraNode.Orientation.ZAxis;
                mGoalDirection += mKeyDirection.x * mCameraNode.Orientation.XAxis;
                mGoalDirection.y = 0;
                mGoalDirection.Normalise();

                Quaternion toGoal = mBodyNode.Orientation.ZAxis.GetRotationTo(mGoalDirection);

                // calculate how much the character has to turn to face goal direction
                float yawToGoal = toGoal.Yaw.ValueDegrees;
                // this is how much the character CAN turn this frame
                float yawAtSpeed = yawToGoal / Mogre.Math.Abs(yawToGoal) * deltaTime * TURN_SPEED;
                // reduce "turnability" if we're in midair
                if (mBaseAnimID == AnimID.ANIM_JUMP_LOOP) yawAtSpeed *= 0.2f;

                // turn as much as we can, but not more than we need to
                if (yawToGoal < 0) yawToGoal = System.Math.Min(0, System.Math.Max(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, yawAtSpeed, 0);
                else if (yawToGoal > 0) yawToGoal = System.Math.Max(0, System.Math.Min(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, 0, yawAtSpeed);

                mBodyNode.Yaw(new Degree(yawToGoal));

                // move in current body direction (not the goal direction)
                mBodyNode.Translate(0, 0, deltaTime * RUN_SPEED * mAnims[(int)mBaseAnimID].Weight,
                    Node.TransformSpace.TS_LOCAL);
            }

            if (mBaseAnimID == AnimID.ANIM_JUMP_LOOP)
            {
                // if we're jumping, add a vertical offset too, and apply gravity
                mBodyNode.Translate(0, mVerticalVelocity * deltaTime, 0, Node.TransformSpace.TS_LOCAL);
                mVerticalVelocity -= GRAVITY * deltaTime;

                Mogre.Vector3 pos = mBodyNode.Position;
                if (pos.y <= CHAR_HEIGHT)
                {
                    // if we've hit the ground, change to landing state
                    pos.y = CHAR_HEIGHT;
                    mBodyNode.Position = pos;
                    setBaseAnimation(AnimID.ANIM_JUMP_END, true);
                    mTimer = 0;
                }
            }
        }

        private void updateAnimations(float deltaTime)
        {
            float baseAnimSpeed = 1;
            float topAnimSpeed = 1;

            mTimer += deltaTime;

            if (mTopAnimID == AnimID.ANIM_DRAW_SWORDS)
            {
                mSword1 = itemAttached.Where(o => o.Name == "SinbadSword1").First();
                mSword2 = itemAttached.Where(o => o.Name == "SinbadSword2").First();
                // flip the draw swords animation if we need to put it back
                topAnimSpeed = mSwordsDrawn ? -1 : 1;

                // half-way through the animation is when the hand grasps the handles...
                if (mTimer >= mAnims[(int)mTopAnimID].Length / 2 &&
                    mTimer - deltaTime < mAnims[(int)mTopAnimID].Length / 2)
                {
                    // so transfer the swords from the sheaths to the hands
                    mBodyEnt.DetachAllObjectsFromBone();
                    mBodyEnt.AttachObjectToBone(mSwordsDrawn ? "Sheath.L" : "Handle.L", mSword1);
                    mBodyEnt.AttachObjectToBone(mSwordsDrawn ? "Sheath.R" : "Handle.R", mSword2);
                    // change the hand state to grab or let go
                    mAnims[(int)AnimID.ANIM_HANDS_CLOSED].Enabled = !mSwordsDrawn;
                    mAnims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled = mSwordsDrawn;

                    // toggle sword trails
                    if (mSwordsDrawn)
                    {
                        //mSwordTrail.Visible = false;
                        //mSwordTrail.RemoveNode(mSword1.ParentNode);
                        //mSwordTrail.RemoveNode(mSword2.ParentNode);
                    }
                    else
                    {
                        //mSwordTrail.Visible = true;
                        //mSwordTrail.AddNode(mSword1.ParentNode);
                        //mSwordTrail.AddNode(mSword2.ParentNode);
                    }
                }

                if (mTimer >= mAnims[(int)mTopAnimID].Length)
                {
                    // animation is finished, so return to what we were doing before
                    if (mBaseAnimID == AnimID.ANIM_IDLE_BASE) setTopAnimation(AnimID.ANIM_IDLE_TOP);
                    else
                    {
                        setTopAnimation(AnimID.ANIM_RUN_TOP);
                        mAnims[(int)AnimID.ANIM_RUN_TOP].TimePosition = mAnims[(int)AnimID.ANIM_RUN_BASE].TimePosition;
                    }
                    mSwordsDrawn = !mSwordsDrawn;
                }
            }
            else if (mTopAnimID == AnimID.ANIM_SLICE_VERTICAL || mTopAnimID == AnimID.ANIM_SLICE_HORIZONTAL)
            {
                if (mTimer >= mAnims[(int)mTopAnimID].Length)
                {
                    // animation is finished, so return to what we were doing before
                    if (mBaseAnimID == AnimID.ANIM_IDLE_BASE) setTopAnimation(AnimID.ANIM_IDLE_TOP);
                    else
                    {
                        setTopAnimation(AnimID.ANIM_RUN_TOP);
                        mAnims[(int)AnimID.ANIM_RUN_TOP].TimePosition = mAnims[(int)AnimID.ANIM_RUN_BASE].TimePosition;
                    }
                }

                // don't sway hips from side to side when slicing. that's just embarrasing.
                if (mBaseAnimID == AnimID.ANIM_IDLE_BASE) baseAnimSpeed = 0;
            }
            else if (mBaseAnimID == AnimID.ANIM_JUMP_START)
            {
                if (mTimer >= mAnims[(int)mBaseAnimID].Length)
                {
                    // takeoff animation finished, so time to leave the ground!
                    setBaseAnimation(AnimID.ANIM_JUMP_LOOP, true);
                    // apply a jump acceleration to the character
                    mVerticalVelocity = JUMP_ACCEL;
                }
            }
            else if (mBaseAnimID == AnimID.ANIM_JUMP_END)
            {
                if (mTimer >= mAnims[(int)mBaseAnimID].Length)
                {
                    // safely landed, so go back to running or idling
                    if (mKeyDirection == Mogre.Vector3.ZERO)
                    {
                        setBaseAnimation(AnimID.ANIM_IDLE_BASE);
                        setTopAnimation(AnimID.ANIM_IDLE_TOP);
                    }
                    else
                    {
                        setBaseAnimation(AnimID.ANIM_RUN_BASE, true);
                        setTopAnimation(AnimID.ANIM_RUN_TOP, true);
                    }
                }
            }

            // increment the current base and top animation times
            if (mBaseAnimID != AnimID.ANIM_NONE) mAnims[(int)mBaseAnimID].AddTime(deltaTime * baseAnimSpeed);
            if (mTopAnimID != AnimID.ANIM_NONE) mAnims[(int)mTopAnimID].AddTime(deltaTime * topAnimSpeed);

            // apply smooth transitioning between our animations
            fadeAnimations(deltaTime);
        }

        private void fadeAnimations(float deltaTime)
        {
            for (int i = 0; i < NUM_ANIMS; i++)
            {
                if (mFadingIn[i])
                {
                    // slowly fade this animation in until it has full weight
                    float newWeight = mAnims[i].Weight + deltaTime * ANIM_FADE_SPEED;
                    mAnims[i].Weight = Utilities.Helper.Clamp(newWeight, 0, 1);
                    if (newWeight >= 1) mFadingIn[i] = false;
                }
                else if (mFadingOut[i])
                {
                    // slowly fade this animation out until it has no weight, and then disable it
                    float newWeight = mAnims[i].Weight - deltaTime * ANIM_FADE_SPEED;
                    mAnims[i].Weight = Utilities.Helper.Clamp(newWeight, 0, 1);
                    if (newWeight <= 0)
                    {
                        mAnims[i].Enabled = false;
                        mFadingOut[i] = false;
                    }
                }
            }
        }

        private void updateCamera(float deltaTime)
        {
            // place the camera pivot roughly at the character's shoulder
            mCameraPivot.Position = mBodyNode.Position + Mogre.Vector3.UNIT_Y * CAM_HEIGHT;
            // move the camera smoothly to the goal
            Mogre.Vector3 goalOffset = mCameraGoal._getDerivedPosition() - mCameraNode.Position;
            mCameraNode.Translate(goalOffset * deltaTime * 9.0f);
            // always look at the pivot
            mCameraNode.LookAt(mCameraPivot._getDerivedPosition(), Node.TransformSpace.TS_WORLD);
        }

        private void updateCameraGoal(float deltaYaw, float deltaPitch, float deltaZoom)
        {
            mCameraPivot.Yaw(new Degree(deltaYaw), Node.TransformSpace.TS_WORLD);

            // bound the pitch
            if (!(mPivotPitch + deltaPitch > 25 && deltaPitch > 0) &&
                !(mPivotPitch + deltaPitch < -60 && deltaPitch < 0))
            {
                mCameraPivot.Pitch(new Degree(deltaPitch), Node.TransformSpace.TS_LOCAL);
                mPivotPitch += deltaPitch;
            }

            float dist = mCameraGoal._getDerivedPosition().DotProduct(mCameraPivot._getDerivedPosition());
            float distChange = deltaZoom * dist;

            // bound the zoom
            if (!(dist + distChange < 8 && distChange < 0) &&
                !(dist + distChange > 25 && distChange > 0))
            {
                mCameraGoal.Translate(0, 0, distChange, Node.TransformSpace.TS_LOCAL);
            }
        }

        private void setBaseAnimation(AnimID id, bool reset = false)
        {
            if (mBaseAnimID >= 0 && (int)mBaseAnimID < NUM_ANIMS)
            {
                // if we have an old animation, fade it out
                mFadingIn[(int)mBaseAnimID] = false;
                mFadingOut[(int)mBaseAnimID] = true;
            }

            mBaseAnimID = id;

            if (id != AnimID.ANIM_NONE)
            {
                // if we have a new animation, enable it and fade it in
                mAnims[(int)id].Enabled = (true);
                mAnims[(int)id].Weight = 0;
                mFadingOut[(int)id] = false;
                mFadingIn[(int)id] = true;
                if (reset) mAnims[(int)id].TimePosition = 0;
            }
        }

        private void setTopAnimation(AnimID id, bool reset = false)
        {
            if (mTopAnimID >= 0 && (int)mTopAnimID < NUM_ANIMS)
            {
                // if we have an old animation, fade it out
                mFadingIn[(int)mTopAnimID] = false;
                mFadingOut[(int)mTopAnimID] = true;
            }

            mTopAnimID = id;

            if (id != AnimID.ANIM_NONE)
            {
                // if we have a new animation, enable it and fade it in
                mAnims[(int)id].Enabled = true;
                mAnims[(int)id].Weight = 0;
                mFadingOut[(int)id] = false;
                mFadingIn[(int)id] = true;
                if (reset) mAnims[(int)id].TimePosition = 0;
            }
        }
        public bool GetControlled()
        {
            return mControlled;
        }

        public void WalkTo(Mogre.Vector3 position)
        {
            mWalkList = new Queue<Mogre.Vector3>();

            float a = (position.z - Position.z) / (position.x - Position.x);
            float b = position.z - a * position.x;

            for (int i = 0; i < Mogre.Math.Abs(position.x - Position.x) / 5; i++)
            {
                mWalkList.Enqueue(new Mogre.Vector3(position.x + i, 0, a * (position.x + i) + b));
            }
        }

        private void WalkState(float deltaTime)
        {
            if (Direction == Mogre.Vector3.ZERO)
            {
                if (nextLocation())
                {
                    setTopAnimation(AnimID.ANIM_RUN_TOP, true);
                    setBaseAnimation(AnimID.ANIM_RUN_TOP, true);
                }
            }
            else
            {
                float move = RUN_SPEED * deltaTime;
                mDistance -= move;
                if (mDistance <= 0.0f)
                {
                    mBodyNode.SetPosition(mTargetDestinaton.x, mTargetDestinaton.y, mTargetDestinaton.z);
                    mDirection = Mogre.Vector3.ZERO;
                    if (!nextLocation())
                    {
                        setTopAnimation(AnimID.ANIM_IDLE_TOP, true);
                        setBaseAnimation(AnimID.ANIM_IDLE_BASE, true);
                    }
                    else
                    {
                        Mogre.Vector3 src = mBodyNode.Orientation * Mogre.Vector3.NEGATIVE_UNIT_Z;
                        if ((1.0f + src.DotProduct(Direction)) < 0.0001f)
                        {
                            mBodyNode.Yaw(new Degree(180));
                        }
                        else
                        {
                            Quaternion quat = src.GetRotationTo(Direction);
                            mBodyNode.Rotate(quat);
                        }
                    }
                }
                else
                {
                    mBodyNode.Translate(Direction * move);
                }
            }

            //mAnimationState->addTime(evt.timeSinceLastFrame);
        }

        private bool nextLocation()
        {
            if (mWalkList != null)
            {
                if (mWalkList.Count == 0)
                    return false;
                mTargetDestinaton = mWalkList.Peek(); // 取得队列的头部
                mWalkList.Dequeue(); // 删除已走过的点
                mDirection = mTargetDestinaton - mBodyNode.Position;//距离由目的地减去当前位置得到
                mDistance = mDirection.Normalise();//normalise()作用是将方向向量转换成单位向量，返回向量的原始长度
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
