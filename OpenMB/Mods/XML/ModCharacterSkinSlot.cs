using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("Slot")]
    public class ModCharacterSkinSlot
    {
        [XmlElement]
        public string AcceptType { get; set; }
    }
}
