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
        [XmlElement("HasParts")]
        public bool HasParts { get; set; }
        [XmlElement("SkinParts")]
        public List<CharacterSkinPartsDfnXML> SkinParts { get; set; }
        [XmlArray("Animations")]
        [XmlArrayItem("Animation")]
        public List<ModRaceAnimationDfnXml> RaceAnimations { get; set; }
    }

    [XmlRoot("SkinParts")]
    public class CharacterSkinPartsDfnXML
    {
        [XmlElement("SkinPart")]
        public List<CharacterSkinPartDfnXML> SkinParts { get; set; }
    }

    public enum CharacterSkinPartType
    {
        HEAD,
        HALF_HEAD,
        BODY,
        LEFT_HAND,
        RIGHT_HAND,
        LEFT_CALF,
        RIGHT_CALF
    }

    public class CharacterSkinPartDfnXML
    {
        [XmlAttribute]
        public CharacterSkinPartType Type;
        [XmlAttribute]
        public string BindBone;
        [XmlText]
        public string Mesh;
    }

    public class ModRaceAnimationDfnXml
    {
        [XmlAttribute("Type")]
        public CharacterAnimationType type { get; set; }

        [XmlText()]
        public string Name { get; set; }
    }
}
