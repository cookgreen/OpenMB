using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
	public enum AnimationFlag
	{
		ANF_HAS_TOPBASE,
		ANF_SKELETON_ANIM
	}

	[XmlRoot("Animations")]
	public class ModAnimationsDfnXml
	{
		[XmlElement("Animation")]
		public List<ModAnimationDfnXml> Animations { get; set; }

		public ModAnimationsDfnXml()
		{
			Animations = new List<ModAnimationDfnXml>();
		}
	}

	[XmlRoot("Animation")]
	public class ModAnimationDfnXml
	{
		[XmlAttribute]
		public string ID { get; set; }
		[XmlAttribute]
		public string Resource { get; set; }
		[XmlArray("Flags")]
		[XmlArrayItem("Flag")]
		public List<AnimationFlag> Flags { get; set; }
		[XmlElement("SubAnimation")]
		public List<ModSubAnimationDfnXml> SubAnimations { get; set; }

		[XmlIgnore]
		public ModSubAnimationDfnXml this[AnimPlayType playType]
		{
			get
			{
				return SubAnimations.Where(o => o.PlayType == playType).FirstOrDefault();
			}
		}

		public ModAnimationDfnXml()
		{
			SubAnimations = new List<ModSubAnimationDfnXml>();
		}

		public bool HasFlags(AnimationFlag flag)
		{
			return Flags.Contains(flag);
		}
	}

	[XmlRoot("SubAnimation")]
	public class ModSubAnimationDfnXml
	{
		[XmlAttribute("Type")]
		public AnimPlayType PlayType { get; set; }
		[XmlText]
		public string Name { get; set; }
	}
}
