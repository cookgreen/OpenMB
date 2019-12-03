using OpenMB.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    public enum TrackType
    {
        EngineTrack,
        ModuleTrack,
        Scene
    }

    [XmlRoot("Track")]
    public class ModTrackDfnXML
    {
        [XmlElement("Id")]
        public string ID { get; set; }
        [XmlElement("File")]
        public string File { get; set; }
        [XmlElement("Type")]
        public TrackType Type { get; set; }
        [XmlElement("PlayType")]
        public SoundType PlayType { get; set; }
    }

    [XmlRoot("Tracks")]
    public class ModTracksDfnXML
    {
        [XmlElement("Track")]
        public List<ModTrackDfnXML> Tracks { get; set; }
    }
}
