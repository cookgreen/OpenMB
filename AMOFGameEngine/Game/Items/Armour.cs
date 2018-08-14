using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class Armour : Item
    {
        public Armour(string name, string meshName, Camera cam, Scene physicsScene)
            : base(name, meshName, ItemType.IT_INVALID, physicsScene, cam)
        {
            HeadArmourNum = 0;
            BodyArmourNum = 0;
            FootArmourNum = 0;
            HandArmourNum = 0;
        }

        public double HeadArmourNum { get; set; }
        public double BodyArmourNum { get; set; }
        public double FootArmourNum { get; set; }
        public double HandArmourNum { get; set; }
    }
}
