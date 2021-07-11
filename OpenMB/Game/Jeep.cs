using Mogre;
using OpenMB.Game.Controller;
using OpenMB.Game.ControllerType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
    public class Jeep : DynamicGameObject
    {
        private DriveController driveController;
        private IControllerType jeepControllerType;

        public Jeep(GameWorld world) : base(-1, world)
        {
            jeepControllerType = new JeepControllerType();
            driveController = new DriveController(world, jeepControllerType, world.Camera);
        }

        public void MoveTo(Vector3 position)
        {
            driveController.MoveTo(position);
        }
    }
}
