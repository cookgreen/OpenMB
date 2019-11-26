using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Widgets
{
	public class WidgetFactoryMethodDefine
	{
		public string MethodName { get; set; }
		public int TotalParamterNumber { get; set; }
		public int NecessaryParamtersNumber { get; set; }
		public int OptionalParamtersNumber { get; set; }
		public bool AbosoluteOrRelative { get; set; }

		public WidgetFactoryMethodDefine(
			string methodName, 
			int totalParamterNumber,
			int necessaryParamtersNumber,
			int optionalParamtersNumber,
			bool abosoluteOrRelative)
		{
			MethodName = methodName;
			TotalParamterNumber = totalParamterNumber;
			NecessaryParamtersNumber = necessaryParamtersNumber;
			OptionalParamtersNumber = optionalParamtersNumber;
			AbosoluteOrRelative = abosoluteOrRelative;
		}
	}
}
