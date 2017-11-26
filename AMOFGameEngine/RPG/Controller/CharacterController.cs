using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
namespace AMOFGameEngine.RPG.Controller
{
public class CharacterController : ControllerBase
{       
    public const int CHAR_HEIGHT = 5;      
    public const int CAM_HEIGHT = 2;          
    public const int RUN_SPEED = 17;           
    public const float TURN_SPEED = 500.0f;      
    public const float ANIM_FADE_SPEED = 7.5f;   
    public const float JUMP_ACCEL = 30.0f;       
    public const float GRAVITY = 90.0f;

    private Mogre.Vector3 position;
    private SceneNode cameraPivot;
    private SceneNode cameraGoal;
    private SceneNode cameraNode;
    private float pivotPitch;
    private AnimID baseAnimID;                   // current base (full- or lower-body) animation
    private AnimID topAnimID;                    // current top (upper-body) animation
    private bool[] fadingIn;
    private bool[] fadingOut;
    private bool swordsDrawn;
    private Mogre.Vector3 keyDirection;      // player's local intended direction based on WASD keys
    private Mogre.Vector3 goalDirection;     // actual intended direction in world-space
    private float verticalVelocity;     // for jumping
    private float timer;                // general timer to see how long animations have been playing
    private enum AnimID
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

    public CharacterController(string name, string meshName, Camera cam, bool isInManualMode = false)
        : base(name, meshName, cam, isInManualMode)
    {
    }

    public override bool ControllerSetup()
    {
        setupBody();
        setupAnimations();
        setupCamera(objectCam);
        return true;
    }

