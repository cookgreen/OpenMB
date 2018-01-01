using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using AMOFGameEngine.Game.Traits;
using AMOFGameEngine.Game.Data;
using AMOFGameEngine.Game.Controller;

namespace AMOFGameEngine.Game.Objects
{
    /// <summary>
    /// Aircraft State
    /// </summary>
    public enum AircraftState
    {
        /// <summary>
        /// When a new aircraft created or aircraft stopped
        /// </summary>
        Idle,
        /// <summary>
        /// When a aircraft has driver
        /// </summary>
        Standby,
        /// <summary>
        /// When a aircraft driving
        /// </summary>
        Driving,
        /// <summary>
        /// When a aircraft destroyed
        /// </summary>
        Destroyed
    }

    public class AircraftInfo
    {
        private string name;
        private string meshName;

        public string Mesh
        {
            get { return meshName; }
            set { meshName = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
    /// <summary>
    /// Aircraft Class
    /// </summary>
    public class Aircraft : MoveableObject,IAttackable, IDirvable, IDestroyable, INodifyStateChanged
    {
        private AircraftInfo info;
        private AircraftController controller;
        private Keyboard keyboard;
        private Mouse mouse;

        public Aircraft(Keyboard key,Mouse ms)
        {
            keyboard = key;
            mouse = ms;

            keyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyboard_KeyPressed);
            keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyboard_KeyReleased);
            mouse.MouseMoved += new MouseListener.MouseMovedHandler(mouse_MouseMoved);
            mouse.MousePressed += new MouseListener.MousePressedHandler(mouse_MousePressed);
        }

        bool mouse_MousePressed(MouseEvent arg, MouseButtonID id)
        {
            return controller.InjectMousePressed(arg, id);
        }

        bool mouse_MouseMoved(MouseEvent arg)
        {
            return controller.InjectMouseMoved(arg);
        }

        bool keyboard_KeyReleased(KeyEvent arg)
        {
            return controller.InjectKeyReleased(arg);
        }

        bool keyboard_KeyPressed(KeyEvent arg)
        {
            return controller.InjectKeyPressed(arg);
        }

        public void Setup(string name, string mesh, Camera cam)
        {
            info = new AircraftInfo();
            info.Name = name;
            info.Mesh = mesh;
            controller = new AircraftController(info.Name, info.Mesh, cam);
            controller.SetPosition(initPos);
        }

        public void Attack(GameObject target)
        {
            throw new NotImplementedException();
        }

        public void Defence()
        {
        }

        public void Drive(Character driver)
        {
        }

        public void Stop()
        {
        }

        public void Turn()
        {

        }

        public void Destroyed()
        {
        }

        public void StateChanged(int oldState, int newState)
        {
            throw new NotImplementedException();
        }

        public override void Update(float deltaTime)
        {
            controller.ControllerUpdate(deltaTime);
        }
    }
}
