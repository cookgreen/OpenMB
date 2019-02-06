using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;

namespace AMOFGameEngine.Game
{
    public class Rifle : Item
    {
        private Camera cam;
        private int id;
        private Scene physicsScene;

        public Rifle(Camera cam, Scene physicsScene, int id) : base(cam, physicsScene, id)
        {
            this.cam = cam;
            this.physicsScene = physicsScene;
            this.id = id;
        }
    }
}
