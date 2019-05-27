using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("Skin")]
    public class ModSkinDfnXML
    {
        [XmlElement("CharacterSkin")]
        public List<ModCharacterSkinDfnXML> CharacterSkinList { get; set; }
    }

    [XmlRoot("CharacterSkin")]
    public class ModCharacterSkinDfnXML
    {
        [XmlElement("ID")]
        public string skinID { get; set; }
        [XmlElement("Name")]
        public string skinName { get; set; }
        [XmlArray("Animations")]
        [XmlArrayItem("Animation")]
        public List<ModRaceAnimationDfnXml> RaceAnimations { get; set; }
    }

    public class ModRaceAnimationDfnXml
    {
        [XmlAttribute("Type")]
        public CharacterAnimationType type { get; set; }

        [XmlText()]
        public string Name { get; set; }
    }
}
