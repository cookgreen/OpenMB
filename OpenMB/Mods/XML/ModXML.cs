using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("Module")]
    public class ModXML
    {
        [XmlElement("Info")]
        public ModInfoXML ModInfo{ get; set;}

        [XmlElement("Assembly")]
        public string Assembly { get; set; }

        [XmlElement("Data")]
        public ModDataXML Data { get; set; }

        [XmlElement("Media")]
        public ModMediaXml Media { get; set; }
    }
}
