using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("ItemTypes")]
    public class ModItemTypesDfnXml
    {
        [XmlElement("ItemType")]
        public List<ModItemTypeDfnXml> ItemTypes;
    }

    [XmlRoot("ItemType")]
    public class ModItemTypeDfnXml
    {
        [XmlElement]
        public string ID { get; set; }
        [XmlElement]
        public string Name { get; set; }
        [XmlArray("SubTypes")]
        [XmlArrayItem("ItemType")]
        public List<ModItemTypeDfnXml> SubTypes { get; set; }
    }
}
