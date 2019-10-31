using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using org.critterai.nav;
using Mogre.PhysX;
using OpenMB.Sound;
using OpenMB.Utilities;
using OpenMB.Mods.XML;

namespace OpenMB.Game
{
    /// <summary>
    /// Controller used by Character
    /// </summary>
    public class CharacterController
    {
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
        private List<CharacterAnimation> anims = new List<CharacterAnimation>();    // master animation list
        private CharacterAnimation baseAnim;                   // current base (full- or lower-body) animation
        private CharacterAnimation topAnim;                    // current top (upper-body) animation
        private List<bool> fadingIn = new List<bool>();            // which animations are fading in
        private List<bool> fadingOut = new List<bool>();           // which animations are fading out
        private Mogre.Vector3 keyDirection;      // player's local intended direction based on WASD keys
        private Mogre.Vector3 goalDirection;     // actual intended direction in world-space
        private float verticalVelocity;     // for jumping
        private float timer;                // general timer to see how long animations have been playing
        private string charaMeshName;
        private bool controlled;
        private NavmeshQuery query;
        private Physics physics;
        private ControllerManager cotrollerManager;
        private Scene physicsScene;
        private Mogre.Vector3 direction;
        private ModCharacterSkinDfnXML skin;
        private CapsuleController physicsController;
        private Mogre.Vector3 lastPosition;
		private GameWorld world;

