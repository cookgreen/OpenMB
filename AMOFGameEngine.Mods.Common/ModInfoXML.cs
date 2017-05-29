using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AMOFGameEngine.Mods.Common
{
    public class ModInfoXML
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Description")]
        public string Description { get; set; }
        [XmlElement("Version")]
        public string Version { get; set; }
        [XmlElement("Thumb")]
        public string Thumb { get; set; }
    }
}
