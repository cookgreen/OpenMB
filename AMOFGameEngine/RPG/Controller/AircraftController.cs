using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.RPG.Controller
{
    public class AircraftController : ControllerBase
    {
        public AircraftController(string name, string meshName, Camera cam)
            : base(name, meshName, cam)
        {

        }

        public override bool InjectKeyPressed(MOIS.KeyEvent evt)
        {
            return true;
        }

        public override bool InjectKeyReleased(MOIS.KeyEvent evt)
        {
            return true;
        }

        public override bool InjectMouseMoved(MOIS.MouseEvent evt)
        {
            return true;
        }

        public override bool InjectMousePressed(MOIS.MouseEvent evt, MOIS.MouseButtonID id)
        {
            return true;
        }

        public override bool InjectMouseReleased(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            return true;
        }

        public override bool ControllerSetup()
        {
            return true;
        }

        public override bool ControllerUpdateAnimations(float deltaTime)
        {
            return true;
        }

        public override bool ControllerUpdateCamera(float deltaTime)
        {
            return true;
        }

        public override bool ControllerUpdateBody(float deltaTime)
        {
            return true;
        }

        internal void SetPosition(Mogre.Vector3 initPos)
        {
            objectSceneNode.Position = initPos;
        }
    }
}
