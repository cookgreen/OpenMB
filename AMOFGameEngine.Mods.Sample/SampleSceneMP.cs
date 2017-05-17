using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Mods;

namespace AMOFGameEngine.Mods.Sample
{
    public class SampleSceneMP
    {
        public event EventHandler<ModEventArgs> ModStateChangedEvent;//0-Stop;1-Run
        private SceneManager scm;
        private Camera cam;
        private Viewport vp;
        private Mouse mouse;
        private Keyboard keyboard;
        private SdkTrayManager trayMgr;
        public SampleSceneMP(SceneManager scm, Viewport vp, SdkTrayManager trayMgr, Mouse mouse, Keyboard keyboard)
        {
            this.scm = scm;
            this.vp = vp;
            this.mouse = mouse;
            this.keyboard = keyboard;
            this.trayMgr = trayMgr;
        }

        public void Enter()
        {
            buildScene();
        }

        public void Leave()
        {
            if (ModStateChangedEvent != null)
            {
                ModStateChangedEvent(this, new ModEventArgs() {  modName="AMOFGameEngine.Mods.Sample",modState = ModState.Stop });
            }
        }

        private void buildScene()
        {
            trayMgr.destroyAllWidgets();

            vp.BackgroundColour = new ColourValue(1.0f,1.0f,1.0f);
            cam = scm.CreateCamera("SampleCamMP");
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
                    ModStateChangedEvent(this, new ModEventArgs() { modState = ModState.Stop });
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
    }
}
