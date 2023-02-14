using OpenMB.Script.Lua;
using OpenMB.Script.Python;
using System;
using System.Collections.Generic;
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

        public bool Parse(string typeName, string groupName, out IGameScript gameScript)
        {
            gameScript = null;

            if (!loaders.ContainsKey(typeName))
            {
                EngineManager.Instance.log.LogMessage("Error: No loader named '" + typeName + "' found!");
                return false;
            }

            gameScript = loaders[typeName].Parse(groupName);
            return true;
        }
    }
}
