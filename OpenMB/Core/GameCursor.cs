using OpenMB.Mods.XML;
using OpenMB.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Core
{
	public class GameCursor
	{
		public string Name { get; private set; }
		public GameCursor() { }

		public void ChangeCursor(List<ModCursorDfnXml> cursorsData, string name)
		{
			Name = name;
			var cursorData = cursorsData.Where(o => o.Name == name).FirstOrDefault();
			if (cursorData != null)
			{
				UIManager.Instance.ShowCursor(cursorData.Resource);
			}
		}
	}
}
