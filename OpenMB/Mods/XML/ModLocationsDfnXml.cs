using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("Locations")]
    public class ModLocationsDfnXml
    {
        [XmlElement("Location")]
        public List<ModLocationDfnXml> Locations { get; set; }
    }
    [XmlRoot("Locations")]
    public class ModLocationDfnXml
    {
        [XmlAttribute]
        public string ID { get; set; }
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public ModLocationPositionDfnXml Position { get; set; }
        [XmlElement]
        public ModLocationModelDfnXml Model { get; set; }
        [XmlArray("Flags")]
        [XmlArrayItem("Flag")]
        public List<ModLocationFlagDfnXml> Flags { get; set; }
	}
	[XmlRoot("Model")]
	public class ModLocationModelDfnXml
	{
		[XmlAttribute]
		public string Type { get; set; }
		[XmlText]
		public string Resource { get; set; }
	}

	public class ModLocationPositionDfnXml
    {
        [XmlAttribute]
        public float X { get; set; }
        [XmlAttribute]
        public float Y { get; set; }
    }

    public class ModLocationFlagDfnXml
    {
        [XmlText]
        public string Flag { get; set; }
    }
}
