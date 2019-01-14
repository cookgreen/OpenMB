using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Core
{
    public class Argument
    {
        private Dictionary<string, string> arguments;
        public Argument(string[] args)
        {
            arguments = new Dictionary<string, string>();
            arguments.Add("Engine.ShowConfig", "");
            arguments.Add("Engine.Mod", "");
        }

        public string GetArgValue(string argumentName)
        {
            return arguments.ContainsKey(argumentName) ? arguments[argumentName] : null;
        }

        public void AddArg(string argumentName, string argumentValue)
        {
            if (!arguments.ContainsKey(argumentName))
            {
                arguments.Add(argumentName, argumentValue);
            }
            else
            {
                arguments[argumentName] = argumentValue;
            }
        }
    }
}
