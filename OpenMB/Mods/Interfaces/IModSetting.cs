using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods
{
	/// <summary>
	/// Define a mod setting to change some mod specific value
	/// </summary>
	public interface IModSetting
	{
		/// <summary>
		/// Setting Name
		/// </summary>
		string Name { get; }
		/// <summary>
		/// Setting Value
		/// </summary>
		string Value { get; set; }
		/// <summary>
		/// Execute logic when load this setting
		/// </summary>
		/// <param name="mod"></param>
		void Load(ModData mod);
	}
}
