using OpenMB.Game.ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods.Common.ItemTypes
{
    public class ItemTypeMeleeWeapon : ItemTypeWeapon
	{
		public override string Name
		{
			get { return "MeleeWeapon"; }
		}

        public ItemTypeMeleeWeapon() : base()
        {
            parameters.Add("animation");
        }

        public override void Use(params object[] param)
        {
            //Play the animation
        }
    }
}
