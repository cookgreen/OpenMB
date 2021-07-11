using Mogre;
using OpenMB.Game.ControllerType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game.Controller
{
    /// <summary>
    /// Tank, Aircraft even Artillery will use this
    /// </summary>
    public class DriveController : GameObjectController
    {
        private IControllerType controllerType;

        public DriveController(
            GameWorld world, 
            IControllerType controllerType,
            Camera camera) : base(camera)
        {
            this.controllerType = controllerType;
        }

        public void MoveTo(Vector3 position)
        {
            controllerType.MoveTo(position);
        }
    }
}
