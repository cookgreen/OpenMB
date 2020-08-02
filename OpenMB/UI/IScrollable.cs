using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI
{
	public interface IScrollable
	{
		event Action Scrolled;
	}
}
