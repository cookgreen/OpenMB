using OpenMB.Script.Lua;
using OpenMB.Script.Python;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script
{
    public class GameScriptManager
    {
        private Dictionary<string, IGameScriptLoader> loaders;

        private static GameScriptManager instance;
        public static GameScriptManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameScriptManager();
                }
                return instance;
            }
        }

        public GameScriptManager()
        {
            loaders = new Dictionary<string, IGameScriptLoader>
            {
                { "Script", new ScriptLoader() },
                { "Lua", new LuaScriptLoader() },
                { "Python", new PythonScriptLoader() }
            };
        }

        public bool Parse(string typeName, string scriptFileName, string groupName, out IGameScript gameScript)
        {
            gameScript = null;

            if (!loaders.ContainsKey(typeName))
            {
                EngineManager.Instance.log.LogMessage("Error: No loader named '" + typeName + "' found!");
                return false;
            }

            gameScript = loaders[typeName].Parse(scriptFileName, groupName);
            return true;
        }

        public bool Parse(string scriptFile, string groupName, out IGameScript gameScript)
        {
            gameScript = null;

            IGameScriptLoader loader = getLoaderByExtension(scriptFile);
            if (loader != null)
            {
                gameScript = loader.Parse(scriptFile, groupName);
                return true;
            }
            return false;
        }

        private IGameScriptLoader getLoaderByExtension(string scriptFile)
        {
            string extension = Path.GetExtension(scriptFile);
            foreach(var loader in loaders.Values)
            {
                if(loader.Extension == extension)
                {
                    return loader;
                }
            }

            return null;
        }
    }
}
