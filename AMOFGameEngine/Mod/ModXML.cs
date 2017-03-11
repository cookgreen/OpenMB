using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace AMOFGameEngine.Mod
{
    class ModXML
    {
        string name;
        [XmlElement("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string description;
        [XmlElement("Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        string[] resources;
        [XmlArray("Resources"),XmlArrayItem("Resource")]
        public string[] Resources
        {
            get { return resources; }
            set { resources = value; }
        }
        string[] music;
        [XmlArray("Music"), XmlArrayItem("MusicLocation")]
        public string[] Music
        {
            get { return music; }
            set { music = value; }
        }
    }
}
