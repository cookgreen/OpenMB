using Mogre;
using OpenMB.Game;
using OpenMB.Game.ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods.Common.ItemTypes
{
	public class ItemTypeRifle : PlaceholderItemType
	{
		public override string Name
		{
			get { return "Rifle"; }
		}

		public override string SpawnAttachBoneName
		{
			get
			{
				return "LeftHand";
			}
		}

		public override MaterialPtr RenderPreview(Entity ent)
		{
			return null;
		}

		public override void Use(params object[] param)
		{

		}
	}
}
