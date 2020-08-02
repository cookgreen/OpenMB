using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI
{
	public enum DockMode
	{
		Fill,
		FillWidth,
		FillHeight,
		None,
		Center,
	}

	public enum AlignMode
	{
		Center,
		Left,
		Right
	}

	/// <summary>
	/// Value type
	/// </summary>
	public enum ValueType
	{
		Abosulte,
		Percent,
		Auto,
	}
}
