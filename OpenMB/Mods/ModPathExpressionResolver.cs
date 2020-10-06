using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods
{
    public class ModPathExpression
    {
        private string prefix;
        private string value;

        public string Prefix { get { return prefix; } }
        public string Value { get { return value; } }

        public ModPathExpression(string prefix, string value)
        {
            this.prefix = prefix;
            this.value = value;
        }
    }
    public class ModPathExpressionResolver
    {
        public static ModPathExpression Resolve(string pathExpression)
        {
            if (!pathExpression.StartsWith("|") &&
                pathExpression.Contains("|") &&
                !pathExpression.EndsWith("|"))
            {
                string[] tokens = pathExpression.Split('|');
                string prefix = tokens[0];
                string value = tokens[1];
                ModPathExpression modPathExpression = new ModPathExpression(prefix, value);
                return modPathExpression;
            }
            else
            {
                return null;
            }
        }
    }
}
