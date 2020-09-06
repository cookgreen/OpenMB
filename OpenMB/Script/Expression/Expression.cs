using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Expression
{
    public class Expression
    {
        protected string returnValue;
        protected ExpressionTree exprTree;
        public string ReturnValue { get { return returnValue; } }
        public Expression()
        {
            exprTree = new ExpressionTree();
        }
        public virtual bool Execute(object[] exeArgs)
        {
            return false;
        }
    }

    public class ExpressionTree
    {
        public ExpressionTreeNode Root { get; set; }
    }

    public class ExpressionTreeNode
    {
        public string Str { get; set; }
        public ExpressionTreeNodeType NodeType { get; set; }
        public ExpressionTreeNode LeftNode { get; set; }
        public ExpressionTreeNode RightNode { get; set; }
    }

    public enum ExpressionTreeNodeType
    {
        Expr,
        Operator
    }
}
