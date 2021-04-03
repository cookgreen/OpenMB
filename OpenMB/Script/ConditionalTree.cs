using System.Collections.Generic;
using System.Linq;

namespace OpenMB.Script.Command
{
    public class ConditionalTree
    {
        public ConditionalTreeNode RootNode { get; set; }

        public ConditionalTree()
        {
            RootNode = new ConditionalTreeNode();
        }
        public void Execute(params object[] executeArgs)
        {
            RootNode.Execute(executeArgs);
        }
    }

    public class ConditionalTreeNode
    {
        public List<IScriptCommand> Conditions { get; set; }
        public List<IScriptCommand> Statements { get; set; }

        public List<ConditionalTreeNode> SubNodes { get; set; }

        public ConditionalTreeNode()
        {
            Conditions = new List<IScriptCommand>();
            Statements = new List<IScriptCommand>();
        }

        public void Execute(params object[] executeArgs)
        {
            //Check Condition
            List<bool> results = new List<bool>();
            foreach(var condition in Conditions)
            {
                results.Add(((ConditionalCheckScriptCommand)condition).DoCheck(executeArgs));
            }

            //Execute the statements if condition passed
            if (results.Where(o => o == true).Count() == results.Count || results.Count == 0)
            {
                foreach (var statement in Statements)
                {
                    statement.Execute(executeArgs);
                }

                foreach (var subNode in SubNodes)
                {
                    subNode.Execute(executeArgs);
                }
            }
        }
    }
}