        public SceneNode BodyNode
        {
            get { 
                return bodyNode; 
            }
        }
        public Mogre.Vector3 Position
        {
            get
            {
                return bodyNode.Position;
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

        List<Entity> itemAttached;//Item that attached to character

		public CharacterController(
			GameWorld world,
			ModCharacterDfnXML chaData,
			ModCharacterSkinDfnXML chaSkin,
			Mogre.Vector3 initPosition,
			bool isBot = true)
		{
			this.world = world;
			camera = world.Camera;
			controlled = !isBot;
			itemAttached = new List<Entity>();
			sceneMgr = world.SceneManager;
			charaMeshName = chaData.MeshName;
			physicsScene = world.PhysicsScene;
			physics = physicsScene.Physics;
			cotrollerManager = physics.ControllerManager;
			query = query;
			setupBody(initPosition);
			if (controlled)
			{
				setupCamera(camera);
			}

			this.skin = chaSkin;

			setupAnimations();
			setupPhysics();
		}

		/// <summary>
		/// Create character body
		/// </summary>
		/// <returns>success return True, fail return False</returns>
		private bool setupBody(Mogre.Vector3 initPosition)
        {
            try
            {
                if (!string.IsNullOrEmpty(charaMeshName))
                {
                    bodyNode = sceneMgr.RootSceneNode.CreateChildSceneNode(Mogre.Vector3.UNIT_Y * CHAR_HEIGHT * initPosition.y);
                    bodyEnt = sceneMgr.CreateEntity(Guid.NewGuid().ToString(), charaMeshName);
                    bodyNode.AttachObject(bodyEnt);
                    bodyNode.SetPosition(initPosition.x, bodyNode.Position.y, initPosition.z);
                    keyDirection = Mogre.Vector3.ZERO;
                    verticalVelocity = 0;
                    lastPosition = initPosition;
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
        /// Set Animations for Character
        /// </summary>
        private void setupAnimations()
        {
            bodyEnt.Skeleton.BlendMode = SkeletonAnimationBlendMode.ANIMBLEND_CUMULATIVE;

            // populate our animation list
            for (int i = 0; i < skin.SkinAnimations.Count; i++)
            {
                var skinAnim = skin.SkinAnimations[i];

				var anim = world.ModData.AnimationInfos.Where(o => o.ID == skinAnim.AnimID).FirstOrDefault();
				if (anim == null)
				{
					continue;
				}

				AnimationState animState = null;
                if (!string.IsNullOrEmpty(skinAnim.AnimID))
                {
                    animState = bodyEnt.GetAnimationState(skinAnim.AnimID);
                    animState.Loop = true;
                }
                CharacterAnimation chaAnim = new CharacterAnimation(skinAnim.AnimID, animState, skinAnim.Type);
                anims.Add(chaAnim);
                bool fadingIn = false;
                this.fadingIn.Add(fadingIn);
                bool fadingOut = false;
                this.fadingOut.Add(fadingOut);
            }

            // start off in the idle state (top and bottom together)
            SetBaseAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_IDLE_BASE).First());
            SetTopAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_IDLE_TOP).First());

            // relax the hands since we're not holding anything
            anims.Where(o => o.Type == ChaAnimType.CAT_HANDS_RELAXED).First().AnimationState.Enabled = true;

            //swordsDrawn = false;
        }

        /// <summary>
        /// Set the camera
        /// </summary>
        /// <param name="cam">Camera Instance</param>
        public void setupCamera(Camera cam)
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

        public Camera removeCamera()
        {
            Camera cam = (Camera)cameraNode.GetAttachedObjectIterator().ElementAt(0);
            cameraNode.DetachAllObjects();
            sceneMgr.DestroySceneNode(cameraNode);
            sceneMgr.DestroySceneNode(cameraGoal);
            sceneMgr.DestroySceneNode(cameraPivot);
            cameraNode = null;
            cameraGoal = null;
            cameraPivot = null;
            return cam;
        }

        private void setupPhysics()
        {
            CapsuleControllerDesc desc = new CapsuleControllerDesc();
            desc.Position = bodyNode.Position;
            desc.StepOffset = 0.01f;
            desc.SlopeLimit = 0.5f; // max slope the character can walk
            desc.Radius = 2.5f; //radius of the capsule
            desc.Height = CHAR_HEIGHT; //height of the controller
            desc.ClimbingMode = CapsuleClimbingModes.Easy;
            desc.UpDirection = HeightFieldAxes.Y; // Specifies the 'up' direction
            desc.Position = bodyNode.Position;
            physicsController = cotrollerManager.CreateController(physicsScene, desc);
            physicsController.Actor.Group = 3;
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

            if (!controlled)
            {
                //bodyNode.Position = physicsActor.GlobalPosition;
                //bodyNode.Orientation = physicsActor.GlobalOrientationQuaternion;
            }
        }

        public void injectKeyDown(KeyEvent evt)
        {
            // keep track of the player's intended direction
            if (evt.key == KeyCode.KC_W) keyDirection.z = -1;
            else if (evt.key == KeyCode.KC_A) keyDirection.x = -1;
            else if (evt.key == KeyCode.KC_S) keyDirection.z = 1;
            else if (evt.key == KeyCode.KC_D) keyDirection.x = 1;

            else if (evt.key == KeyCode.KC_SPACE && (topAnim.Type == ChaAnimType.CAT_IDLE_TOP || topAnim.Type == ChaAnimType.CAT_RUN_TOP))
            {
                // jump if on ground
                SetBaseAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_JUMP_START).First(), true);
                SetTopAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_NONE).First());
                timer = 0;
            }

            if (!keyDirection.IsZeroLength && baseAnim.Type == ChaAnimType.CAT_IDLE_BASE)
            {
                // start running if not already moving and the player wants to move
                SetBaseAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_RUN_BASE).First(), true);
                if (topAnim.Type == ChaAnimType.CAT_IDLE_TOP)
                {
                    SetTopAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_RUN_TOP).First(), true);
                }
            }
        }

        public void injectKeyUp(KeyEvent evt)
        {
            if (evt.key == KeyCode.KC_W && keyDirection.z == -1) keyDirection.z = 0;
            else if (evt.key == KeyCode.KC_A && keyDirection.x == -1) keyDirection.x = 0;
            else if (evt.key == KeyCode.KC_S && keyDirection.z == 1) keyDirection.z = 0;
            else if (evt.key == KeyCode.KC_D && keyDirection.x == 1) keyDirection.x = 0;

            if (keyDirection.IsZeroLength && baseAnim.Type == ChaAnimType.CAT_RUN_BASE)
            {
                // stop running if already moving and the player doesn't want to move
                SetBaseAnimation(anims.Where(o=>o.Type== ChaAnimType.CAT_IDLE_BASE).First());
                if (topAnim.Type == ChaAnimType.CAT_RUN_TOP)
                {
                    SetTopAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_IDLE_TOP).First());
                }
            }
        }

        public void injectMouseMove(MouseEvent evt)
        {
            if (controlled)
            {
                updateCameraGoal(-0.05f * evt.state.X.rel, -0.05f * evt.state.Y.rel, -0.0005f * evt.state.Z.rel);
            }
        }

        public void injectMouseReleased(object evt, MouseButtonID id)
        {
            throw new NotImplementedException();
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
            //if (swordsDrawn && (topAnimID == AnimID.ANIM_IDLE_TOP || topAnimID == AnimID.ANIM_RUN_TOP))
            //{
            //    // if swords are out, and character's not doing something weird, then SLICE!
            //    if (id == MouseButtonID.MB_Left) SetTopAnimation(AnimID.ANIM_SLICE_VERTICAL, true);
            //    else if (id == MouseButtonID.MB_Right) SetTopAnimation(AnimID.ANIM_SLICE_HORIZONTAL, true);
            //    timer = 0;
            //}
        }

        private void updateBody(float deltaTime)
        {
            goalDirection = Mogre.Vector3.ZERO;   // we will calculate this

            if (keyDirection != Mogre.Vector3.ZERO && baseAnim.Type != ChaAnimType.CAT_CUSTOME_BLOCK)
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
                if (baseAnim.Type == ChaAnimType.CAT_JUMP_LOOP) yawAtSpeed *= 0.2f;

                // turn as much as we can, but not more than we need to
                if (yawToGoal < 0) yawToGoal = System.Math.Min(0, System.Math.Max(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, yawAtSpeed, 0);
                else if (yawToGoal > 0) yawToGoal = System.Math.Max(0, System.Math.Min(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, 0, yawAtSpeed);

                bodyNode.Yaw(new Degree(yawToGoal));

                // move in current body direction (not the goal direction)
                var dist = deltaTime * RUN_SPEED/* * baseAnim.AnimationState.Weight*/;
                Mogre.Vector3 displacement = new Mogre.Vector3(0, 0, dist);
                bodyNode.Translate(displacement.x, displacement.y, displacement.z,
                    Node.TransformSpace.TS_LOCAL);

                /*Get world movement*/
                displacement = bodyNode.Orientation * displacement;

                ControllerFlags flag;
                physicsController.Move(displacement, 0, 0.01f, out flag);
                bodyNode.Position = new Mogre.Vector3(
                    physicsController.Actor.GlobalPosition.x,
                    bodyNode.Position.y, 
                    physicsController.Actor.GlobalPosition.z);
            }

            if (baseAnim.Type == ChaAnimType.CAT_JUMP_LOOP)
            {
                // if we're jumping, add a vertical offset too, and apply gravity
                bodyNode.Translate(0, verticalVelocity * deltaTime, 0, Node.TransformSpace.TS_LOCAL);
                verticalVelocity -= GRAVITY * deltaTime;

                Mogre.Vector3 pos = bodyNode.Position;
                if (pos.y <= CHAR_HEIGHT * lastPosition.y)
                {
                    // if we've hit the ground, change to landing state
                    pos.y = CHAR_HEIGHT * lastPosition.y;
                    bodyNode.Position = pos;
                    ControllerFlags flag;
                    physicsController.Move(pos, 1 << 3, 0.01f, out flag);
                    bodyNode.Position = physicsController.Actor.GlobalPosition;
                    SetBaseAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_JUMP_END).First(), true);
                    timer = 0;
                }
            }
        }

        private float convertToLessThan360(float p)
        {
            return p % 360;
        }

        private void updateAnimations(float deltaTime)
        {
            float baseAnimSpeed = 1;
            //float topAnimSpeed = 1;

            timer += deltaTime;
            if (baseAnim.Type == ChaAnimType.CAT_JUMP_START)
            {
                if (timer >= baseAnim.AnimationState.Length)
                {
                    // takeoff animation finished, so time to leave the ground!
                    SetBaseAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_JUMP_LOOP).First(), true);
                    // apply a jump acceleration to the character
                    verticalVelocity = JUMP_ACCEL;
                }
            }
            else if (baseAnim.Type == ChaAnimType.CAT_JUMP_END)
            {
                if (timer >= baseAnim.AnimationState.Length)
                {
                    // safely landed, so go back to running or idling
                    if (keyDirection == Mogre.Vector3.ZERO)
                    {
                        SetBaseAnimation(anims.Where(o=>o.Type== ChaAnimType.CAT_IDLE_BASE).First());
                        SetTopAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_IDLE_TOP).First());
                    }
                    else
                    {
                        SetBaseAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_RUN_BASE).First(), true);
                        SetTopAnimation(anims.Where(o => o.Type == ChaAnimType.CAT_RUN_TOP).First(), true);
                    }
                }
            }

            // increment the current base and top animation times
            if (baseAnim.Type != ChaAnimType.CAT_NONE)
            {
                baseAnim.Update(deltaTime * baseAnimSpeed);
            }
            if (topAnim.Type != ChaAnimType.CAT_NONE)
            {
                topAnim.Update(deltaTime * baseAnimSpeed);
            }

            // apply smooth transitioning between our animations
            fadeAnimations(deltaTime);
        }

        private void fadeAnimations(float deltaTime)
        {
            for (int i = 0; i < anims.Count; i++)
            {
                if (fadingIn[i])
                {
                    // slowly fade this animation in until it has full weight
                    float newWeight = anims[i].AnimationState.Weight + deltaTime * ANIM_FADE_SPEED;
                    anims[i].AnimationState.Weight = Utilities.Helper.Clamp(newWeight, 0, 1);
                    if (newWeight >= 1) fadingIn[i] = false;
                }
                else if (fadingOut[i])
                {
                    if (anims[i].AnimationState == null)
                    {
                        continue;
                    }
                    // slowly fade this animation out until it has no weight, and then disable it
                    float newWeight = anims[i].AnimationState.Weight - deltaTime * ANIM_FADE_SPEED;
                    anims[i].AnimationState.Weight = Utilities.Helper.Clamp(newWeight, 0, 1);
                    if (newWeight <= 0)
                    {
                        anims[i].AnimationState.Enabled = false;
                        fadingOut[i] = false;
                    }
                }
            }
        }

        public void updateCamera(float deltaTime)
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

        public void SetBaseAnimation(CharacterAnimation animation, bool reset = false)
        {
            int index = anims.IndexOf(animation);
            int lastIndex = anims.IndexOf(baseAnim);
            if (lastIndex != -1)
            {
                // if we have an old animation, fade it out
                fadingIn[lastIndex] = false;
                fadingOut[lastIndex] = true;
            }

            baseAnim = animation;

            if (animation.Type != ChaAnimType.CAT_NONE)
            {
                // if we have a new animation, enable it and fade it in
                anims[index].AnimationState.Enabled = true;
                anims[index].AnimationState.Weight = 0;
                fadingOut[index] = false;
                fadingIn[index] = true;
                if (reset) anims[index].AnimationState.TimePosition = 0;
            }
        }

        public void SetTopAnimation(CharacterAnimation animation, bool reset = false)
        {
            int index = anims.IndexOf(animation);
            int lastIndex = anims.IndexOf(topAnim);
            if (lastIndex != -1)
            {
                // if we have an old animation, fade it out
                fadingIn[lastIndex] = false;
                fadingOut[lastIndex] = true;
            }

            topAnim = animation;

            if (animation.Type != ChaAnimType.CAT_NONE)
            {
                // if we have a new animation, enable it and fade it in
                anims[index].AnimationState.Enabled = true;
                anims[index].AnimationState.Weight = 0;
                fadingOut[index] = false;
                fadingIn[index] = true;
                if (reset) anims[index].AnimationState.TimePosition = 0;
            }
        }
        public bool GetControlled()
        {
            return controlled;
        }

        public void AttachItem(ItemUseAttachOption itemAttachOption, Item item)
        {

        }

        public CharacterAnimation GetAnimationByName(string animName)
        {
            return anims.Where(o => o.Name == animName).First();
        }

        public string GetAnimationNameByType(ChaAnimType characterAnimationType)
        {
            var findAnims = anims.Where(o=>o.Type == characterAnimationType);
            if(findAnims.Count() > 0)
            {
                return findAnims.ElementAt(0).AnimationState.AnimationName;
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
            bodyNode.DetachAllObjects();
            sceneMgr.RootSceneNode.RemoveChild(bodyNode);
            if(controlled)
            {
                removeCamera();
            }
        }
    }
}
