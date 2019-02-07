using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using org.critterai.nav;
using Mogre.PhysX;
using AMOFGameEngine.Sound;
using AMOFGameEngine.Utilities;

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
        public const int RUN_SPEED = 17;           // character running speed in units per second
        public const float TURN_SPEED = 500.0f;      // character turning in degrees per second
        private const float ANIM_FADE_SPEED = 7.5f;   // animation crossfade speed in % of full weight per second
        private const float JUMP_ACCEL = 30.0f;       // character jump acceleration in upward units per squared second
        private const float GRAVITY = 90.0f;          // gravity in downward units per squared second
        private SceneManager sceneMgr;
        private Camera camera;
        private SceneNode bodyNode;
        private SceneNode cameraPivot;
        private SceneNode cameraGoal;
        private SceneNode cameraNode;
        private float pivotPitch;
        private Entity bodyEnt;
        private Entity sword1;
        private Entity sword2;
        private List<AnimationState> anims = new List<AnimationState>();    // master animation list
        private AnimID baseAnimID;                   // current base (full- or lower-body) animation
        private AnimID topAnimID;                    // current top (upper-body) animation
        private List<bool> fadingIn = new List<bool>();            // which animations are fading in
        private List<bool> fadingOut = new List<bool>();           // which animations are fading out
        private bool swordsDrawn;
        private Mogre.Vector3 keyDirection;      // player's local intended direction based on WASD keys
        private Mogre.Vector3 goalDirection;     // actual intended direction in world-space
        private float verticalVelocity;     // for jumping
        private float timer;                // general timer to see how long animations have been playing
        private string charaName;
        private string charaMeshName;
        private bool controlled;
        private NavmeshQuery query;
        private Actor physicsActor;
        private Physics physics;
        private Scene physicsScene;
        private Mogre.Vector3 direction;
        private Dictionary<ItemUseAttachOption, string> useAttachBoneDic;
        private Dictionary<ItemHaveAttachOption, string> haveAttachBoneDic;
        public Mogre.Vector3 Position
        {
            get
            {
                return bodyNode.Position;
            }
            set
            {
                bodyNode.Position = value;
            }
        }
        public Mogre.Vector3 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }

        public enum AnimID
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

        public CharacterController(
            Camera cam,
            NavmeshQuery query,
            Scene physicsScene,
            string name, 
            string meshName, 
            bool controlled)
        {
            camera = cam;
            this.controlled = controlled;
            itemAttached = new List<Entity>();
            this.sceneMgr = cam.SceneManager;
            charaName = name;
            charaMeshName = meshName;
            this.physicsScene = physicsScene;
            physics = physicsScene.Physics;
            this.query = query;
            setupBody();
            if (controlled)
            {
                setupCamera(cam);
            }
            setupAnimations();
            setupPhysics();

            useAttachBoneDic = new Dictionary<ItemUseAttachOption, string>();
            haveAttachBoneDic = new Dictionary<ItemHaveAttachOption, string>();
        }

        private void setupPhysics()
        {
            BodyDesc bodyDesc = new BodyDesc();
            bodyDesc.LinearVelocity = new Mogre.Vector3(0, 2, 5);

            ActorDesc actorDesc = new ActorDesc();
            actorDesc.Density = 4;
            actorDesc.Body = bodyDesc;
            actorDesc.GlobalPosition = bodyNode.Position;
            actorDesc.GlobalOrientation = bodyNode.Orientation.ToRotationMatrix();

            // a quick trick the get the size of the physics shape right is to use the bounding box of the entity
            actorDesc.Shapes.Add(
                physics.CreateConvexHull(new
                StaticMeshData(bodyEnt.GetMesh())));

            // finally, create the actor in the physics scene
            physicsActor = physicsScene.CreateActor(actorDesc);
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
                bodyEnt.DetachObjectFromBone(itemEnt);
            }
            bodyEnt.AttachObjectToBone(boneName, itemEnt);
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
                bodyEnt.DetachObjectFromBone(itemEnt);
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
                    bodyNode = sceneMgr.RootSceneNode.CreateChildSceneNode(Mogre.Vector3.UNIT_Y * CHAR_HEIGHT);
                    bodyEnt = sceneMgr.CreateEntity(charaName, charaMeshName);
                    bodyNode.AttachObject(bodyEnt);
                    keyDirection = Mogre.Vector3.ZERO;
                    verticalVelocity = 0;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                GameManager.Instance.log.LogMessage("Engine Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Update Character
        /// </summary>
        /// <param name="deltaTime"></param>
        public void update(float deltaTime)
        {
            updateBody(deltaTime);
            updateAnimations(deltaTime);
            if (controlled)
            {
                updateCamera(deltaTime);
            }

            physicsScene.FlushStream();
            physicsScene.FetchResults(SimulationStatuses.AllFinished, true);
            physicsScene.Simulate(deltaTime);
        }

        public void injectKeyDown(KeyEvent evt)
        {
            if (evt.key == KeyCode.KC_Q && (topAnimID == AnimID.ANIM_IDLE_TOP || topAnimID == AnimID.ANIM_RUN_TOP))
            {
                // take swords out (or put them back, since it's the same animation but reversed)
                SetTopAnimation(AnimID.ANIM_DRAW_SWORDS, true);
                timer = 0;
            }
            else if (evt.key == KeyCode.KC_E && !swordsDrawn)
            {
                if (topAnimID == AnimID.ANIM_IDLE_TOP || topAnimID == AnimID.ANIM_RUN_TOP)
                {
                    // start dancing
                    SetBaseAnimation(AnimID.ANIM_DANCE, true);
                    SetTopAnimation(AnimID.ANIM_NONE);
                    // disable hand animation because the dance controls hands
                    anims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled = false;
                }
                else if (baseAnimID == AnimID.ANIM_DANCE)
                {
                    // stop dancing
                    SetBaseAnimation(AnimID.ANIM_IDLE_BASE);
                    SetTopAnimation(AnimID.ANIM_IDLE_TOP);
                    // re-enable hand animation-
                    anims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled = true;
                }
            }

            // keep track of the player's intended direction
            else if (evt.key == KeyCode.KC_W) keyDirection.z = -1;
            else if (evt.key == KeyCode.KC_A) keyDirection.x = -1;
            else if (evt.key == KeyCode.KC_S) keyDirection.z = 1;
            else if (evt.key == KeyCode.KC_D) keyDirection.x = 1;

            else if (evt.key == KeyCode.KC_SPACE && (topAnimID == AnimID.ANIM_IDLE_TOP || topAnimID == AnimID.ANIM_RUN_TOP))
            {
                // jump if on ground
                SetBaseAnimation(AnimID.ANIM_JUMP_START, true);
                SetTopAnimation(AnimID.ANIM_NONE);
                timer = 0;
            }

            else if(evt.key == KeyCode.KC_C)
            {
                //Battle Cry
                SoundManager.Instance.PlaySoundAtNode(bodyNode,"battle_cry_1");
            }

            if (!keyDirection.IsZeroLength && baseAnimID == AnimID.ANIM_IDLE_BASE)
            {
                // start running if not already moving and the player wants to move
                SetBaseAnimation(AnimID.ANIM_RUN_BASE, true);
                if (topAnimID == AnimID.ANIM_IDLE_TOP) SetTopAnimation(AnimID.ANIM_RUN_TOP, true);
            }
        }

        public void injectKeyUp(KeyEvent evt)
        {
            if (evt.key == KeyCode.KC_W && keyDirection.z == -1) keyDirection.z = 0;
            else if (evt.key == KeyCode.KC_A && keyDirection.x == -1) keyDirection.x = 0;
            else if (evt.key == KeyCode.KC_S && keyDirection.z == 1) keyDirection.z = 0;
            else if (evt.key == KeyCode.KC_D && keyDirection.x == 1) keyDirection.x = 0;

            if (keyDirection.IsZeroLength && baseAnimID == AnimID.ANIM_RUN_BASE)
            {
                // stop running if already moving and the player doesn't want to move
                SetBaseAnimation(AnimID.ANIM_IDLE_BASE);
                if (topAnimID == AnimID.ANIM_RUN_TOP) SetTopAnimation(AnimID.ANIM_IDLE_TOP);
            }
        }

        public void injectMouseMove(MouseEvent evt)
        {
            if (controlled)
            {
                updateCameraGoal(-0.05f * evt.state.X.rel, -0.05f * evt.state.Y.rel, -0.0005f * evt.state.Z.rel);
            }
        }

        public void YawBody(Degree degree)
        {
            bodyNode.Yaw(degree);
        }

        public Quaternion GetBodyOrientation()
        {
            return bodyNode.Orientation;
        }

        public void RotateBody(Quaternion quat)
        {
            bodyNode.Rotate(quat);
        }

        public void TranslateBody(Mogre.Vector3 vector3)
        {
            bodyNode.Translate(vector3);
        }

        public void SetBodyPos(float x, float y, float z)
        {
            bodyNode.SetPosition(x, y, z);
        }

        public void injectMouseDown(MouseEvent evt, MouseButtonID id)
        {
            if (swordsDrawn && (topAnimID == AnimID.ANIM_IDLE_TOP || topAnimID == AnimID.ANIM_RUN_TOP))
            {
                // if swords are out, and character's not doing something weird, then SLICE!
                if (id == MouseButtonID.MB_Left) SetTopAnimation(AnimID.ANIM_SLICE_VERTICAL, true);
                else if (id == MouseButtonID.MB_Right) SetTopAnimation(AnimID.ANIM_SLICE_HORIZONTAL, true);
                timer = 0;
            }
        }

        /// <summary>
        /// Set Animations for Character
        /// </summary>
        private void setupAnimations()
        {
            bodyEnt.Skeleton.BlendMode = SkeletonAnimationBlendMode.ANIMBLEND_CUMULATIVE;

            var animNames = new string[]
	    {"IdleBase", "IdleTop", "RunBase", "RunTop", "HandsClosed", "HandsRelaxed", "DrawSwords",
	    "SliceVertical", "SliceHorizontal", "Dance", "JumpStart", "JumpLoop", "JumpEnd"};

            // populate our animation list
            for (int i = 0; i < NUM_ANIMS; i++)
            {
                AnimationState animState;
                animState = bodyEnt.GetAnimationState(animNames[i]);
                anims.Add(animState);
                animState.Loop = (true);
                bool fadingIn = false;
                this.fadingIn.Add(fadingIn);
                bool fadingOut = false;
                this.fadingOut.Add(fadingOut);
            }

            // start off in the idle state (top and bottom together)
            SetBaseAnimation(AnimID.ANIM_IDLE_BASE);
            SetTopAnimation(AnimID.ANIM_IDLE_TOP);

            // relax the hands since we're not holding anything
            anims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled = (true);

            swordsDrawn = false;
        }

        /// <summary>
        /// Set the camera
        /// </summary>
        /// <param name="cam">Camera Instance</param>
        private void setupCamera(Camera cam)
        {
            cameraPivot = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
            // this is where the camera should be soon, and it spins around the pivot
            cameraGoal = cameraPivot.CreateChildSceneNode(new Mogre.Vector3(0, 0, 15));
            // this is where the camera actually is
            cameraNode = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
            cameraNode.Position = cameraPivot.Position + cameraGoal.Position;

            cameraPivot.SetFixedYawAxis(true);
            cameraGoal.SetFixedYawAxis(true);
            cameraNode.SetFixedYawAxis(true);

            // our model is quite small, so reduce the clipping planes
            cam.NearClipDistance = 0.1f;
            cam.FarClipDistance = 10000f;
            cameraNode.AttachObject(cam);

            pivotPitch = 0;
        }

        private void updateBody(float deltaTime)
        {
            goalDirection = Mogre.Vector3.ZERO;   // we will calculate this

            if (keyDirection != Mogre.Vector3.ZERO && baseAnimID != AnimID.ANIM_DANCE)
            {
                // calculate actually goal direction in world based on player's key directions
                goalDirection += keyDirection.z * cameraNode.Orientation.ZAxis;
                goalDirection += keyDirection.x * cameraNode.Orientation.XAxis;
                goalDirection.y = 0;
                goalDirection.Normalise();

                Quaternion toGoal = bodyNode.Orientation.ZAxis.GetRotationTo(goalDirection);

                // calculate how much the character has to turn to face goal direction
                float yawToGoal = toGoal.Yaw.ValueDegrees;
                // this is how much the character CAN turn this frame
                float yawAtSpeed = yawToGoal / Mogre.Math.Abs(yawToGoal) * deltaTime * TURN_SPEED;
                // reduce "turnability" if we're in midair
                if (baseAnimID == AnimID.ANIM_JUMP_LOOP) yawAtSpeed *= 0.2f;

                // turn as much as we can, but not more than we need to
                if (yawToGoal < 0) yawToGoal = System.Math.Min(0, System.Math.Max(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, yawAtSpeed, 0);
                else if (yawToGoal > 0) yawToGoal = System.Math.Max(0, System.Math.Min(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, 0, yawAtSpeed);

                bodyNode.Yaw(new Degree(yawToGoal));

                // move in current body direction (not the goal direction)
                bodyNode.Translate(0, 0, deltaTime * RUN_SPEED * anims[(int)baseAnimID].Weight,
                    Node.TransformSpace.TS_LOCAL);
            }

            if (baseAnimID == AnimID.ANIM_JUMP_LOOP)
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
                    SetBaseAnimation(AnimID.ANIM_JUMP_END, true);
                    timer = 0;
                }
            }
        }

        private void updateAnimations(float deltaTime)
        {
            float baseAnimSpeed = 1;
            float topAnimSpeed = 1;

            timer += deltaTime;

            if (topAnimID == AnimID.ANIM_DRAW_SWORDS)
            {
                sword1 = itemAttached.Where(o => o.Name == "SinbadSword1").First();
                sword2 = itemAttached.Where(o => o.Name == "SinbadSword2").First();
                // flip the draw swords animation if we need to put it back
                topAnimSpeed = swordsDrawn ? -1 : 1;

                // half-way through the animation is when the hand grasps the handles...
                if (timer >= anims[(int)topAnimID].Length / 2 &&
                    timer - deltaTime < anims[(int)topAnimID].Length / 2)
                {
                    // so transfer the swords from the sheaths to the hands
                    bodyEnt.DetachAllObjectsFromBone();
                    bodyEnt.AttachObjectToBone(swordsDrawn ? "Sheath.L" : "Handle.L", sword1);
                    bodyEnt.AttachObjectToBone(swordsDrawn ? "Sheath.R" : "Handle.R", sword2);
                    // change the hand state to grab or let go
                    anims[(int)AnimID.ANIM_HANDS_CLOSED].Enabled = !swordsDrawn;
                    anims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled = swordsDrawn;

                    // toggle sword trails
                    if (swordsDrawn)
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

                if (timer >= anims[(int)topAnimID].Length)
                {
                    // animation is finished, so return to what we were doing before
                    if (baseAnimID == AnimID.ANIM_IDLE_BASE) SetTopAnimation(AnimID.ANIM_IDLE_TOP);
                    else
                    {
                        SetTopAnimation(AnimID.ANIM_RUN_TOP);
                        anims[(int)AnimID.ANIM_RUN_TOP].TimePosition = anims[(int)AnimID.ANIM_RUN_BASE].TimePosition;
                    }
                    swordsDrawn = !swordsDrawn;
                }
            }
            else if (topAnimID == AnimID.ANIM_SLICE_VERTICAL || topAnimID == AnimID.ANIM_SLICE_HORIZONTAL)
            {
                if (timer >= anims[(int)topAnimID].Length)
                {
                    // animation is finished, so return to what we were doing before
                    if (baseAnimID == AnimID.ANIM_IDLE_BASE) SetTopAnimation(AnimID.ANIM_IDLE_TOP);
                    else
                    {
                        SetTopAnimation(AnimID.ANIM_RUN_TOP);
                        anims[(int)AnimID.ANIM_RUN_TOP].TimePosition = anims[(int)AnimID.ANIM_RUN_BASE].TimePosition;
                    }
                }

                // don't sway hips from side to side when slicing. that's just embarrasing.
                if (baseAnimID == AnimID.ANIM_IDLE_BASE) baseAnimSpeed = 0;
            }
            else if (baseAnimID == AnimID.ANIM_JUMP_START)
            {
                if (timer >= anims[(int)baseAnimID].Length)
                {
                    // takeoff animation finished, so time to leave the ground!
                    SetBaseAnimation(AnimID.ANIM_JUMP_LOOP, true);
                    // apply a jump acceleration to the character
                    verticalVelocity = JUMP_ACCEL;
                }
            }
            else if (baseAnimID == AnimID.ANIM_JUMP_END)
            {
                if (timer >= anims[(int)baseAnimID].Length)
                {
                    // safely landed, so go back to running or idling
                    if (keyDirection == Mogre.Vector3.ZERO)
                    {
                        SetBaseAnimation(AnimID.ANIM_IDLE_BASE);
                        SetTopAnimation(AnimID.ANIM_IDLE_TOP);
                    }
                    else
                    {
                        SetBaseAnimation(AnimID.ANIM_RUN_BASE, true);
                        SetTopAnimation(AnimID.ANIM_RUN_TOP, true);
                    }
                }
            }

            // increment the current base and top animation times
            if (baseAnimID != AnimID.ANIM_NONE) anims[(int)baseAnimID].AddTime(deltaTime * baseAnimSpeed);
            if (topAnimID != AnimID.ANIM_NONE) anims[(int)topAnimID].AddTime(deltaTime * topAnimSpeed);

            // apply smooth transitioning between our animations
            fadeAnimations(deltaTime);
        }

        private void fadeAnimations(float deltaTime)
        {
            for (int i = 0; i < NUM_ANIMS; i++)
            {
                if (fadingIn[i])
                {
                    // slowly fade this animation in until it has full weight
                    float newWeight = anims[i].Weight + deltaTime * ANIM_FADE_SPEED;
                    anims[i].Weight = Utilities.Helper.Clamp(newWeight, 0, 1);
                    if (newWeight >= 1) fadingIn[i] = false;
                }
                else if (fadingOut[i])
                {
                    // slowly fade this animation out until it has no weight, and then disable it
                    float newWeight = anims[i].Weight - deltaTime * ANIM_FADE_SPEED;
                    anims[i].Weight = Utilities.Helper.Clamp(newWeight, 0, 1);
                    if (newWeight <= 0)
                    {
                        anims[i].Enabled = false;
                        fadingOut[i] = false;
                    }
                }
            }
        }

        private void updateCamera(float deltaTime)
        {
            // place the camera pivot roughly at the character's shoulder
            cameraPivot.Position = bodyNode.Position + Mogre.Vector3.UNIT_Y * CAM_HEIGHT;
            // move the camera smoothly to the goal
            Mogre.Vector3 goalOffset = cameraGoal._getDerivedPosition() - cameraNode.Position;
            cameraNode.Translate(goalOffset * deltaTime * 9.0f);
            // always look at the pivot
            cameraNode.LookAt(cameraPivot._getDerivedPosition(), Node.TransformSpace.TS_WORLD);
        }

        private void updateCameraGoal(float deltaYaw, float deltaPitch, float deltaZoom)
        {
            cameraPivot.Yaw(new Degree(deltaYaw), Node.TransformSpace.TS_WORLD);

            // bound the pitch
            if (!(pivotPitch + deltaPitch > 25 && deltaPitch > 0) &&
                !(pivotPitch + deltaPitch < -60 && deltaPitch < 0))
            {
                cameraPivot.Pitch(new Degree(deltaPitch), Node.TransformSpace.TS_LOCAL);
                pivotPitch += deltaPitch;
            }

            float dist = cameraGoal._getDerivedPosition().DotProduct(cameraPivot._getDerivedPosition());
            float distChange = deltaZoom * dist;

            // bound the zoom
            if (!(dist + distChange < 8 && distChange < 0) &&
                !(dist + distChange > 25 && distChange > 0))
            {
                cameraGoal.Translate(0, 0, distChange, Node.TransformSpace.TS_LOCAL);
            }
        }

        public void SetBaseAnimation(AnimID id, bool reset = false)
        {
            if (baseAnimID >= 0 && (int)baseAnimID < NUM_ANIMS)
            {
                // if we have an old animation, fade it out
                fadingIn[(int)baseAnimID] = false;
                fadingOut[(int)baseAnimID] = true;
            }

            baseAnimID = id;

            if (id != AnimID.ANIM_NONE)
            {
                // if we have a new animation, enable it and fade it in
                anims[(int)id].Enabled = (true);
                anims[(int)id].Weight = 0;
                fadingOut[(int)id] = false;
                fadingIn[(int)id] = true;
                if (reset) anims[(int)id].TimePosition = 0;
            }
        }

        public void SetTopAnimation(AnimID id, bool reset = false)
        {
            if (topAnimID >= 0 && (int)topAnimID < NUM_ANIMS)
            {
                // if we have an old animation, fade it out
                fadingIn[(int)topAnimID] = false;
                fadingOut[(int)topAnimID] = true;
            }

            topAnimID = id;

            if (id != AnimID.ANIM_NONE)
            {
                // if we have a new animation, enable it and fade it in
                anims[(int)id].Enabled = true;
                anims[(int)id].Weight = 0;
                fadingOut[(int)id] = false;
                fadingIn[(int)id] = true;
                if (reset) anims[(int)id].TimePosition = 0;
            }
        }
        public bool GetControlled()
        {
            return controlled;
        }

        public void AttachItem(ItemUseAttachOption itemAttachOption, Item item)
        {

        }
    }
}
