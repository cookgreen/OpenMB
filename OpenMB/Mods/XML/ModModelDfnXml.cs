using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
	[XmlRoot("Models")]
	public class ModModelsDfnXml
	{
		[XmlElement("Model")]
		public List<ModModelDfnXml> Models { get; set; }
	}

	[XmlRoot("Model")]
	public class ModModelDfnXml
	{
		[XmlAttribute]
		public string ID { get; set; }
		[XmlElement]
		public string Material { get; set; }
		[XmlElement]
		public string Mesh { get; set; }
		[XmlIgnore]
		public IModModelType ModelType { get; set; }
	}
}
