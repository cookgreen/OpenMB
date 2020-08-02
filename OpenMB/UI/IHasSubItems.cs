using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI
{
	public interface IHasSubItems
	{
		List<string> Items { get; }

		void AddItem(string item);

		void RemoveItem(string item);

		void ClearItems();
	}
}
