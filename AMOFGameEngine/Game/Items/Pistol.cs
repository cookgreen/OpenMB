using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;

namespace AMOFGameEngine.Game
{
    class Pistol : Item
    {
        private Camera cam;
        private int id;
        private Scene physicsScene;

        public Pistol(Camera cam, Scene physicsScene, int id) : base(cam, physicsScene, id)
        {
            this.cam = cam;
            this.physicsScene = physicsScene;
            this.id = id;
        }
    }
}
