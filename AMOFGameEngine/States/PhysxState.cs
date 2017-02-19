using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;
using MOIS;
using Vector3 = Mogre.Vector3;
using Mogre_Procedural.MogreBites.Addons;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Sound;
using AMOFGameEngine.Utilities;

namespace AMOFGameEngine
{
    class PhysxState : AppState, IUserContactReport
    {
        public bool setupPhysx()
        {
            physx = Physics.Create();
            physx.Parameters.SkinWidth = 0.0025f;
            SceneDesc scenedesc = new SceneDesc();
            scenedesc.SetToDefault();
            scenedesc.Gravity = new Mogre.Vector3(0, -9.8f, 0);
            scenedesc.UpAxis = 1;

            scenedesc.UserContactReport = this;

            this.scene = physx.CreateScene(scenedesc);
            this.scene.Materials[0].Restitution = 0.5f;
            this.scene.Materials[0].StaticFriction = 0.5f;
            this.scene.Materials[0].DynamicFriction = 0.5f;

            this.scene.Simulate(0);
            physx.RemoteDebugger.Connect("localhost", 5425);
            return true;
        }
        public void setupContent()
        {
            m_SceneMgr = GameManager.Singleton.mRoot.CreateSceneManager(SceneType.ST_GENERIC, "PhysxSceneMgr");
            m_Camera = m_SceneMgr.CreateCamera("PhysxCam");
            //AdvancedMogreFramework.Singleton.m_Viewport = m_SceneMgr.CurrentViewport;
            //AdvancedMogreFramework.Singleton.m_Viewport.Camera = m_Camera;
            ColourValue cvBg = new ColourValue(16.0f / 255.0f, 16.0f / 255.0f, 16.0f / 255.0f);
            GameManager.Singleton.mViewport.BackgroundColour = cvBg;
            m_SceneMgr.SetFog(FogMode.FOG_EXP, cvBg, 0.001f, 800f, 1000f);
            m_SceneMgr.ShadowTechnique=ShadowTechnique.SHADOWTYPE_TEXTURE_MODULATIVE;
            m_SceneMgr.ShadowColour = new ColourValue(0.5f,05f,0.5f);
            m_SceneMgr.SetShadowTextureSize(1024);
            m_SceneMgr.ShadowTextureCount = 1;

            MeshManager.Singleton.CreatePlane("floor", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, 
                new Plane(Vector3.UNIT_Y, 0), 1000, 1000, 1, 1, true, 1, 1, 1, Vector3.UNIT_Z);
            Entity floor=m_SceneMgr.CreateEntity("Floor","floor");
            floor.SetMaterialName("ground-from-nxogre.org");
            floor.CastShadows = false;
            m_SceneMgr.RootSceneNode.AttachObject(floor);

            m_SceneMgr.AmbientLight = new ColourValue(0.3f,0.3f,0.3f);
            Light l = m_SceneMgr.CreateLight();
            l.Type = Light.LightTypes.LT_POINT;
            l.Position = new Vector3(-10,40,20);

            m_Camera.Position = new Vector3(10,10,10);
            m_Camera.LookAt(0,0,0);
            m_Camera.NearClipDistance = 0.02f;
            m_Camera.FarClipDistance = 1000f;

            m_Camera.AspectRatio = GameManager.Singleton.mViewport.ActualWidth / GameManager.Singleton.mViewport.ActualHeight;

            GameManager.Singleton.mViewport.Camera = m_Camera;

        }
        public void makeCloth()
        {
            using (var cd = new ClothDesc())
            {
                MeshManager.Singleton.CreatePlane("flag",
                    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, new Plane(Vector3.UNIT_Y, 0), 8, 1, 1, 1, true, 1, 1, 1, Vector3.UNIT_Z);
                Entity entFlag = m_SceneMgr.CreateEntity("Flag", "flag");
                MeshPtr mp = entFlag.GetMesh();
                StaticMeshData meshdata = new StaticMeshData(mp);
                cd.ClothMesh = CookClothMesh(meshdata);

                cd.BendingStiffness = cd.StretchingStiffness = 0.1f;
                cd.GlobalPose = Matrix4.GetTrans(new Vector3(0, 0, 0));
                cd.Flags |= ClothFlags.Gravity | ClothFlags.Visualization;

                c = scene.CreateCloth(cd);
            }
        }
        private ClothMesh CookClothMesh(StaticMeshData meshdata)
        {
            CookingInterface.InitCooking();
            using (var cd = new ClothMeshDesc())
            {
                cd.PinPoints<float>(meshdata.Points, 0, 12);
                cd.PinTriangles<uint>(meshdata.Indices, 0, 12);

                cd.VertexCount = 4;
                cd.TriangleCount = 2;
                ClothMesh cookedcm = null;
                using (System.IO.MemoryStream str = new System.IO.MemoryStream(1024))
                {
                    if (CookingInterface.CookClothMesh(cd, str))
                    {
                        str.Seek(0, System.IO.SeekOrigin.Begin);
                        cookedcm = physx.CreateClothMesh(str);
                        CookingInterface.CloseCooking();
                    }
                    return cookedcm;
                }
            }
        }
        public override void enter()
        {
            GameManager.Singleton.mLog.LogMessage("Entering PhysxState...");
            GameManager.LastStateName = "PhysxState";

            setupPhysx();

            setupContent();

            makeCloth();
 /*           ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            m_SceneMgr.AmbientLight = cvAmbineLight;//(Ogre::ColourValue(0.7f, 0.7f, 0.7f));

            m_Camera = m_SceneMgr.CreateCamera("PhysxCamera");
            Mogre.Vector3 vectCameraPostion = new Mogre.Vector3(5, 60, 60);
            m_Camera.Position = vectCameraPostion;
            Mogre.Vector3 vectorCameraLookAt = new Mogre.Vector3(5, 20, 0);
            m_Camera.LookAt(vectorCameraLookAt);
            m_Camera.NearClipDistance = 5;

            m_Camera.AspectRatio = AdvancedMogreFramework.Singleton.m_Viewport.ActualWidth / AdvancedMogreFramework.Singleton.m_Viewport.ActualHeight;

            AdvancedMogreFramework.Singleton.m_Viewport.Camera = m_Camera;*/

            float scale = 0.1f;
            int id = 0;
            Entity entity = m_SceneMgr.CreateEntity("ogrehead"+id.ToString(),"ogrehead.mesh");
            SceneNode scenenode = m_SceneMgr.RootSceneNode.CreateChildSceneNode();
            scenenode.AttachObject(entity);
            scenenode.Position = new Vector3(0,10,0);
            scenenode.Scale (Vector3.UNIT_SCALE * scale);

            BodyDesc bodydesc = new BodyDesc();
            bodydesc.LinearVelocity = new Vector3(0, 2, 5);
            bodydesc.Mass = 40.0f;

            ActorDesc actordesc = new ActorDesc();
            actordesc.Density = 4;
            actordesc.Body = bodydesc;
            actordesc.GlobalPosition = scenenode.Position;
            actordesc.GlobalOrientation = scenenode.Orientation.ToRotationMatrix();

            actordesc.Shapes.Add(new SphereShapeDesc((float)System.Math.Sqrt((double)(entity.BoundingBox.HalfSize * scale).SquaredLength), entity.BoundingBox.Center * scale));

            Actor actor = scene.CreateActor(actordesc);
       //     ActorNode actornode = new ActorNode(scenenode, actor);
       //     actornodeList.Add(actornode);

            GameManager.Singleton.mMouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            GameManager.Singleton.mMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            GameManager.Singleton.mMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            GameManager.Singleton.mKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            GameManager.Singleton.mKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);

