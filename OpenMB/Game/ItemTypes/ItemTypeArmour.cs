using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game.ItemTypes
{
    //This kind of item can reduce the damage receive
    public class ItemTypeArmour : IItemType
    {
        public int Armour { get; set; }

        public string Name
        {
            get
            {
                return "IT_Armour";
            }
        }

        public string AttachBoneName
        {
            get
            {
                return "Spin";
            }
        }

        public void Use(params object[] param)
        {
            GameWorld world = param[0] as GameWorld;
            int userID = int.Parse(param[1].ToString());
            int damage = int.Parse(param[2].ToString());

            Character character = null;
            if (userID == -1)
            {

            }
            else
            {
                character = world.GetAgentById(userID);
            }
            
        }
    }
}
