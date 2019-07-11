using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOIS;

namespace OpenMB.Game.ControlObjType
{
    public class ControlObjectTypeCharacter : IControlObjectType
    {
        private Character character;
        public ControlObjectTypeCharacter(Character character)
        {
            this.character = character;
        }

        public bool KeyPressed(KeyEvent arg)
        {
            character.InjectKeyPressed(arg);
            return true;
        }

        public bool KeyReleased(KeyEvent arg)
        {
            character.InjectKeyUp(arg);
            return true;
        }

        public bool MouseClick(MouseEvent arg, MouseButtonID id)
        {
            character.InjectMouseClick(arg, id);
            return true;
        }

        public bool MouseMoved(MouseEvent arg)
        {
            character.InjectMouseMove(arg);
            return true;
        }

        public bool MouseReleased(MouseEvent arg, MouseButtonID id)
        {
            character.InjectMouseReleased(arg, id);
            return true;
        }

        public void Update(float timeSinceLastFrame)
        {
            character.Update(timeSinceLastFrame);
        }
    }
}
