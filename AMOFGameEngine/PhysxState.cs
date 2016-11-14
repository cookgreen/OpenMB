using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;
using MOIS;
using Vector3 = Mogre.Vector3;

namespace AMOFGameEngine
{
    class PhysxState : AppState
    {
        public bool InitPhysics()
        {
            physx = Physics.Create();
            physx.Parameters.SkinWidth = 0.0025f;
            SceneDesc scenedesc = new SceneDesc();
            scenedesc.SetToDefault();
            scenedesc.Gravity = new Mogre.Vector3(0, -9.8f, 0);
            scenedesc.UpAxis = 1;
            this.scene = physx.CreateScene(scenedesc);
            this.scene.Materials[0].Restitution = 0.5f;
            this.scene.Materials[0].StaticFriction = 0.5f;
            this.scene.Materials[0].DynamicFriction = 0.5f;

            this.scene.Simulate(0);

            return true;
        }
        public override void enter()
        {
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Entering PhysxState...");
            AdvancedMogreFramework.LastStateName = "PhysxState";
            InitPhysics();
            m_SceneMgr=AdvancedMogreFramework.Singleton.m_Root.CreateSceneManager(SceneType.ST_GENERIC, "PhysxSceneMgr");

            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            m_SceneMgr.AmbientLight = cvAmbineLight;//(Ogre::ColourValue(0.7f, 0.7f, 0.7f));

            m_Camera = m_SceneMgr.CreateCamera("GameCamera");
            Mogre.Vector3 vectCameraPostion = new Mogre.Vector3(5, 60, 60);
            m_Camera.Position = vectCameraPostion;
            Mogre.Vector3 vectorCameraLookAt = new Mogre.Vector3(5, 20, 0);
            m_Camera.LookAt(vectorCameraLookAt);
            m_Camera.NearClipDistance = 5;

            m_Camera.AspectRatio = AdvancedMogreFramework.Singleton.m_Viewport.ActualWidth / AdvancedMogreFramework.Singleton.m_Viewport.ActualHeight;

            AdvancedMogreFramework.Singleton.m_Viewport.Camera = m_Camera;

            float scale = 0.1f;
            int id = 0;
            Entity entity = m_SceneMgr.CreateEntity("ogrehead"+id.ToString(),"ogrehead.mesh");
            SceneNode scenenode = m_SceneMgr.RootSceneNode.CreateChildSceneNode();
            scenenode.AttachObject(entity);
            scenenode.Position = new Vector3(0,10,0);
            scenenode.Scale (Vector3.UNIT_SCALE * scale);

            BodyDesc bodydesc = new BodyDesc();
            bodydesc.LinearVelocity = new Vector3(0, 2, 5);

            ActorDesc actordesc = new ActorDesc();
            actordesc.Density = 4;
            actordesc.Body = bodydesc;
            actordesc.GlobalPosition = scenenode.Position;
            actordesc.GlobalOrientation = scenenode.Orientation.ToRotationMatrix();

            actordesc.Shapes.Add(new SphereShapeDesc((float)System.Math.Sqrt((double)(entity.BoundingBox.HalfSize * scale).SquaredLength), entity.BoundingBox.Center * scale));

            Actor actor = scene.CreateActor(actordesc);
            ActorNode actornode = new ActorNode(scenenode, actor);
            actornodeList.Add(actornode);

            AdvancedMogreFramework.Singleton.m_Mouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            AdvancedMogreFramework.Singleton.m_Mouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            AdvancedMogreFramework.Singleton.m_Mouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            AdvancedMogreFramework.Singleton.m_Keyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            AdvancedMogreFramework.Singleton.m_Keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);
        }
        public override void update(double timeSinceLastFrame)
        {
            foreach (ActorNode actornode in actornodeList)
                actornode.Update((float)timeSinceLastFrame);
            this.scene.FlushStream();
            this.scene.FetchResults(SimulationStatuses.AllFinished,true);
            this.scene.Simulate(timeSinceLastFrame);
        }
        public override void Dispose()
        {
            this.physx.Dispose();
        }
        public override void exit()
        {
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Leaving PhysxState...");
        }
        public override bool pause()
        {
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Pausing PhysxState...");

            return true;
        }
        public override void resume()
        {
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Resuming PhysxState...");
            InitPhysics();
            AdvancedMogreFramework.Singleton.m_Viewport.Camera = m_Camera;
        }
        public bool keyPressed(KeyEvent keyEventRef)
        {
            if (AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_ESCAPE))
            {
                pushAppState(findByName("PauseState"));
                return true;
            }

            AdvancedMogreFramework.Singleton.keyPressed(keyEventRef);

            return true;
        }
        public bool keyReleased(KeyEvent keyEventRef)
        {
            AdvancedMogreFramework.Singleton.keyReleased(keyEventRef);

            return true;
        }

        public bool mouseMoved(MouseEvent evt)
        {
            if (AdvancedMogreFramework.Singleton.m_TrayMgr.injectMouseMove(evt)) return true;
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            if (AdvancedMogreFramework.Singleton.m_TrayMgr.injectMouseDown(evt, id)) return true;
            return true;
        }
        public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            if (AdvancedMogreFramework.Singleton.m_TrayMgr.injectMouseUp(evt, id)) return true;
            return true;
        }
        private Physics physx;
        private Scene scene;
        private List<ActorNode> actornodeList = new List<ActorNode>();
    }
}
