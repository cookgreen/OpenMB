using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
	[XmlRoot("Cursor")]
	public class ModCursorDfnXml
	{
		[XmlAttribute]
		public string Name { get; set; }
		[XmlAttribute]
		public string Resource { get; set; }
	}

	[XmlRoot("Cursors")]
	public class ModCursorsDfnXml
	{
		[XmlElement("Cursor")]
		public List<ModCursorDfnXml> Cursors { get; set; }

		public ModCursorsDfnXml()
		{
			Cursors = new List<ModCursorDfnXml>();
		}

		public string GetCursorByName(string name)
		{
			string cursor = null;
			var cur = Cursors.Where(o => o.Name == name).FirstOrDefault();
			if (cur != null)
			{
				cursor = cur.Resource;
			}
			return cursor;
		}
	}
}
