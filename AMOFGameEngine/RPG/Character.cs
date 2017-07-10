using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.RPG
{
    public class Character
    {
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

        public const int NUM_ANIMS = 13;          // number of animations the character has
        public const int CHAR_HEIGHT = 5;      // height of character's center of mass above ground
        public const int CAM_HEIGHT = 2;          // height of camera above character's center of mass
        public const int RUN_SPEED = 17;           // character running speed in units per second
        public const float TURN_SPEED = 500.0f;      // character turning in degrees per second
        public const float ANIM_FADE_SPEED = 7.5f;   // animation crossfade speed in % of full weight per second
        public const float JUMP_ACCEL = 30.0f;       // character jump acceleration in upward units per squared second
        public const float GRAVITY = 90.0f;          // gravity in downward units per squared second

        string charaTypeID;
        string charaID;
        string charaName;
        string charaMeshName;
        LevelInfo level;
        int hitpoint;
        InventoryInfo inventory;
        bool alive;
        bool wielded;
        bool wieldedR;
        Item currentWieldItem;
        Item currentWieldItemR;

        Camera cam;
        Entity bodyEnt;
        SceneNode bodyNode;
        Keyboard keyboard;
        Mouse mouse;
        bool controlled;
        AnimationState[] anims = new AnimationState[NUM_ANIMS];
        Mogre.Vector3 keyDirection;
        float verticalVelocity;
        AnimID mBaseAnimID;                   // current base (full- or lower-body) animation
        AnimID mTopAnimID;                    // current top (upper-body) animation
        bool[] mFadingIn = new bool[NUM_ANIMS];            // which animations are fading in
        bool[] mFadingOut = new bool[NUM_ANIMS];           // which animations are fading out
        bool mSwordsDrawn;
        Mogre.Vector3 mGoalDirection;     // actual intended direction in world-space
        Camera mCamera;
        SceneNode mCameraPivot;
        SceneNode mCameraGoal;
        SceneNode mCameraNode;
        float mPivotPitch;
        float mTimer;
        Mogre.Vector3 position;

        public Mogre.Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Mogre.Vector3 Direction
        {
            get { return bodyNode.Orientation * Mogre.Vector3.UNIT_Z; ; }
        }
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
        public LevelInfo Level
        {
            get { return level; }
            set { level = value; }
        }
        public string CharaMeshName
        {
            get { return charaMeshName; }
            set { charaMeshName = value; }
        }
        public string CharaName
        {
            get { return charaName; }
            set { charaName = value; }
        }
        public string CharaID
        {
            get { return charaID; }
        }
        public string CharaTypeID
        {
            get { return charaTypeID; }
            set { charaTypeID = value; }
        }
        public Item CurrentWieldItem
        {
            get { return currentWieldItem; }
            set { currentWieldItem = value; }
        }
        public int HitPoint
        {
            get { return hitpoint; }
        }

        public Character(Camera cam, Keyboard keyboard, Mouse mouse, bool controlled = false)
        {
            this.cam = cam;
            this.controlled = controlled;
            this.keyboard = keyboard;
            this.mouse = mouse;

            inventory = new InventoryInfo();
            level = new LevelInfo();
            currentWieldItem = null;
            currentWieldItemR = null;
            wielded = false;
            wieldedR = false;
            hitpoint = 100;
            alive = true;

            keyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyboard_KeyPressed);
            keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyboard_KeyReleased);
            mouse.MouseMoved += new MouseListener.MouseMovedHandler(mouse_MouseMoved);
            mouse.MousePressed += new MouseListener.MousePressedHandler(mouse_MousePressed);
            mouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouse_MouseReleased);
            level.NewLevelReached += new Action<bool>(level_NewLevelReached);
        }

        void level_NewLevelReached(bool obj)
        {
            if (controlled)
            {
                GameManager.Singleton.mSoundMgr.PlaySoundByID("YouReachNewlevel");
            }
        }

        #region event handler
        bool mouse_MouseReleased(MouseEvent arg, MouseButtonID id)
        {
            return true;
        }

        bool mouse_MousePressed(MouseEvent arg, MouseButtonID id)
        {
            if (controlled)
            {
                switch (id)
                {
                    case MouseButtonID.MB_Left:
                        AttackWithCurrentWeapon();
                        break;
                    case MouseButtonID.MB_Right:
                        DefenceWithShieldOrWeapon();
                        break;
                }
            }

            return true;
        }

        bool mouse_MouseMoved(MouseEvent arg)
        {
            if (controlled)
            {
                MouseState_NativePtr state = arg.state;
                if (arg.state.Z.rel != 0 && inventory.Weapons.Length > 0)
                {
                    float newIndex = inventory.CurrentWeaponIndex - arg.state.Z.rel / Mogre.Math.Abs((float)arg.state.Z.rel);
                    float finalIndex = GameManager.Singleton.Clamp(newIndex, 0.0f, (float)(4 - 1));
                    inventory.CurrentWeapon = inventory.Weapons[(int)finalIndex];
                }
                updateCameraGoal(-0.05f * arg.state.X.rel, -0.05f * arg.state.Y.rel, 0);
            }

            return true;
        }

        private void SetWieldItem()
        {
            currentWieldItem = inventory.Weapons[0];
            if (currentWieldItem != null && (mTopAnimID == AnimID.ANIM_IDLE_TOP || mTopAnimID == AnimID.ANIM_RUN_TOP))
            {
                setTopAnimation(AnimID.ANIM_DRAW_SWORDS, true);
                mTimer = 0;
            }
        }

        bool keyboard_KeyReleased(KeyEvent arg)
        {
            if (controlled)
            {
                if (arg.key == KeyCode.KC_W && keyDirection.z == -1) keyDirection.z = 0;
                else if (arg.key == KeyCode.KC_A && keyDirection.x == -1) keyDirection.x = 0;
                else if (arg.key == KeyCode.KC_S && keyDirection.z == 1) keyDirection.z = 0;
                else if (arg.key == KeyCode.KC_D && keyDirection.x == 1) keyDirection.x = 0;

                if (keyDirection.IsZeroLength && mBaseAnimID == AnimID.ANIM_RUN_BASE)
                {
                    // stop running if already moving and the player doesn't want to move
                    setBaseAnimation(AnimID.ANIM_IDLE_BASE);
                    if (mTopAnimID == AnimID.ANIM_RUN_TOP) setTopAnimation(AnimID.ANIM_IDLE_TOP);
                }
            }

            return true;
        }

        bool keyboard_KeyPressed(KeyEvent arg)
        {
            if (controlled)
            {
                switch (arg.key)
                {
                    case KeyCode.KC_Q:
                        SetWieldItem();
                        break;
                    case KeyCode.KC_W:
                        MoveForward();
                        break;
                    case KeyCode.KC_A:
                        MoveLeft();
                        break;
                    case KeyCode.KC_S:
                        MoveBackward();
                        break;
                    case KeyCode.KC_D:
                        MoveRight();
                        break;
                    case KeyCode.KC_E:
                        Kick();
                        break;
                    case KeyCode.KC_SPACE:
                        Jump();
                        break;
                    case KeyCode.KC_Z:
                        break;
                }
                if (!keyDirection.IsZeroLength && mBaseAnimID == AnimID.ANIM_IDLE_BASE)
                {
                    // start running if not already moving and the player wants to move
                    setBaseAnimation(AnimID.ANIM_RUN_BASE, true);
                    if (mTopAnimID == AnimID.ANIM_IDLE_TOP) setTopAnimation(AnimID.ANIM_RUN_TOP, true);
                }
            }

            return true;
        }
        #endregion

        #region Attack & Defence
        private void AttackWithCurrentWeapon()
        {
            if (currentWieldItem != null && (mTopAnimID == AnimID.ANIM_IDLE_TOP || mTopAnimID == AnimID.ANIM_RUN_TOP))
            {
                setTopAnimation(AnimID.ANIM_SLICE_VERTICAL, true);
            }
        }

        private void DefenceWithShieldOrWeapon()
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region character special action
        private void Jump()
        {
            
        }

        private void Kick()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region character movement action
        private void MoveRight()
        {
            keyDirection.x = 1;
        }

        private void MoveBackward()
        {
            keyDirection.z = 1;
        }

        private void MoveLeft()
        {
            keyDirection .x = -1;
        }

        private void MoveForward()
        {
            keyDirection.z = -1;
        }
        #endregion

        #region update animation, body and camera
        private void updateBody(float deltaTime)
        {
            mGoalDirection = Mogre.Vector3.ZERO;   // we will calculate this

            if (keyDirection != Mogre.Vector3.ZERO && mBaseAnimID != AnimID.ANIM_DANCE)
            {
                // calculate actually goal direction in world based on player's key directions
                mGoalDirection += keyDirection.z * mCameraNode.Orientation.ZAxis;
                mGoalDirection += keyDirection.x * mCameraNode.Orientation.XAxis;
                mGoalDirection.y = 0;
                mGoalDirection.Normalise();

                Quaternion toGoal = bodyNode.Orientation.ZAxis.GetRotationTo(mGoalDirection);

                // calculate how much the character has to turn to face goal direction
                float yawToGoal = toGoal.Yaw.ValueDegrees;
                // this is how much the character CAN turn this frame
                float yawAtSpeed = yawToGoal / Mogre.Math.Abs(yawToGoal) * deltaTime * TURN_SPEED;
                // reduce "turnability" if we're in midair
                if (mBaseAnimID == AnimID.ANIM_JUMP_LOOP) yawAtSpeed *= 0.2f;

                // turn as much as we can, but not more than we need to
                if (yawToGoal < 0) yawToGoal = System.Math.Min(0, System.Math.Max(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, yawAtSpeed, 0);
                else if (yawToGoal > 0) yawToGoal = System.Math.Max(0, System.Math.Min(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, 0, yawAtSpeed);

                bodyNode.Yaw(new Degree(yawToGoal));

                // move in current body direction (not the goal direction)
                bodyNode.Translate(0, 0, deltaTime * RUN_SPEED * anims[(int)mBaseAnimID].Weight,
                    Node.TransformSpace.TS_LOCAL);

                position = bodyNode.Position;
            }

            if (mBaseAnimID == AnimID.ANIM_JUMP_LOOP)
            {
                // if we're jumping, add a vertical offset too, and apply gravity
                bodyNode.Translate(0, verticalVelocity * deltaTime, 0, Node.TransformSpace.TS_LOCAL);
                verticalVelocity -= GRAVITY * deltaTime;

                Mogre.Vector3 pos = bodyNode.Position;
                if (pos.y <= CHAR_HEIGHT)
                {
                    // if we've hit the ground, change to landing state
                    pos.y = CHAR_HEIGHT;
                    bodyNode.Position = pos;
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
                // flip the draw swords animation if we need to put it back
                topAnimSpeed = mSwordsDrawn ? -1 : 1;

                // half-way through the animation is when the hand grasps the handles...
                if (mTimer >= anims[(int)mTopAnimID].Length / 2 &&
                    mTimer - deltaTime < anims[(int)mTopAnimID].Length / 2)
                {
                    // so transfer the swords from the sheaths to the hands
                    bodyEnt.DetachAllObjectsFromBone();
                    if (currentWieldItem != null)
                        bodyEnt.AttachObjectToBone(wielded ? "Sheath.L" : "Handle.L", currentWieldItem.ItemEnt);
                    if (currentWieldItemR != null && wieldedR)
                        bodyEnt.AttachObjectToBone(!wieldedR ? "Sheath.R" : "Handle.R", currentWieldItemR.ItemEnt);
                    // change the hand state to grab or let go
                    anims[(int)AnimID.ANIM_HANDS_CLOSED].Enabled = !wielded;
                    anims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled = wielded;

                    // toggle sword trails
                    //if (mSwordsDrawn)
                    //{
                    //    mSwordTrail.Visible = false;
                    //    mSwordTrail.RemoveNode(mSword1.ParentNode);
                    //    mSwordTrail.RemoveNode(mSword2.ParentNode);
                    //}
                    //else
                    //{
                    //    mSwordTrail.Visible = true;
                    //    mSwordTrail.AddNode(mSword1.ParentNode);
                    //    mSwordTrail.AddNode(mSword2.ParentNode);
                    //}
                }

                if (mTimer >= anims[(int)mTopAnimID].Length)
                {
                    // animation is finished, so return to what we were doing before
                    if (mBaseAnimID == AnimID.ANIM_IDLE_BASE) setTopAnimation(AnimID.ANIM_IDLE_TOP);
                    else
                    {
                        setTopAnimation(AnimID.ANIM_RUN_TOP);
                        anims[(int)AnimID.ANIM_RUN_TOP].TimePosition = anims[(int)AnimID.ANIM_RUN_BASE].TimePosition;
                    }
                    wielded = !wielded;
                }
            }
            else if (mTopAnimID == AnimID.ANIM_SLICE_VERTICAL || mTopAnimID == AnimID.ANIM_SLICE_HORIZONTAL)
            {
                if (mTimer >= anims[(int)mTopAnimID].Length)
                {
                    // animation is finished, so return to what we were doing before
                    if (mBaseAnimID == AnimID.ANIM_IDLE_BASE) setTopAnimation(AnimID.ANIM_IDLE_TOP);
                    else
                    {
                        setTopAnimation(AnimID.ANIM_RUN_TOP);
                        anims[(int)AnimID.ANIM_RUN_TOP].TimePosition = anims[(int)AnimID.ANIM_RUN_BASE].TimePosition;
                    }
                }

                // don't sway hips from side to side when slicing. that's just embarrasing.
                if (mBaseAnimID == AnimID.ANIM_IDLE_BASE) baseAnimSpeed = 0;
            }
            else if (mBaseAnimID == AnimID.ANIM_JUMP_START)
            {
                if (mTimer >= anims[(int)mBaseAnimID].Length)
                {
                    // takeoff animation finished, so time to leave the ground!
                    setBaseAnimation(AnimID.ANIM_JUMP_LOOP, true);
                    // apply a jump acceleration to the character
                    verticalVelocity = JUMP_ACCEL;
                }
            }
            else if (mBaseAnimID == AnimID.ANIM_JUMP_END)
            {
                if (mTimer >= anims[(int)mBaseAnimID].Length)
                {
                    // safely landed, so go back to running or idling
                    if (keyDirection == Mogre.Vector3.ZERO)
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
            if (mBaseAnimID != AnimID.ANIM_NONE) anims[(int)mBaseAnimID].AddTime(deltaTime * baseAnimSpeed);
            if (mTopAnimID != AnimID.ANIM_NONE) anims[(int)mTopAnimID].AddTime(deltaTime * topAnimSpeed);

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
                    float newWeight = anims[i].Weight + deltaTime * ANIM_FADE_SPEED;
                    anims[i].Weight = GameManager.Singleton.Clamp(newWeight, 0, 1);
                    if (newWeight >= 1) mFadingIn[i] = false;
                }
                else if (mFadingOut[i])
                {
                    // slowly fade this animation out until it has no weight, and then disable it
                    float newWeight = anims[i].Weight - deltaTime * ANIM_FADE_SPEED;
                    anims[i].Weight = GameManager.Singleton.Clamp(newWeight, 0, 1);
                    if (newWeight <= 0)
                    {
                        anims[i].Enabled = false;
                        mFadingOut[i] = false;
                    }
                }
            }
        }

        private void updateCamera(float deltaTime)
        {
            // place the camera pivot roughly at the character's shoulder
            mCameraPivot.Position = bodyNode.Position + Mogre.Vector3.UNIT_Y * CAM_HEIGHT;
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
        #endregion

        #region create animation, body and camera
        private void setupAnimations()
        {
            bodyEnt.Skeleton.BlendMode = SkeletonAnimationBlendMode.ANIMBLEND_CUMULATIVE;

            var animNames = new String[]
	    {"IdleBase", "IdleTop", "RunBase", "RunTop", "HandsClosed", "HandsRelaxed", "DrawSwords",
	    "SliceVertical", "SliceHorizontal", "Dance", "JumpStart", "JumpLoop", "JumpEnd"};

            // populate our animation list
            for (int i = 0; i < NUM_ANIMS; i++)
            {
                anims[i] = bodyEnt .GetAnimationState(animNames[i]);
                anims[i].Loop = (true);
                mFadingIn[i] = false;
                mFadingOut[i] = false;
            }

            // start off in the idle state (top and bottom together)
            setBaseAnimation(AnimID.ANIM_IDLE_BASE);
            setTopAnimation(AnimID.ANIM_IDLE_TOP);

            // relax the hands since we're not holding anything
            anims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled = (true);
        }

        private void setupCamera()
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
            cam.FarClipDistance = 100;
            mCameraNode.AttachObject(cam);

            mPivotPitch = 0;
        }

        private void setupBody()
        {
            bodyNode = cam.SceneManager.RootSceneNode.CreateChildSceneNode(Mogre.Vector3.UNIT_Y * 23);
            bodyEnt = cam.SceneManager.CreateEntity(charaName, charaMeshName);
            bodyNode.AttachObject(bodyEnt);

            keyDirection = Mogre.Vector3.ZERO;
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
                anims[(int)id].Enabled = (true);
                anims[(int)id].Weight = 0;
                mFadingOut[(int)id] = false;
                mFadingIn[(int)id] = true;
                if (reset) anims[(int)id].TimePosition = 0;
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
                anims[(int)id].Enabled = true;
                anims[(int)id].Weight = 0;
                mFadingOut[(int)id] = false;
                mFadingIn[(int)id] = true;
                if (reset) anims[(int)id].TimePosition = 0;
            }
        }
        #endregion

        public void Create()
        {
            setupBody();
            setupAnimations();
            if (controlled)
            {
                setupCamera();
            }
        }

        public void Update(float deltaTime)
        {
            updateBody(deltaTime);
            updateAnimations(deltaTime);
            if (controlled)
            {
                updateCamera(deltaTime);
            }
        }

        public void AddEquipment(Item item, bool isEquipNow)
        {
            if (isEquipNow)
            {
                inventory.AddItemToInventory(item);
                inventory.Weapons[0] = item;
                if (item != null && item.ItemType == ItemType.IT_WEAPON)
                {
                    string boneName = "";
                    switch (item.ItemAttachDir)
                    {
                        case ItemAttachOption.IAO_LEFT:
                            boneName = "Sheath.L";
                            break;
                        case ItemAttachOption.IAO_RIGHT:
                            boneName = "Sheath.R";
                            break;
                        case ItemAttachOption.IAO_LEFTFLANK:
                            break;
                        case ItemAttachOption.IAO_RIGHTFLANK:
                            break;
                        case ItemAttachOption.IAO_FRONT:
                            break;
                    }
                    if (!item.ItemEnt.IsAttached)
                        bodyEnt.AttachObjectToBone(boneName, item.ItemEnt);
                }
            }
            else
            {
                inventory.AddItemToInventory(item);
            }
        }

        public void DerivedDamage(int damage)
        {
            hitpoint = hitpoint - damage;
            if (hitpoint <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            alive = false;
            bodyNode.DetachObject(bodyEnt);
            bodyEnt.Dispose();
            bodyEnt = null;
            bodyNode.Dispose();
            bodyNode = null;
        }

        public void Hide()
        {
            bodyNode.DetachObject(bodyEnt);
        }

        public void Show()
        {
            bodyNode.AttachObject(bodyEnt);
        }
    }
}
