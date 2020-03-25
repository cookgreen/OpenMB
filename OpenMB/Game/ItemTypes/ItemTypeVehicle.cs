using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game.ItemTypes
{
	public class ItemTypeVehicle : ItemType
	{
		public override string Name
		{
			get
			{
				return "IST_VEHICLE";
			}
		}

		public override string SpawnAttachBoneName { get; }

		public override MaterialPtr RenderInventoryPreview(Entity ent)
		{
			return null;
		}

		public override void SpawnIntoWorld()
		{

		}

		public override void SpawnIntoCharacter(Character character)
		{
			var vehicleId = Item.ItemMeshName;
		}

		public override void Use(params object[] param)
		{
		}
	}
}
