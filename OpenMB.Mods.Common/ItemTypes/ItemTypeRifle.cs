using Mogre;
using OpenMB.Game.ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods.Common.ItemTypes
{
    public class ItemTypeRifle : IItemType
    {
        public string Name
        {
            get
            {
                return "IT_Rifle";
            }
        }

        public string AttachBoneName
        {
            get
            {
                return "LeftHand";
            }
        }

		public MaterialPtr RenderPreview(Entity ent)
		{
			return null;
		}

		public void Use(params object[] param)
        {

        }
    }
}
