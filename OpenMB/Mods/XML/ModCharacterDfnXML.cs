using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("Characters")]
    public class ModCharactersDfnXML
    {
        [XmlElement("Character")]
        public List<ModCharacterDfnXML> CharacterDfns { get; set; }
    }

    [XmlRoot("Character")]
    public class ModCharacterDfnXML
    {
        [XmlElement("ID")]
        public string ID { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("MeshName")]
        public string MeshName { get; set; }
        [XmlElement("SkinID")]
        public string SkinID { get; set; }
    }
}
