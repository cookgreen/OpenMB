using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Mods;
using AMOFGameEngine.UI;

namespace AMOFGameEngine.Mods.Sample
{
    public class SampleSceneSP : SampleScene
    {
        public event EventHandler<ModEventArgs> ModStateChangedEvent;//0-Stop;1-Run
        Mogre.Vector3 m_TranslateVector;
        public SampleSceneSP( SceneManager scm,Viewport vp,SdkTrayManager trayMgr,Mouse mouse,Keyboard keyboard)
        {
            this.scm = scm;
            this.vp = vp;
            this.mouse = mouse;
            this.keyboard = keyboard;
            this.trayMgr = trayMgr;
            m_TranslateVector = new Mogre.Vector3();
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

            scm.SetSkyBox(true, "Examples/SpaceSkyBox");

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
                m_TranslateVector.x = 0.1f;
            }
            if (keyboard.IsKeyDown (KeyCode.KC_S))
            {
                m_TranslateVector.z = 0.1f;
            }
            if (keyboard.IsKeyDown (KeyCode.KC_D))
            {
                m_TranslateVector.x = -0.1f;
            }
        }

        void CamMove()
        {
            cam.MoveRelative(m_TranslateVector);
        }

        public override void Update(float timeSinceLastFrame)
        {
            m_TranslateVector = Mogre.Vector3.ZERO;

            getInput();
            CamMove();
        }
    }
}
