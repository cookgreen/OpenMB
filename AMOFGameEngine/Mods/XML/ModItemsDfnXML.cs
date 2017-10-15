using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AMOFGameEngine.RPG.Object;
using AMOFGameEngine.RPG.Data;

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
        [XmlElement("ID")]
        public string ID { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("MeshName")]
        public string MeshName { get; set; }
        [XmlElement("Type")]
        public ItemType Type { get; set; }
    }
}
