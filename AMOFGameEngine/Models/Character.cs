using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Models
{
    public class Character
    {
        CharacterController characterController;
        CharacterDesc characterDesc;

        public CharacterController CharacterController
        {
            get { return characterController; }
            set { characterController = value; }
        }

        public CharacterDesc CharacterDesc
        {
            get { return characterDesc; }
            set { characterDesc = value; }
        }
    }
}
