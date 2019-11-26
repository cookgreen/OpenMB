using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
	[XmlRoot("Strings")]
	public class ModStringsDfnXml
	{
		[XmlElement("String")]
		public List<ModStringDfnXml> Strings { get; set; }
	}

	[XmlRoot("String")]
	public class ModStringDfnXml
	{
		[XmlAttribute("ID")]
		public string ID { get; set; }
		[XmlText]
		public string Content { get; set; }
	}
}
