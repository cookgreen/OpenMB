using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Mods;
using AMOFGameEngine.UI;
using AMOFGameEngine.Data;

namespace AMOFGameEngine.Mods.Sample
{
    public class SampleSceneSP : SampleScene
    {
        public event EventHandler<ModEventArgs> ModStateChangedEvent;//0-Stop;1-Run
        Mogre.Vector3 m_TranslateVector;
        float rotateAngle;
        RaySceneQuery raySceneQuery;

        public SampleSceneSP( SceneManager scm,Viewport vp,SdkTrayManager trayMgr,Mouse mouse,Keyboard keyboard)
        {
            this.scm = scm;
            this.vp = vp;
            this.mouse = mouse;
            this.keyboard = keyboard;
            this.trayMgr = trayMgr;
            m_TranslateVector = new Mogre.Vector3();
            rotateAngle = 0.1f;
        }

        public override void Enter()
        {
            buildScene();
        }

        public override void Leave()
        {
            if (ModStateChangedEvent != null)
            {
                ModStateChangedEvent(this, new ModEventArgs() { modState = ModState.Stop });
            }
        }

        private void buildScene()
        {
            trayMgr.destroyAllWidgets();

            vp.BackgroundColour = new ColourValue(1.0f,1.0f,1.0f);
            cam = scm.CreateCamera("SampleCam");
            vp.Camera = cam;
            cam.AspectRatio = vp.ActualWidth / vp.ActualHeight;
            cam.NearClipDistance = 5;

            scm.AmbientLight = new ColourValue(1.0f, 1.0f, 1.0f);

            Light light = scm.CreateLight("MainLight");
            light.Type = Light.LightTypes.LT_DIRECTIONAL;
            light.Position = new Mogre.Vector3(0, 20, 0);
            light.DiffuseColour = new ColourValue(1.0f, 1.0f, 1.0f);

            Plane plane=new Plane(Mogre.Vector3.UNIT_Y,-10);
            MeshManager.Singleton.CreatePlane("plane", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 1500, 1500, 20, 20, true, 1, 5, 5, Mogre.Vector3.UNIT_Z);
            Entity planeEnt = scm.CreateEntity("plane", "plane");
            scm.RootSceneNode.CreateChildSceneNode().AttachObject(planeEnt);
            planeEnt.SetMaterialName("Examples/BeachStones");

            scm.SetSkyBox(true, "Examples/SpaceSkyBox");
            raySceneQuery = scm.CreateRayQuery(new Ray());
            characterMgr = new Data.CharacterManager(cam,keyboard,mouse);
            characterMgr.SpawnCharacter(new Mogre.Vector3(0, 0, 0), "ogre1", "Sinbad");
            characterMgr.SpawnCharacter(new Mogre.Vector3(10, 0, 0), "ogre2", "Sinbad");
            characterMgr.SpawnCharacter(new Mogre.Vector3(20, 0, 0), "ogre3", "Sinbad");
            characterMgr.SpawnCharacter(new Mogre.Vector3(0, 0, 10), "ogre4", "Sinbad");
            characterMgr.SpawnCharacter(new Mogre.Vector3(0, 0, 20), "ogre5", "Sinbad");
            characterMgr.SpawnCharacter(new Mogre.Vector3(100, 0, 100), "ogre6", "Sinbad");

            mouse.MouseMoved += new MouseListener.MouseMovedHandler(mouse_MouseMoved);
            mouse.MousePressed += new MouseListener.MousePressedHandler(mouse_MousePressed);
            mouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouse_MouseReleased);
            keyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyboard_KeyPressed);
            keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyboard_KeyReleased);
            
        }

        bool keyboard_KeyReleased(KeyEvent arg)
        {
            return true;
        }

        bool keyboard_KeyPressed(KeyEvent arg)
        {
            if (arg.key == KeyCode.KC_ESCAPE)
            {
                if (ModStateChangedEvent != null)
                {
                    ModStateChangedEvent(this, new ModEventArgs() { modName="AMOFGameEngine.Mods.Sample", modState = ModState.Stop });
                }
            }
            return true;
        }

        bool mouse_MouseReleased(MouseEvent arg, MouseButtonID id)
        {
            return true;
        }

        bool mouse_MousePressed(MouseEvent arg, MouseButtonID id)
        {
            if (id == MouseButtonID.MB_Left)
            {
                Ray mouseRay = cam.GetCameraToViewportRay(mouse.MouseState.X.abs ,
            mouse.MouseState.Y.abs);
                raySceneQuery.Ray = mouseRay;
                RaySceneQueryResult result = raySceneQuery.Execute();

                foreach(RaySceneQueryResultEntry itr in result)
                {
                    if(itr.movable!=null)
                    {
                        characterMgr.DamageCharacter(itr.movable.Name,1000);
                        break;
                    }
                }
            }
            return true;
        }

        bool mouse_MouseMoved(MouseEvent arg)
        {
            
            return true;
        }

        void getInput()
        {
            if (keyboard.IsKeyDown (KeyCode.KC_W))
            {
                m_TranslateVector.z = -0.1f;
            }
            if (keyboard.IsKeyDown (KeyCode.KC_A))
            {
                m_TranslateVector.x = -0.1f;
            }
            if (keyboard.IsKeyDown (KeyCode.KC_S))
            {
                m_TranslateVector.z = 0.1f;
            }
            if (keyboard.IsKeyDown (KeyCode.KC_D))
            {
                m_TranslateVector.x = 0.1f;
            }
            if (keyboard.IsKeyDown(KeyCode.KC_E))
            {
                m_TranslateVector.y = 0.1f;
            }
            if (keyboard.IsKeyDown(KeyCode.KC_C))
            {
                m_TranslateVector.y = -0.1f;
            }
            cam.MoveRelative(m_TranslateVector);
        }

        void CamMove()
        {
            cam.MoveRelative(m_TranslateVector);
        }

        void CamRotate()
        {
            if (keyboard.IsKeyDown(KeyCode.KC_Z))
            {
                cam.Yaw(new Degree(rotateAngle));
            }
            if (keyboard.IsKeyDown(KeyCode.KC_X))
            {
                cam.Yaw(new Degree(-rotateAngle));
            }
            if (keyboard.IsKeyDown(KeyCode.KC_V))
            {
                cam.Pitch(new Degree(rotateAngle));
            }
            if (keyboard.IsKeyDown(KeyCode.KC_B))
            {
                cam.Pitch(new Degree(-rotateAngle));
            }
        }

        public override void Update(float timeSinceLastFrame)
        {
            m_TranslateVector = Mogre.Vector3.ZERO;

            getInput();
            //CamMove();
            CamRotate();
            characterMgr.MoveCharacter("ogre6");
            characterMgr.Update(timeSinceLastFrame);
            //characontroller.addTime(timeSinceLastFrame);
        }
    }
}
