using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
	public enum VehicleFlags
	{
		VF_Has_Turrent,
		VF_Wheels,
		VF_Tracked,
	}

	public enum VehiclePartType
	{
		VPT_Turrent,
		VPT_Turrent_Barrel,
		VPT_Body,
		VPT_Wheels,
		VPT_Tracked
	}

	public class VehiclePart
	{
		private Entity ent;
		private SceneNode entNode;
		private Vehicle vehicle;
		private VehiclePart parentPart;

		public VehiclePart(Vehicle vehicle, VehiclePart parentPart)
		{
			this.vehicle = vehicle;
			this.parentPart = parentPart;
		}

		public VehiclePartType Type { get; set; }
		public string PartMesh { get; set; }
		public Vector3 Offset { get; set; }
		public SceneNode Node { get { return entNode; } }
		public VehiclePart ParentPart { get { return parentPart; } }
		public List<VehiclePart> SubParts { get; set; }

		public SceneNode Create(SceneManager sceneManager)
		{
			ent = sceneManager.CreateEntity("VEHICLE-" + vehicle.Name + "-TURRENT-" + Guid.NewGuid().ToString());
			entNode = vehicle.Mesh.EntityNode.CreateChildSceneNode();
			entNode.AttachObject(ent);
			return entNode;
		}
	}

	public class Vehicle : GameObject
	{
		private VehicleController controller;

		public string KindID { get; set; }
		public string Name { get; set; }
		public string Desc { get; set; }
		public List<VehicleFlags> Flags { get; set; }
		public string FullPartMesh { get; set; }
		public string DestroyedMesh { get; set; }
		public List<VehiclePart> Parts { get; set; }
		public Vector3 SpawnScale { get; set; }

		public Vehicle(int id, GameWorld world, Character driver) : base(id, world)
		{
			Parts = new List<VehiclePart>();
			controller = new VehicleController(this, driver);
		}

		protected override void create()
		{
			if (Parts.Count == 0)
			{
				renderable.Entity = renderable.SceneManager.CreateEntity("", FullPartMesh);
				renderable.EntityNode = renderable.SceneManager.RootSceneNode.CreateChildSceneNode();
				renderable.EntityNode.AttachObject(renderable.Entity);
			}
			else
			{
				if (Parts.Where(o => o.Type == VehiclePartType.VPT_Body).Count() == 1)
				{
					var vehicleBody = Parts.Where(o => o.Type == VehiclePartType.VPT_Body).First();
					renderable.Entity = renderable.SceneManager.CreateEntity("VEHICLE-" + Name + "-BODY-" + Guid.NewGuid().ToString());
					renderable.EntityNode = renderable.SceneManager.RootSceneNode.CreateChildSceneNode();
					renderable.EntityNode.AttachObject(renderable.Entity);

					if (Flags.Exists(o => o == VehicleFlags.VF_Has_Turrent))
					{
						var turrentParts = Parts.Where(o => o.Type == VehiclePartType.VPT_Turrent);
						if (turrentParts.Count() > 0)
						{
							foreach (var turrentPart in turrentParts)
							{
								turrentPart.Create(renderable.SceneManager);
							}
						}
					}
					else
					{
						for (int i = 0; i < Parts.Count; i++)
						{
							Parts[i].Create(renderable.SceneManager);
						}
					}
				}
				else
				{
					//Invalid vehicle
				}
			}
		}

		public VehiclePart GetVehiclePart(VehiclePartType partType)
		{
			var part = Parts.Where(o => o.Type == partType).FirstOrDefault();
			return part;
		}

		public List<VehiclePart> GetVehicleParts(VehiclePartType partType)
		{
			var parts = Parts.Where(o => o.Type == partType);
			return parts.ToList();
		}

		public override void Update(float timeSinceLastFrame)
		{
			controller.Update(timeSinceLastFrame);
		}
	}
}
