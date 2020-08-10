using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mogre;
using OpenMB.Game.ItemTypes;

namespace OpenMB.Mods.Common.ItemTypes
{
	public class ItemTypeRideDrive : ItemType
	{

		public override string Name
		{
			get
			{
				return "IT_RIDEDRIVE";
			}
		}

		public override string SpawnAttachBoneName
		{
			get
			{
				return "Spin";
			}
		}

		public override void Use(params object[] param)
		{
		}

		public override MaterialPtr RenderPreview(Entity ent)
		{
			return null;
		}
	}
}
