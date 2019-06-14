using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("WorldMaps")]
    public class ModWorldMapsDfnXml
    {
        [XmlElement("WorldMap")]
        public List<ModWorldMapDfnXml> WorldMaps { get; set; }
    }
    [XmlRoot("WorldMap")]
    public class ModWorldMapDfnXml
    {
        [XmlElement]
        public string ID { get; set; }
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public string Map { get; set; }
        [XmlArray("Flags")]
        [XmlArrayItem("Flag")]
        public List<ModMapFlagDfnXml> Flags { get; set; }
    }

    public class ModWorldMapFlagDfnXml
    {
        [XmlText]
        public string Flag { get; set; }
    }
}
