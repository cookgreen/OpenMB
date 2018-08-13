using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class ItemFactory
    {
        private Camera cam;
        private Scene physicsScene;
        private ItemFactory instance;
        public ItemFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemFactory();
                }
                return instance;
            }
        }

        public void Initization(Camera cam, Scene physicsScene)
        {
            this.cam = cam;
            this.physicsScene = physicsScene;
        }

        public Item Produce(string type, double damage, int range)
        {
            Item item = null;
            switch(type)
            {
                case "Bow":
                    item = new Bow(cam, physicsScene, -1);
                    break;
                case "Crossbow":
                    item = new Crossbow(cam, physicsScene, -1);
                    break;
                case "Arrow":
                    item = new Arrow(cam, physicsScene, -1);
                    break;
                case "Bolt":
                    item = new Bolt(cam, physicsScene, -1);
                    break;
            }
            return item;
        }
    }
}
