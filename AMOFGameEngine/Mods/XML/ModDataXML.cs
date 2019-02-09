using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AMOFGameEngine.Mods.XML
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
        [XmlElement("Races")]
        public string raceXML { get; set; }
    }
}
