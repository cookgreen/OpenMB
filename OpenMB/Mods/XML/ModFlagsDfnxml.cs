using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("Flags")]
    public class ModFlagsDfnXml
    {
        [XmlElement("Flag")]
        public List<ModFlagDfnXml> Flags { get; set; }
    }

    [XmlRoot("Flag")]
    public class ModFlagDfnXml
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
    }
}
