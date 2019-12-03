using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("Sounds")]
    public class ModSoundsDfnXML
    {
        [XmlElement("Sound")]
        public List<ModSoundDfnXML> Sounds { get; set; }
    }
    [XmlRoot("Sound")]
    public class ModSoundDfnXML
    {
        [XmlElement("ID")]
        public string ID { get; set; }
        [XmlElement("File")]
        public string File { get; set; }
    }
}
