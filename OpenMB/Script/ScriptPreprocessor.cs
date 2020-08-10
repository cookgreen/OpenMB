using OpenMB.Script.Command;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script
{
	public class ScriptPreprocessor
	{
		private Dictionary<string, List<ScriptFile>> namespaceFileDic;

		private static ScriptPreprocessor instance;
		public static ScriptPreprocessor Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ScriptPreprocessor();
				}
				return instance;
			}
		}

		public ScriptPreprocessor()
		{
			namespaceFileDic = new Dictionary<string, List<ScriptFile>>();
		}

		public void Process(List<string> resourceList)
		{
			foreach (var res in resourceList)
			{
				ScriptLoader loader = new ScriptLoader();
				ScriptFile file;
				file = loader.Parse(res, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
				if (file.Commands.Count > 0)
				{
					ScriptCommand namespaceCmd = (ScriptCommand)file.Commands[0];
					if (namespaceCmd != null && namespaceCmd.GetType().Equals(typeof(NamespaceScriptCommand)))
					{
						namespaceCmd.Execute(this);
					}
					else
					{
						if (namespaceFileDic.ContainsKey("(default)"))
						{
							if (namespaceFileDic["(default)"] == null)
							{
								namespaceFileDic["(default)"] = new List<ScriptFile>();
							}
							namespaceFileDic["(default)"].Add(file);
						}
						else
						{
							namespaceFileDic.Add("(default)", new List<ScriptFile>()
							{
								file
							});
						}
					}
				}
			}
		}

		public void LoadSpecificFunction(string function, params object[] executeArgs)
		{
			bool findedSpecificFunction = false;
			foreach (var kpl in namespaceFileDic)
			{
				for (int i = 0; i < kpl.Value.Count; i++)
				{
					if (findedSpecificFunction)
					{
						break;
					}

					var func = kpl.Value[i].FindFunction(function);
					if (func != null)
					{
						func.Execute(executeArgs);

						GameManager.Instance.log.LogMessage(
							string.Format("Execute Function `{0}` in file `{1}`",
							func.Name, kpl.Value[i].FileName));
						findedSpecificFunction = true;
					}
				}
				if (findedSpecificFunction)
				{
					break;
				}
			}
		}

		public List<ScriptFile> FindFile(string namespaceName)
		{
			List<ScriptFile> files = new List<ScriptFile>();
			if (namespaceFileDic.ContainsKey(namespaceName))
			{
				files.AddRange(namespaceFileDic[namespaceName]);
			}
			return files;
		}

		public void Add(string namespaceName, ScriptFile file)
		{
			if (namespaceFileDic.ContainsKey(namespaceName))
			{
				if (namespaceFileDic[namespaceName] == null)
				{
					namespaceFileDic[namespaceName] = new List<ScriptFile>();
				}
				namespaceFileDic[namespaceName].Add(file);
			}
			else
			{
				namespaceFileDic.Add(namespaceName, new List<ScriptFile>() {
					file
				});
			}
		}
	}
}
