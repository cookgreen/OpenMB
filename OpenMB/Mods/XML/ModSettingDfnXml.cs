using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("Settings")]
    public class ModSettingsDfnXml
    {
        [XmlElement("Setting")]
        public List<ModSettingDfnXml> Settings { get; set; }
    }

    [XmlRoot("Setting")]
    public class ModSettingDfnXml
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
