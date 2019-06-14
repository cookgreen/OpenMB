using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    public class ModDataXML
    {
        [XmlElement("Characters")]
        public string characterXML { get; set; }
        [XmlElement("Music")]
        public string musicXML { get; set; }
        [XmlElement("Sound")]
        public string soundXML { get; set; }
        [XmlElement("Items")]
        public string itemXML { get; set; }
        [XmlElement("Sides")]
        public string sideXML { get; set; }
        [XmlElement("Skin")]
        public string skinXML { get; set; }
        [XmlElement("Maps")]
        public string mapsXml { get; set; }
        [XmlElement("WorldMaps")]
        public string worldMapsXML { get; set; }
        [XmlElement("Locations")]
        public string locationsXML { get; set; }
    }
}
