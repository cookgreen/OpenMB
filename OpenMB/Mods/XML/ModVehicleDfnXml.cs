using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
	[XmlRoot("Vehicles")]
	public class ModVehiclesDfnXml
	{
		[XmlElement("Vehicle")]
		public List<ModVehicleDfnXml> VehicleDfns { get; set; }
	}

	public class ModVehicleDfnXml
	{
		[XmlElement]
		public string ID { get; set; }
		[XmlElement]
		public string Name { get; set; }
		[XmlElement]
		public string Desc { get; set; }
		[XmlElement]
		public string ControllerType { get; set; }
		[XmlElement]
		public string FullPartMesh { get; set; }
		[XmlElement]
		public string DestroyedMesh { get; set; }
		[XmlElement]
		public string Material { get; set; }
		[XmlElement]
		public string Scale { get; set; }
		[XmlElement]
		public List<ModVehiclePartDfnXml> Parts { get; set; }
		[XmlArray("Flags")]
		[XmlArrayItem("Flag")]
		public List<VehicleFlags> Flags { get; set; }
	}

	public class ModVehiclePartDfnXml
	{
		[XmlElement]
		public VehiclePartType Type { get; set; }
		[XmlElement]
		public string Mesh { get; set; }
		[XmlElement]
		public string Material { get; set; }
		[XmlElement]
		public List<ModVehiclePartDfnXml> SubParts { get; set; }
	}
}
