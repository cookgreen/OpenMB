using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
	[XmlRoot("MapTemplates")]
	public class ModMapTemplatesDfnXml
	{
		[XmlElement("MapTemplate")]
		public List<ModMapTemplateDfnXml> MapTemplates { get; set; }
	}

	[XmlRoot("MapTemplate")]
	public class ModMapTemplateDfnXml
	{
		[XmlAttribute]
		public string ID { get; set; }
		[XmlElement]
		public string Logic { get; set; }

		[XmlArray("EntryPoints")]
		[XmlArrayItem("EntryPoint")]
		public List<ModMapTemplateEntryPointDfnXml> EntryPoints { get; set; }
	}

	[XmlRoot("EntryPoint")]
	public class ModMapTemplateEntryPointDfnXml
	{
		[XmlAttribute]
		public string ID { get; set; }
		[XmlAttribute]
		public string Team { get; set; }
	}
}
