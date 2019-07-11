using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("SceneProps")]
    public class ModScenePropsDfnXml
    {
        [XmlElement("SceneProp")]
        public List<ModScenePropDfnXml> SceneProps { get; set; }
    }
    [XmlRoot("SceneProp")]
    public class ModScenePropDfnXml
    {
        [XmlElement]
        public string ID { get; set; }
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public string Model { get; set; }
        [XmlElement]
        public string ModelType { get; set; }
    }
}
