using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game.ItemTypes
{
	public class ItemTypeVehicle : IItemType
	{
		public string Name
		{
			get
			{
				return "IST_VEHICLE";
			}
		}

		public string AttachBoneName { get; }

		public MaterialPtr RenderPreview(Entity ent)
		{
			return null;
		}

		public void Use(params object[] param)
		{
		}
	}
}
