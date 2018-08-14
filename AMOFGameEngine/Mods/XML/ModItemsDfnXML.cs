using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AMOFGameEngine.Game;

namespace AMOFGameEngine.Mods.XML
{
    [XmlRoot("Items")]
    public class ModItemsDfnXML
    {
        [XmlElement("Item")]
        public List<ModItemDfnXML> Items { get; set; }
    }

    public class ModItemDfnXML
    {
        [XmlAttribute("damage")]
        public string Damage { get; set; }
        [XmlAttribute("range")]
        public string Range { get; set; }
        [XmlElement("ID")]
        public string ID { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("MeshName")]
        public string MeshName { get; set; }
        [XmlElement("Type")]
        public string Type { get; set; }
    }
}
