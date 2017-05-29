using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AMOFGameEngine.Mods.Common
{
    public class ModMediaXML
    {
        [XmlArrayItem("MediaSection",typeof(ModSubMediaXML))]
        List<ModSubMediaXML> media;
    }
}
