using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("MediaSection")]
    public class ModMediaSectionXml
    {
        [XmlAttribute]
        public string Type { get; set; }
        [XmlText]
        public string Directory { get; set; }
    }

    [XmlRoot("Media")]
    public class ModMediaXml
    {
        [XmlElement("MediaSection")]
        public List<ModMediaSectionXml> MediaSections { get; set; }
    }
}
