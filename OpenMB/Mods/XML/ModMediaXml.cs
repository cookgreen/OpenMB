using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    public enum ResourceType
    {
        /// <summary>
        /// Common resources managed by Ogre
        /// </summary>
        Common,
        Models,
        Textures,
        Maps,
        Music,
        Sound,
        Scripts,
    }
    [XmlRoot("MediaSection")]
    public class ModMediaSectionXml
    {
        [XmlAttribute]
        public string ResourceLoadType { get; set; }
        [XmlAttribute]
        public ResourceType ResourceType { get; set; }
        [XmlText]
        public string Directory { get; set; }
    }

    [XmlRoot("Media")]
    public class ModMediaXml
    {
        [XmlElement("MediaSection")]
        public List<ModMediaSectionXml> MediaSections { get; set; }
    }
}
