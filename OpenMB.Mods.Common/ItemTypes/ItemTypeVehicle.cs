using Mogre;
using OpenMB.Game;
using OpenMB.Game.ItemTypes;
using OpenMB.Mods.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods.Common.ItemTypes
{
	public class ItemTypeVehicle : ItemTypeRideDrive
	{
		public override string Name
		{
			get { return "Vehicle"; }
		}

		public override string SpawnAttachBoneName { get; }

		public override MaterialPtr RenderPreview(Entity ent)
		{
			return null;
		}

		public override void SpawnIntoWorld()
		{

		}

		public override void SpawnIntoCharacter(GameWorld world, Character character)
		{
			var vehicleId = Item.ItemMeshName;
			var vehicleXmls = ModData.VehicleInfos.Where(o => o.ID == vehicleId);
			if (vehicleXmls.Count() == 0)
			{
				throw new Exception("Invalid vehicle id!");
			}

			var vehicleXml = vehicleXmls.First();
			Vehicle vehicle = new Vehicle(-1, world, character);
			vehicle.FullPartMesh = vehicleXml.FullPartMesh;
			for (int i = 0; i < vehicleXml.Parts.Count; i++)
			{
				var part = new VehiclePart(vehicle, null)
				{
					Type = vehicleXml.Parts[i].Type,
					PartMesh = vehicleXml.Parts[i].Mesh,
				};

				AddSubPart(vehicle, part, vehicleXml.Parts[i]);

				vehicle.Parts.Add(part);
			}

			Item.LinkedGameObject = vehicle;
		}

		private void AddSubPart(Vehicle vehicle, VehiclePart part, ModVehiclePartDfnXml xml)
		{
			for (int i = 0; i < xml.SubParts.Count; i++)
			{
				var subPart = new VehiclePart(vehicle, part)
				{
					Type = xml.SubParts[i].Type,
					PartMesh = xml.SubParts[i].Mesh,
				};
				AddSubPart(vehicle, subPart, xml.SubParts[i]);

				vehicle.Parts.Add(subPart);
			}
		}

		public override void Use(params object[] param)
		{
		}
	}
}
