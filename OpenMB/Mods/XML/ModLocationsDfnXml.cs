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
        [XmlElement]
        public string ID { get; set; }
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public string Mesh { get; set; }
        [XmlArray("Flags")]
        [XmlArrayItem("Flag")]
        public List<ModLocationFlagDfnXml> Flags { get; set; }
    }

    public class ModLocationFlagDfnXml
    {
        [XmlText]
        public string Flag { get; set; }
    }
}
