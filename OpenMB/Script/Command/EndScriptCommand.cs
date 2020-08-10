using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	class EndScriptCommand : ScriptCommand
	{
		public override string CommandName
		{
			get
			{
				return "end";
			}
		}
		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.End;
			}
		}
	}
}
