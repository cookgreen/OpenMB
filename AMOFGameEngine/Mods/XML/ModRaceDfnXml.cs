using AMOFGameEngine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AMOFGameEngine.Mods.XML
{
    [XmlRoot("Races")]
    public class ModRacesDfnXml
    {
        [XmlElement("Race")]
        public List<ModRaceDfnXml> Races { get; set; }
    }

    [XmlRoot("Race")]
    public class ModRaceDfnXml
    {
        [XmlElement("ID")]
        public string RaceID { get; set; }
        [XmlElement("Name")]
        public string RaceName { get; set; }
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
