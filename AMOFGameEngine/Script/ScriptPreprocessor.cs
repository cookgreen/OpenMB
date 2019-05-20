using AMOFGameEngine.Script.Command;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script
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
                var cmd = loader.ParseOneLine(res, out file, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, 1);
                if (cmd != null && cmd.GetType().Equals(typeof(NamespaceScriptCommand)))
                {
                    cmd.Execute(this);
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
