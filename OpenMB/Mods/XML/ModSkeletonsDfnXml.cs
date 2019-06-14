using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("Skeletons")]
    public class ModSkeletonsDfnXML
    {
        [XmlElement("Skeleton")]
        public List<ModSkeletonDfnXML> Skeletons { get; set; }
    }
    public class ModSkeletonDfnXML
    {
        [XmlElement]
        public string ID { get; set; }
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public string Mesh { get; set; }
    }
}
