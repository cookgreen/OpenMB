using OpenMB.Localization;
using OpenMB.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Core
{
	public class GameString
	{
		private string str;
		public GameString(string str)
		{
			this.str = str;
		}

		public static GameString FromString(string str, string originalString = null)
		{
			string localizedStr = LocateSystem.Instance.GetLocalizedStringMod(str, originalString);
			if (string.IsNullOrEmpty(localizedStr))
			{
				localizedStr = "$![No such key]";
			}
			GameString gameString = new GameString(localizedStr);
			return gameString;
		}

		public override string ToString()
		{
			bool entered = false;
			StringBuilder registerBlock = new StringBuilder();
			StringBuilder registerDeclareBlock = new StringBuilder();

			foreach (char c in str)
			{
				if (c == '{')
				{
					entered = true;
					registerBlock.Append(c);
				}
				else if(entered && c != '}')
				{
					registerBlock.Append(c);
					registerDeclareBlock.Append(c);
				}
				else if (c == '}')
				{
					entered = false;
					registerBlock.Append(c);

					string registerName = registerDeclareBlock.ToString();
					string registerValue = GameRegisterManager.Instance.GetRegisterValue(registerName);

					str = str.Replace(registerBlock.ToString(), registerValue);

					registerBlock.Clear();
					registerDeclareBlock.Clear();
				}
			}

			return str;
		}
	}
}