            //AdvancedMogreFramework.Singleton.m_Root.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(frameRenderingQueued);
        }
        public override void update(double timeSinceLastFrame)
        {
            m_TranslateVector = Mogre.Vector3.ZERO;

            getInput();
            moveCamera();
            //foreach (ActorNode actornode in actornodeList)
            //    actornode.Update((float)timeSinceLastFrame);
            this.scene.FlushStream();
            this.scene.FetchResults(SimulationStatuses.AllFinished,true);
            this.scene.Simulate(1/60.0f);
        }
        public override void exit()
        {
            GameManager.Singleton.mLog.LogMessage("Leaving PhysxState...");
            if (m_SceneMgr!=null)
            {
                m_SceneMgr.DestroyCamera(m_Camera);
                GameManager.Singleton.mRoot.DestroySceneManager(m_SceneMgr);
            }
            this.physx.Dispose();
        }
        public override bool pause()
        {
            GameManager.Singleton.mLog.LogMessage("Pausing PhysxState...");

            return true;
        }
        public override void resume()
        {
            GameManager.Singleton.mLog.LogMessage("Resuming PhysxState...");
            setupPhysx();
            GameManager.Singleton.mViewport.Camera = m_Camera;
        }
        public bool keyPressed(KeyEvent keyEventRef)
        {
            if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_ESCAPE))
            {
                pushAppState(findByName("PauseState"));
                return true;
            }

            GameManager.Singleton.keyPressed(keyEventRef);
            //cameraman.injectKeyDown(keyEventRef);
            return true;
        }
        public bool keyReleased(KeyEvent keyEventRef)
        {
            //cameraman.injectKeyUp(keyEventRef);
            GameManager.Singleton.keyReleased(keyEventRef);
            return true;
        }

        public bool mouseMoved(MouseEvent evt)
        {
            if (GameManager.Singleton.mTrayMgr.injectMouseMove(evt)) return true;
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Singleton.mTrayMgr.injectMouseDown(evt, id)) return true;
            return true;
        }
        public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Singleton.mTrayMgr.injectMouseUp(evt, id)) return true;
            return true;
        }
        public bool frameRenderingQueued(FrameEvent evt) 
        {
            this.scene.FlushStream();
            this.scene.FetchResults(SimulationStatuses.AllFinished, true);
            this.scene.Simulate(evt.timeSinceLastEvent);
            return true; 
        }
        public void getInput()
        {
                if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_A))
                    m_TranslateVector.x = -10;

                if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_D))
                    m_TranslateVector.x = 10;

                if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_W))
                    m_TranslateVector.z = -10;

                if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_S))
                    m_TranslateVector.z = 10;

                if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_Q))
                    m_TranslateVector.y = -10;

                if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_E))
                    m_TranslateVector.y = 10;

                //camera Yaw
                if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_Z))
                    m_Camera.Yaw(10);

                if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_X))
                    m_Camera.Pitch(-10);

                //reset roll
                if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_C))
                    m_Camera.Roll(-(m_Camera.RealOrientation.Roll));
        }
        public void moveCamera()
        {
            if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                m_Camera.MoveRelative(m_TranslateVector);
            m_Camera.MoveRelative(m_TranslateVector / 10);
        }
        public void OnContactNotify(ContactPair contactPair, ContactPairFlags contactPairFlags)
        {
            Actor actor_0 = contactPair.ActorFirst;
            Actor actor_1 = contactPair.ActorSecond;

            if (actor_0 != null)
            {
                actor_0.UserData = contactPairFlags;
            }
            if (actor_1 != null)
            {
                actor_1.UserData = contactPairFlags;
            }
        }
        private Physics physx;
        private Scene scene;
        private List<ActorNode> actornodeList = new List<ActorNode>();
        private Cloth c;
        Vector3 m_TranslateVector;

        
    }
}
