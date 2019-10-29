using System;

namespace OpenMB.Game
{
	public class DescriptionAttribute : Attribute
	{
		private string text;
		public DescriptionAttribute(string text)
		{
			this.text = text;
		}
	}
}