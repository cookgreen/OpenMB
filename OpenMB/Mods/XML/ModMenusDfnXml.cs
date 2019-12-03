using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
	[XmlRoot("Menus")]
	public class ModMenusDfnXml
	{
		[XmlElement("Menu")]
		public List<ModMenuDfnXml> Menus { get; set; }
	}

	[XmlRoot("Menu")]
	public class ModMenuDfnXml
	{
		[XmlAttribute]
		public string ID { get; set; }
		[XmlElement]
		public string Title { get; set; }
		[XmlElement]
		public string Logic { get; set; }
		[XmlArray("Children")]
		[XmlArrayItem("MenuItem")]
		public List<ModMenuItemDfnXml> Children { get; set; }
	}

	[XmlRoot("MenuItem")]
	public class ModMenuItemDfnXml
	{
		[XmlAttribute]
		public string id { get; set; }
		[XmlText]
		public string Text { get; set; }
	}
}
