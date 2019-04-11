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
        public Armour(int id, string name, string meshName, GameWorld world)
            : base(id, name, meshName, ItemType.IT_BODY_ARMOUR,
                  ItemHaveAttachOption.IHAO_NO_VALUE,
                  ItemUseAttachOption.IAO_BODY,
                  world)
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
