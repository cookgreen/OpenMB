using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
	[XmlRoot("SceneProps")]
	public class ModScenePropsDfnXml
	{
		[XmlElement("SceneProp")]
		public List<ModScenePropDfnXml> SceneProps { get; set; }
	}
	[XmlRoot("SceneProp")]
	public class ModScenePropDfnXml
	{
		[XmlAttribute]
		public string ID { get; set; }
		[XmlAttribute]
		public bool Combined { get; set; }
		[XmlElement]
		public string Name { get; set; }
		[XmlArray("Models")]
		[XmlArrayItem("Model")]
		public List<ModScenePropModsDfnXml> Models { get; set; }
	}

	[XmlRoot("Model")]
	public class ModScenePropModsDfnXml
	{
		[XmlAttribute("Type")]
		public string ModelType { get; set; }
		[XmlText]
		public string ModelID { get; set; }
	}
}
