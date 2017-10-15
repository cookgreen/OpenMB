using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using AMOFGameEngine.RPG.Traits;

namespace AMOFGameEngine.RPG.Object
{

    public enum VehicleState
    {
        New,
        Destroyed
    }
    /// <summary>
    /// Vehicle Class
    /// </summary>
    public class Vehicle : MoveableObject,IDirvable
    {
        private string vehicleName;
        private string vehicleMeshName;
        private Camera cam;
        private Entity vehicleEnt;
        private SceneNode vehicleNode;
        private Mouse mouse;
        private Keyboard keyboard;
        private int speed;

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Vehicle(Camera cam, Mouse mouse, Keyboard keyboard)
        {
            this.cam = cam;
            this.keyboard = keyboard;
            this.mouse = mouse;
        }

        public void Create()
        {

        }

        public void Update()
        {
            updateVehicleCamera();
            updateVehicleParticle();
        }

        void setupVehicle()
        {
            vehicleEnt = cam.SceneManager.CreateEntity(vehicleName, vehicleMeshName);
            vehicleNode = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
            vehicleNode.AttachObject(vehicleEnt);
        }

        void setupVehicleCam()
        {

        }

        void updateVehicleCamera()
        {

        }
        void updateVehicleParticle()
        {

        }

        public void Drive()
        {

        }

        public void Stop()
        {

        }

        public void Turn()
        {

        }
    }
}
