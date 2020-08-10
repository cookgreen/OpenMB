using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using OpenMB.Mods.XML;

namespace OpenMB
{
	/// <summary>
	/// Display on the ModChooser
	/// </summary>
	public class ModBaseInfo
	{
		public readonly string InstallPath;
		public readonly string Name;
		public readonly string Description;
		public readonly string Author;
		public readonly string Icon;
		public readonly string Thumb;
		public readonly StartupBackground StartupBackground;
		public readonly List<string> Assemblies;
		public readonly bool DisplayInChooser;

		public ModBaseInfo(
			string installPath,
			string name,
			string description,
			string author,
			string icon,
			string thumb,
			StartupBackground startupBackground,
			List<string> assemblies,
			bool displayInChooser)
		{
			InstallPath = installPath;
			Name = name;
			Description = description;
			Author = author;
			Icon = icon;
			Thumb = thumb;
			StartupBackground = startupBackground;
			Assemblies = assemblies;
			DisplayInChooser = displayInChooser;
		}
	}
}
