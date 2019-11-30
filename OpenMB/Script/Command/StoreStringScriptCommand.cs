using OpenMB.Game;
using OpenMB.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class StoreStringScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public StoreStringScriptCommand()
		{
			commandArgs = new string[2] {
				"Dest variable",
				"string text"
			};
		}
		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public override string CommandName
		{
			get
			{
				return "store_string";
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Line;
			}
		}

		public override void Execute(params object[] executeArgs)
		{
			if (CommandArgs.Length == 2)
			{
				GameWorld world = executeArgs[0] as GameWorld;
				string destVar = (string)CommandArgs[0];
				string srcString = (string)CommandArgs[1];
				if (destVar.StartsWith("%"))//local var
				{
                    if (srcString.StartsWith("@["))
                    {
                        string str = getQuickString(srcString);
                        str = str.Replace("_", " ");
                        str = str.Replace("^", Environment.NewLine);
                        Context.ChangeLocalValue(destVar.Substring(1), str);
                    }
                    else if (srcString.StartsWith("str_"))
                    {
                        var stringInfo = world.ModData.StringInfos.Where(o => o.ID == srcString).FirstOrDefault();
                        if (stringInfo != null)
                        {
                            Context.ChangeLocalValue(destVar.Substring(1), LocateSystem.Instance.GetLocalizedString(srcString, stringInfo.Content));
                        }
                    }
				}
				else if (destVar.StartsWith("$"))//global var
				{
					if (srcString.StartsWith("@["))
					{
						string str = getQuickString(srcString);
						str = str.Replace("_", " ");
						str = str.Replace("^", Environment.NewLine);
						world.ChangeGobalValue(destVar.Substring(1), str);
                    }
                    else if (srcString.StartsWith("str_"))
                    {
                        var stringInfo = world.ModData.StringInfos.Where(o => o.ID == srcString).FirstOrDefault();
                        if (stringInfo != null)
                        {
                            world.ChangeGobalValue(destVar.Substring(1), LocateSystem.Instance.GetLocalizedString(srcString, stringInfo.Content));
                        }
                    }
                }
			}
		}

		private string getQuickString(string srcString)
		{
			StringBuilder stringBuilder = new StringBuilder();

			foreach (var c in srcString)
			{
				if (c != '@' && c != '[')
				{
					stringBuilder.Append(c);
				}
				else if (c == ']')
				{
					break;
				}
			}

			return stringBuilder.ToString();
		}
	}
}
