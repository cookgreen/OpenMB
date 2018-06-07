using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script
{
    public class ScriptLinkTableNode
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<ScriptLinkTableNode> NextNodes { get; set; }
        public ScriptLinkTableNode()
        {
            NextNodes = new List<ScriptLinkTableNode>();
            Value = "0";
        }
    }
}
