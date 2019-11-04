using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("Maps")]
    public class ModMapsDfnXml
    {
        [XmlElement("Map")]
        public List<ModMapDfnXml> Maps { get; set; }
    }
    [XmlRoot("Map")]
    public class ModMapDfnXml
    {
        [XmlAttribute]
        public string ID { get; set; }
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public string File { get; set; }
        [XmlAttribute]
        public string Loader { get; set; }
        [XmlArray("Flags")]
        [XmlArrayItem("Flag")]
        public List<ModMapFlagDfnXml> Flags { get; set; }
    }

    public class ModMapFlagDfnXml
    {
        [XmlText]
        public string Flag { get; set; }
    }
}
