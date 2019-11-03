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
        [XmlAttribute("ID")]
        public string ID { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Skin")]
        public string Skin { get; set; }
        [XmlElement("Side")]
        public string Side { get; set; }
        [XmlElement("Equips")]
		public ModCharacterEquipmentsDfnXml Equipments { get; set; }
	}

	[XmlRoot("Equips")]
	public class ModCharacterEquipmentsDfnXml
	{
		[XmlElement("EquipItem")]
		public List<string> EquipmentItems { get; set; }
		public ModCharacterEquipmentsDfnXml()
		{
			EquipmentItems = new List<string>();
		}
	}
}
