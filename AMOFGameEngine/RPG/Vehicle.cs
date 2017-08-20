using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.RPG
{

    public enum VehicleState
    {
        New,
        Destroyed
    }
    /// <summary>
    /// Vehicle Class
    /// </summary>
    public class Vehicle : RPGObject
    {
        string vehicleName;
        string vehicleMeshName;
        Camera cam;
        Entity vehicleEnt;
        SceneNode vehicleNode;
        Mouse mouse;
        Keyboard keyboard;

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
