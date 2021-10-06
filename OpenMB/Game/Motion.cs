using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
    public class Motion
    {
        private Character character;

        public Motion(Character character)
        {
            this.character = character;
        }

        public string GetSkeletonAnimationName()
        {
            //Return current character skeleton animation
            return string.Empty;
        }
    }
}