    public override bool InjectKeyPressed(KeyEvent evt)
    { 
        if (evt.key ==  KeyCode.KC_Q && (topAnimID == AnimID. ANIM_IDLE_TOP || topAnimID == AnimID.ANIM_RUN_TOP))
	    {
		// take swords out (or put them back, since it's the same animation but reversed)
		    setTopAnimation(AnimID.ANIM_DRAW_SWORDS, true);
		    timer = 0;
	    }
	    else if (evt.key == KeyCode.KC_E && !swordsDrawn)
	    {
		    if (topAnimID == AnimID.ANIM_IDLE_TOP || topAnimID == AnimID.ANIM_RUN_TOP)
		    {
			    // start dancing
			    setBaseAnimation(AnimID.ANIM_DANCE, true);
			    setTopAnimation(AnimID.ANIM_NONE);
			    // disable hand animation because the dance controls hands
			    objectAnims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled=false;
		    }
		    else if (baseAnimID == AnimID.ANIM_DANCE)
		    {
			    // stop dancing
			    setBaseAnimation(AnimID.ANIM_IDLE_BASE);
			    setTopAnimation(AnimID.ANIM_IDLE_TOP);
			    // re-enable hand animation
			    objectAnims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled=true;
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
            setBaseAnimation(AnimID.ANIM_JUMP_START, true);
            setTopAnimation(AnimID.ANIM_NONE);
		    timer = 0;
	    }

        if (!keyDirection.IsZeroLength && baseAnimID == AnimID.ANIM_IDLE_BASE)
	    {
		    // start running if not already moving and the player wants to move
            setBaseAnimation(AnimID.ANIM_RUN_BASE, true);
            if (topAnimID == AnimID.ANIM_IDLE_TOP) setTopAnimation(AnimID.ANIM_RUN_TOP, true);
	     }

        return true;
    }

    public override bool InjectKeyReleased(KeyEvent evt)
    {
        if (evt.key == KeyCode.KC_W && keyDirection.z == -1) keyDirection.z = 0;
	    else if (evt.key == KeyCode.KC_A && keyDirection.x == -1) keyDirection.x = 0;
	    else if (evt.key == KeyCode.KC_S && keyDirection.z == 1) keyDirection.z = 0;
        else if (evt.key == KeyCode.KC_D && keyDirection.x == 1) keyDirection.x = 0;

        if (keyDirection.IsZeroLength && baseAnimID == AnimID.ANIM_RUN_BASE)
	    {
		    // stop running if already moving and the player doesn't want to move
            setBaseAnimation(AnimID.ANIM_IDLE_BASE);
            if (topAnimID == AnimID.ANIM_RUN_TOP) setTopAnimation(AnimID.ANIM_IDLE_TOP);
	    }

        return true;
    }

    public override bool InjectMouseMoved(MouseEvent evt)
    {
        updateCameraGoal(-0.05f * evt.state.X.rel, -0.05f * evt.state.Y.rel, -0.0005f * evt.state.Z.rel);
        return true;
    }

    public override bool InjectMousePressed(MouseEvent evt, MouseButtonID id)
    {
        if (swordsDrawn && (topAnimID == AnimID.ANIM_IDLE_TOP || topAnimID == AnimID.ANIM_RUN_TOP))
	    {
		    // if swords are out, and character's not doing something weird, then SLICE!
		    if (id == MouseButtonID.MB_Left) setTopAnimation(AnimID.ANIM_SLICE_VERTICAL, true);
            else if (id == MouseButtonID.MB_Right) setTopAnimation(AnimID.ANIM_SLICE_HORIZONTAL, true);
		    timer = 0;
	    }
        return true;
    }

    public override bool InjectMouseReleased(MouseEvent arg, MouseButtonID id)
    {
        return true;
    }

    public override bool ControllerUpdateAnimations(float deltaTime)
    {
        float baseAnimSpeed = 1;
        float topAnimSpeed = 1;

        timer += deltaTime;

        if (topAnimID == AnimID.ANIM_DRAW_SWORDS)
        {
            // flip the draw swords animation if we need to put it back
            topAnimSpeed = swordsDrawn ? -1 : 1;

            // half-way through the animation is when the hand grasps the handles...
            if (timer >= objectAnims[(int)topAnimID].Length / 2 &&
                timer - deltaTime < objectAnims[(int)topAnimID].Length / 2)
            {
                // so transfer the swords from the sheaths to the hands
                objectEntity.DetachAllObjectsFromBone();;
                // change the hand state to grab or let go
                objectAnims[(int)AnimID.ANIM_HANDS_CLOSED].Enabled = !swordsDrawn;
                objectAnims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled = swordsDrawn;

                // toggle sword trails
                if (swordsDrawn)
                {
                }
                else
                {
                }
            }

            if (timer >= objectAnims[(int)topAnimID].Length)
            {
                // animation is finished, so return to what we were doing before
                if (baseAnimID == AnimID.ANIM_IDLE_BASE) setTopAnimation(AnimID.ANIM_IDLE_TOP);
                else
                {
                    setTopAnimation(AnimID.ANIM_RUN_TOP);
                    objectAnims[(int)AnimID.ANIM_RUN_TOP].TimePosition = objectAnims[(int)AnimID.ANIM_RUN_BASE].TimePosition;
                }
                swordsDrawn = !swordsDrawn;
            }
        }
        else if (topAnimID == AnimID.ANIM_SLICE_VERTICAL || topAnimID == AnimID.ANIM_SLICE_HORIZONTAL)
        {
            if (timer >= objectAnims[(int)topAnimID].Length)
            {
                // animation is finished, so return to what we were doing before
                if (baseAnimID == AnimID.ANIM_IDLE_BASE) setTopAnimation(AnimID.ANIM_IDLE_TOP);
                else
                {
                    setTopAnimation(AnimID.ANIM_RUN_TOP);
                    objectAnims[(int)AnimID.ANIM_RUN_TOP].TimePosition = objectAnims[(int)AnimID.ANIM_RUN_BASE].TimePosition;
                }
            }

            // don't sway hips from side to side when slicing. that's just embarrasing.
            if (baseAnimID == AnimID.ANIM_IDLE_BASE) baseAnimSpeed = 0;
        }
        else if (baseAnimID == AnimID.ANIM_JUMP_START)
        {
            if (timer >= objectAnims[(int)baseAnimID].Length)
            {
                // takeoff animation finished, so time to leave the ground!
                setBaseAnimation(AnimID.ANIM_JUMP_LOOP, true);
                // apply a jump acceleration to the character
                verticalVelocity = JUMP_ACCEL;
            }
        }
        else if (baseAnimID == AnimID.ANIM_JUMP_END)
        {
            if (timer >= objectAnims[(int)baseAnimID].Length)
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
        if (baseAnimID != AnimID.ANIM_NONE) objectAnims[(int)baseAnimID].AddTime(deltaTime * baseAnimSpeed);
        if (topAnimID != AnimID.ANIM_NONE) objectAnims[(int)topAnimID].AddTime(deltaTime * topAnimSpeed);

        // apply smooth transitioning between our animations
        fadeAnimations(deltaTime);

        return true;
    }

    public override bool ControllerUpdateCamera(float deltaTime)
    {
        // place the camera pivot roughly at the character's shoulder
        cameraPivot.Position = objectSceneNode.Position + Mogre.Vector3.UNIT_Y * CAM_HEIGHT;
        // move the camera smoothly to the goal
        Mogre.Vector3 goalOffset = cameraGoal._getDerivedPosition() - cameraNode.Position;
        cameraNode.Translate(goalOffset * deltaTime * 9.0f);
        // always look at the pivot
        cameraNode.LookAt(cameraPivot._getDerivedPosition(), Node.TransformSpace.TS_WORLD);

        return true;
    }

    public override bool ControllerUpdateBody(float deltaTime)
    {
        goalDirection = Mogre.Vector3.ZERO;   // we will calculate this

        if (keyDirection != Mogre.Vector3.ZERO && baseAnimID != AnimID.ANIM_DANCE)
        {
            // calculate actually goal direction in world based on player's key directions
            goalDirection += keyDirection.z * cameraNode.Orientation.ZAxis;
            goalDirection += keyDirection.x * cameraNode.Orientation.XAxis;
            goalDirection.y = 0;
            goalDirection.Normalise();

            Quaternion toGoal = objectSceneNode.Orientation.ZAxis.GetRotationTo(goalDirection);

            // calculate how much the character has to turn to face goal direction
            float yawToGoal = toGoal.Yaw.ValueDegrees;
            // this is how much the character CAN turn this frame
            float yawAtSpeed = yawToGoal / Mogre.Math.Abs(yawToGoal) * deltaTime * TURN_SPEED;
            // reduce "turnability" if we're in midair
            if (baseAnimID == AnimID.ANIM_JUMP_LOOP) yawAtSpeed *= 0.2f;

            // turn as much as we can, but not more than we need to
            if (yawToGoal < 0) yawToGoal = System.Math.Min(0, System.Math.Max(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, yawAtSpeed, 0);
            else if (yawToGoal > 0) yawToGoal = System.Math.Max(0, System.Math.Min(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, 0, yawAtSpeed);

            objectSceneNode.Yaw(new Degree(yawToGoal));

            // move in current body direction (not the goal direction)
            objectSceneNode.Translate(0, 0, deltaTime * RUN_SPEED * objectAnims[(int)baseAnimID].Weight,
                Node.TransformSpace.TS_LOCAL);

            position = objectSceneNode._getDerivedPosition();
        }

        if (baseAnimID == AnimID.ANIM_JUMP_LOOP)
        {
            // if we're jumping, add a vertical offset too, and apply gravity
            objectSceneNode.Translate(0, verticalVelocity * deltaTime, 0, Node.TransformSpace.TS_LOCAL);
            verticalVelocity -= GRAVITY * deltaTime;

            Mogre.Vector3 pos = objectSceneNode.Position;
            if (pos.y <= CHAR_HEIGHT)
            {
                // if we've hit the ground, change to landing state
                pos.y = CHAR_HEIGHT;
                objectSceneNode.Position = pos;
                setBaseAnimation(AnimID.ANIM_JUMP_END, true);
                timer = 0;
            }
        }

        return true;
    }

    public void Move(Mogre.Vector3 destPosition)
    {
        ///Animation
        setTopAnimation(AnimID.ANIM_RUN_TOP, true);

        ///Turn around
        Mogre.Vector3 vector = destPosition - objectSceneNode.Position;
        Mogre.Vector3 faceTo = objectSceneNode.Orientation * objectSceneNode.Position;

        float angleCos = faceTo.Normalise() * vector.Normalise();
        Radian r = Mogre.Math.ACos(angleCos);
        objectSceneNode.Rotate(Mogre.Vector3.UNIT_Y, r);

        ///Move
        keyDirection.z += 1;
    }

    private void setupBody()
    {
	    keyDirection = Mogre.Vector3.ZERO;
	    verticalVelocity = 0;
    }

    private void setupAnimations()
    {
        fadingIn = new bool[objectAnimNum];
        fadingOut = new bool[objectAnimNum];

        objectEntity.Skeleton.BlendMode=SkeletonAnimationBlendMode.ANIMBLEND_CUMULATIVE;

	    var animNames =new String[]
	    {"IdleBase", "IdleTop", "RunBase", "RunTop", "HandsClosed", "HandsRelaxed", "DrawSwords",
	    "SliceVertical", "SliceHorizontal", "Dance", "JumpStart", "JumpLoop", "JumpEnd"};

	    // populate our animation list
	    for (int i = 0; i < objectAnimNum; i++)
	    {
            objectAnims[i] = objectEntity.GetAnimationState(animNames[i]);
		    fadingIn[i] = false;
		    fadingOut[i] = false;
	    }

	    // start off in the idle state (top and bottom together)
        setBaseAnimation(AnimID.ANIM_IDLE_BASE);
        setTopAnimation(AnimID.ANIM_IDLE_TOP);

	    // relax the hands since we're not holding anything
        objectAnims[(int)AnimID.ANIM_HANDS_RELAXED].Enabled=(true);

	    swordsDrawn = false;
    }

    private void setupCamera(Camera cam)
    {
        cameraPivot = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
	    // this is where the camera should be soon, and it spins around the pivot
	    cameraGoal = cameraPivot.CreateChildSceneNode(new Mogre.Vector3(0, 0, 15));
	    // this is where the camera actually is
	    cameraNode = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
	    cameraNode.Position=cameraPivot.Position + cameraGoal.Position;
    
	    cameraPivot.SetFixedYawAxis(true);
	    cameraGoal.SetFixedYawAxis(true);
	    cameraNode.SetFixedYawAxis(true);

	    // our model is quite small, so reduce the clipping planes
	    cam.NearClipDistance=0.1f;
	    cam.FarClipDistance=100;
	    cameraNode.AttachObject(cam);

	    pivotPitch = 0;
    }

    private void fadeAnimations(float deltaTime)
    {
        for (int i = 0; i < objectAnimNum; i++)
	    {
		    if (fadingIn[i])
		    {
			    // slowly fade this animation in until it has full weight
			    float newWeight = objectAnims[i].Weight + deltaTime * ANIM_FADE_SPEED;
			    objectAnims[i].Weight=AMOFGameEngine.Utilities.Helper. Clamp(newWeight, 0, 1);
			    if (newWeight >= 1) fadingIn[i] = false;
		    }
		    else if (fadingOut[i])
		    {
			    // slowly fade this animation out until it has no weight, and then disable it
			    float newWeight = objectAnims[i].Weight - deltaTime * ANIM_FADE_SPEED;
                objectAnims[i].Weight = AMOFGameEngine.Utilities.Helper.Clamp(newWeight, 0, 1);
			    if (newWeight <= 0)
			    {
				    objectAnims[i].Enabled=false;
				    fadingOut[i] = false;
			    }
		    }
	    }
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

    private void setBaseAnimation(AnimID id, bool reset = false)
    {
        if (baseAnimID >= 0 && (int)baseAnimID < objectAnimNum)
        {
            // if we have an old animation, fade it out
            fadingIn[(int)baseAnimID] = false;
            fadingOut[(int)baseAnimID] = true;
        }

        baseAnimID = id;

        if (id != AnimID.ANIM_NONE)
        {
            // if we have a new animation, enable it and fade it in
            objectAnims[(int)id].Enabled=(true);
            objectAnims[(int)id].Weight=0;
            fadingOut[(int)id] = false;
            fadingIn[(int)id] = true;
            if (reset) objectAnims[(int)id].TimePosition=0;
        }
    }

    private void setTopAnimation(AnimID id, bool reset = false)
    {
        if (topAnimID >= 0 && (int)topAnimID < objectAnimNum)
        {
            // if we have an old animation, fade it out
            fadingIn[(int)topAnimID] = false;
            fadingOut[(int)topAnimID] = true;
        }

        topAnimID = id;

        if (id != AnimID.ANIM_NONE)
        {
            // if we have a new animation, enable it and fade it in
            objectAnims[(int)id].Enabled=true;
            objectAnims[(int)id].Weight=0;
            fadingOut[(int)id] = false;
            fadingIn[(int)id] = true;
            if (reset) objectAnims[(int)id].TimePosition=0;
        }
    }
}
}
