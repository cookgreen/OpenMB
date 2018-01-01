using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Game.Controller
{
    public class DragoonController : ManualControllerBase
    {
        public DragoonController(string name, string meshName, Camera cam)
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
            throw new NotImplementedException();
        }

        public override bool ControllerUpdateAnimations(float deltaTime)
        {
            throw new NotImplementedException();
        }

        public override bool ControllerUpdateCamera(float deltaTime)
        {
            throw new NotImplementedException();
        }

        public override bool ControllerUpdateBody(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
