using Mogre_Procedural.MogreBites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OpenMB.UI;

namespace OpenMB.Mods.XML
{
	[XmlRoot("UILayouts")]
	public class ModUILayoutsDfnXml
	{
		[XmlElement("UILayout")]
		public List<ModUILayoutDfnXml> UILayouts { get; set; }
	}
	public class ModUILayoutDfnXml
	{
		[XmlAttribute]
		public string ID { get; set; }
		[XmlElement]
		public StartupBackground Background { get; set; }
		[XmlElement]
		public string Script { get; set; }
		[XmlArray("Widgets")]
		[XmlArrayItem("Widget")]
		public List<ModUILayoutWidgetDfnXml> Widgets { get; set; }
	}
	public class ModUILayoutWidgetDfnXml
	{
		[XmlAttribute]
		public string Type { get; set; }
		[XmlAttribute]
		public string Text { get; set; }
		[XmlAttribute]
		public string Name { get; set; }
		[XmlAttribute]
		public UIWidgetLocation TrayLocation { get; set; }
		[XmlAttribute]
		public float Width { get; set; }
		[XmlAttribute]
		public float Height { get; set; }
		[XmlAttribute]
		public float Top { get; set; }
		[XmlAttribute]
		public float Left { get; set; }
		[XmlArray("WidgetParameters")]
		[XmlArrayItem("WidgetParameter")]
		public List<ModUILayoutWidgetParameterDfnXml> WidgetParameters { get; set; }
	}

	[XmlRoot("WidgetParameter")]
	public class ModUILayoutWidgetParameterDfnXml
	{
		[XmlAttribute]
		public string Name { get; set; }
		[XmlAttribute]
		public string Type { get; set; }
		[XmlText]
		public string Value { get; set; }
	}
}
