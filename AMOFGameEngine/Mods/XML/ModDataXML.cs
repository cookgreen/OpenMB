using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AMOFGameEngine.Mods.XML
{
    public class ModDataXML
    {
        [XmlElement("Character")]
        public string characterXML { get; set; }
        [XmlElement("Music")]
        public string musicXML { get; set; }
        [XmlElement("Sound")]
        public string soundXML { get; set; }
        [XmlElement("Item")]
        public string itemXML { get; set; }
    }
}
