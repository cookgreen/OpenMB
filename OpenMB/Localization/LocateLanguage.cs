using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Localization
{
	public enum LanguageHandleType
	{
		Default,
		Unicode
	}

	public class LocateLanguage
	{
		public virtual string ID { get; set; }
		public virtual string FullName { get; set; }
		public virtual LanguageHandleType LanguageHandleType { get; set; }
	}
}
