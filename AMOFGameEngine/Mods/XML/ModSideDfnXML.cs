using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AMOFGameEngine.Mods.XML
{
    [XmlRoot("Sides")]
    public class ModSidesDfnXML
    {
        [XmlElement("Side")]
        public List<ModSideDfnXML> Sides { get; set; }
    }

    public class ModSideDfnXML
    {
        [XmlElement("ID")]
        public string ID { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        //public ModSideRelationshipDfnXML relations { get; set; }
    }
    public class ModSideRelationshipDfnXML
    {
        public string SideName { get; set; }
        public string RelationValue { get; set; }

    }
}
