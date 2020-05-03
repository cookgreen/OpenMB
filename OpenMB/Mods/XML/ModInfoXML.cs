using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    public class ModInfoXML
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Description")]
        public string Description { get; set; }
        [XmlElement("Version")]
        public string Version { get; set; }
        [XmlElement("Icon")]
        public string Icon { get; set; }
        [XmlElement("Thumb")]
        public string Thumb { get; set; }
        [XmlElement("StartupBackground")]
        public StartupBackground StartupBackground { get; set; }
        [XmlArray("Assemblies")]
        [XmlArrayItem("Assembly")]
        public List<string> Assemblies { get; set; }
        [XmlElement]
        public bool DisplayInChooser { get; set; }
    }
	[XmlRoot("StartupBackground")]
	public class StartupBackground
	{
		[XmlAttribute]
		public string Type { get; set; }
		[XmlText]
		public string Value { get; set; }
	}
}
